using System;
using System.Collections.Generic;
using KloudCodingChallenge.Model.Interfaces;
using KloudCodingChallenge.Model.Entities;
using KloudCodingChallenge.Service.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace KloudCodingChallenge.Service.Services
{
    
    public class MemoryCacheService : ICacheService
    {
        // This is just an example to centralize all cache inside 1 single place, IMemory can be injected inside Controller and use directly from there.
        public const string DATA_CACHE_NAME = "DATA_CACHE_NAME";

		private IMemoryCache cache;
        readonly ServiceConfig config;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:Hiring.Kloud.CodeChallenge.Service.Services.MemoryCacheService"/> class.
        /// </summary>
        /// <param name="cache">Cache.</param>
        /// <param name="serviceConfig">This will auto inject from appconfig.json.</param>
		public MemoryCacheService(IMemoryCache cache, IOptions<ServiceConfig> serviceConfig)
        {
            this.cache = cache;
            this.config = serviceConfig.Value;
        }

        /// <summary>
        /// Caches the service data. 
        /// The catch service will be able to config in appconfigs.json
        /// In the real world project, This function will be implement as generic method to avoid duplicate code
        /// </summary>
        /// <param name="data">Data.</param>
        public void CacheServiceData(List<IData> data)
        {
            if (!this.config.EnableCache) return;

            this.cache.Set(DATA_CACHE_NAME, data, TimeSpan.FromSeconds(this.config.DataCacheTimeout));
        }
		/// <summary>
		/// Gets the service data form cache , the cache duration is base on service config in appconfig.json. DataCacheTimeout is in seconds
		/// In the real world project, This function will be implement as generic method to avoid duplicate code
		/// </summary>
		/// <returns>The service data.</returns>
		public List<IData> GetServiceData()
        {
            return this.cache.Get<List<IData>>(DATA_CACHE_NAME);
        }
    }
}
