/*
 * 5. Write a program which creates two threads and a shared collection:
 * the first one should add 10 elements into the collection and the second should print all elements
 * in the collection after each adding.
 * Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.
 */
using System;
using System.Collections.Generic;
using System.Threading;

namespace MultiThreading.Task5.Threads.SharedCollection
{
    class Program
    {
        private static readonly List<int> sharedCollection = new List<int>();

        private static void AddElement(int element)
        {
            lock (sharedCollection)
            {
                sharedCollection.Add(element);
                Console.WriteLine($"Added element {element} to the collection");
                Thread.Sleep(100);
            }
        }

        private static void PrintCollection()
        {
            lock (sharedCollection)
            {
                foreach (var item in sharedCollection)
                {

                    Console.Write($"{item} ");
                }
                Thread.Sleep(100);
                Console.WriteLine();
            }
        }
        static void Main(string[] args)
        {
            Console.WriteLine("5. Write a program which creates two threads and a shared collection:");
            Console.WriteLine("the first one should add 10 elements into the collection and the second should print all elements in the collection after each adding.");
            Console.WriteLine("Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.");
            Console.WriteLine();
            // Create two threads
            Thread addingThread = new Thread(() =>
            {
                for (int i = 1; i <= 10; i++)
                {
                    AddElement(i);
                }
            });

            Thread printingThread = new Thread(() =>
            {
                while (true)
                {
                    PrintCollection();

                    // Wait for a short time before printing the collection again
                    if (sharedCollection.Count == 10) break;
                }
            });

            // Start the threads
            addingThread.Start();
            printingThread.Start();

            // Wait for the threads to finish
            addingThread.Join();
            printingThread.Join();
            Console.ReadLine();
        }

    }
}
