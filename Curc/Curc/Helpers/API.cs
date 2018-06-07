using Curc.Extensions;
using Curc.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Curc.Helpers
{
    public class API
    {
        private static API _instance;
        public static API instance {
            get {
                _instance = _instance ?? new API();
                return _instance;
            }
        }

        private HttpClient client;

        private API()
        {
            client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
        }

        public async Task<Tuple<bool, T>> get<T>(string url, string access_token=null)where T:BaseModel
        {
            string requestType = $"GET: {url}\nACCESS_TOKEN: {access_token}\n";
            Debug.WriteLine($"\n\n############### REQUEST STARTED ###############\n{requestType}");

            HttpResponseMessage response;
            try {
                addAccessToken(access_token);
                response = await client.GetAsync(url);
            } catch (Exception ex) {
                Debug.WriteLine(ex.StackTrace);
                return null;
            }
            return await getObjectOfResponse<T>(response, requestType);
        }

        private void addAccessToken(string access_token)
        {
            if (!string.IsNullOrEmpty(access_token)) {
                if (client.DefaultRequestHeaders.Contains("Authorization"))
                    client.DefaultRequestHeaders.Remove("Authorization");
                client.DefaultRequestHeaders.Add("Authorization", access_token);
            }
        }

        private static async Task<Tuple<bool, T>> getObjectOfResponse<T>(HttpResponseMessage response, string requestType) where T : BaseModel
        {
            T modelType = null;
            Tuple<bool, T> result = null;
            string stringResult = string.Empty;
            try {
                stringResult = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                modelType = JsonConvert.DeserializeObject<T>(stringResult);
                result = new Tuple<bool, T>(response.IsSuccessStatusCode, modelType);
                Debug.WriteLine("\n\n############### REQUEST ENDED ###############\n" +
                                $"{requestType}" +
                                $"IsSuccessStatusCode: {result.Item1}\n" +
                                $"ModelType: {modelType}\n" +
                                $"Response in JSON: {stringResult?.toJObject()}\n" +
                                "#############################\n\n\n");
            } catch (Exception ex) {
                Debug.WriteLine(ex.StackTrace);
            }
            response.Dispose();
            return result;
        }
    }
}
