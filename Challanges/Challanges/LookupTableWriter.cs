using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Challenges
{
    class LookupTableWriter
    {
        public static int CountOnes(uint x)
        {
            int result = 0;
            while (x != 0)
            {
                result++;
                x = x & (x - 1);
            }
            return result;
        }

        public static Dictionary<uint, int> GetHammingWeightLookup(uint n)
        {
            Dictionary<uint, int> lookup = new Dictionary<uint, int>();
            for(uint i = 0; i < n; i++)
            {
                lookup.Add(i, CountOnes(i));
            }
            return lookup;
        }

        public static string GetHammingWeightLookupCodeString  (uint to)
        {
            StringBuilder code = new StringBuilder("Dictionary<uint, int> lookup = new Dictionary<uint, int>()\n{");
            var lookup = GetHammingWeightLookup(to);
            for (uint i = 0; i < lookup.Count; i++)
            {
                
                code.Append($"\t[{i}] = {lookup[i]},\n");
            }
            string codeStr = code.ToString();
            codeStr = codeStr.TrimEnd('\n', ',');
            codeStr += "\n};";

            return codeStr;
        }
    }
}
