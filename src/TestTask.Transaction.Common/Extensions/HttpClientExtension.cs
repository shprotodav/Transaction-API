using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using TestTask.Transaction.Common.Exceptions;

namespace TestTask.Transaction.Common.Extensions
{
    public static class HttpClientExtension
    {
        private const int _totalNumberOfAttempts = 3;

        public static async Task<HttpResponseMessage> GetAsyncWithRetry(this HttpClient client, string requestUri)
        {
            var numberOfAttempts = 0;
            while (true)
            {
                try
                {
                    return await client.GetAsync(new Uri(requestUri, UriKind.Relative));
                }
                catch (Exception)
                {
                    numberOfAttempts++;
                    if (numberOfAttempts >= _totalNumberOfAttempts)
                    {
                        throw;
                    }
                }
            }
        }

        public static async Task<T> GetData<T>(this HttpClient client, string path, string parameters = "")
        {
            using (var response = await client.GetAsyncWithRetry(path + parameters))
            {
                return await GetDataFromResponse<T>(response);
            }
        }

        public static async Task GetData(this HttpClient client, string path, string parameters = "")
        {
            using (var response = await client.GetAsyncWithRetry(path + parameters))
            {
                if (!response.IsSuccessStatusCode)
                {
                    throw await GetError(response);
                }
            }
        }

        public static async Task<R> PostData<T, R>(this HttpClient client, string path, T data, string parameters = "")
        {
            using (var response = await client.PostAsJsonAsync(new Uri(path + parameters, UriKind.Relative), data))
            {
                return await GetDataFromResponse<R>(response);
            }
        }

        public static async Task PostData<T>(this HttpClient client, string path, T data, string parameters = "")
        {
            using (var response = await client.PostAsJsonAsync(new Uri(path + parameters, UriKind.Relative), data))
            {
                if (!response.IsSuccessStatusCode)
                {
                    throw await GetError(response);
                }
            }
        }

        static async Task<Exception> GetError(HttpResponseMessage response)
        {
            try
            {
                try
                {
                    var exception = await response.Content.ReadAsAsync<Exception>();
                    return new Exception(exception.Message, exception);
                }
                catch (Exception)
                {
                    var exceptionStr = await response.Content.ReadAsStringAsync();
                    var exception = JsonConvert.DeserializeObject<ExceptionResponse>(exceptionStr);
                    return CreateExceptionFromResponce(exception);
                }
            }
            catch (Exception)
            {
                return new Exception("Returned exception which cannot parsed. Uri: " + response.RequestMessage.RequestUri);
            }
        }

        static Exception CreateExceptionFromResponce(ExceptionResponse response)
        {
            var formattedException = new
            {
                response.Message,
                response.ExceptionMessage,
                response.StackTrace,
                InnerException = response.InnerException == null ? null : CreateExceptionFromResponce(response.InnerException)
            };
            return new Exception(JsonConvert.SerializeObject(formattedException));
        }

        static async Task<T> GetDataFromResponse<T>(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<T>();
            }

            throw await GetError(response);
        }
    }
}
