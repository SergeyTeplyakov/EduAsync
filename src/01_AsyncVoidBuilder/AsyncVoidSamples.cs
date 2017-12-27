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
            AsyncVoidMethodBuilder.Inspector.Clear();
            VoidAsync();
            Thread.Sleep(42);
            AsyncVoidMethodBuilder.Inspector.Print();

            Assert.AreEqual(
                new string[]
                {
                    ".ctor", // AsyncVoidMethodBuilder.ctor
                    "Start", // AsyncVoidMethodBuilder.Start
                    // Here the method builder calls the MoveNext on the state machine,
                    // and the state machine runs the code from the method body.
                    "VoidAsync_Start",
                    // The state machine notifies the builder that we're done,
                    // because method is finished synchronously
                    "SetResult",
                },
                AsyncVoidMethodBuilder.Inspector.InvokedMembers);
        }

        [Test]
        public void RunAsyncVoidWithAwait()
        {
            AsyncVoidMethodBuilder.Inspector.Clear();
            VoidAsyncWithAwait();
            Thread.Sleep(42);
            AsyncVoidMethodBuilder.Inspector.Print();

            Assert.AreEqual(
                new string[]
                {
                    ".ctor", // AsyncVoidMethodBuilder.ctor
                    "Start", // AsyncVoidMethodBuilder.Start
                    // Here the method builder calls the MoveNext on the state machine
                    "VoidAsync_Start",
                    // Task.Yield is finished, and the state machine calls Builder.AwaitUnsafeOnCompleted
                    "AwaitUnsafeOnCompleted",

                    // The builder moves the state machine
                    "VoidAsync_AfterAwait",

                    // The state machine notifies the builder that we're done
                    "SetResult",
                },
                AsyncVoidMethodBuilder.Inspector.InvokedMembers);
        }

        [Test]
        public void RunAsyncVoidThatFails()
        {
            AsyncVoidMethodBuilder.Inspector.Clear();
            VoidAsyncThatThrows();
            Thread.Sleep(42);
            AsyncVoidMethodBuilder.Inspector.Print();

            Assert.AreEqual(
                new string[]
                {
                    ".ctor", // AsyncVoidMethodBuilder.ctor
                    "Start", // AsyncVoidMethodBuilder.Start
                    // Here the method builder calls the MoveNext on the state machine
                    "VoidAsync_Start",
                    
                    // The state machine calls SetException on the builder instance
                    "SetException",
                },
                AsyncVoidMethodBuilder.Inspector.InvokedMembers);
        }

        [Test]
        public void RunAsyncVoidWithCancellation()
        {
            AsyncVoidMethodBuilder.Inspector.Clear();
            VoidAsyncWithCancellation();
            Thread.Sleep(42);
            AsyncVoidMethodBuilder.Inspector.Print();

            Assert.AreEqual(
                new string[]
                {
                    ".ctor", // AsyncVoidMethodBuilder.ctor
                    "Start", // AsyncVoidMethodBuilder.Start
                    // Here the method builder calls the MoveNext on the state machine
                    "VoidAsync_Start",
                    
                    // The state machine calls SetException on the builder instance with TaskCanceledException
                    "SetException",
                },
                AsyncVoidMethodBuilder.Inspector.InvokedMembers);

            Assert.IsInstanceOf<TaskCanceledException>(AsyncVoidMethodBuilder.LastInstance.Exception);
        }

        public async void VoidAsync()
        {
            AsyncVoidMethodBuilder.Inspector.Record("VoidAsync_Start");
        }

        public async void VoidAsyncWithAwait()
        {
            AsyncVoidMethodBuilder.Inspector.Record("VoidAsync_Start");
            await Task.Yield();
            AsyncVoidMethodBuilder.Inspector.Record("VoidAsync_AfterAwait");
        }

        public async void VoidAsyncThatThrows()
        {
            AsyncVoidMethodBuilder.Inspector.Record("VoidAsync_Start");
            throw new InvalidOperationException();
        }

        public async void VoidAsyncWithCancellation()
        {
            AsyncVoidMethodBuilder.Inspector.Record("VoidAsync_Start");

            var cts = new CancellationTokenSource();
            cts.Cancel();

            await Task.Delay(42, cts.Token);
        }
    }
}
