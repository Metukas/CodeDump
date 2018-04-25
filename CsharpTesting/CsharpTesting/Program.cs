using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsharpTesting
{
    class Program
    {
        static void Main(string[] args)
        {
            // 20 20 20 ....
            List<Func<int>> actions = new List<Func<int>>();
            for (int i = 0; i < 10; i++)
            {
                actions.Add(() => i * 2);
            }
            foreach (var action in actions)
            {
                Console.WriteLine(action());
            }

            // 0 2 4 6 .....
            Console.WriteLine();
            actions = new List<Func<int>>();
            for (int i = 0; i < 10; i++)
            {
                int copy = i;
                actions.Add(() => copy * 2);
            }
            foreach (var action in actions)
            {
                Console.WriteLine(action());
            }

            Console.WriteLine();
            actions = new List<Func<int>>();
            foreach (int i in Enumerable.Range(0, 10))
            {
                actions.Add(() => i * 2);
            }
            foreach (var action in actions)
            {
                Console.WriteLine(action());
            }

            // functional test
            // fold
            Func<int, int, int> add = (x, y) => x + y;
            List<int> nums = MakeIntList(0, 100);
            int sum = Functional.ListFold(add, 0, nums);
            Console.WriteLine($"sum: {sum}");
        }
        static List<int> MakeIntList(int from, int inclusiveTo)
        {
            List<int> list = new List<int>();
            for(int i = from; i <= inclusiveTo; i++)
            {
                list.Add(i);
            }
            return list;
        }
    }
}
