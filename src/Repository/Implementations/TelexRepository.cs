using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using Newtonsoft.Json.Linq;
using src.Models;
using src.Models.Response;
using src.Repository.Interface;

namespace src.Repository.Implmentations{
    public class TelexRepository : ITelex
    {
        private readonly IConfiguration _config;
        public TelexRepository(IConfiguration config)
        {
            _config = config;
        }

        private static List<string> _inMemoryDb = new List<string>();
        public async Task<string> GetToken()
        {
            try{
                var clientId = _config["Spotify:clientId"];
                var secret = _config["Spotify:secret"];
                var baseURL = _config["Spotify:tokenURL"];

                var data = new Dictionary<string, string>(){
                    { "grant_type", "client_credentials" },
                    { "client_id", clientId },
                    { "client_secret", secret }
                };

                HttpClient client = new();
                var encodedData = new FormUrlEncodedContent(data);
                var clientResponse = await client.PostAsync(baseURL, encodedData);
                var jsonObject = JsonObject.Parse(await clientResponse.Content.ReadAsStringAsync());
                string responseToken;
                if(jsonObject is  null)
                {
                    return string.Empty;
                }

                responseToken = jsonObject["access_token"]!.ToString();
                _inMemoryDb.Clear();
                _inMemoryDb.Add(responseToken);

                return responseToken;

            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return string.Empty;
            }            
        }

        public TelexConfig GetTelexConfiguration()
        {
            try{
                if(_config.GetSection("TelexIntegration").Key == null)
                return new TelexConfig();

                return _config.GetSection("TelexIntegration").Get<TelexConfig>()!;

            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new TelexConfig();
            }            
        }

        public async Task<TelexResponse> TelexReport()
        {
            var genrateToken = await GetToken();
            if(_inMemoryDb.FirstOrDefault() == null)
            {
                return new TelexResponse();
            }

            try{
                var baseUrl = _config["Spotify:baseUrl"];
                if(string.IsNullOrEmpty(baseUrl))
                return new TelexResponse();

                var response = await GetBarrz(baseUrl, _inMemoryDb.FirstOrDefault()!);
                return response;

            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new TelexResponse();
            }
        }

        public async Task<string> BingTelex()
        {
            var report = await TelexReport();

            if(report is null)
            return "Couldn't retrieve report";
            try{
                var _json = JsonSerializer.Serialize(report);
                var content = new StringContent(_json, Encoding.UTF8, "application/json");
                var client = new HttpClient();

                var clientResponse = await client.PostAsync(_config["TelexIntegration:data:target_url"], content);
                return await clientResponse.Content.ReadAsStringAsync();

            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
        }

        private async Task<TelexResponse> GetBarrz(string baseUrl, string token)
        {
            try{
                var client = new HttpClient();
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                List<ResponseItem> response = new();

                int offset = 0;
                int count = 0;

                while (count < 3)
                {
                    int limit = 50;
                    var clientResponse = await client.GetAsync($"{baseUrl}/browse/new-releases?limit={limit}&offset={offset}"); 
                    var jObj = JObject.Parse(await clientResponse.Content.ReadAsStringAsync());

                    var albumList = jObj["albums"]["items"];
                    var currentDate = DateTime.UtcNow.Date;
                    foreach(var item in albumList)
                    {
                        var itemReleaseDate = DateTime.Parse(item["release_date"].ToString());
                        // if (currentDate.Year == itemReleaseDate.Date.Year)
                        // {
                            var albumEntity = new ResponseItem()
                            {
                                AlbumName = item["name"].ToString(),
                                Artist = item["artists"]
                                    .Select(m => m["name"]?.ToString())
                                    .Where(name => !string.IsNullOrEmpty(name))
                                    .ToList()!,
                                ReleaseDate = item["release_date"].ToString(),
                            };

                            response.Add(albumEntity);
                        // }
                    }   
                    
                    offset += 50;
                    count ++;
                    // Thread.Sleep(1000);
                }

                string formattedString = string.Join("/n", response.Select(item => $"{item.Artist} - {item.AlbumName}"));                  
                string items = string.Join("\n", response.Select(item => 
                        $"{string.Join(", ", item.Artist)} - {item.AlbumName}"
                    ));                

                TelexResponse telexResponse  = new(){
                    message = items,
                    username = "Devteam",
                    event_name = "barrzupdate",
                    status = "success"
                };

                return telexResponse;

            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new TelexResponse();
            }
        }
    }
}