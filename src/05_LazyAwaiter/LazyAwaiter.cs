using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace _05_LazyAwaiter
{
    public struct LazyAwaiter<T> : INotifyCompletion
    {
        private readonly Lazy<T> _lazy;

        public LazyAwaiter(Lazy<T> lazy) => _lazy = lazy;

        public T GetResult() => _lazy.Value;

        public bool IsCompleted => true;

        public void OnCompleted(Action continuation) { }
    }

    public static class LazyAwaiterExtensions
    {
        public static LazyAwaiter<T> GetAwaiter<T>(this Lazy<T> lazy)
        {
            return new LazyAwaiter<T>(lazy);
        }
    }

}
