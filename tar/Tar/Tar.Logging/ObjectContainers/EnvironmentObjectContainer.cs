using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Tar.Logging.ObjectContainers
{
    public class EnvironmentObjectContainer : IObjectContainer
    {
        private static readonly object LockObject = new object();

        private static readonly IDictionary<object, Dictionary<object, object>> Objects =
            new ConcurrentDictionary<object, Dictionary<object, object>>();
            //<object, Dictionary<object, object>>();

        public EnvironmentObjectContainer(bool log)
        {
            _log = log;
        }

        public EnvironmentObjectContainer()
            : this(false)
        {
        }

        private static object EnvironmentKey
        {
            get
            {
                if (System.Web.HttpContext.Current == null) return Thread.CurrentThread;

                return System.Web.HttpContext.Current.Items["EnvironmentObjectContainer.EnvironmentKey"] ??
                           (System.Web.HttpContext.Current.Items["EnvironmentObjectContainer.EnvironmentKey"] = Guid.NewGuid());
            }
        }

        public object Get(object key)
        {
            var env = GetEnvironmentContainer();
            return env.ContainsKey(key) ? env[key] : null;
        }

        private Dictionary<object, object> GetEnvironmentContainer()
        {
            Dictionary<object, object> environmentContainer;

            if (!Objects.ContainsKey(EnvironmentKey))
            {
                environmentContainer = new Dictionary<object, object>();
                Objects.Add(EnvironmentKey, environmentContainer);

                if (Objects.Keys.OfType<Thread>().Count(t => !t.IsAlive) > 0)
                    RemoveNotAliveThreads();
            }
            else
                environmentContainer = Objects[EnvironmentKey];
            return environmentContainer;
        }

        private int _setCounter;
        private void RemoveNotAliveThreads()
        {
            var threads = Objects.Keys.OfType<Thread>().ToList();

            if (threads.Count > 0)
            {
                var isAliveThreads = threads.Where(t => t.IsAlive).ToList();
                var notAliveThreads = threads.Where(t => !t.IsAlive).ToList();

                var isAliveThreadCount = isAliveThreads.Count;
                var notAliveThreadCount = notAliveThreads.Count;

                WriteToLog(new
                {
                    SetCounter = _setCounter,
                    EnvironmentCount = Objects.Count,
                    Index = 1,
                    IsAliveThreadCount = isAliveThreadCount,
                    NotAliveThreadCount = notAliveThreadCount,
                    Time = DateTime.Now
                });

                var index = 2;
                while (notAliveThreadCount > 0)
                {
                    notAliveThreads.ToList().ForEach(t => Objects.Remove(t));

                    threads = Objects.Keys.OfType<Thread>().ToList();

                    isAliveThreads = threads.Where(t => t.IsAlive).ToList();
                    notAliveThreads = threads.Where(t => !t.IsAlive).ToList();

                    isAliveThreadCount = isAliveThreads.Count();
                    notAliveThreadCount = notAliveThreads.Count();

                    WriteToLog(new
                    {
                        SetCounter = _setCounter,
                        EnvironmentCount = Objects.Count,
                        Index = index++,
                        IsAliveThreadCount = isAliveThreadCount,
                        NotAliveThreadCount = notAliveThreadCount,
                        Time = DateTime.Now
                    }
                        );
                }
            }
        }

        private readonly bool _log;

        private void WriteToLog(object message)
        {
            if (_log && message != null)
                Console.WriteLine(message);
        }

        public void Set(object key, object value)
        {
            lock (LockObject)
            {
                _setCounter++;
                //var env = Objects.ContainsKey(EnvironmentKey)
                //              ? Objects[EnvironmentKey]
                //              : new Dictionary<object, object>();
                var env = GetEnvironmentContainer();
                env[key] = value;
            }
        }
    }
}