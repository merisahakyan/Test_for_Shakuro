using System;

namespace TG.Exam.Algorithms
{
    class Program
    {
        /// <summary>
        /// Calculates c'th element of the sequence with a3=a1+a2 algorithm, where a1=a and a2=b
        /// </summary>
        /// <param name="a">first element of the sequence</param>
        /// <param name="b">second element of the sequence</param>
        /// <param name="c">number of the element to find in a sequence</param>
        /// <returns></returns>
        /// Disadvantages : 
        /// Using reflection performance can be hampered a lot.
        static int Foo(int a, int b, int c)
        {
            if (1 < c)
                return Foo(b, b + a, c - 1);
            else
                return a;
        }

        static int Foo2(int a, int b, int c)
        {
            int temp;
            for (int i = 2; i <= c; i++)
            {
                temp = a;
                a = b;
                b = temp + b;
            }
            return a;
        }

        /// <summary>
        /// this method sorts an array using bubble sort
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        /// Advantages of bubble sort : 
        /// easy to implement and understand
        /// Disadvantages : 
        /// it takes a lot of time to sort.
        static int[] Bar(int[] arr)
        {
            for (int i = 0; i < arr.Length; i++)
                for (int j = 0; j < arr.Length - 1; j++)
                    if (arr[j] > arr[j + 1])
                    {
                        int t = arr[j];
                        arr[j] = arr[j + 1];
                        arr[j + 1] = t;
                    }
            return arr;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Foo result: {0}", Foo(7, 2, 8));
            Console.WriteLine("Bar result: {0}", string.Join(", ", Bar(new[] { 7, 2, 8 })));

            Console.ReadKey();
        }
    }
}
