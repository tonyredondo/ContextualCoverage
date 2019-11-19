using Xunit;

namespace ContextualCoverage.Tests
{
    public class AlgorithmTests
    {
        [Fact]
        public void FactorialTest()
        {
            FactorialAlgorithm.Factorial(4500);
        }

        [Fact]
        public void FibonacciTest()
        {
            for (var i = 0; i < 1000; i++)
                FibonacciAlgorithm.Fibonacci(i);
        }
    }
}
