using System;

namespace Targil0
{
    partial class Program
    {
        static void Main(string[] args)
        {
            Welcome5970();
            Welcome5654();
            Console.ReadKey();
        }

        static partial void Welcome5654();
        private static void Welcome5970()
        {
            Console.WriteLine("Enter your name: ");
            string name = Console.ReadLine();
            Console.WriteLine("{0},Imanuel welcome to my first console application", name);
        }
    }
}