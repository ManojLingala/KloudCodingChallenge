using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KloudCodingChallenge.Model.Interfaces;

namespace KloudCodingChallenge.Service.Interfaces
{
    public interface IDataService
    {
        List<IData> FetchData();
        Task<List<IData>> FetchDataAsync();
    }
}
