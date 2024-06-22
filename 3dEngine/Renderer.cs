using static OpenGL.GL;

namespace _3dEngine
{
    internal class Renderer
    {
        public Matrix projMat ;
        public Camera defaultCamera;
        public Renderer( Camera _defaultCamera)
        {
            defaultCamera = _defaultCamera;
            //projMat = defaultCamera.getProjectionMatrix();
            glEnable(GL_DEPTH_TEST);
        }
        public void Render(Scene scene)
        {
            foreach (Mesh mesh in scene.Meshes)
            {
                mesh.Render(defaultCamera);
            }
        }
        public void Render(Scene scene, Camera camera)
        {
            foreach( Mesh mesh in scene.Meshes)
            {
                mesh.Render( camera );
            }
        }
    }
}
