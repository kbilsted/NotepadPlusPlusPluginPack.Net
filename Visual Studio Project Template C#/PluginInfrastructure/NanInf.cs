namespace Kbg.NppPluginNET.PluginInfrastructure
{
    /// <summary>
    /// holds NaN, Infinity, and -Infinity as things generated at runtime<br></br>
    /// because the compiler freaks out if it sees any of<br></br>
    /// double.NegativeInfinity, double.PositiveInfinity, or double.NaN<br></br>
    /// or any statically analyzed function of two constants that makes one of those constants<br></br>
    /// like 1d/0d, 0d/0d, or -1d/0d.
    /// </summary>
    public class NanInf
    {
        /// <summary>
        /// a/b<br></br>
        /// may be necessary to generate infinity or nan at runtime
        /// to avoid the compiler pre-computing things<br></br>
        /// since if the compiler sees literal 1d/0d in the code
        /// it just pre-computes it at compile time
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static double Divide(double a, double b) { return a / b; }

        public static readonly double inf = Divide(1d, 0d);
        public static readonly double neginf = Divide(-1d, 0d);
        public static readonly double nan = Divide(0d, 0d);
    }
}
