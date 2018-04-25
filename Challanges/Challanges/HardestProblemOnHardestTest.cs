using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Challenges
{
    // https://www.youtube.com/watch?v=OkmNXy7er84
    // Bet tik 2D :). 3D too hard
    class HardestProblemOnHardestTest
    {
        public static double CalculateProbability(int testCount)
        {
            //Vector2 p1 = new Vector2(1, 0); //Vector2.GetRandomNormalizedStrechingRand();//DoublePoint.GetRandomNormalized();
            //Vector2 p2 = new Vector2(-1, 1);//Vector2.GetRandomNormalizedStrechingRand();//DoublePoint.GetRandomNormalized();
            //Vector2 p3 = new Vector2(0, 1); //Vector2.GetRandomNormalizedStrechingRand();//DoublePoint.GetRandomNormalized();

            long hitCount = 0;
            for (int i = 0; i < testCount; i++)
            {
                Vector2 p1 = Vector2.GetRandomNormalizedStrechingRand(); //DoublePoint.GetRandomNormalized();
                Vector2 p2 = Vector2.GetRandomNormalizedStrechingRand(); //DoublePoint.GetRandomNormalized();
                Vector2 p3 = Vector2.GetRandomNormalizedStrechingRand(); //DoublePoint.GetRandomNormalized();

                p1.Normalize();
                p2.Normalize();
                p3.Normalize();

                AngleBetweenPointsOnCircleArc p1_p2 = new AngleBetweenPointsOnCircleArc(p1, p2);
                AngleBetweenPointsOnCircleArc p2_p3 = new AngleBetweenPointsOnCircleArc(p2, p3);
                AngleBetweenPointsOnCircleArc p1_p3 = new AngleBetweenPointsOnCircleArc(p1, p3);
                AngleBetweenPointsOnCircleArc min1 = p1_p2 < p2_p3 ? p1_p2 : p1_p3;
                AngleBetweenPointsOnCircleArc min = min1 < p1_p3 ? min1 : p1_p3;
                Vector2 unitX = new Vector2(1, 0);
                Vector3 cross = Vector3.CrossProduct(min.P1, min.P2);

                Vector2 p1_;
                Vector2 p2_;
                Vector2 p3_ = p3;
                if (cross.Z > 0)
                {
                    p1_ = min.P1;
                    p2_ = min.P2;
                }
                else
                {
                    p1_ = min.P2;
                    p2_ = min.P1;
                }

                // nei vienu atveju neteisingai, todėl klaida kažkur kitur
                //if (IsP3BetweenP1AndP2(Math.Acos(p1_.GetMirrored() * unitX), Math.Acos(p2_.GetMirrored() * unitX), Math.Acos(p3_ * unitX)))
                if (IsP3BetweenP1AndP2(Math.Acos(p1_ * unitX), Math.Acos(p2_ * unitX), Math.Acos(p3_.GetMirrored() * unitX)))
                {
                    hitCount++;
                }

            }

            return (double)hitCount / (double)testCount;

            //Vector2 p1Miror = p1.GetMirrored();
            //Vector2 p2Miror = p2.GetMirrored();

            //var tep = IsP3BetweenP1AndP2(test1, test2, test3);

            
        }

        public static double CalculateProbabilityComplexWay(int testCount)
        {
            Random rand = new Random();
            double[] randomAngles = new double[3];
            List<Complex> positivePhase = new List<Complex>(3);
            List<Complex> negativePhase = new List<Complex>(3);
            

            int hitCount = 0;
            for (int i = 0; i < testCount; i++)
            {
                positivePhase.Clear();
                negativePhase.Clear();

                // Generuojam tris random taškus ant apskritimo:
                for (int j = 0; j < randomAngles.Length; j++)
                {
                    randomAngles[j] = rand.NextDouble().ChangeRange(0, 1, 0, 360);
                    Complex point = PointOnCircle(1, randomAngles[j], Complex.Zero);
                    if (point.Phase >= 0)
                    {
                        positivePhase.Add(point);
                    }
                    else
                    {
                        negativePhase.Add(point);
                    }
                }

                // jeigu visi taškai vienoj apskritimo pusėj, tada neįmanoma, kad centras būtų trikampio viduje
                if (!positivePhase.Any() || !negativePhase.Any())
                {
                    continue;
                }

                double p1 = 0;
                double p2 = 0;
                double p3 = 0;

                // jeigu du taškai yra "teigiamoj" apskritimo pusėj (viršuj)
                if(positivePhase.Count == 2)
                {
                                                                //(1)
                    int smallerAngleIndex = positivePhase[0].Phase < positivePhase[1].Phase ? 0 : 1;
                    int otherIndex = 1 - smallerAngleIndex;

                    p1 = positivePhase[smallerAngleIndex].Phase;
                    p2 = positivePhase[otherIndex].Phase;
                    p3 = negativePhase[0].Phase;
                }
                // jeigu du taškai yra "neigiamoj" apskritimo pusėj (apačioj)
                else if(negativePhase.Count == 2)
                {
                                                                //(2)
                    int smallerAngleIndex = negativePhase[0].Phase < negativePhase[1].Phase ? 0 : 1;
                    int otherIndex = 1 - smallerAngleIndex;

                    p1 = negativePhase[smallerAngleIndex].Phase;
                    p2 = negativePhase[otherIndex].Phase;
                    p3 = positivePhase[0].Phase;
                }
                //if((IsP3BetweenP1AndP2(p1, p2, -p3))) // jeigu (1) ir (2) >
                if (IsP3BetweenP1AndP2(-p1, -p2, p3))   // jeigu (1) ir (2) <    ...wut?
                {
                    hitCount++;
                }
            }
            return (double)hitCount / (double)testCount;
        }

        public static Complex PointOnCircle(double radius, double angleInDegrees, Complex centre)
        {
            return centre + radius * Complex.Exp(Math.PI * Complex.ImaginaryOne * (angleInDegrees / 180.0));
        }

        public static Complex PointOnCircle0To1(double radius, double angleFrom0To1, Complex centre)
        {
            return centre + radius * Complex.Exp(Math.PI * Complex.ImaginaryOne * (angleFrom0To1 / 0.5));
        }

        // radianais 
        // p1 < p2
        static bool IsP3BetweenP1AndP2(double p1, double p2, double p3)
        {
            double p1_p2, p1_p3;
            p1_p2 = p2 - p1 + 2 * Math.PI % 2 * Math.PI;
            p1_p3 = p3 - p1 + 2 * Math.PI % 2 * Math.PI;

            return (p1_p2 <= Math.PI) != (p1_p3 > p1_p2);
        }
    }

    struct AngleBetweenPointsOnCircleArc
    {
        public Vector2 P1 { get; }
        public Vector2 P2 { get; }

        double CosAngle { get => Vector2.DotProduct(P1, P2); }
            
        public AngleBetweenPointsOnCircleArc(Vector2 p1, Vector2 p2)
        {
            P1 = p1;
            P2 = p2;
            P1.Normalize();
            P2.Normalize();
        }

        // kuo didesnis kosinusas, tuo mažesnis kampas tarp taškų
        public static bool operator >(AngleBetweenPointsOnCircleArc l, AngleBetweenPointsOnCircleArc r)
        {
            return l.CosAngle < r.CosAngle;
        }

        public static bool operator <(AngleBetweenPointsOnCircleArc l, AngleBetweenPointsOnCircleArc r)
        {
            return l.CosAngle > r.CosAngle;
        }
    }

    struct Vector2
    {
        static Random rand = new Random();
        public double X { get; private set; }
        public double Y { get; private set; }

        public double Length { get => Math.Sqrt((X * X) + (Y * Y)); }

        public Vector2(double x, double y)
        {
            X = x;
            Y = y;
        }

        public void Normalize()
        {
            double oldLength = this.Length;
            X = X / oldLength;
            Y = Y / oldLength;
        }

        public Vector2 GetNormalized()
        {
            Vector2 doublePoint = new Vector2(X, Y);
            doublePoint.Normalize();
            return doublePoint;
        }

        public void Mirror()
        {
            X = -X;
            Y = -Y;
        }

        public Vector2 GetMirrored()
        {
            Vector2 doublePoint = new Vector2(X, Y);
            doublePoint.Mirror();
            return doublePoint;
        }

        public static Vector2 GetRandomNormalized()
        {
            Vector2 point = new Vector2(RandomSign() * rand.NextDouble(), RandomSign() * rand.NextDouble());
            point.Normalize();
            return point;
        }

        public static double DotProduct(Vector2 left, Vector2 right)
        {
            return right.X * left.X + right.Y * left.Y;
        }

        Vector2 MultiplyWithScalar(double a)
        {
            return new Vector2(a * X, a * Y);
        }

        // DotProduct
        public static double operator *(Vector2 left, Vector2 right)
        {
            return DotProduct(right, left);
        }

        public static Vector2 operator *(double a, Vector2 vec)
        {
            return vec.MultiplyWithScalar(a);
        }

        public static Vector2 operator +(Vector2 a, Vector2 b)
        {
            return new Vector2(a.X + b.X, a.Y + b.Y);
        }

        public static Vector2 GetRandomNormalizedStrechingRand()
        {
            double pointX = ChangeValueToRange(0, 1, -1, 1, rand.NextDouble());
            double pointY = ChangeValueToRange(0, 1, -1, 1, rand.NextDouble());
            Vector2 point = new Vector2(pointX, pointY);
            point.Normalize();
            return point;
        }

        static double ChangeValueToRange(double oldMin, double oldMax, double newMin, double newMax, double value)
        {
            return (((value - oldMin) * (newMax - newMin)) / (oldMax - oldMin)) + newMin;
        }

        static double RandomSign()
        {
            return rand.Next(1, 100001) % 2 == 0 ? 1 : -1;
        }
    }

    struct Vector3
    {
        public double X, Y, Z;

        public static Vector3 UnitX { get => new Vector3(1, 0, 0); }
        public static Vector3 UnitY { get => new Vector3(0, 1, 0); }
        public static Vector3 UnitZ { get => new Vector3(0, 0, 1); }

        public Vector3(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }
        
        //u x v = |u2 u3|i - |u1 u3|j + |u1 u2|k  u == left
        //        |v2 v3|    |v1 v3|    |v1 v2|   v == right
        public static Vector3 CrossProduct(Vector2 left, Vector2 right)
        {
            //double dot = Vector2.DotProduct(left, right) / (left.Length * right.Length);
            //double sinTheta = Math.Sqrt(1 - (dot * dot));
            //double iDet = new Matrix2x2(left.Y, 0, right.Y, 0).Determinant();
            //double jDet = new Matrix2x2(left.X, 0, right.X, 0).Determinant();
            //double kDet = new Matrix2x2(left.X, left.Y, right.X, right.Y).Determinant();
            //
            //return new Vector3(iDet, jDet, kDet);

            //cx = ay bz - az by == 0 2d plokštumoj
            //cy = az bx - ax bz --,,--
            //cz = ax by - ay bx
            double cx = 0;
            double cy = 0;
            double cz = left.X * right.Y - left.Y * right.X;
            return new Vector3(cx, cy, cz);

        }
    }

    static partial class __Extensions__
    {
        public static double ChangeRange(this double value, double oldMin, double oldMax, double newMin, double newMax)
        {
            return (((value - oldMin) * (newMax - newMin)) / (oldMax - oldMin)) + newMin;
        }

        public static double InvertedAngle(this double value)
        {
            value += 0.5;
            return value % 1;
        }
    }
    struct Matrix2x2
    {
        // |a b|
        // |c d|
        double a, b, c, d;

        public static Matrix2x2 Identity { get => new Matrix2x2(1, 0, 0, 1); }

        public Matrix2x2(double a, double b, double c, double d)
        {
            this.a = a;
            this.b = b;
            this.c = c;
            this.d = d;
        }

        // |Aa Ab| |Ba Bb|
        // |Ac Ad| |Bc Bd|
        public static Matrix2x2 MultiplyMatrices(Matrix2x2 A, Matrix2x2 B)
        {
            double newA = B.a * A.a + B.c * A.b;
            double newB = B.b * A.a + B.d * A.b;
            double newC = B.a * A.c + B.c * A.d;
            double newD = B.b * A.c + B.d * A.d;

            return new Matrix2x2(newA, newB, newC, newD);
        }

        // |Aa Ab| |Ba Bb|
        // |Ac Ad| |Bc Bd|
        public static Matrix2x2 MultiplyMatricesV(Matrix2x2 A, Matrix2x2 B)
        {
            Vector2 firstCollumnOfAMatrix = new Vector2(A.a, A.c);
            Vector2 secondCollumnOfAMatrix = new Vector2(A.b, A.d);
            Vector2 newFirstColumn = B.a * firstCollumnOfAMatrix + B.c * secondCollumnOfAMatrix;
            Vector2 newSecondColumn = B.b * firstCollumnOfAMatrix + B.d * secondCollumnOfAMatrix;

            return new Matrix2x2(newFirstColumn.X, newSecondColumn.X, newFirstColumn.Y, newSecondColumn.Y);
        }

        public double Determinant()
        {
            return a * c - b * d;
        }

        public override string ToString()
        {
            return $"|{a} {b}|\n|{c} {d}|";
        }
        // |a b| |X|
        // |c d| |Y|
        public static Vector2 operator *(Matrix2x2 mat, Vector2 vec)
        {
            return new Vector2(vec.X * mat.a + vec.Y * mat.b, vec.X * mat.c + vec.Y * mat.d);

        }

        public static implicit operator Matrix2x2(ValueTuple<int, int, int, int> tup)
        {
            return new Matrix2x2(tup.Item1, tup.Item2, tup.Item3, tup.Item4);
        }
    }
}
