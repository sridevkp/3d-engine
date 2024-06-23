using System.Drawing;
using System.Numerics;
using GLFW;
using static OpenGL.GL;

namespace _3dEngine
{
    class App
    {
        private Window win;
        private Random random = new Random();

        private int Width = 1366;//screen.Width;
        private int Height = 768;//screen.Height;
        float delta ;

        private Camera camera;
        private Scene scene;
        private Renderer renderer;
        private Timer timer;

        Mesh mesh;
        private float camSpeed = 5f;
        public void Run()
        {
            Rectangle screen = Glfw.PrimaryMonitor.WorkArea;
           
            PrepareContext();
            win = CreateWindow("3d engine", Width, Height);
            Glfw.SetWindowPosition(win, screen.Width/2 -Width/2, screen.Height/2 -Height/2);
            glViewport(0, 0, Width, Height);

            Glfw.SetCursorPositionCallback(win, MouseCallback);
            Glfw.SetMouseButtonCallback(win, MouseButtonCallback);
            Glfw.SetKeyCallback(win, KeyCallback);
            Glfw.SetWindowSizeCallback(win, SizeCallback);


            timer = new Timer(HandleTimeout, null, 10, 1000);
            Setup();

            double previousTime = Glfw.Time;
            while (!Glfw.WindowShouldClose(win))
            {
                double currentTime = Glfw.Time;
                delta = (float)(currentTime - previousTime);
                previousTime = currentTime;
                //DebugFPS(delta);
                Glfw.SwapBuffers(win);
                Glfw.PollEvents();

                glClear(GL_COLOR_BUFFER_BIT);  // Clear screen
                ProcessInput(win, delta);             // Input handling
                Update(delta);                 // Update
                renderer.Render( scene );      // Drawing

            }
            Glfw.Terminate();
        }
        private void Setup()
        {
            scene = new Scene();
            camera = new Camera()
            {
                FovDegree = 60,
                AspectRatio = Width / (float)Height,
                Near = 1,
                Far = 100,
            };
            camera.Position = new Vec3(0,0,-5);
            camera.Dir = new Vec3(0, 0, -1);

            DirectionalLight light = new DirectionalLight(1, new Vec3(0, 0, 1), new Vec3(1, 1, 1));

            mesh = Mesh.Load(@"../../../models/teapot.obj");
            scene.Add(mesh);
            //scene.Add(cube);
            scene.Add(light);

            renderer = new Renderer(camera);
        }
        private void Update(float delta)
        {
            mesh.rotation.X += float.Pi * 0.2f * delta;
            mesh.rotation.Y += float.Pi * 0.2f * delta;
        }
        private void SizeCallback(Window win, int width, int height)
        {
            Width = width;
            Height = height;
            glViewport(0, 0, Width, Height);
            camera.AspectRatio = Width / (float)Height;
        }
        private void MouseButtonCallback(Window win, MouseButton button, InputState state, ModifierKeys mods)
        {
            if (state == InputState.Press && button == MouseButton.Left)
            {
                Glfw.SetInputMode(win, InputMode.Cursor, (int)CursorMode.Hidden);
            }
        }
        private void MouseCallback(Window window, double xpos, double ypos)
        {
            if (Glfw.GetInputMode(win, InputMode.Cursor) == (int)CursorMode.Hidden)
            {
                Vector2 newMouse = new Vector2((float)xpos, (float)ypos);
                Vector2 mouseMotion = newMouse - new Vector2(Width / 2f, Height / 2f);
                //Mouse = newMouse;
                Glfw.SetCursorPosition(win, Width / 2, Height / 2);

                camera.Dir *= Matrix.MatrixRotationY(-mouseMotion.X * delta);
                camera.Dir *= Matrix.MatrixRotationAxis(camera.Basis.X, -mouseMotion.Y * delta);
            }
        }
        private void KeyCallback(Window window, Keys key, int scancode, InputState state, ModifierKeys mods)
        {
            if (state == InputState.Press)
            {
                switch (key)
                {
                    case Keys.Escape:
                        Glfw.SetInputMode(win, InputMode.Cursor, (int)CursorMode.Normal);
                        //Glfw.SetWindowShouldClose(win, true);
                        break;

                }
            }
        }
        private void ProcessInput(Window win, float delta)
        {
            Basis camBasis = camera.Basis;
            if (Glfw.GetKey(win, Keys.Up) == InputState.Press)
                camera.Position += camBasis.Y * camSpeed * delta;

            if (Glfw.GetKey(win, Keys.Down) == InputState.Press)
                camera.Position -= camBasis.Y * camSpeed * delta;

            if (Glfw.GetKey(win, Keys.D) == InputState.Press)
                camera.Position += camBasis.X * camSpeed  * delta;

            if (Glfw.GetKey(win, Keys.A) == InputState.Press)
                camera.Position -= camBasis.X * camSpeed * delta;

            if (Glfw.GetKey(win, Keys.W) == InputState.Press)
                camera.Position -= camBasis.Z * camSpeed * delta;

            if (Glfw.GetKey(win, Keys.S) == InputState.Press)
                camera.Position += camBasis.Z * camSpeed * delta;
        }
        public void HandleTimeout(object state)
        {
            DebugFPS();
        }
        public void DebugFPS()
        {
            int fps = (int)(1 / delta);
            Glfw.SetWindowTitle(win, fps.ToString());
        }
        private Window CreateWindow(string title, int Width, int Height)
        {
            Window window = Glfw.CreateWindow(Width, Height, title, GLFW.Monitor.None, Window.None);
            window.Opacity = 1f;

            Glfw.MakeContextCurrent(window);
            Import(Glfw.GetProcAddress);

            return window;
        }
        private Vector2 GetWindowPosition(Window window)
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