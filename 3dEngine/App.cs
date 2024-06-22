using System.Numerics;
using GLFW;
using static OpenGL.GL;

namespace _3dEngine
{
    class App
    {
        private Window win;
        private Random random = new Random();

        private int width = 1024;//screen.Width;
        private int height = 900;//screen.Height;

        private Camera camera;
        private Scene scene;
        //public  Timer timer;
        public void Run()
        {
            var screen = Glfw.PrimaryMonitor.WorkArea;
           
            PrepareContext();
            win = CreateWindow("3d engine", width, height);
            //glViewport(0, 0, width, height);

            Renderer renderer = new Renderer( camera );

            //timer = new Timer(HandleTimeout, null, 10, 100);
            Setup();

            double previousTime = Glfw.Time;
            double delta = 0.0;
            while (!Glfw.WindowShouldClose(win))
            {
                double currentTime = Glfw.Time;
                delta = currentTime - previousTime;
                previousTime = currentTime;
                DebugFPS(delta);

                glClear(GL_COLOR_BUFFER_BIT);  // Clear screen
                ProcessInput(win);             // Input handling
                Update(delta);                 // Update
                renderer.Render( scene );      // Drawing

                Glfw.SwapBuffers(win);
                Glfw.PollEvents();
            }
            Glfw.Terminate();
        }
        public void Setup()
        {

            scene = new Scene();
            camera = new Camera(45, width / height, 1, 100);
            DirectionalLight light = new DirectionalLight(1, new Vec3(0, 0, 1), new Vec3(1, 1, 1));
            
            float[] vertices = new float[]
            {
            -0.5f, -0.5f, 0.0f,
             0.5f, -0.5f, 0.0f,
             0.0f,  0.5f, 0.0f
            };
            uint[] faces = new uint[] { 0, 1, 2 };

            Mesh mesh = new Mesh(vertices, faces);

            //Cube cube = new Cube(1);
            //scene.Add(cube);
            scene.Add(mesh);
            scene.Add(light);
        }
        public void Update(double delta)
        {
            //Matrix rotY = Matrix.MatrixRotationY((float)delta * 3.14f);

        }
        private void ProcessInput(Window win)
        {
            if (Glfw.GetKey(win, Keys.Escape) == InputState.Press)
                Glfw.SetInputMode(win, InputMode.Cursor, (int)CursorMode.Normal);
            //Glfw.SetWindowShouldClose(win, true);

            if (Glfw.GetMouseButton(win, MouseButton.Left) == InputState.Press)
                Glfw.SetInputMode(win, InputMode.Cursor, (int)CursorMode.Hidden);

            if (Glfw.GetInputMode(win, InputMode.Cursor) == (int)CursorMode.Hidden)
            {
                Glfw.GetCursorPosition(win, out double mousex, out double mousey);
                Glfw.SetCursorPosition(win, width / 2, height / 2);
            }
        }
        public void DebugFPS(double delta)
        {
            float fps = (float)(1 / delta);
            Glfw.SetWindowTitle(win, fps.ToString());
        }
        private Window CreateWindow(string title, int width, int height)
        {
            Window window = Glfw.CreateWindow(width, height, title, GLFW.Monitor.None, Window.None);
            window.Opacity = 1f;

            Glfw.MakeContextCurrent(window);
            Import(Glfw.GetProcAddress);

            return window;
        }
        public Vector2 GetWindowPosition(Window window)
        {
            int x = 0, y = 0;
            Glfw.GetWindowPosition(win, out x, out y);
            return new Vector2(x, y);
        }
        private void PrepareContext()
        {
            Glfw.WindowHint(Hint.ClientApi, ClientApi.OpenGL);
            Glfw.WindowHint(Hint.ContextVersionMajor, 3);
            Glfw.WindowHint(Hint.ContextVersionMinor, 3);
            Glfw.WindowHint(Hint.OpenglProfile, Profile.Core);
            Glfw.WindowHint(Hint.Doublebuffer, true);
            Glfw.WindowHint(Hint.Decorated, true);
            //Glfw.WindowHint(Hint.Maximized, true);
        }
        
    }
}