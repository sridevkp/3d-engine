
namespace _3dEngine
{
    internal class Camera
    {
        public float fovDegree, far, near, aspectRatio;
        public Vec3 position, dir;
        public Camera( float _fovDegree, float aspectRation, float _near=1, float _far=100)
        {
            fovDegree = _fovDegree;
            dir = new Vec3(0, 0, 1);
            position = new Vec3();
            near = _near;
            far = _far;
        }
        public Matrix createMatrix()
        {
            return new Matrix( 4, 4 );
        }
        public Matrix getProjectionMatrix()
        {
            return Matrix.MatrixPerspectiveProjection( fovDegree, aspectRatio, near, far );
        }
        public void rotateY(float yaw) { 
            Matrix matCameraRot = Matrix.MatrixRotationY(yaw);
            dir = dir * matCameraRot ;
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


