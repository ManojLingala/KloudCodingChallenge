using System;
using System.Net.Http;
using KloudCodingChallenge.Model.Entities;
using KloudCodingChallenge.Service.Interfaces;
using KloudCodingChallenge.Service.Services;
using Microsoft.Extensions.Options;
using Xunit;
using Moq;
using RichardSzalay.MockHttp;
using System.Net;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Internal;
using KloudCodingChallenge.Model.Interfaces;
using System.Collections.Generic;

namespace KloudCodingChallenge.Test
{
    public class DataServiceTest
    {
        IOptions<ServiceConfig> serviceConfig;
        Mock<ICacheService> mockCacheService;
        Mock<ILogger<DataService>> mockLogger;

        public DataServiceTest()
        {
            serviceConfig = Options.Create(new ServiceConfig() { EnableCache = true, RootAPIUrl = "http://localhost" });
            mockCacheService = new Mock<ICacheService>();
            mockLogger = new Mock<ILogger<DataService>>();
        }
        private HttpClient MockHttpClient(string successJson, HttpStatusCode errorCode = HttpStatusCode.OK, Action<MockHttpMessageHandler> handle = null)
        {
            var mockHttp = new MockHttpMessageHandler();
            var request = mockHttp.When("http://localhost/api/cars");

            if (!string.IsNullOrEmpty(successJson))
            {
                request.Respond("application/json", successJson);
            }
            else
            {
                if (errorCode > 0)
                {
                    request.Respond(errorCode, new StringContent(""));
                }
                else
                    handle?.Invoke(mockHttp);
            }
            var client = mockHttp.ToHttpClient();
            client.BaseAddress = new Uri("http://localhost");
            return client;
        }

        [Fact]
        public void FetchData_ShouldNotThowException_WhenServerError()
        {
            var httpClient = MockHttpClient(null, HttpStatusCode.InternalServerError);

            IDataService service = new DataService(httpClient, serviceConfig, mockCacheService.Object, mockLogger.Object);

            var results = service.FetchData();

            Assert.Empty(results);

        }

        [Theory]
        [InlineData(HttpStatusCode.Forbidden)] //4xxx
        [InlineData(HttpStatusCode.InternalServerError)] //
        public void FetchData_ShouldWriteLog_WhenExceptionOccurs(HttpStatusCode httpErrorCode)
        {
            var httpClient = MockHttpClient(null, httpErrorCode);
            Mock<ILogger<DataService>> logger = new Mock<ILogger<DataService>>();

            IDataService service = new DataService(httpClient, serviceConfig, mockCacheService.Object, logger.Object);
            var results = service.FetchData();

            logger.Verify(l => l.Log(LogLevel.Error, 0, It.IsAny<FormattedLogValues>(), It.IsAny<Exception>(),It.IsAny<Func<object, Exception, string>>()));
        }
        [Fact]
        public void FetchData_ShouldReadDataFromCache_WhenCacheHasData()
        {
            var httpClient = MockHttpClient(null, HttpStatusCode.InternalServerError);
            Mock<ICacheService> mock = new Mock<ICacheService>();
            List<IData> data = new List<IData>()
            {
                new Data() {BrandName = "Brand", Color="Red", OwnerName="Daniel"}
            };

            mock.Setup(x => x.GetServiceData()).Returns(data);
            IDataService service = new DataService(httpClient, serviceConfig, mock.Object, mockLogger.Object);
            var results = service.FetchData();
            Assert.Single(results);
            Assert.Equal(data, results);
        }

        [Fact]
        public void FetchData_ShouldUpdateCache_WhenSuccessfulFetchData()
        {
            string json  = "[{\"name\":\"Bradley\",\"cars\":[{\"brand\":\"MG\",\"colour\":\"Blue\"}]},{\"name\":\"Marry\",\"cars\":[{\"brand\":\"Toyota\",\"colour\":\"Blue\"}]},{\"name\":\"Josh\",\"cars\":[{\"brand\":\"BMW\",\"colour\":\"Green\"}]}]";

            var httpClient = MockHttpClient(json);
            Mock<ICacheService> mock = new Mock<ICacheService>();
            
            mock.Setup(x => x.GetServiceData()).Returns(() => null);
            IDataService service = new DataService(httpClient, serviceConfig, mock.Object, mockLogger.Object);
            var results = service.FetchData();
            Assert.Equal(3, results.Count);

            mock.Verify(m => m.CacheServiceData(It.IsAny<List<IData>>()));
        }


        [Fact]
        public void FetchData_ShouldReturn_2Items()
        {
            string json = "[{\"name\":\"Bradley\",\"cars\":[{\"brand\":\"MG\",\"colour\":\"Blue\"}]},{\"name\":\"Marry\",\"cars\":[{\"brand\":\"Toyota\",\"colour\":\"Blue\"}]}]";
            var httpClient = MockHttpClient(json);

            IDataService service = new DataService(httpClient, serviceConfig, mockCacheService.Object, mockLogger.Object);

            Assert.True(serviceConfig.Value.EnableCache);
            var results = service.FetchData();

            Assert.Equal(2, results.Count);

        }


        [Fact]
        public void FetchData_ShouldReturn_ZeroItems_WhenServiceResponseEmpty()
        {

            string json = "[]";
            var httpClient = MockHttpClient(json);
            IDataService service = new DataService(httpClient, serviceConfig, mockCacheService.Object, mockLogger.Object);
            var results = service.FetchData();
            Assert.Empty(results);

        }

        [Fact]
        public void FetchData_ShouldNotCrash_WhenServiceResponseInvalidFormat()
        {
            string json = "[{\"abc\": \"xyz\"}]";
            var httpClient = MockHttpClient(json);
            IDataService service = new DataService(httpClient, serviceConfig, mockCacheService.Object, mockLogger.Object);
            var results = service.FetchData();

            Assert.Empty(results);
        }

        [Fact]
        public void FetchData_ShouldNotCrash_WhenServiceResponseNotJSONValid()
        {
            string json = "random string here.....";
            var httpClient = MockHttpClient(json);
            IDataService service = new DataService(httpClient, serviceConfig, mockCacheService.Object, mockLogger.Object);
            var results = service.FetchData();

            Assert.Empty(results);
        }

    }
}
