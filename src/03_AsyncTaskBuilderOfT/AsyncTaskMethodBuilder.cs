using System.Threading.Tasks;
using Utilities;
using static System.Runtime.CompilerServices.GlobalInspector;

namespace System.Runtime.CompilerServices
{
    public static class GlobalInspector
    {
        // Need to move the inspector here from AsyncTaskMethodBuilder<T>
        // because we don't want to share the instance across all generic instantiations.
        public static readonly Inspector Inspector = new Inspector();
    }

    public class AsyncTaskMethodBuilder<T>
    {
        public static AsyncTaskMethodBuilder<T> LastInstance { get; private set; }

        public Exception Exception { get; private set; }

        public AsyncTaskMethodBuilder()
        {
            GlobalInspector.Inspector.RecordInvocation();
        }

        public static AsyncTaskMethodBuilder<T> Create() => LastInstance = new AsyncTaskMethodBuilder<T>();

        public void SetException(Exception e)
        {
            Exception = e;
            GlobalInspector.Inspector.RecordInvocation();
        }

        public Task<T> Task
        {
            get
            {
                GlobalInspector.Inspector.RecordInvocation();
                return Threading.Tasks.Task.FromResult(default(T));
            }
        }

        public void SetResult(T result) => GlobalInspector.Inspector.RecordInvocation();

        public void AwaitOnCompleted<TAwaiter, TStateMachine>(
            ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : INotifyCompletion
            where TStateMachine : IAsyncStateMachine
        {
            GlobalInspector.Inspector.RecordInvocation();
            stateMachine.MoveNext();
        }

        public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(
            ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : ICriticalNotifyCompletion
            where TStateMachine : IAsyncStateMachine
        {
            GlobalInspector.Inspector.RecordInvocation();
            stateMachine.MoveNext();
        }

        public void Start<TStateMachine>(ref TStateMachine stateMachine) where TStateMachine : IAsyncStateMachine
        {
            GlobalInspector.Inspector.RecordInvocation();
            stateMachine.MoveNext();
        }

        public void SetStateMachine(IAsyncStateMachine stateMachine) =>
            GlobalInspector.Inspector.RecordInvocation();
    }
}