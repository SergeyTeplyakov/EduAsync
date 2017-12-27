using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace _01_AsyncTaskOfTBuilder
{
    [TestFixture]
    public class AsyncTaskSamples
    {
        [Test]
        public void RunAsyncTask()
        {
            GlobalInspector.Inspector.Clear();
            AsyncTask();
            Thread.Sleep(42);
            GlobalInspector.Inspector.Print();

            Assert.AreEqual(
                new string[]
                {
                    ".ctor", // AsyncTaskMethodBuilder.ctor
                    "Start", // AsyncTaskMethodBuilder.Start
                    // Here the method builder calls the MoveNext on the state machine,
                    // and the state machine runs the code from the method body.
                    "AsyncTask_Start",
                    // The state machine notifies the builder that we're done,
                    // because method is finished synchronously
                    "SetResult",

                    // The generated method access 'Task' property to await for
                    "Task",
                },
                GlobalInspector.Inspector.InvokedMembers);
        }

        [Test]
        public void RunAsyncTaskWithAwait()
        {
            GlobalInspector.Inspector.Clear();
            AsyncTaskWithAwait();
            Thread.Sleep(42);
            GlobalInspector.Inspector.Print();

            Assert.AreEqual(
                new string[]
                {
                    ".ctor", // AsyncTaskMethodBuilder.ctor
                    "Start", // AsyncTaskMethodBuilder.Start
                    // Here the method builder calls the MoveNext on the state machine
                    "AsyncTask_Start",
                    // Task.Yield is finished, and the state machine calls Builder.AwaitUnsafeOnCompleted
                    "AwaitUnsafeOnCompleted",

                    // The builder moves the state machine
                    "AsyncTask_AfterAwait",

                    // The state machine notifies the builder that we're done
                    "SetResult",

                    // The generated method access 'Task' property to await for
                    "Task",
                },
                GlobalInspector.Inspector.InvokedMembers);
        }

        [Test]
        public void RunAsyncTaskThatFails()
        {
            GlobalInspector.Inspector.Clear();
            AsyncTaskThatThrows();
            Thread.Sleep(42);
            GlobalInspector.Inspector.Print();

            Assert.AreEqual(
                new string[]
                {
                    ".ctor", // AsyncTaskMethodBuilder.ctor
                    "Start", // AsyncTaskMethodBuilder.Start
                    // Here the method builder calls the MoveNext on the state machine
                    "AsyncTask_Start",
                    
                    // The state machine calls SetException on the builder instance
                    "SetException",

                    // The generated method access 'Task' property to await for
                    "Task",
                },
                GlobalInspector.Inspector.InvokedMembers);
        }

        [Test]
        public void RunAsyncTaskWithCancellation()
        {
            GlobalInspector.Inspector.Clear();
            AsyncTaskWithCancellation();
            Thread.Sleep(42);
            GlobalInspector.Inspector.Print();

            Assert.AreEqual(
                new string[]
                {
                    ".ctor", // AsyncTaskMethodBuilder.ctor
                    "Start", // AsyncTaskMethodBuilder.Start
                    // Here the method builder calls the MoveNext on the state machine
                    "AsyncTask_Start",
                    
                    // The state machine calls SetException on the builder instance with TaskCanceledException
                    "SetException",

                    // The generated method access 'Task' property to await for
                    "Task",
                },
                GlobalInspector.Inspector.InvokedMembers);

            Assert.IsInstanceOf<TaskCanceledException>(AsyncTaskMethodBuilder<int>.LastInstance.Exception);
        }

        public async Task<int> AsyncTask()
        {
            GlobalInspector.Inspector.Record("AsyncTask_Start");
            return 42;
        }

        public async Task<int> AsyncTaskWithAwait()
        {
            GlobalInspector.Inspector.Record("AsyncTask_Start");
            await Task.Yield();
            GlobalInspector.Inspector.Record("AsyncTask_AfterAwait");
            return 42;
        }

        public async Task<int> AsyncTaskThatThrows()
        {
            GlobalInspector.Inspector.Record("AsyncTask_Start");
            throw new InvalidOperationException();
        }

        public async Task<int> AsyncTaskWithCancellation()
        {
            GlobalInspector.Inspector.Record("AsyncTask_Start");

            var cts = new CancellationTokenSource();
            cts.Cancel();

            await Task.Delay(42, cts.Token);

            return 42;
        }
    }
}
