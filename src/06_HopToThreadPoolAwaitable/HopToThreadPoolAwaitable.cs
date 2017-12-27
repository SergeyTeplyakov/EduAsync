using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace _06_HopToThreadPoolAwaitable
{
public struct HopToThreadPoolAwaitable : INotifyCompletion
{
    public HopToThreadPoolAwaitable GetAwaiter() => this;
    public bool IsCompleted => false;

    public void OnCompleted(Action continuation) => Task.Run(continuation);
    public void GetResult() { }
}
}
