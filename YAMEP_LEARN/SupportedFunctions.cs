using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YAMEP_LEARN {
    public class SupportedFunctions {
        public static double Sin(double a) => Math.Sin(a);
        public static double Cos(double a) => Math.Cos(a);
        public static double Tan(double a) => Math.Tan(a);
        public static double Tg(double a) => Math.Tan(a);
        public static double Ctg(double a) => 1.0 / Math.Tan(a);
        public static double Log(double a) => Math.Log(a);
        public static double Log10(double a) => Math.Log10(a);
        public static double Log(double a, double newBase) => Math.Log(a, newBase);
        public static double Pow(double a, double b) => Math.Pow(a, b);

        public static string Not_Me_1(double a) => $"{a}";
        public static double Not_Me_2(string a) => a.Length;
        public static double Not_Me_3(string a, double b) => a.Length + b;
    }
}
