using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MAUI1.Models;

namespace MAUI1.Services
{
    public class ApiService{
        private readonly string _baseUrl = "https://localhost:7162/api/plato";
        public async Task<List<Plato>> GetPlatosAsync(){
            var request = (HttpWebRequest)WebRequest.Create(_baseUrl);
            request.Method = "GET";
            using (var response = (HttpWebResponse)await request.GetResponseAsync())
            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                var jsonResponse = await reader.ReadToEndAsync();
                return JsonSerializer.Deserialize<List<Plato>>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
        }

        public async Task AddPlatoAsync(Plato plato){
            var json = JsonSerializer.Serialize(plato);
            var request = (HttpWebRequest)WebRequest.Create(_baseUrl);
            request.Method = "POST";
            request.ContentType = "application/json";
            using (var streamWriter = new StreamWriter(await request.GetRequestStreamAsync())){
                await streamWriter.WriteAsync(json);
            }
            using (var response = (HttpWebResponse)await request.GetResponseAsync()){
                if (response.StatusCode != HttpStatusCode.Created){
                    throw new WebException("Error al agregar el plato");
                }
            }
        }

        public async Task UpdatePlatoAsync(Plato plato){
            var json = JsonSerializer.Serialize(plato);
            var request = (HttpWebRequest)WebRequest.Create($"{_baseUrl}/{plato.Id}");
            request.Method = "PUT";
            request.ContentType = "application/json";
            using (var streamWriter = new StreamWriter(await request.GetRequestStreamAsync())){
                await streamWriter.WriteAsync(json);
            }
            using (var response = (HttpWebResponse)await request.GetResponseAsync()){
                if (response.StatusCode != HttpStatusCode.NoContent){
                    throw new WebException("Error al actualizar el plato");
                }
            }
        }

        public async Task<bool> DeletePlatoAsync(int id){
            var request = (HttpWebRequest)WebRequest.Create($"{_baseUrl}/{id}");
            request.Method = "DELETE";
            try{
                using (var response = (HttpWebResponse)await request.GetResponseAsync()){
                    return response.StatusCode == HttpStatusCode.NoContent; 
                }
            }
            catch (WebException ex){              
                var response = (HttpWebResponse)ex.Response;
                if (response != null){
                    using (var reader = new StreamReader(response.GetResponseStream())){
                        var errorResponse = await reader.ReadToEndAsync();
                        Console.WriteLine($"Error: {errorResponse}");
                    }
                }
                return false; 
            }
        }
    }
}
