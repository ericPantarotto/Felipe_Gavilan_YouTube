using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ConcurrencyTest;

//NOTE: https://hamidmosalla.com/2017/02/25/xunit-theory-working-with-inlinedata-memberdata-classdata/

public class GetCardsTestX
{
    [Theory]
    [MemberData(nameof(TestDataGeneratorMemberData))]
    public Task<List<string>> GetCards(int amountofCardsToGenerate, 
        CancellationToken token = default,
        IProgress<(int, int)>? progress =default)
    {
        List<string> cards = new()
        {
            "card1",
            "card2"
        };

        return Task.FromResult(cards);
    }
    public static IEnumerable<object[]> TestDataGeneratorMemberData()
    {
        yield return new object[] {
                1000,
                new CancellationTokenSource().Token , 
                new Progress<(int, int)>()
            };
        yield return new object[] {
                10000,
                new CancellationTokenSource().Token , 
                new Progress<(int, int)>()
            };
    }

    //NOTE: https://gist.github.com/alefranz/59f3fbd2f4f301d0f4696a3bdbd6575e
    [Fact]
    public async Task CreateTaskWithException()
    {
        var task = TaskWithException();
        Assert.True(task.IsFaulted);
        await Assert.ThrowsAsync<ApplicationException>(() => task);
    }
    
    private Task TaskWithException()
    {
        return Task.FromException(new ApplicationException());
    }


    [Fact]
    public void  CreateTaskWithCancelled()
    {
        CancellationTokenSource cts = new();
        cts.Cancel();
        var task = TaskWithCancelled(cts.Token);
        Assert.True(task.IsCanceled);
        Assert.ThrowsAsync<OperationCanceledException>(() => task);
        
    }
    public Task TaskWithCancelled(CancellationToken token = default)
    {
        
        return Task.FromCanceled( token);
    }

    
}