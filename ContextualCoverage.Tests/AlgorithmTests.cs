using Xunit;

namespace ContextualCoverage.Tests
{
    public class AlgorithmTests
    {
        [Fact]
        public void FactorialTest()
        {
            using var _ = Coverage.CreateSession();
            CoveredTest();

            static void CoveredTest()
            {
                FactorialAlgorithm.Factorial(4500);
            }
        }

        [Fact]
        public void FibonacciTest()
        {
            using var _ = Coverage.CreateSession();
            CoveredTest();

            static void CoveredTest()
            {
                for (var i = 0; i < 1000; i++)
                    FibonacciAlgorithm.Fibonacci(i);
            }
        }
    }
}
