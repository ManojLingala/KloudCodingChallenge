using System;
using System.Collections.Generic;
using KloudCodingChallenge.Model.Interfaces;

namespace KloudCodingChallenge.Service.Interfaces
{
    public interface ICacheService
    {
        void CacheServiceData(List<IData> data);
        List<IData> GetServiceData();
    }
}
