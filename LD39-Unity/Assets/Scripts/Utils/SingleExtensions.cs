namespace Utils
{
    public static class SingleExtensions
    {
        public static bool IsInfinity(this float source)
        {
            return float.IsInfinity(source);
        }

        public static bool IsNan(this float source)
        {
            return float.IsNaN(source);
        }

        public static bool IsInfinityOrNan(this float source)
        {
            return source.IsInfinity() || source.IsNan();
        }
    }
}
