using Utilities;

namespace System.Runtime.CompilerServices
{
    public class AsyncVoidMethodBuilder
    {
        public static readonly Inspector Inspector = new Inspector();

        public static AsyncVoidMethodBuilder LastInstance { get; private set; }

        public Exception Exception { get; private set; }

        public AsyncVoidMethodBuilder()
        {
            Inspector.RecordInvocation();
        }

        public static AsyncVoidMethodBuilder Create() => LastInstance = new AsyncVoidMethodBuilder();

        public void SetException(Exception e)
        {
            Exception = e;
            Inspector.RecordInvocation();
        }

        public void SetResult() => Inspector.RecordInvocation();

        public void AwaitOnCompleted<TAwaiter, TStateMachine>(
            ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : INotifyCompletion
            where TStateMachine : IAsyncStateMachine
        {
            Inspector.RecordInvocation();
            stateMachine.MoveNext();
        }

        public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(
            ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : ICriticalNotifyCompletion
            where TStateMachine : IAsyncStateMachine
        {
            Inspector.RecordInvocation();
            stateMachine.MoveNext();
        }

        public void Start<TStateMachine>(ref TStateMachine stateMachine) where TStateMachine : IAsyncStateMachine
        {
            Inspector.RecordInvocation();
            stateMachine.MoveNext();
        }

        public void SetStateMachine(IAsyncStateMachine stateMachine) =>
            Inspector.RecordInvocation();
    }
}