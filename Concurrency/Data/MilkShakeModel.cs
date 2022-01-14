using System.ComponentModel.DataAnnotations;

namespace Concurrency.Data
{
    public class MilkShakeModel
    {
        [Required]
        public MilkshakeSize? MilkshakeSize { get; set; }= null;
    }
}