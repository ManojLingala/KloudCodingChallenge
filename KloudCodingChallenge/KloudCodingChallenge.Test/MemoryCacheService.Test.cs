using KloudCodingChallenge.Model.Interfaces;
using KloudCodingChallenge.Model.Entities;
using KloudCodingChallenge.Service.Interfaces;
using KloudCodingChallenge.Service.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Xunit;

namespace KloudCodingChallenge.Test
{
    public class MemoryCacheServiceTest
    {
        IOptions<ServiceConfig> serviceConfig;
        Mock<ILogger<DataService>> mockLogger;
        IMemoryCache cache;
        public MemoryCacheServiceTest()
        {
            cache = new MemoryCache(Options.Create<MemoryCacheOptions>(new MemoryCacheOptions()));

            serviceConfig = Options.Create(new ServiceConfig() { EnableCache = true, RootAPIUrl = "http://localhost", DataCacheTimeout=3 });
            mockLogger = new Mock<ILogger<DataService>>();
        }

        [Fact]
        public void MemoryCacheService_ShouldWriteCache()
        {
            ICacheService service = new MemoryCacheService(cache, serviceConfig);
            List<IData> data = new List<IData>()
            {
                new Data {BrandName = "Sample", Color="Black", OwnerName="David Jonh"}
            };
            var cachedValue = cache.Get<List<IData>>(MemoryCacheService.DATA_CACHE_NAME);
            Assert.Null(cachedValue);

            service.CacheServiceData(data);
            cachedValue = cache.Get<List<IData>>(MemoryCacheService.DATA_CACHE_NAME);

            Assert.NotNull(cachedValue);
        }

        [Fact]
        public void MemoryCacheService_ShouldReturnDataFromCache()
        {
            ICacheService service = new MemoryCacheService(cache, serviceConfig);
            List<IData> data = new List<IData>()
            {
                new Data {BrandName = "Sample", Color="Black", OwnerName="David Jonh"}
            };

            service.CacheServiceData(data);
            var cachedValue = service.GetServiceData();
            Assert.NotNull(cachedValue);
            Assert.Single(cachedValue);
            Assert.Equal(data, cachedValue);
            
        }

        [Fact]
        public void MemoryCacheService_ShouldReturnNull_AfterTimeout()
        {
            ICacheService service = new MemoryCacheService(cache, serviceConfig);
            List<IData> data = new List<IData>()
            {
                new Data {BrandName = "Sample", Color="Black", OwnerName="David Jonh"}
            };
            
            service.CacheServiceData(data);
            var cachedValue = service.GetServiceData();
            Assert.NotNull(cachedValue);

            Thread.Sleep(serviceConfig.Value.DataCacheTimeout * 1000 + 1);
            cachedValue = service.GetServiceData();
            Assert.Null(cachedValue);
        }

    }
}
