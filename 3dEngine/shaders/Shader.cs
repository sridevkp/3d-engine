using static OpenGL.GL;

namespace _3dEngine
{
    internal class Shader
    {
        public uint program;
        public Shader()
        {
            string vertsrc = File.ReadAllText(@"../../../shaders/default.vert");
            string fragsrc = File.ReadAllText(@"../../../shaders/default.frag");
            uint program = CreateProgram(vertsrc, fragsrc);
        }
        public Shader(string vertfilepath, string fragfilepath) 
        {
            string vertsrc = File.ReadAllText(vertfilepath);
            string fragsrc = File.ReadAllText(fragfilepath);
            uint program = CreateProgram(vertsrc, fragsrc);
        }
        public void Activate()
        {
            glUseProgram(program);
        }
        public void Delete()
        {
            glDeleteProgram(program); 
        }
        public static uint CreateProgram(string vertsrc, string fragsrc)
        {
            uint vertex = CreateShader(GL_VERTEX_SHADER, vertsrc);
            uint fragment = CreateShader(GL_FRAGMENT_SHADER, fragsrc);
            uint program = glCreateProgram();

            glAttachShader(program, vertex);
            glAttachShader(program, fragment);

            glLinkProgram(program);

            glDeleteShader(vertex);
            glDeleteShader(fragment);

            glUseProgram(program);
            return program;
        }
        public static uint CreateShader(int type, string source)
        {
            uint shader = glCreateShader(type);
            glShaderSource(shader, source); 
            glCompileShader(shader);
            return shader;
        }
    }
}
