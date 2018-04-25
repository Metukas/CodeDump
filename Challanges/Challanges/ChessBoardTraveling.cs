using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Challenges
{
    // Have the function ChessboardTraveling(str) read str which will be a string consisting of the location
    // of a space on a standard 8x8 chess board with no pieces on the board along with another space on the chess board.
    // The structure of str will be the following: "(x y)(a b)" where (x y) represents the position
    // you are currently on with x and y ranging from 1 to 8 and (a b) represents some other space on the chess board
    // with a and b also ranging from 1 to 8 where a > x and b > y. 
    // Your program should determine how many ways there are of traveling from (x y)
    // on the board to (a b) moving only up and to the right. For example:
    // if str is (1 1)(2 2) then your program should output 2 because there are only two possible ways to travel
    // from space (1 1) on a chessboard to space (2 2) while making only moves up and to the right. 
    public class ChessBoardTraveling
    {
        /// <summary>
        /// Apskaičiuoja, kiek šachmatų lentoj yra įmanomų ėjimų iš (x y) langelio į (a b), judant tik į viršų ir į dešinę
        /// </summary>
        /// <param name="str">"(x y)(a b)" formato stringas</param>
        /// <returns></returns>
        public static int ChessboardTraveling(string str)
        {
            // parse string ir tada:

            Point startPosition = new Point(0, 0);
            Point destination = new Point(7, 7);
            var chessBoard = new ChessBoard(startPosition, destination);
            return chessBoard.CalculateRightUpWaysCount();
        }
    }

    class ChessBoard
    {
        Point StartPosition;
        Point Destination;

        public ChessBoard(Point startPosition, Point destinationPostion)
        {
            this.StartPosition = startPosition;
            this.Destination = destinationPostion;
        }

        // c = stepCount! / (x! * y!)
        public int CalculateRightUpWaysCount()
        {
            Point direction = Destination - StartPosition;
            int stepToDestinationCount = direction.X + direction.Y;
            return (int) (Factorial(stepToDestinationCount) / (Factorial(direction.X) * Factorial(direction.Y)));
        }

        public ulong Factorial(int x)
        {
            if (x == 0)
                return 1;
            ulong result = 1;
            for(ulong i = 2; i <= (ulong)x; i++)
            {
                result *= i;
            }
            return result;
        }

        // Lol bulshit :)
        int CalculateWaysCountFromDimension(int dimension)
        {
            if (dimension <= 1)
                return 0;
            if (dimension == 2)
                return 2;

            //skaičiuoja teisingai iki 4x4 dydžio lentos :D
            int insideDiagonalCount = dimension - 2;
            var result = CalculateWaysCountFromDimension(dimension - 1) * insideDiagonalCount;
            return result + (int)Math.Pow(2, dimension - 1);
        }

        public static PointBinaryTree MakePathTree()
        {
            PointBinaryTree tree = new PointBinaryTree();
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    tree.Add(new Point(j, i));
                }
            }

            return tree;
        }

    }

    struct Point
    {
        public int X { get; }
        public int Y { get; }

        public static Point Zero { get => new Point(0, 0); }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static Point operator -(Point right, Point left)
        {
            return new Point(right.X - left.X, right.Y - left.Y);
        }

        public static Point operator +(Point right, Point left)
        {
            return new Point(right.X + left.X, right.Y + left.Y);
        }

        public override string ToString()
        {
            return $"({X}, {Y})";
        }
    }
}
