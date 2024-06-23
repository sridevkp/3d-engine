
using System.Numerics;

namespace _3dEngine
{
    internal class Light
    {
        public float intensity;
        public Vec3 position;
        public Vec3 color;
        public float Ambient;
        public float Diffuse;
        public float Specular;

     }

    class DirectionalLight : Light
    {
        public Vec3 direction;
        public DirectionalLight( float intensity, Vec3 direction, Vec3 color )
        {
            this.direction = direction;
        }
    }
}
