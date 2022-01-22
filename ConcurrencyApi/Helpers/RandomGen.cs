using System.Security.Cryptography;
namespace ConcurrencyApi.Helpers
{
    public class RandomGen
    {
        //HACK: commmented below class as this is depreciated 
        // private static RNGCryptoServiceProvider _global = new();
        
        // [ThreadStatic]
        // private static Random? _local;

        // public static double NextDouble()
        // {
        //     Random? inst = _local;
        //     if (inst is null)
        //     {
        //         byte[] buffer = new byte[4];
        //         _global.GetBytes(buffer);
        //         _local = inst = new Random(
        //             BitConverter.ToInt32(buffer,0));
        //     }
        //     return inst.NextDouble();
        // }
    }
}