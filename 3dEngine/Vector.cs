
using System.Numerics;

namespace _3dEngine
{
    internal class Vec3
    {
        public float X, Y, Z;

        public Vec3(float x, float y, float z ) { 
            X = x; Y = y; Z = z;
        }
        public Vec3()
        {
            X = 0; Y = 0; Z = 0;
        }

        public string toString()
        {
            return $"<{X},{Y},{Z}>";
        }
        public Matrix Matrix() { 
            return new Matrix( new float[,] {{ X, Y, Z, 1 }} );
        }
        public float Length()
        {
            return MathF.Sqrt( X*X + Y*Y + Z*Z );
        }
        public Vec3 Normalized()
        {
            return this / this.Length();
        }

        public static Vec3 UnitY => new Vec3(0, 1, 0);
        public static Vec3 UnitZ => new Vec3(0, 0, 1);
        public static Vec3 UnitX => new Vec3(1, 0, 0);

        public static Vec3 Mult( Vec3 lhs, float n)
        {
            return new Vec3(lhs.X * n, lhs.Y * n, lhs.Z * n);
        }

        public static Vec3 operator *(Vec3 lhs, float n) {
            return Mult(lhs, n);
        }
        public static Vec3 operator /(Vec3 lhs, float n)
        {
            return Mult(lhs, 1/n);
        }
        public static Vec3 operator -(Vec3 vec)
        {
            return new Vec3(-vec.X, -vec.Y, -vec.Z);
        }
        public static Vec3 operator +(Vec3 lhs, Vec3 rhs)
        {
            return new Vec3(lhs.X+rhs.X, lhs.Y+rhs.Y, lhs.Z+rhs.Z);
        }
        public static Vec3 operator -(Vec3 lhs, Vec3 rhs)
        {
            return new Vec3(lhs.X - rhs.X, lhs.Y - rhs.Y, lhs.Z - rhs.Z);
        }
        public static float operator *(Vec3 lhs, Vec3 rhs)
        {
            return lhs.X * rhs.X + lhs.Y * rhs.Y + lhs.Z * rhs.Z;
        }
        public static Vec3 operator *(Vec3 vec, Matrix xform)
        {
            Matrix vmat = vec.Matrix();
            Matrix m3 = vmat * xform;
            return new Vec3(m3[0, 0], m3[0,1], m3[0, 2]);
        }
        public static Vec3 Cross(Vec3 a, Vec3 b)
        {
            return new Vec3(
                a.Y * b.Z - a.Z * b.Y,
                a.Z * b.X - a.X * b.Z,
                a.X * b.Y - a.Y * b.X
            );
        }
    }

    class Basis
    {
        public Vec3 X;  
        public Vec3 Y;
        public Vec3 Z;
        public Basis() { }
        public Basis( Vec3 Dir, Vec3 Up )
        {
            Z = Dir.Normalized();
            X = Vec3.Cross(Z, Up).Normalized();
            Y = Vec3.Cross(X,  Z).Normalized();
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
