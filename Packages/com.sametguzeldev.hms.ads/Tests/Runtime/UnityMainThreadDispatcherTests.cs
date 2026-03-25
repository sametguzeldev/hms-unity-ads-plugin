using System;
using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using SametGuzelDev.HMS.Ads;

namespace SametGuzelDev.HMS.Ads.Tests.Runtime
{
    [TestFixture]
    public class UnityMainThreadDispatcherTests
    {
        [Test]
        public void Enqueue_WithNullAction_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => UnityMainThreadDispatcher.Enqueue(null));
        }

        [UnityTest]
        public IEnumerator Enqueue_ExecutesActionOnNextFrame()
        {
            bool executed = false;

            UnityMainThreadDispatcher.Enqueue(() => executed = true);

            Assert.IsFalse(executed, "Action should not execute immediately");

            yield return null;

            Assert.IsTrue(executed, "Action should execute after one frame");
        }

        [UnityTest]
        public IEnumerator Enqueue_MultipleActions_ExecuteInOrder()
        {
            var order = new System.Collections.Generic.List<int>();

            UnityMainThreadDispatcher.Enqueue(() => order.Add(1));
            UnityMainThreadDispatcher.Enqueue(() => order.Add(2));
            UnityMainThreadDispatcher.Enqueue(() => order.Add(3));

            yield return null;

            Assert.AreEqual(3, order.Count);
            Assert.AreEqual(1, order[0]);
            Assert.AreEqual(2, order[1]);
            Assert.AreEqual(3, order[2]);
        }

        [UnityTest]
        public IEnumerator Enqueue_ActionThatThrows_DoesNotBreakSubsequentActions()
        {
            bool secondExecuted = false;

            UnityMainThreadDispatcher.Enqueue(() => throw new Exception("test exception"));
            UnityMainThreadDispatcher.Enqueue(() => secondExecuted = true);

            LogAssert.Expect(LogType.Exception, "Exception: test exception");

            yield return null;

            Assert.IsTrue(secondExecuted, "Second action should still execute after first throws");
        }
    }
}
