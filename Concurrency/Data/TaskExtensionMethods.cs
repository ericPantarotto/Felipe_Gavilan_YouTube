namespace Concurrency.Data
{
   public static class TaskExtensionMethods
   {
        public static async Task<T> WithCancellation<T>(this Task<T> task, CancellationToken cancellationToken){
            
            TaskCompletionSource<object> tcs = new(TaskCreationOptions.RunContinuationsAsynchronously);
            
            using (cancellationToken.Register(state =>
            {
               ( (TaskCompletionSource<object>)state! ).TrySetResult(TaskStatus.Running);
            }, tcs))
            {
               Task resultTask = await Task.WhenAny(task, tcs.Task);
               if (resultTask == tcs.Task){
                   throw new OperationCanceledException(cancellationToken);
               }
               return await task;
            }

   
        }
       
   }
    
}