/*
 * 4.	Write a program which recursively creates 10 threads.
 * Each thread should be with the same body and receive a state with integer number, decrement it,
 * print and pass as a state into the newly created thread.
 * Use Thread class for this task and Join for waiting threads.
 * 
 * Implement all of the following options:
 * - a) Use Thread class for this task and Join for waiting threads.
 * - b) ThreadPool class for this task and Semaphore for waiting threads.
 */

using System;
using System.Threading;

namespace MultiThreading.Task4.Threads.Join
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("4.	Write a program which recursively creates 10 threads.");
            Console.WriteLine("Each thread should be with the same body and receive a state with integer number, decrement it, print and pass as a state into the newly created thread.");
            Console.WriteLine("Implement all of the following options:");
            Console.WriteLine();
            Console.WriteLine("- a) Use Thread class for this task and Join for waiting threads.");

            Console.WriteLine();

            StateUpdate(10);

            Console.WriteLine("- b) ThreadPool class for this task and Semaphore for waiting threads.");
            UpdateState(15);

            Console.WriteLine();
            Console.ReadLine();
        }

        static void StateUpdate(int state)
        {
            if (state == 0)
            {
                return;
            }
            Thread thread = new Thread(() =>
            {
                state--;
                Console.WriteLine(state);
            });
            thread.Start();
            thread.Join();
            StateUpdate(state);
        }

        static void UpdateState(int state)
        {

            var semaphore = new SemaphoreSlim(0, 1);
            if (state == 0)
            {
                return;
            }
            ThreadPool.QueueUserWorkItem((obj) =>
            {
                semaphore.Wait();
                try
                {
                    Interlocked.Decrement(ref state);
                    Console.WriteLine(state);
                }
                finally
                {
                    semaphore.Release();
                }
            });
            UpdateState(state);
        }
    }
}
