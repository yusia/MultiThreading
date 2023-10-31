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

        static void Main(string[] args)
        {
            Console.WriteLine("5. Write a program which creates two threads and a shared collection:");
            Console.WriteLine("the first one should add 10 elements into the collection and the second should print all elements in the collection after each adding.");
            Console.WriteLine("Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.");
            Console.WriteLine();

            createThreads();

            Console.ReadLine();
        }

        private static void createThreads()
        {
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

                    if (sharedCollection.Count == 10) break;
                }
            });

            addingThread.Start();
            printingThread.Start();

            addingThread.Join();
            printingThread.Join();
        }

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
    }
}
