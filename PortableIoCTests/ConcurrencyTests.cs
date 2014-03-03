using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using PortableIoC;
using PortableIoCTests.TestHelpers;
using NUnit.Framework;

namespace PortableIoCTests
{
    /// <summary>
    /// These tests check for high-concurrency situations and are obviously more valid when run on processors 
    /// with multiple CPUs 
    /// </summary>
    /// <remarks>
    /// Will run more slowly in the debugger but will allow you to see the results of various concurrent operations
    /// </remarks>
    [TestFixture]
    public class ConcurrencyTests
    {
        private IPortableIoC _target;

        private Task ResolveSomething()
        {
            return Task.Run(() =>
                                {
                                    IBar instance;
                                    var result = _target.TryResolve(out instance);
                                    if (result)
                                    {
                                        Debug.WriteLine(Thread.CurrentThread.ManagedThreadId + " Shared: " + instance.UniqueIdentifier);
                                    }
                                    else
                                    {
                                        Debug.WriteLine(Thread.CurrentThread.ManagedThreadId + " Not available.");
                                    }
                                });
        }

        private Task ResolveSomethingNew()
        {
            return Task.Run(() =>
                                {
                                    IBar instance;
                                    var result = _target.TryResolve(out instance, true);
                                    if (result)
                                    {
                                        Debug.WriteLine(Thread.CurrentThread.ManagedThreadId + " New: " + instance.UniqueIdentifier);
                                    }
                                    else
                                    {
                                        Debug.WriteLine(Thread.CurrentThread.ManagedThreadId + " Not available.");
                                    }
                                });
        }

        private Task DestroySomething()
        {
            return Task.Run(() => Debug.WriteLine(Thread.CurrentThread.ManagedThreadId + " Destroyed: " + _target.Destroy<IBar>()));
        }

        private Task UnregisterSomething()
        {
            return Task.Run(() =>
                                {
                                    var actual = _target.Unregister<IBar>();
                                    Debug.WriteLine(Thread.CurrentThread.ManagedThreadId + " Unregistered: " + actual);
                                    try
                                    {
                                        _target.Register<IBar>(ioc => new SimpleBar());
                                        Debug.WriteLine(Thread.CurrentThread.ManagedThreadId + " Re-registered.");
                                    }
                                    catch
                                    {
                                        Debug.WriteLine(Thread.CurrentThread.ManagedThreadId + " Already re-registered.");
                                    }
                                });
        }  

        [SetUp]
        public void TestInitialize()
        {
            _target = new PortableIoc();
        }

		// TODO there should actually be assertions here, or test for errors
        [Test]
        public void GivenThereAreMultipleThreadsRunningWhenDestroyAndResolveAreCalledThenTheResultShouldBeThreadSafeOperation()
        {
            _target.Register<IBar>(ioc => new SimpleBar());
            var tasks = new List<Task>();
            var random = new Random();
            for(var x = 0; x < 10000; x++)
            {
                var randomValue = random.Next(100);
                if (randomValue < 50)
                {
                    tasks.Add(ResolveSomething());
                }
                else if (randomValue < 75)
                {
                    tasks.Add(ResolveSomethingNew());
                }
                else if (randomValue < 95)
                {
                    tasks.Add(DestroySomething());
                }
                else
                {
                    tasks.Add(UnregisterSomething());
                }                
            }
			Task.WhenAll(tasks).Wait ();
        }
    }
}
