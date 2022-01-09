using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ConcurrencyTest;

//NOTE: https://hamidmosalla.com/2017/02/25/xunit-theory-working-with-inlinedata-memberdata-classdata/

public class ProcessCardsTest
{
    [Theory]
    [ClassData(typeof(TestDataGenerator))]
    public Task ProcessCardsCompletedTaskWithClassData(
        List<string> cards,
        IProgress<(int, int)>? progress,
        CancellationToken token = default,
        CancellationToken tokenProgress = default
        )
    {
        return Task.CompletedTask;
    }

    [Theory]
    [MemberData(nameof(TestDataGeneratorMemberData))]
    public Task ProcessCardsCompletedTaskWithMemberData(
        List<string> cards,
        IProgress<(int, int)>? progress,
        CancellationToken token = default,
        CancellationToken tokenProgress = default
        )
    {
        return Task.CompletedTask;
    }

    public static IEnumerable<object[]> TestDataGeneratorMemberData()
    {
        yield return new object[] {
                new List<string>{"card1", "card2"},
                new Progress<(int, int)>(),
                new CancellationTokenSource().Token , 
                new CancellationTokenSource().Token 
            };
        yield return new object[] {
                new List<string>{"card3", "card4", "card5"},
                new Progress<(int, int)>(),
                new CancellationTokenSource().Token , 
                new CancellationTokenSource().Token 
            };
    }

    public class TestDataGenerator : IEnumerable<object[]>
    {
        private readonly List<object[]> _data = new List<object[]>
        {
            new object[] {
                new List<string>{"card1", "card2"},
                new Progress<(int, int)>(),
                new CancellationTokenSource().Token , 
                new CancellationTokenSource().Token 
            }
        };

        public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }


    
}