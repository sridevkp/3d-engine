using System.Numerics;
using static OpenGL.GL;

namespace _3dEngine
{
    internal class Shader
    {
        public uint program { get; private set; }
        public Shader()
        {
            string vertsrc = File.ReadAllText(@"../../../shaders/default.vert");
            string fragsrc = File.ReadAllText(@"../../../shaders/default.frag");
            program = CreateProgram(vertsrc, fragsrc);
        }
        public Shader(string vertfilepath, string fragfilepath) 
        {
            string vertsrc = File.ReadAllText(vertfilepath);
            string fragsrc = File.ReadAllText(fragfilepath);
            program = CreateProgram(vertsrc, fragsrc);
        }
        public void Activate()
        {
            glUseProgram(program);
        }
        public void Delete()
        {
            glDeleteProgram(program); 
        }
        public void SetColor(float r, float g, float b)
        {
            SetVector3("color", new Vec3(r, g, b));
        }
        public unsafe void SetMatrix4(string name, Matrix matrix)
        {
            int location = glGetUniformLocation(program, name);
            fixed ( float* m = &matrix.mat[0, 0])
            {
                glUniformMatrix4fv(location, 1, false, m);
            }
        }
        public void SetVector3(string name, Vec3 vector)
        {
            int location = glGetUniformLocation(program, name);
            glUniform3f(location, vector.X, vector.Y, vector.Z);
        }
        public static uint CreateProgram(string vertsrc, string fragsrc)
        {
            uint vertex = CreateShader(GL_VERTEX_SHADER, vertsrc);
            uint fragment = CreateShader(GL_FRAGMENT_SHADER, fragsrc);

            uint program = glCreateProgram();

            glAttachShader(program, vertex);
            glAttachShader(program, fragment);

            glLinkProgram(program);
            CheckCompileErrors(program, "PROGRAM");

            glDeleteShader(vertex);
            glDeleteShader(fragment);

            glUseProgram(program);
            return program;
        }
        private static unsafe void CheckCompileErrors(uint shader, string type)
        {
            int success;

            if (type != "PROGRAM")
            {
                glGetShaderiv(shader, GL_COMPILE_STATUS, &success);
                if (success == GL_FALSE)
                {
                    string infoLog = glGetShaderInfoLog(shader);
                    throw new System.Exception($"{type} shader compilation error: {infoLog}");
                }
            }
            else
            {
                glGetProgramiv(shader, GL_LINK_STATUS, &success);
                if (success == GL_FALSE)
                {
                    string infoLog = glGetProgramInfoLog(shader);
                    throw new System.Exception($"Program linking error: {infoLog}");
                }
            }
        }
        public static uint CreateShader(int type, string source)
        {
            uint shader = glCreateShader(type);
            glShaderSource(shader, source); 
            glCompileShader(shader);
            CheckCompileErrors(shader, "SHADER");
            return shader;
        }
    }
}
