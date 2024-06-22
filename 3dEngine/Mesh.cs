using OpenGL;
using static OpenGL.GL;

namespace _3dEngine
{
    internal class Mesh
    {
        private uint vao, vbo, ebo;
        private float[] verts;
        private uint[] faces;
        public Vec3 position;
        public Vec3 rotation;
        private Shader shader = new Shader();
        public Mesh(float[] _verts, uint[] _faces )
        {
            verts = _verts;
            faces = _faces;
            position = new Vec3();
            rotation = new Vec3();
            Init();

        }
        public unsafe void Init() 
        {
            vao = glGenVertexArray();
            vbo = glGenBuffer();
            ebo = glGenBuffer();

            glBindVertexArray( vao );

            glBindBuffer(GL_ARRAY_BUFFER, vbo);
            fixed (float* v = &verts[0])
            {
                glBufferData(GL_ARRAY_BUFFER, sizeof(float) * verts.Length, v, GL_STATIC_DRAW);
            }

            glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, ebo);
            fixed (uint* f = &faces[0])
            {
                glBufferData(GL_ELEMENT_ARRAY_BUFFER, sizeof(uint) * faces.Length, f, GL_STATIC_DRAW);
            }
            glVertexAttribPointer(0, 3, GL_FLOAT, false, 3 * sizeof(float), NULL);
            glEnableVertexAttribArray(0);
            glBindVertexArray(0);

            int location = glGetUniformLocation(shader.program, "color");
            glUniform3f(location, 1f, 0f, 0f);
        }
        public unsafe void Render( Camera camera )
        {
            shader.Activate();
            glBindVertexArray(vao);
            glDrawElements(GL_TRIANGLES, faces.Length, GL_UNSIGNED_INT, (int*)0);
        }
        //public void ApplyTransform( Matrix transformMatrix )
        //{
        //    verts = verts * transformMatrix;
        //}

    }
    class Cube : Mesh
    {
        public Cube(float side) : base( new float[]
            { -.5f,-.5f, .5f,
               .5f,-.5f, .5f,
               .5f, .5f, .5f,
              -.5f, .5f, .5f,
              -.5f,-.5f,-.5f,
               .5f,-.5f,-.5f,
               .5f, .5f,-.5f,
              -.5f, .5f,-.5f } ,
            new uint[]{
                3, 1, 0,
                3, 2, 1,
                6, 4, 5,
                7, 4, 6,
                2, 5, 1,
                6, 5, 2,
                7, 0, 4,
                3, 0, 7,
                0, 1, 5,
                4, 0, 1,
                3, 6, 2,
                3, 7, 6})
        {

        }
    }
}
