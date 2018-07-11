using System;
using System.Collections.Generic;
using System.Threading;

namespace Tar.Core.Scheduling
{
    public static class ScheduleManager
    {
        private static readonly List<Timer> Timers = new List<Timer>();

        public static void Start(TimeSpan period, Action action)
        {
            var thread = new Thread(
                () =>
                {
                    var timer = new Timer(
                        state => action(),
                        null, new TimeSpan(0, 0, 0, 1),
                        period);
                    Timers.Add(timer);
                });
            thread.Start();
        }
    }
}
