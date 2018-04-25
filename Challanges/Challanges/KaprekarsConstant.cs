using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Challenges
{
    // Using the C# language, have the function KaprekarsConstant(num) take the num
    // parameter being passed which will be a 4-digit number with at least two distinct digits.
    // Your program should perform the following routine on the number: 
    // Arrange the digits in descending order and in ascending order (adding zeroes to fit it to a 4-digit number),
    // and subtract the smaller number from the bigger number. Then repeat the previous step. 
    // Performing this routine will always cause you to reach a fixed number: 6174. 
    // Then performing the routine on 6174 will always give you 6174 (7641 - 1467 = 6174). 
    // Your program should return the number of times this routine must be performed until 6174 is reached. 
    // For example: if num is 3524 your program should return 3 because of the following steps: 
    // (1) 5432 - 2345 = 3087, (2) 8730 - 0378 = 8352, (3) 8532 - 2358 = 6174. 

    class KaprekarsConstant
    {
        public static int CountToKaprekarsConstant(int num)
        {
            // 03E8 - 270F
            if (num < 1000 || num > 9999)
                return -1;

            int numToTest = num;
            int[] digits = ToDigitArray(numToTest);

            // jeigu 0 == 1 IR 2 == 3 IR 1 == 2, tada visi skaitmenys vienodi.
            if ((digits[0] == digits[1] && digits[2] == digits[3]) && (digits[1] == digits[2]))
                return -1111;

            int bigger;
            int smaller;
            int count = 0;

            do
            {
                digits = ToDigitArray(numToTest);

                var ascending = digits.OrderBy(x => x).ToArray();
                var descending = digits.OrderByDescending(x => x).ToArray();

                var ascendingInt = FromDigitArray(ascending);
                var descendingInt = FromDigitArray(descending);

                bigger = Math.Max(ascendingInt, descendingInt);
                smaller = Math.Min(ascendingInt, descendingInt);

                numToTest = bigger - smaller;
                count++;
            }
            while (numToTest != 6174);

            return count;
        }

        private static int[] ToDigitArray(int num)
        {
            // indexais: 0 - vienetai .. 3 - tūkstančiai
            int[] digits = new int[4];
            for (int i = 0; i < digits.Length; i++)
            {
                digits[i] = num % 10;
                num /= 10;
            }

            return digits;
        }

        private static int FromDigitArray(int[] digitArray)
        {
            // indexais: 0 - vienetai .. 3 - tūkstančiai
            int intFromDigitArray = 0;
            for (int i = 0; i < 4; i++)
            {
                intFromDigitArray += digitArray[i] * (int)(Math.Pow(10.0, (int)i));
            }
            return intFromDigitArray;
        }
    }
}
