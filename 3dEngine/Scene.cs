
namespace _3dEngine
{
    internal class Scene
    {
        public List<Mesh> Meshes { get; private set; }
        public List<Light> Lights;

        public Scene() {
            Meshes = new List<Mesh>();
            Lights = new List<Light>();
        }
        public void Add(Mesh mesh)
        {
            Meshes.Add(mesh);
        }
        public void Add(Light light)
        {
            Lights.Add(light);
        }
    }
}
