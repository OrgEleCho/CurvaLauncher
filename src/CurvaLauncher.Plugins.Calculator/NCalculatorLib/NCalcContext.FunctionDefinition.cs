namespace NCalculatorLib
{
    public partial class NCalcContext
    {
        private static class FunctionDefinition
        {

            public static double Abs(double[] args)
            {
                if (args.Length != 1)
                    throw new ArgumentException("One parameter is required.");
                return Math.Abs(args[0]);
            }


            public static double Ceil(double[] args)
            {
                if (args.Length != 1)
                    throw new ArgumentException("One parameter is required.");
                return Math.Ceiling(args[0]);
            }


            public static double Floor(double[] args)
            {
                if (args.Length != 1)
                    throw new ArgumentException("One parameter is required.");
                return Math.Floor(args[0]);
            }


            public static double Min(double[] args)
            {
                if (args.Length < 1)
                    throw new ArgumentException("One parameter is required at least.");
                return args.Min();
            }


            public static double Max(double[] args)
            {
                if (args.Length < 1)
                    throw new ArgumentException("One parameter is required at least.");
                return args.Max();
            }

            public static double Sum(double[] args)
            {
                if (args.Length < 1)
                    throw new ArgumentException("One parameter is required at least.");
                return args.Sum();
            }


            public static double Ln(double[] args)
            {
                if (args.Length != 1)
                    throw new ArgumentException("One parameter is required.");
                return Math.Log(args[0]);
            }


            public static double Log(double[] args)
            {
                if (args.Length == 1)
                    return Math.Log(args[0]);
                else if (args.Length == 2)
                    return Math.Log(args[0], args[1]);
                else
                    throw new ArgumentException("One or two parameters are required.");
            }


            public static double Log10(double[] args)
            {
                if (args.Length != 1)
                    throw new ArgumentException("One parameter is required.");
                return Math.Log10(args[0]);
            }


            public static double Pow(double[] args)
            {
                if (args.Length != 2)
                    throw new ArgumentException("Two parameters are required.");
                return Math.Pow(args[0], args[1]);
            }


            public static double Sqrt(double[] args)
            {
                if (args.Length != 1)
                    throw new ArgumentException("One parameter is required.");
                return Math.Sqrt(args[0]);
            }


            public static double Exp(double[] args)
            {
                if (args.Length != 1)
                    throw new ArgumentException("One parameter is required.");
                return Math.Exp(args[0]);
            }


            public static double Cbrt(double[] args)
            {
                if (args.Length != 1)
                    throw new ArgumentException("One parameter is required.");
                return Math.Cbrt(args[0]);
            }


            public static double Sin(double[] args)
            {
                if (args.Length != 1)
                    throw new ArgumentException("One parameter is required.");
                return Math.Sin(args[0]);
            }


            public static double Cos(double[] args)
            {
                if (args.Length != 1)
                    throw new ArgumentException("One parameter is required.");
                return Math.Cos(args[0]);
            }


            public static double Tan(double[] args)
            {
                if (args.Length != 1)
                    throw new ArgumentException("One parameter is required.");
                return Math.Tan(args[0]);
            }


            public static double Asin(double[] args)
            {
                if (args.Length != 1)
                    throw new ArgumentException("One parameter is required.");
                return Math.Asin(args[0]);
            }


            public static double Acos(double[] args)
            {
                if (args.Length != 1)
                    throw new ArgumentException("One parameter is required.");
                return Math.Acos(args[0]);
            }


            public static double Atan(double[] args)
            {
                if (args.Length != 1)
                    throw new ArgumentException("One parameter is required.");
                return Math.Atan(args[0]);
            }


            public static double Sinh(double[] args)
            {
                if (args.Length != 1)
                    throw new ArgumentException("One parameter is required.");
                return Math.Sinh(args[0]);
            }


            public static double Cosh(double[] args)
            {
                if (args.Length != 1)
                    throw new ArgumentException("One parameter is required.");
                return Math.Cosh(args[0]);
            }


            public static double Tanh(double[] args)
            {
                if (args.Length != 1)
                    throw new ArgumentException("One parameter is required.");
                return Math.Tanh(args[0]);
            }


            public static double Asinh(double[] args)
            {
                if (args.Length != 1)
                    throw new ArgumentException("One parameter is required.");
                return Math.Asinh(args[0]);
            }


            public static double Acosh(double[] args)
            {
                if (args.Length != 1)
                    throw new ArgumentException("One parameter is required.");
                return Math.Acosh(args[0]);
            }


            public static double Atanh(double[] args)
            {
                if (args.Length != 1)
                    throw new ArgumentException("One parameter is required.");
                return Math.Atanh(args[0]);
            }


            public static double Round(double[] args)
            {
                if (args.Length != 1)
                    throw new ArgumentException("One parameter is required.");
                return Math.Round(args[0]);
            }


            public static double Sign(double[] args)
            {
                if (args.Length != 1)
                    throw new ArgumentException("One parameter is required.");
                return Math.Sign(args[0]);
            }


            public static double Truncate(double[] args)
            {
                if (args.Length != 1)
                    throw new ArgumentException("One parameter is required.");
                return Math.Truncate(args[0]);
            }

            public static double Fact(double[] args)
            {
                if (args.Length != 1)
                    throw new ArgumentException("One parameter is required.");
                double rst = 1;
                for (int i = 1; i <= args[0]; i++)
                {
                    rst *= i;
                }

                return rst;
            }
        }
    }
}
