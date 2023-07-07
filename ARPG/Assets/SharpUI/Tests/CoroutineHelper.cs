using System;
using System.Collections;

namespace SharpUI.Tests
{
    public static class CoroutineHelper
    {
        private class CoroutineTimeoutException : Exception
        {
            public CoroutineTimeoutException(string message) : base(message) { }
        }
 
        public static void RunSynchronously(IEnumerator coroutine, int maxNumberOfCalls = 100)
        {
            while (coroutine.MoveNext())
            {
                maxNumberOfCalls--;
                if (maxNumberOfCalls < 0)
                    throw new CoroutineTimeoutException("Coroutine reached maximum number of calls: " + maxNumberOfCalls);
            }
        }
    }
}