using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tak.Tests;

namespace Tak
{
    class Program
    {
        static void Main(string[] args)
        {
            TestClone test = new TestClone();

            test.Test0();

            test.Test1();

            test.Test2();

            Console.Write("\nPress any key to exit...");
            Console.ReadLine();
        }
    }
}
