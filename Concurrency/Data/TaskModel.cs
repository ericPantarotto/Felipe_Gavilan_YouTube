using System.ComponentModel.DataAnnotations;

namespace Concurrency.Data
{
    public class TaskModel
    {
        [Required]
        public TaskResult? TaskResult { get; set; }= null;
    }
}