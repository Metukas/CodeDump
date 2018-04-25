using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Challenges
{
    // https://www.youtube.com/watch?v=_Lf1looyJMU
    //              
    //          ->  0|_0_0_0_0_
    // 0 1 1 1  ->  0| 0 1 1 1
    // 0 1 1 1  ->  0| 0 1 2 2
    // 1 1 1 1  ->  0| 1 1 2 3

    // 1 1 1    ->  1 1 1
    // 1 1 1    ->  1 2 2
    // 1 1 1    ->  1 2 3

    class MaximalSquareOptimized
    {
        public static double FindMaximalSquareArea(params string[] strArr)
        {
            // susidėdam į matricą
            int count = 0;
            int maxCount = strArr.Select(x => x).Max(i => i.Length);
            int[,] matrix = new int[strArr.Length, maxCount];
            foreach (string s in strArr)
            {
                var nums = s.Select(x => x).Select(x => int.Parse(x.ToString())).ToArray();
                for (int i = 0; i < nums.Length; i++)
                {
                    matrix[count, i] = nums[i];
                }
                count++;
            }

            // parodom matricą
            ShowMatrix(matrix);
            ////////////////////////////////////////////////////////////////
            ////////////////////////////////////////////////////////////////

            // pasidarom "koeficientų" matricą
            int[,] kMatrix = new int[strArr.Length, maxCount];

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    Console.Write(". ");
                    if (i == 0 || j == 0 || matrix[i,j] == 0)
                    {
                        kMatrix[i, j] = matrix[i, j];
                    }
                    else
                    {
                        var intermediateMin = Math.Min(kMatrix[i, j - 1], kMatrix[i - 1, j - 1]);
                        var minimum = Math.Min(kMatrix[(i - 1), j], intermediateMin);
                        kMatrix[i, j] = minimum + matrix[i, j];
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine();
            ShowMatrix(kMatrix);

            return Math.Pow((Enumerable.Cast<int>(kMatrix).Max()), 2);
        }

        public static void ShowMatrix(int[,] matrix)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    Console.Write($"{matrix[i, j]} ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}
