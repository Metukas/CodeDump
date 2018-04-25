using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Challenges
{
    // Have the function MaximalSquare(strArr) take the strArr parameter being passed 
    //which will be a 2D matrix of 0 and 1's, and determine the area of the largest square submatrix
    //that contains all 1's. A square submatrix is one of equal width and height,
    //and your program should return the area of the largest submatrix that contains only 1's. 
    //For example: if strArr is ["10100", "10111", "11111", "10010"] then this looks like the following matrix: 
    // 1  0  1  0  0
    // 1  0  1 '1 '1
    // 1  1  1 '1 '1
    // 1  0  0  1  0 
    // 
    // For the input above, you can see the ' marked 1's create the largest square submatrix of size 2x2,
    //so your program should return the area which is 4. You can assume the input will not be empty. 

    // 1 2 3
    // 1 2 3
    // 1 2 3

    class MaximalSquare
    {
        public static (int area, int[] indexes) FindMaximalSquare(params string[] strArr)
        {
            // susidėdam į matricą
            int count = 0;
            int maxCount = strArr.Select(x => x).Max(i=>i.Length);
            int[,] matrix = new int[strArr.Length, maxCount];
            foreach(string s in strArr)
            {
                var nums = s.Select(x => x).Select(x => int.Parse(x.ToString())).ToArray();
                for (int i = 0; i < nums.Length; i++)
                {
                    matrix[count, i] = nums[i];
                }
                count++;
            }

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    Console.Write($"{matrix[i,j]} ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
            //////////////////////////////////////////////////////////////

            // skenuojam
            // prepare to be loop nesting fucked!!
            Dictionary<(int, int), int> lastFoundAll1SubMatrix = new Dictionary<(int, int), int>();
            Dictionary<(int, int), int> positionsToCheck = new Dictionary<(int, int), int>(5);
            int maxDimension = Math.Min(matrix.GetLength(0), matrix.GetLength(1));
            for (int m = 2; m <= maxDimension; m++) // Šitas loopas loopina tarp visų įmanomų kvadratinių dimensijų
            {                                       // (pradėdant (aišku) nuo 2)
                for (int i = 0; i < matrix.GetLength(0); i++)
                {
                    if (i + m > matrix.GetLength(0)) // jeigu tep, tada neįmanoma sudaryt kvadrato
                        break;
                    for (int j = 0; j < matrix.GetLength(1); j++)
                    {
                        if (j + m > matrix.GetLength(1)) // jeigu tep, tada neįmanoma sudaryt kvadrato
                            break;

                        // loopai, kurie ima šalia esančius skaičius
                        // /////////////////////////////////////////
                        for (int k = 0; k < m; k++)
                        {
                            for (int l = 0; l < m; l++)
                            {
                                if (i + k < matrix.GetLength(0) && j + l < matrix.GetLength(1))
                                {
                                    positionsToCheck.Add((i + k, j + l), matrix[i + k, j + l]);
                                    Console.Write($"{matrix[i + k, j + l]} ");
                                }
                                else
                                {
                                    Console.WriteLine("broke");
                                    //break;
                                    goto there;
                                }
                            }
                            Console.WriteLine();
                        }
                        ////////////////////////////////////////////
                        there:

                        if (!positionsToCheck.ContainsValue(0))
                        {
                            lastFoundAll1SubMatrix = new Dictionary<(int, int), int>(positionsToCheck);
                        }

                        positionsToCheck.Clear();
                        Console.WriteLine();
                    }
                }
            }

            if(!lastFoundAll1SubMatrix.Any() && (from int i in matrix select i).Contains(1))
            {
                Console.WriteLine("Largest area: 1");
            }
            Console.WriteLine("_________________________________");
            Console.WriteLine("---------------------------------");
            foreach(var n in lastFoundAll1SubMatrix)
            {
                //Console.WriteLine(n);
            }
            Console.WriteLine($"Largest area: {lastFoundAll1SubMatrix.Count}");

            return (0, null);
        }
    }
}
