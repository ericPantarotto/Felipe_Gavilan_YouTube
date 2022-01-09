using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ConcurrencyTest;

public class UnitTest1
{
    [Theory]
    [MemberData(nameof(Data))]
    public void CanAddTheoryMemberDataProperty(int value1, int value2, int expected)
    {
        var calculator = new Calculator();

        var result = calculator.Add(value1, value2);

        Assert.Equal(expected, result);
    }

    public static IEnumerable<object[]> Data =>
        new List<object[]>
        {
            new object[] { 1, 2, 3 },
            new object[] { -4, -6, -10 },
            new object[] { -2, 2, 0 },
            new object[] { int.MinValue, -1, int.MaxValue },
        };

    public class Calculator
    {
        public int Add(int value1, int value2)
        {
            return value1 + value2;
        }
    }


}