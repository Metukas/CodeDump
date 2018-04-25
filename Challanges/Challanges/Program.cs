using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Challenges
{
    class Program
    {
        [DllImport("kernel32.dll")]
        static extern void OutputDebugString(string lpOutputString);
        static async Task Main(string[] args)
        {
            //Console.WriteLine(KaprekarsConstant.CountToKaprekarsConstant(3458));
            //Console.WriteLine(CollatzSequence.CalculateCollatzSequence(3));
            //Console.WriteLine(CollatzSequence.GetNumberWithLargestSequence(2, 1000000));

            //Console.WriteLine(LookupTableWriter.GetHammingWeightLookupCodeString(1000));
            //Enumerable.Range(0, 100).ForEach(x => Console.WriteLine(x));

            //var area = MaximalSquareOptimized.FindMaximalSquareArea(
            //    "11111111111111011111111111111111", "11111111111110111111111111111111",
            //    "11111111111101111111111111111111", "11111111111111110111111111111111",
            //    "11111111111111111111111111111111", "11111111111111111111111111111111",
            //    "11111111111111111111111111111111", "11111111111111111110111111111111",
            //    "11111111111111111111011111111111", "11111111111111111111111111111111",
            //    "11111111111111111111111111111111", "11111111111111111101111111111111",
            //    "11111111111111111111111111111111", "11111111111110111111111111111111",
            //    "11111111111111111111111110111111", "11111111111111111111111111111111",
            //    "11111111111011111111111110111111", "11111111111111111111111111111111",
            //    "11111111111111111111111111111111", "11111111111111110111111111111111",
            //    "11111111111111111111111111111111", "11111111111111111111111111111111",
            //    "11111111111110111111111111111111", "11111111111111111111111111111111",
            //    "11111111111111111111111111111111", "11111111111111111111111111111111",
            //    "11111111111111101111111111111111", "11111111111111111111111111111111",
            //    "11111111111111111111111111111111", "11111111111111111111111111111111",
            //    "11111111111111111111111111111111", "11111111111011111111111111111111");

            Console.WriteLine(MaximalSquareOptimized.FindMaximalSquareArea("11111", "1011", "1011", "1101", "1111", "1101"));

            Dictionary<long, int> numToCollatzSeqMap = new Dictionary<long, int>();
            for(long i = 2; i< 1000; i++)
            {
                numToCollatzSeqMap.Add(i, i.GetCollatzEnumerator().Count());
            }
            var max = numToCollatzSeqMap.Aggregate((currentMax, next) => currentMax.Value > next.Value ? currentMax : next);
            Console.WriteLine($"({max.Key}, {max.Value})");

            Console.WriteLine(ChessBoardTraveling.ChessboardTraveling(""));
            BinaryTree<int> binTree = new BinaryTree<int>(1);
            PointBinaryTree pathTree = new PointBinaryTree();
            pathTree.GetPathTree(3, 3);
            pathTree.PrintTree();
            binTree.Root.Right = BinaryTreeNode<int>.AddNewNode(binTree.Root, 2);
            binTree.Root.Left = BinaryTreeNode<int>.AddNewNode(binTree.Root, 3);

            int testCount = 100_000_000;           
            
            //Console.WriteLine("Shitty:");
            //Console.WriteLine(Time(() => HardestProblemOnHardestTest.CalculateProbability(testCount)));
            //Console.WriteLine("Kompleksinis:");
            //Console.WriteLine(Time(() => HardestProblemOnHardestTest.CalculateProbabilityComplexWay(testCount)));
            //Console.WriteLine("Tik su kampais:");
            //Console.WriteLine(Time(() => CircleCenterInTriangleProblem.CalculateProbability(testCount)));
            Console.WriteLine("No Bullshit:");
            Console.WriteLine(Time(() => CircleCenterInTriangleProblem.CalculateProbabilityNoBullshit(testCount)));
            Console.WriteLine("(No Bullshit) Parallel For :");
            Console.WriteLine(Time(() => CircleCenterInTriangleProblem.CalculateProbabilityNoBullshitParallelFor(testCount)));
            Console.WriteLine("(No Bullshit) Parallel Task:");
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            var result = await CircleCenterInTriangleProblem.CalculateProbabilityNoBullshitParallelTask(testCount);
            sw.Stop();
            Console.WriteLine($"Time Taken: {sw.Elapsed} Result = {result}");
        }    
        
        static string Time(Func<dynamic> action)
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            dynamic result = action();
            sw.Stop();

            bool resultCorrect = 0.25 >= (result - 0.01) && 0.25 <= (result + 0.01);
            return $"Time Taken: {sw.Elapsed}. Result = {result}. {(resultCorrect ? "Tep" : "Nope")}";
        }
    }

    static partial class __Extensions__
    {
        public static byte[] ToByteArray(this int num)
        {
            byte[] b = new byte[sizeof(int)];
            for(int i = 0; i < b.Length; i++)
            {
                b[i] = (byte)num;
                num >>= 8;
            }
            return b;
        }

        public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
        {
            foreach (T item in enumeration)
            {
                action(item);
            }
        }
    }
}