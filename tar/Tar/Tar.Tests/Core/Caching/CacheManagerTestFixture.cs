using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Tar.Core;
using Tar.Core.Caching;
using NUnit.Framework;

namespace Tar.Tests.Core.Caching
{
    [TestFixture]
    public class CacheManagerTestFixture
    {
        private IServiceLocator _serviceLocator;
        private ICacheManager _cacheManager;

        #region Key/Value List

        public string StringKey = "StringKey";
        public string StringValue = "StringValue";

        public string GuidKey = "GuidKey";
        public Guid GuidValue = Guid.NewGuid();

        #endregion

        private const string ConfigFilePath = @"Core\Caching\Caching.xml";


        [SetUp]
        public void SetUp()
        {
            ServiceLocator.Initialize(ConfigFilePath);
            _serviceLocator = ServiceLocator.Current;
            _cacheManager = _serviceLocator.Get<ICacheManager>();
            Assert.That(_cacheManager, Is.Not.Null);
        }

        [Test]
        public void AddStringItemToCacheManager()
        {
            _cacheManager.Set(StringKey, StringValue);
        }

        [Test]
        public void AddGuidItemToCacheManager()
        {
            _cacheManager.Set(GuidKey, GuidValue);
        }

        [Test]
        public void AddAndLoadStringItemToCacheManager()
        {
            _cacheManager.Set(StringKey, StringValue);
            var value = _cacheManager.Get(StringKey);
            Assert.That(StringValue, Is.EqualTo(value));
        }

        [Test]
        public void AddAndLoadGuidItemToCacheManager()
        {
            _cacheManager.Set(GuidKey, GuidValue);
            var value = _cacheManager.Get(GuidKey);
            Assert.That(GuidValue, Is.EqualTo(value));
        }

        [Test]
        public void SetTwiceSameKey()
        {
            const string sameKey = "SameKey";
            _cacheManager.Set(sameKey, sameKey);
            _cacheManager.Set(sameKey, sameKey);
        }


        public void TimeSpanForOneSecond()
        {
            const string key = "TimeSpan";
            Thread.Sleep(900);
            Assert.That(_cacheManager.Get(key), Is.Not.Null);

            Thread.Sleep(150);
            Assert.That(_cacheManager.Get(key), Is.Null);
        }

        [Test]
        public void WithCacheManagerBootstrapper()
        {
            _cacheManager.Set("30saniye", "test", new TimeSpanCacheValidator(TimeSpan.FromSeconds(30)));
            _cacheManager.Set("75saniye", "test", new TimeSpanCacheValidator(TimeSpan.FromSeconds(75)));
            _cacheManager.Set("100saniye", "test", new TimeSpanCacheValidator(TimeSpan.FromSeconds(100)));
            _cacheManager.Set("130saniye", "test", new TimeSpanCacheValidator(TimeSpan.FromSeconds(150)));
            var cacheBootStrapper = _serviceLocator.GetAll<IBootStrapper>()
                .FirstOrDefault(x => x.GetType().Name.Contains("ache"));
            var cacheManagerBootStrapper = (CacheManagerBootStrapper) cacheBootStrapper;
            if (cacheManagerBootStrapper != null)
                Debug.WriteLine("PeriodAsMinutes:" + cacheManagerBootStrapper.PeriodAsMinutes);
            Thread.Sleep(TimeSpan.FromMinutes(3));
        }

        [Test]
        public void AnItemWithTheSameKeyHasAlreadyBeenAdded()
        {
            // Exception: An item with the same key has already been added.
            var threads= Enumerable.Range(1, 1000).ToList().Select(
                x => new
                {
                    x,
                    Thread = new Thread(y =>
                        _cacheManager.Set("AnItemWithTheSameKeyHasAlreadyBeenAdded", y))
                }).ToList();
            threads.ForEach(x => x.Thread.Start(x));
            threads.ForEach(x => x.Thread.Join());
        }
    }
}