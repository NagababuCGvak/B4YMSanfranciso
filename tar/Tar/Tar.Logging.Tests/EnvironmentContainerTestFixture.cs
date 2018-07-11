using System;
using System.Threading;
using Tar.Logging.ObjectContainers;
using NUnit.Framework;

namespace Tar.Logging.Tests
{
    [TestFixture]
    public class EnvironmentContainerTestFixture
    {
        [Test]
        public void OnOneThread()
        {
            var container = new EnvironmentObjectContainer();

            var obj1 = Guid.NewGuid();
            container.Set("test", obj1);
            var obj2 = container.Get("test");

            Assert.That(obj2,Is.Not.Null);
            Assert.That(obj1, Is.EqualTo(obj2));
        }

        [Test]
        public void SameInstanceOnTwoThreads()
        {
            var container = new EnvironmentObjectContainer();
            var obj1 = Guid.NewGuid();
            Console.WriteLine(obj1);
            container.Set("test", obj1);


            object obj2 = null;

            var thread = new Thread(c =>
                                        {
                                            Thread.Sleep(1000);
                                            var tmp = ((IObjectContainer) c).Get("test");
                                            if (tmp != null) obj2 = (Guid) tmp;
                                            Console.WriteLine("Temp: " + obj2);
                                        });
            thread.Start(container);
            thread.Join();

            Assert.That(obj2, Is.Null);
        }

        [Test]
        public void DiffrentInstancesOnTwoThreads()
        {
            var container = new EnvironmentObjectContainer();
            var obj1 = Guid.NewGuid();
            Console.WriteLine(obj1);
            container.Set("test", obj1);

            object obj2 = null;
            var thread = new Thread(()=>
                                        {
                                            Thread.Sleep(1000);
                                            var c = new EnvironmentObjectContainer();
                                            var tmp = ((IObjectContainer)c).Get("test");
                                            if (tmp != null) obj2 = (Guid)tmp;
                                            Console.WriteLine("Temp: " + tmp);
                                        });
            thread.Start();
            thread.Join();

            Assert.That(obj2, Is.Null);
        }
    }
}