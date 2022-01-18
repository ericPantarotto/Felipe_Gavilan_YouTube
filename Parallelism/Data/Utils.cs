namespace Parallelism.Data
{
    public class Utils
    {
          public static string WriteComparison(double time1, double time2)
        {
            double difference = time2 - time1;
            difference = Math.Round(difference, 2);
            double percentageIncrement = ((time2 - time1) / time1) * 100;
            percentageIncrement = Math.Round(percentageIncrement, 2);
            return $"Difference {difference} seconds ({percentageIncrement}%)";
        }
    }
    
}