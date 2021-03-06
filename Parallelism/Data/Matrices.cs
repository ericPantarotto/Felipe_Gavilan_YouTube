using System.Security.Cryptography;


namespace Parallelism.Data
{
    public static class Matrices
    {

        //static RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
        static RandomNumberGenerator rng = RandomNumberGenerator.Create();
        static int matriceNumber  = 1;
        //static int matriceNumber = 1;
        [ThreadStatic]
        static Random? random;

        //HACK: making sure that number restart at 1
        public static void resetMatriceNumber()
        {
            matriceNumber = 1;
        }
        public static double[,] InitializeMatrixForAll(int rows, int columns)
        {
            //NOTE: For visualization purpose
            Thread.Sleep(2000);

            double[,] matrix = new double[rows, columns];

            //for (int i = 0; i < rows; i++)
            Parallel.For(0, rows, i =>
            {   
                //using var rng = RandomNumberGenerator.Create();
                var intBuffer = new byte[sizeof(int)];
                rng.GetBytes(intBuffer);
                random = new Random(BitConverter.ToInt32(intBuffer, 0));

                for (int j = 0; j < columns; j++)
                {
                    matrix[i, j] = random.Next(100);
                }
            });
            
            Console.WriteLine($"processed matrice {matriceNumber}");
            Interlocked.Increment(ref matriceNumber);

            return matrix;
        }
        public static double[,] InitializeMatrix(int rows, int columns)
        {
            double[,] matrix = new double[rows, columns];

            //for (int i = 0; i < rows; i++)
            Parallel.For(0, rows, i =>
            {
                // if (random == null)
                // {
                //    var buffer = new byte[4];
                //    rng.GetBytes(buffer);
                //    random = new Random(BitConverter.ToInt32(buffer, 0));
                // }
                
                //using var rng = RandomNumberGenerator.Create();
                var intBuffer = new byte[sizeof(int)];
                rng.GetBytes(intBuffer);
                random = new Random(BitConverter.ToInt32(intBuffer, 0));

                for (int j = 0; j < columns; j++)
                {
                    matrix[i, j] = random.Next(100);
                }
            });
            
            return matrix;
        }

        public static double[,] InitializeMatrixSaturated(int rows, int columns)
        {
            double[,] matrix = new double[rows, columns];

            Parallel.For(0, rows, i =>
            {
                Parallel.For(0, columns, j =>
                {
                    //if (random == null)
                    //{
                    //    var buffer = new byte[4];
                    //    rng.GetBytes(buffer);
                    //    random = new Random(BitConverter.ToInt32(buffer, 0));
                    //}
                    using var rng = RandomNumberGenerator.Create();
                    var intBuffer = new byte[sizeof(int)];
                    rng.GetBytes(intBuffer);
                    random = new Random(BitConverter.ToInt32(intBuffer, 0));

                    matrix[i, j] = random.Next(100);
                });
            });

            return matrix;
        }

        public static double[,] AddMatricesSequential(double[,] matA, double[,] matB)
        {
            int matARows = matA.GetLength(0);
            int matACols = matA.GetLength(1);
            int matBRows = matB.GetLength(0);
            int matBCols = matB.GetLength(1);

            if (matARows != matBRows || matACols != matBCols)
            {
                throw new ApplicationException("Matrices should have the same dimensions");
            }

            double[,] result = new double[matARows, matACols];

            for (int i = 0; i < matARows; i++)
            {
                for (int j = 0; j < matBCols; j++)
                {
                    result[i, j] = matA[i, j] + matB[i, j];
                }
            }

            return result;
        }

        public static void MultiplyMatricesSequential(double[,] matA, double[,] matB,
                                                double[,] result)
        {
            int matACols = matA.GetLength(1);
            int matBCols = matB.GetLength(1);
            int matARows = matA.GetLength(0);

            for (int i = 0; i < matARows; i++)
            {
                for (int j = 0; j < matBCols; j++)
                {
                    double temp = 0;
                    for (int k = 0; k < matACols; k++)
                    {
                        temp += matA[i, k] * matB[k, j];
                    }
                    result[i, j] += temp;
                }
                System.Console.WriteLine($"computed row {i} of {matARows}");
            }
        }

        public static void MultiplyMatricesParallel(double[,] matA, double[,] matB,
                                                double[,] result,
                                                CancellationToken token = default,
                                                int maxDegreeOfParalelism = -1)
        {
            int matACols = matA.GetLength(1);
            int matBCols = matB.GetLength(1);
            int matARows = matA.GetLength(0);

            //for (int i = 0; i < matARows; i++)

            Parallel.For(0, matARows,
                new ParallelOptions
                {
                    CancellationToken = token,
                    MaxDegreeOfParallelism = maxDegreeOfParalelism
                },
                i =>
                {
                    for (int j = 0; j < matBCols; j++)
                    {
                        double temp = 0;
                        for (int k = 0; k < matACols; k++)
                        {
                            temp += matA[i, k] * matB[k, j];
                        }
                        result[i, j] += temp;
                    }
                    System.Console.WriteLine($"computed row {i} of {matARows}");
                });
        }
    }
}