using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Challenges
{
    public class CollatzSequenceEnumerator
    {
        long current;

        public CollatzSequenceEnumerator(long current)
        {
            this.current = current;
        }

        bool MoveNext()
        {
            if (current == 1)
                return false;
            if ((current & 1) == 0)
            {
                current /= 2;
                return true;
            }
            else
            {
                current = current * 3 + 1;
                return true;
            }
        }

        public int Count()
        {
            int counter = 1;
            while(MoveNext())
            {
                counter++;
            }
            return counter;
        }
}

public static partial class __Extensions__
    {
        public static CollatzSequenceEnumerator GetCollatzEnumerator(this long i)
        {
            return new CollatzSequenceEnumerator(i);
        }
    }
}


