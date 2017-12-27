using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace _01_AsyncVoidBuilder
{
    [TestFixture]
    public class AsyncVoidSamples
    {
        [Test]
        public void RunAsyncVoid()
        {
            Console.WriteLine("Before VoidAsync");
            VoidAsync();
            Console.WriteLine("After VoidAsync");

            async void VoidAsync() { }
        }
    }
}
