
using System.Numerics;

namespace _3dEngine
{
    internal class Matrix
    {
        public int row { get; }
        public int col { get; }
        public float[,] mat { get; private set; }

        public Matrix(int _row, int _col)
        {
            row = _row;
            col = _col;
            mat = new float[_row, _col];
        }
        public Matrix(float[,] mat)
        {
            this.mat = mat;
            row = mat.GetLength(0);
            col = mat.GetLength(1);
        }

        public void print()
        {
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    Console.Write(mat[i, j]);
                    Console.Write('\t');
                }
                Console.Write('\n');
            }
        }
        public Matrix Transposed()
        {
            Matrix transposed = new Matrix( col, row );
            for (int i = 0; i < col; i++) {
                for (int j = 0; j < row; j++) {
                    transposed[i, j] = this[j, i];
                }
            }
            return transposed;
        }
        public float this[int i, int j] {
            get
            {
                if (i < 0 || i >= row || col < 0 || j >= col)
                {
                    throw new IndexOutOfRangeException("Index out of bounds.");
                }

                return mat[i, j];
            }
            set
            {
                if (i < 0 || i >= row || j < 0 || j >= col)
                {
                    throw new IndexOutOfRangeException("Index out of bounds.");
                }

                mat[i, j] = value;
            }
        }

        public static Matrix ScalarMultiply( Matrix m, float n )
        {
            Matrix result = new Matrix(m.row, m.col);
            for (int i = 0; i < m.row; i++)
            {
                for (int j = 0; j < m.col; j++)
                {
                    result[i, j] = m[i, j] * n;
                }
            }
            return result;
        }
        public static Matrix Add(Matrix m1, Matrix m2)
        {
            if (m1.row != m2.row )
            {
                throw new ArgumentException("Matrix must have the same dimensions for addition.");
            }

            Matrix result = new Matrix(m1.row,m1.col);
            for (int i = 0; i < m1.row; i++)
            {
                for (int j = 0; j < m1.col; j++)
                {
                    result[i,j] = m1[i,j] + m2[i,j];
                }
            }
            return result;
        }
        public static Matrix Subtract(Matrix m1, Matrix m2)
        {
            return Add(m1, ScalarMultiply( m2, -1));
        }

        public static Matrix operator +(Matrix m1, Matrix m2)
        {
            return Add(m1, m2);
        }
        public static Matrix operator -(Matrix m1, Matrix m2) {
            return Subtract(m1, m2);
        }
        public static Matrix operator *(Matrix m, float n) { 
            return ScalarMultiply( m, n );
        }
        public static Matrix operator /(Matrix m1, float n)
        {
            return m1 * 1 / n;
        }
        public static Matrix operator *(Matrix m1, Matrix m2)
        {
            if (m1.col != m2.row) {
                throw new ArgumentException("Invalid matrix shape: matrix1 col should match matrix2 row");
            }
            Matrix result = new Matrix(m1.row, m2.col);
            for (int i = 0; i < m1.row; i++) {
                for (int j = 0; j < m2.col; j++) {
                    result[i, j] = 0;
                    for (int k = 0; k < m1.col; k++) {
                        result[i,j] += m1[i,k] * m2[k,j];
                    }
                }
            }
            return result;
        }
        public static Matrix operator *(Matrix m1, Vec3 vec)
        {
            Matrix m2 = new Matrix(new float[,] { { vec.X, vec.Y, vec.Z, 1 } });
            return m1 * m2;
        }

        public static Matrix Identity(int dim)
        {
            Matrix mat = new Matrix(dim, dim);
            for (int i = 0; i <dim; i++)
            {
                mat[i, i] = 1;
            }
            return mat;
        }
        public static Matrix MatrixScale(float X, float Y, float Z)
        {
            return Identity(4);
        }
        public static Matrix MatrixTranslation(float tx, float ty, float tz)
        {
            return new Matrix(new float[,] { { 1, 0, 0, 0 },
                                             { 0, 1, 0, 0 },
                                             { 0, 0, 1, 0 },
                                             {tx,ty,tz, 1  } }) ;
        }
        public static Matrix MatrixRotationX(float angleRad)
        {
            float cos = (float)Math.Cos(angleRad);
            float sin = (float)Math.Sin(angleRad);
            return new Matrix(new float[,] { { 1, 0,    0,   0 },
                                             { 0,  cos, sin, 0 },
                                             { 0, -sin, cos, 0 },
                                             { 0, 0,    0,   1 } });
        }
        public static Matrix MatrixRotationY(float angleRad)
        {
            float cos = (float)Math.Cos(angleRad);
            float sin = (float)Math.Sin(angleRad);
            return new Matrix(new float[,] { { cos, 0, sin, 0 },
                                             { 0,   1, 0,   0 },
                                             {-sin, 0, cos, 0 },
                                             { 0,   0, 0,   1 }});
        }
        public static Matrix MatrixRotationZ(float angleRad)
        {
            float cos = (float)Math.Cos(angleRad);
            float sin = (float)Math.Sin(angleRad);
            return new Matrix(new float[,] { { cos, sin, 0, 0 },
                                             {-sin, cos, 0, 0 },
                                             { 0,   0,   1, 0 },
                                             { 0,   0,   0, 1 } });
        }
        public static Matrix MatrixRotationAxis(Vec3 axis, float angleRad)
        {
            axis = axis.Normalized();
            float cos = (float)Math.Cos(angleRad);
            float sin = (float)Math.Sin(angleRad);
            float icos = 1.0f - cos;

            float xx = axis.X * axis.X;
            float yy = axis.Y * axis.Y;
            float zz = axis.Z * axis.Z;
            float xy = axis.X * axis.Y;
            float xz = axis.X * axis.Z;
            float yz = axis.Y * axis.Z;
            float xs = axis.X * sin;
            float ys = axis.Y * sin;
            float zs = axis.Z * sin;

            Matrix rotationMatrix = new Matrix( new float[,]{
                { cos  + icos * xx,  icos * xy   - zs,  icos * xz   + ys, 0.0f },
                { icos * xy   + zs,  cos  + icos * yy,  icos * yz   - xs, 0.0f },
                { icos * xz   - ys,  icos * yz   + xs,  cos  + icos * zz, 0.0f },
                { 0.0f,              0.0f,              0.0f,             1.0f }
            });

            return rotationMatrix;
        }
        public static Matrix MatrixPerspectiveProjection(float fovDegress, float aspectRatio, float near, float far)
        {
            float fovRad = 1.0f / MathF.Tan(fovDegress * 0.5f / 180.0f * MathF.PI);
            Matrix mat = new Matrix(4,4);
            mat[0, 0] = fovRad /aspectRatio;
            mat[1, 1] = fovRad;
            mat[2, 2] = far / (far - near);
            mat[2, 3] = 1.0f;
            mat[3, 2] = (-far * near) / (far - near);
            return mat;
        }
        public static Matrix LookAt( Vec3 pos, Vec3 target, Vec3 up)
        {
            Basis basis = new Basis( target -pos, up);

            return new Matrix( new float[,]{
                { basis.X.X, basis.Y.X, -basis.Z.X, 0 },
                { basis.X.Y, basis.Y.Y, -basis.Z.Y, 0},
                { basis.X.Z, basis.Y.Z, -basis.Z.Z, 0},
                { -basis.X * pos, -basis.Y * pos, basis.Z* pos, 1}});
        }
    }
}
