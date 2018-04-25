using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsharpTesting
{
    class Functional
    {
        // (state -> t -> state) -> state -> t list -> state
        public static TState ListFold<TState, T>(Func<TState, T, TState> folder, TState initState, List<T> list)
        {
            TState currentState = initState;
            foreach(T item in list)
            {
                currentState = folder(currentState, item);
            }
            return currentState;
        }

        public static T ListReduce<T>(Func<T, T, T> reducer, List<T> list)
        {
            T accumulator = default(T);
            foreach(T item in list)
            {
                accumulator = reducer(accumulator, item);
            }
            return accumulator;
        }
    }
}
