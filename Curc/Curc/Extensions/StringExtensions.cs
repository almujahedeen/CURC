using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Curc.Extensions
{
    public static class StringExtensions
    {
        public static async Task<byte[]> toStreamAsync(this string url)
        {
            try {
                using (var client = new HttpClient()) {
                    client.MaxResponseContentBufferSize = 256000;
                    return await client.GetByteArrayAsync(url);
                }
            } catch (Exception ex) {
                Debug.WriteLine(ex.StackTrace);
            }
            return null;
        }

        public static JObject toJObject(this string str)
        {
            return JObject.Parse(str);
        }
    }
}
