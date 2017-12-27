using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;


namespace _07_CustomTaskLikeTypes
{
    public sealed class TaskLikeMethodBuilder
    {
        public TaskLikeMethodBuilder()
            => Console.WriteLine(".ctor");

        public static TaskLikeMethodBuilder Create()
            => new TaskLikeMethodBuilder();

        public void SetResult() => Console.WriteLine("SetResult");

        public void Start<TStateMachine>(ref TStateMachine stateMachine)
            where TStateMachine : IAsyncStateMachine
        {
            Console.WriteLine("Start");
            stateMachine.MoveNext();
        }

        public TaskLike Task => default(TaskLike);

        // AwaitOnCompleted, AwaitUnsafeOnCompleted, SetException 
        // and SetStateMachine are empty
        public void AwaitOnCompleted<TAwaiter, TStateMachine>(
            ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : INotifyCompletion
            where TStateMachine : IAsyncStateMachine
        { }

        public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(
            ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : ICriticalNotifyCompletion
            where TStateMachine : IAsyncStateMachine
        { }

        public void SetException(Exception e) { }

        public void SetStateMachine(IAsyncStateMachine stateMachine) { }
    }

    [System.Runtime.CompilerServices.AsyncMethodBuilder(typeof(TaskLikeMethodBuilder))]
    public struct TaskLike
    {
        public TaskLikeAwaiter GetAwaiter() => default(TaskLikeAwaiter);
    }

    public struct TaskLikeAwaiter : INotifyCompletion
    {
        public void GetResult() { }

        public bool IsCompleted => true;

        public void OnCompleted(Action continuation) { }
    }
}
