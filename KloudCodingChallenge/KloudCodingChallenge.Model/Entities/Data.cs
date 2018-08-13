using System;
using KloudCodingChallenge.Model.Interfaces;

namespace KloudCodingChallenge.Model.Entities
{
    public class Data :IData
    {
        public Data()
        {
        }
		public string OwnerName { get; set; }
		public string BrandName { get; set; }
		public string Color { get; set; }
    }
}
