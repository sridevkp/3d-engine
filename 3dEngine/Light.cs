
namespace _3dEngine
{
    internal class Light
    {
        public float intensity;
        public Vec3 position;
        public Vec3 color;
        public Light(float intensity, Vec3 position, Vec3 color)
        {
            this.intensity = intensity;
            this.color = color;
            this.position = position;   
        }
     }

    class DirectionalLight : Light
    {
        public Vec3 direction;
        public DirectionalLight( float intensity, Vec3 direction, Vec3 color ) : base( intensity, new Vec3(), color )
        {
            this.direction = direction;
        }
    }
}
