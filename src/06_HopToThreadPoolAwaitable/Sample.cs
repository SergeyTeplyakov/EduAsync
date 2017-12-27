using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace _06_HopToThreadPoolAwaitable
{
    [TestFixture]
    public class Sample
    {
        [Test]
        public async Task Test()
        {
            var testThreadId = Thread.CurrentThread.ManagedThreadId;
            await Sample();

            async Task Sample()
            {
                Assert.AreEqual(Thread.CurrentThread.ManagedThreadId, testThreadId);

                await default(HopToThreadPoolAwaitable);
                Assert.AreNotEqual(Thread.CurrentThread.ManagedThreadId, testThreadId);
            }
        }
    }
}
