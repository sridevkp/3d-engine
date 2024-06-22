
namespace _3dEngine
{
    internal class Camera
    {
        public float FovDegree, Far, Near, AspectRatio;
        public Vec3 Position { get; set; }
        private Vec3 dir;
        public Vec3 Dir { get { return dir; } set {
                dir = value.Normalized();
            } }
        public Camera()
        {
            Dir = Vec3.UnitX;
            Position = new Vec3();
        }

        public Matrix GetViewMatrix()
        {
            return Matrix.LookAt( Position, Position +Dir, Vec3.UnitY);
        }
        public Matrix GetProjectionMatrix()
        {
            return Matrix.MatrixPerspectiveProjection(FovDegree, AspectRatio, Near, Far );
        }

        public static bool operator ==( Camera left, Camera right ) {
            return left.Equals(right);
        }
        public static bool operator !=(Camera left, Camera right)
        {
            return ! left.Equals(right);
        }
    }
}


