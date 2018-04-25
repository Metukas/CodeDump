using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Security.Cryptography;

namespace Challenges
{
    class CircleCenterInTriangleProblem
    {
        public static double CalculateProbability(int testCount)
        {
            Random rand = new Random();
            ThreeOrderedAngles01 random0To1Angles;
            //Func<double> randAngle = rand.NextDouble;
            int hitCount = 0;
            for (int i = 0; i < testCount; i++)
            {
                random0To1Angles = new ThreeOrderedAngles01(rand.NextDouble(), rand.NextDouble(), rand.NextDouble());
                if (random0To1Angles.GetTwoAnglesInSameSemiCircle(out Angle01[] angles, out Angle01 third))
                {
                    if (IsP3BetweenP1AndP2(angles[0], angles[1], third.Inverted))
                    {
                        hitCount++;
                    }
                }
            }
            return (double)hitCount / (double)testCount;
        }

        public static double CalculateProbabilityNoBullshit(int testCount)
        {
            Random rand = new Random();
            double[] angles = new double[3];
            int hitCount = 0;
            for (int i = 0; i < testCount; i++)
            {
                angles[0] = rand.NextDouble();
                angles[1] = rand.NextDouble();
                angles[2] = rand.NextDouble();
                Array.Sort(angles);

                bool isBetween1 = IsP3BetweenP1AndP2(angles[0], angles[1], angles[2].InvertedAngle());
                bool isBetween2 = IsP3BetweenP1AndP2(angles[1], angles[2], angles[0].InvertedAngle());
                //bool isBetween3 = IsP3BetweenP1AndP2(angles[1].InvertedAngle(), angles[2].InvertedAngle(), angles[0]);
                if (isBetween1 && isBetween2) //&& isBetween3) // užtenka tik dviejų taškų
                    hitCount++;
            }

            return (double)hitCount / (double)testCount;
        }

        public static double CalculateProbabilityNoBullshitParallelFor(int testCount)
        {
            object locker = new object();
            Random rand = new Random();
            int hitCount = 0;
            Parallel.For(0, testCount, () => new double[3], 
            (_, state, angles) => 
            {
                lock (locker)
                {
                    angles[0] = rand.NextDouble();
                    angles[1] = rand.NextDouble();
                    angles[2] = rand.NextDouble();
                }
                Array.Sort(angles);
                
                bool isBetween1 = IsP3BetweenP1AndP2(angles[0], angles[1], angles[2].InvertedAngle());
                bool isBetween2 = IsP3BetweenP1AndP2(angles[1], angles[2], angles[0].InvertedAngle());
                
                if (isBetween1 && isBetween2) //&& isBetween3) // užtenka tik dviejų taškų
                    hitCount++;

                return angles;
            }, 
            (_) => {});

            return (double)hitCount / (double)testCount;
        }

        public static async Task<double> CalculateProbabilityNoBullshitParallelTask(int testCount)
        {
            object locker = new object();
            Random rand = new Random();

            double ProbTask()
            {
                int hitCount = 0;
                double[] angles = new double[3];
                for (int i = 0; i < (testCount / 4); i++)
                {
                    lock (locker)
                    {
                        angles[0] = rand.NextDouble();
                        angles[1] = rand.NextDouble();
                        angles[2] = rand.NextDouble();
                    }

                    Array.Sort(angles);

                    bool isBetween1 = IsP3BetweenP1AndP2(angles[0], angles[1], angles[2].InvertedAngle());
                    bool isBetween2 = IsP3BetweenP1AndP2(angles[1], angles[2], angles[0].InvertedAngle());

                    if (isBetween1 && isBetween2) //&& isBetween3) // užtenka tik dviejų taškų
                        hitCount++;
                }
                return hitCount;
            }

            var tasks = new Task<double>[4];
            tasks[0] = Task.Factory.StartNew(ProbTask);
            tasks[1] = Task.Factory.StartNew(ProbTask);
            tasks[2] = Task.Factory.StartNew(ProbTask);
            tasks[3] = Task.Factory.StartNew(ProbTask);

            Task.WaitAll(tasks);

            return (double)(tasks[0].Result + tasks[1].Result + tasks[2].Result + tasks[3].Result) / (double)testCount;
        }

        static bool IsP3BetweenP1AndP2(double p1, double p2, double p3)
        {
            return p3 >= p1 && p3 <= p2;
        }

        public static Complex PointOnCircle0To1(double radius, double angleFrom0To1, Complex centre)
        {
            return centre + radius * Complex.Exp(Math.PI * Complex.ImaginaryOne * (angleFrom0To1 / 0.5));
        }
    }
    struct Angle01
    {
        private double angle;
        public double Angle
        {
            get => angle;
            private set
            {
                angle = value % 1;
            }
        }
    public bool IsYpositive { get => Angle <= 0.5; }


    public Angle01 Inverted { get => new Angle01(this.Angle + 0.5); }

        public Angle01(double angle) : this()
        {
            Angle = angle;
        }

        public static implicit operator Angle01(double num)
        {
            return new Angle01(num);
        }

        public static implicit operator double(Angle01 num)
        {
            return num.Angle;
        }

        public static bool operator >(Angle01 left, Angle01 right)
        {
            return left.Angle > right.Angle;
        }

        public static bool operator <(Angle01 left, Angle01 right)
        {
            return left.Angle < right.Angle;
        }

        public static bool operator ==(Angle01 left, Angle01 right)
        {
            return left.Angle == right.Angle;
        }

        public static bool operator !=(Angle01 left, Angle01 right)
        {
            return left.Angle != right.Angle;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Angle01))
            {
                return false;
            }

            var angle = (Angle01)obj;
            return Angle == angle.Angle;
        }

        public override int GetHashCode()
        {
            var hashCode = 969184184;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + Angle.GetHashCode();
            return hashCode;
        }
    }

    struct ThreeOrderedAngles01
    {
        public Angle01 Angle1 { get; }
        public Angle01 Angle2 { get; }
        public Angle01 Angle3 { get; }

        public int YpositiveCount { get; }
        public int YnegativeCount { get => 3 - YpositiveCount; }

        public bool AllInSameVerticalSemiCircle => Angle1.IsYpositive && Angle2.IsYpositive && Angle3.IsYpositive ||
                !(Angle1.IsYpositive || Angle2.IsYpositive || Angle3.IsYpositive);

        public ThreeOrderedAngles01(Angle01 angle1, Angle01 angle2, Angle01 angle3) : this()
        {
            YpositiveCount = 0;
            // TODO išorderint
            Angle1 = Math.Min(angle1, Math.Min(angle2, angle3));
            Angle3 = Math.Max(angle1, Math.Max(angle2, angle3));
            Angle2 = Between(Angle1, Angle3, angle1, angle2, angle3);

            System.Diagnostics.Debug.Assert(Angle1 <= Angle2 && Angle2 <= Angle3);

            if (angle1.IsYpositive) YpositiveCount++;
            if (angle2.IsYpositive) YpositiveCount++;
            if (angle3.IsYpositive) YpositiveCount++;
        }

        private Angle01 Between(Angle01 min, Angle01 max, Angle01 angle1, Angle01 angle2, Angle01 angle3)
        {
            if (angle1 > min && angle1 < max) return angle1;
            if (angle2 > min && angle2 < max) return angle2;
            if (angle3 > min && angle3 < max) return angle3;

            return angle3;
        }

        public bool GetTwoAnglesInSameSemiCircle(out Angle01[] angles, out Angle01 opposite)
        {
            angles = new Angle01[2];
            if (YpositiveCount == 2)
            {
                angles[0] = Angle1;
                angles[1] = Angle2;
                opposite = Angle3;
                return true;
            }
            if (YnegativeCount == 2)
            {
                angles[0] = Angle2;
                angles[1] = Angle3;
                opposite = Angle1;
                return true;
            }

            opposite = default;
            return false;
        }
    }
}
