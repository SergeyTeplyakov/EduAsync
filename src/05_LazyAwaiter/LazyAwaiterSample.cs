using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _05_LazyAwaiter
{
    class LazyAwaiterSample
    {
        public static async Task Foo()
        {
            var lazy = new Lazy<int>(() => 42);
            var result = await lazy;
            Console.WriteLine(result);
        }
    }
}
