using System.Security.Cryptography;
namespace ConcurrencyApi.Helpers
{
    public class RandomNumberGen
    {
        [ThreadStatic]
        private static Random? _local;
        public static double NextDouble()
        {
            Random? inst;

            using var rng = RandomNumberGenerator.Create();
            var intBuffer = new byte[sizeof(int)];
            rng.GetBytes(intBuffer);
            _local = inst = new Random(BitConverter.ToInt32(intBuffer, 0));

            return inst.NextDouble();
        }
    }
}