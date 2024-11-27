using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Wood_STF.Models.Login;

namespace Wood_STF.Data
{
    public class RestService : IRestService
    {
        HttpClient client;
        JsonSerializerOptions serializerOptions;

        public List<CelularModel> Items { get; private set; }
        public CelularModel Item { get; set; }
        public String URL;

        public RestService()
        {
            client = new HttpClient();
            URL = "https://apiarbitat.azurewebsites.net/api/Celular";
            serializerOptions = new JsonSerializerOptions
            {
                //PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
        }

        public async Task DeleteCelularAsync(string id)
        {
            Uri uri = new Uri(string.Format(URL, id));

            try
            {
                HttpResponseMessage response = await client.DeleteAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine(@"\tTodoItem successfully deleted.");
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }
        }

        public async Task<List<CelularModel>> RefreshDataAsync()
        {
            Items = new List<CelularModel>();

            Uri uri = new Uri(string.Format(URL, string.Empty));
            try
            {
                HttpResponseMessage response = await client.GetAsync(uri).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    Items = JsonSerializer.Deserialize<List<CelularModel>>(content, serializerOptions);
                }
                else { Items = null; }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }

            return Items;
        }

        public async Task SaveCelularAsync(CelularModel item, bool isNewItem)
        {
            Uri uri = new Uri(string.Format(URL, string.Empty));

            try
            {
                string json = JsonSerializer.Serialize<CelularModel>(item, serializerOptions);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = null;
                if (isNewItem)
                {
                    response = await client.PostAsync(uri, content).ConfigureAwait(false);
                    if (response.IsSuccessStatusCode)
                    {
                        //App.Current.Properties["IDCEL"] = item.IPCell;
                    }
                }
                else
                {
                    response = await client.PutAsync(uri, content);
                }

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine(@"\tTodoItem successfully saved.");
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }
        }
    }
}
