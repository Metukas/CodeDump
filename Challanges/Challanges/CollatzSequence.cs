using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Challenges
{
    class CollatzSequence
    {
        public static long CalculateCollatzSequence(long n)
        {
            long count = 0;
            while(n != 1)
            {
                if ((n & 1) == 0)
                    n = n / 2;
                else
                    n = 3 * n + 1;

                count++;
            }
            return count;
        }

        public static long GetNumberWithLargestSequence(int from, int to)
        {
            return Enumerable.Range(from, to).AsParallel().Select(x => CollatzSequence.CalculateCollatzSequence(x)).Max();
            //long biggest = 0;
            //for(int i = from; i < to; i++)
            //{
            //    long temp = CalculateCollatzSequence(i);
            //    if (temp > biggest)
            //        biggest = temp;
            //}
            //return biggest;
        }

        public static bool IsPowerOf2(int num)
        {
            return (num != 0) && ((num & (num - 1)) == 0);
        }

        public static int Double(int num)
        {
            return num - ~num - 1;
        }

        public static int NextPowerOf2(int num)
        {
            num--;
            num |= num >> 1;
            num |= num >> 2;
            num |= num >> 4;
            num |= num >> 8;
            num |= num >> 16;
            num++;

            return num;
        }

        public static int PreviousPowerOf2(int num)
        {
            num = num | (num >> 1);
            num = num | (num >> 2);
            num = num | (num >> 4);
            num = num | (num >> 8);
            num = num | (num >> 16);
            return num - (num >> 1);
        }
    }
}
