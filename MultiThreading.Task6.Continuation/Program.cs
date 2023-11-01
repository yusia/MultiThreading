/*
*  Create a Task and attach continuations to it according to the following criteria:
   a.    Continuation task should be executed regardless of the result of the parent task.
   b.    Continuation task should be executed when the parent task finished without success.
   c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation
   d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled
   Demonstrate the work of the each case with console utility.
*/
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Task6.Continuation
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Create a Task and attach continuations to it according to the following criteria:");
            Console.WriteLine("a.    Continuation task should be executed regardless of the result of the parent task.");

            Console.WriteLine("b.    Continuation task should be executed when the parent task finished without success.");

            Console.WriteLine("c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation.");
            Console.WriteLine("d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled.");

            Console.WriteLine("Demonstrate the work of the each case with console utility.");
            Console.WriteLine();
            ContinuationRegardlessResult();
            ContinuationWithoutSuccess();
            ContinuationParentReused();
            ContinuationOutsideThreadPool();
            Console.ReadLine();
        }
        static void ContinuationRegardlessResult()
        {
            var parentTask = Task.Run(() =>
                {
                    Console.WriteLine("a. ParentTask is running");
                });

            var continuationTask = parentTask.ContinueWith(continuatianCallback);
            WaitAndPrintStatus(parentTask, continuationTask);
        }
        static void ContinuationWithoutSuccess()
        {
            var parentTask = Task.Run(async () =>
            {
                Console.WriteLine("b. Parent task throw exeption");

                await Task.Delay(500);
                throw new Exception("ParentTask is fauled");
            });
            var continuationTask = parentTask.ContinueWith(continuatianCallback,
                TaskContinuationOptions.NotOnRanToCompletion);

            WaitAndPrintStatus(parentTask, continuationTask);
        }

        static void ContinuationParentReused()
        {
            Task parentTask = Task.Run(async () =>
            {
                Console.WriteLine("c. ParentTask is running");
                await Task.Delay(500);
                throw new Exception("ParentTask is fauled");
            });

            Task continuationTask = parentTask.ContinueWith(continuatianCallback,
                TaskContinuationOptions.OnlyOnFaulted | TaskContinuationOptions.ExecuteSynchronously);

            WaitAndPrintStatus(parentTask, continuationTask);
        }


        static void ContinuationOutsideThreadPool()
        {
            using var cts = new CancellationTokenSource();
            CancellationToken token = cts.Token;

            var timer = new Timer(Elapsed, cts, 100, Timeout.Infinite);

            Task parentTask = Task.Run(async () =>
            {
                Console.WriteLine("d. ParentTask is running");
                await Task.Delay(500);

                if (token.IsCancellationRequested)
                {
                    Console.WriteLine("\nCancellation requested in parentTask...\n");

                    token.ThrowIfCancellationRequested();
                }
            }, token);

            Task continuationTask = parentTask.ContinueWith((previousTask) =>
            {
                if (previousTask.IsCanceled)
                {
                    Console.WriteLine("d. ContinuationTask is running after cancellation");
                }

            }, TaskContinuationOptions.OnlyOnCanceled | TaskContinuationOptions.RunContinuationsAsynchronously);

            WaitAndPrintStatus(parentTask, continuationTask);
        }
        static void continuatianCallback(Task previousTask)
        {
            Console.WriteLine($"Continuation running always runs!");
        }

        static void Elapsed(object? state)
        {
            if (state is CancellationTokenSource cts)
            {
                cts.Cancel();
                Console.WriteLine("\nCancellation request issued...\n");
            }
        }

        static void WaitAndPrintStatus(Task parentTask, Task continuationTask)
        {
            try
            {
                parentTask.Wait();
                continuationTask.Wait();
            }
            catch (AggregateException ex)
            {
                Console.WriteLine("Exception: {0}", ex.InnerException.Message);
            }

            Console.WriteLine("\nAntecedent Status: {0}", parentTask.Status);
            Console.WriteLine("Continuation Status: {0}", continuationTask.Status);
        }

    }
}