/*
 * 2.	Write a program, which creates a chain of four Tasks.
 * First Task – creates an array of 10 random integer.
 * Second Task – multiplies this array with another random integer.
 * Third Task – sorts this array by ascending.
 * Fourth Task – calculates the average value. All this tasks should print the values to console.
 */
using System;
using System.Linq;
using System.Threading.Tasks;

Console.WriteLine(".Net Mentoring Program. MultiThreading V1 ");
Console.WriteLine("2.	Write a program, which creates a chain of four Tasks.");
Console.WriteLine("First Task – creates an array of 10 random integer.");
Console.WriteLine("Second Task – multiplies this array with another random integer.");
Console.WriteLine("Third Task – sorts this array by ascending.");
Console.WriteLine("Fourth Task – calculates the average value. All this tasks should print the values to console");
Console.WriteLine();


var task = Task.Run(initArray)
    .ContinueWith(multipliesArray)
    .ContinueWith(sortedArray)
    .ContinueWith(averageValue);

task.Wait();
Console.ReadLine();

int[] initArray()
{
    var array = new int[10];
    Random rnd = new Random();

    Console.WriteLine("Init array: ");
    for (int j = 0; j < array.Length; j++)
    {
        array[j] = rnd.Next(0, 10000);
        Console.Write($"{array[j]} ");
    }
    Console.WriteLine();
    return array;
}
int[] multipliesArray(Task<int[]> task)
{
    int[] array = task.Result;
    Random rnd = new Random();
    var n = rnd.Next(0, 10000);
    Console.WriteLine($"number: {n} ");

    for (int j = 0; j < array.Length; j++)
    {
        array[j] = n * array[j];
        Console.Write($"{array[j]} ");
    }
    Console.WriteLine();
    return array;
}
int[] sortedArray(Task<int[]> task)
{
    int[] array = task.Result;
    {
        Console.WriteLine("Sorted array: ");
        Array.Sort(array);
        for (int j = 0; j < array.Length; j++)
        {
            Console.Write($"{array[j]} ");
        }
        Console.WriteLine();
        return array;
    }
}
int averageValue(Task<int[]> task)
{
    int[] array = task.Result;
    {
        var average = array.Sum() / array.Length;

        Console.WriteLine($"Average value: {average} ");
        return average;
    }
}

