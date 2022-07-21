using System.Web;
using Newtonsoft.Json;

public class FeeUtils
    {
        public async Task<double> get_current_fee()
            {
                string API_KEY = Environment.GetEnvironmentVariable("API_KEY");
                // Console.WriteLine(API_KEY);
                string EXCHANGE_API_KEY = API_KEY;
                var builder = new UriBuilder("http://api.exchangeratesapi.io/v1/latest");
                var query = HttpUtility.ParseQueryString(builder.Query);
                query["access_key"] = EXCHANGE_API_KEY;
                //query["base"] = "USD"; // not supported on free plan (default EUR)
                query["symbols"] = "BRL";
                query["format"] = "1";
                builder.Query = query.ToString();
                string url = builder.ToString();

                var httpClient = HttpClientFactory.Create();
                HttpResponseMessage resp = await httpClient.GetAsync(url);

                if (resp.StatusCode == System.Net.HttpStatusCode.OK) {
                    var content = resp.Content;
                    var data = await content.ReadAsAsync<Dictionary<String,Object>>();
                    var rates = JsonConvert.SerializeObject(data["rates"]);
                    var rates_dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(rates);
                    if (rates_dict != null) {
                        return double.Parse(rates_dict["BRL"], System.Globalization.CultureInfo.InvariantCulture);
                    }
                }
                return 4.7776;
            }
        public static double get_fixed_fee()
            {
                return 4.7776;
            }
    }
// }