using System;
namespace KloudCodingChallenge.Model.Entities
{
    public class ServiceConfig
    {
		public string RootAPIUrl { get; set; }
        public int DataCacheTimeout { get; set; }
        public bool EnableCache { get; set; }

		public ServiceConfig()
        {
        }
    }
}
