
namespace _3dEngine
{
    internal class Vec3
    {
        public float x, y, z;
        public Vec3(float _x, float _y, float _z ) { 
            x = _x; y = _y; z = _z;
        }
        public Vec3()
        {
            x = 0; y = 0; z = 0;
        }
        public Matrix Matrix() { 
            return new Matrix( new float[,] {{ x, y, z, 1 }} ).Transposed();
        }
        public static Vec3 Mult( Vec3 lhs, float n)
        {
            return new Vec3(lhs.x * n, lhs.y * n, lhs.z * n);
        }
        public static Vec3 Div(Vec3 lhs, float n)
        {
            return Mult(lhs, 1/n);
        }
        public static Vec3 operator *(Vec3 lhs, float n) {
            return Mult(lhs, n);
        }
        public static Vec3 operator *(Vec3 vec, Matrix m1)
        {
            Matrix m2 = vec.Matrix();
            Matrix m3 = m1 * m2;
            return new Vec3(m1[0, 0], m1[1,0], m1[2, 0]);
        }
        public static Vec3 operator /(Vec3 lhs, float n)
        {
            return Div(lhs, n);
        }
        public float Length()
        {
            return MathF.Sqrt( x*x + y*y + z*z );
        }
        public Vec3 Normalized()
        {
            return new Vec3();
        }
    }
    class Vec3Pool
    {
        public int size;
        public Vec3[] vectors;
        public int Length => vectors.Length;
        public Vec3Pool(int size) { 
            vectors = new Vec3[size];
            this.size = size;
        }
        public Vec3Pool(Vec3[] v) {
            vectors = v;
            size = v.Length;
        }
        public Vec3 this[int i] {
            get {
                if (i < 0 || i >= size )
                {
                    throw new IndexOutOfRangeException("Index out of bounds.");
                }

                return vectors[i];
            }
            set
            {
                if (i < 0 || i >= size )
                {
                    throw new IndexOutOfRangeException("Index out of bounds.");
                }

                vectors[i] = value;
            }
        }
        public static Vec3Pool operator *( Vec3Pool pool, float n) { 
            Vec3Pool result = new Vec3Pool( pool.size );
            for (int i = 0; i < pool.size; i++)
            {
                result[i] = pool[i] *n;
            }
            return result;
        }
        public static Vec3Pool operator *(Vec3Pool pool, Matrix m1)
        {
            Vec3Pool result = new Vec3Pool(pool.size);
            for (int i = 0; i < pool.size; i++)
            {
                result[i] = pool[i] * m1;
            }
            return result;
        }
    }
}
