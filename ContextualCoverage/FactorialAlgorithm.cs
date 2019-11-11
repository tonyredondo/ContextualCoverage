using System;
namespace ContextualCoverage
{
    public static class FactorialAlgorithm
    {
        public static int Factorial(int value)
        {
            if (value == 1)
                return 1;
            return value * Factorial(value - 1);
        }
    }
}
