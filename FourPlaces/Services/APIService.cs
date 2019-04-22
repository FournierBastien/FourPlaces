using MonkeyCache.SQLite;
using Newtonsoft.Json;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace FourPlaces.Model.Services
{
    public class APIService
    {
        HttpClient httpClient;
        String API_Address;

        public APIService()
        {
            httpClient = new HttpClient();
            API_Address = "https://td-api.julienmialon.com/";
        }


        public async Task<String> GetAPI()
        {
            using (HttpClient client = new HttpClient())
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "https://td-api.julienmialon.com/");

                HttpResponseMessage response = await client.SendAsync(request);

                string result = await response.Content.ReadAsStringAsync();

                return result;
            }
        }

        // requete à l'API pour enregistrer un utilisateur
        public async Task<bool> Register(string email, string password, string first_name, string last_name)
        {
             using (HttpClient client = new HttpClient())
             {
                 try
                 {
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://td-api.julienmialon.com/auth/register");

                    RegisterRequest register = new RegisterRequest(email,first_name,last_name,password);
                    string data = JsonConvert.SerializeObject(register);
                    request.Content = new StringContent(data, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.SendAsync(request);
                    
                    // si la requête est un succès
                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("Code 200 Register");
                        var contentResponse = await response.Content.ReadAsStringAsync();
                        Response<LoginResult> res = JsonConvert.DeserializeObject<Response<LoginResult>>(contentResponse);
                        Barrel.Current.Add(key: "Login", data: res.Data, expireIn: TimeSpan.FromDays(1));

                        // si l'utilisateur est effectivement bien inscrit
                        if (await GetUser())
                        {
                            return true;
                        }
                        else
                        {
                            Console.WriteLine("Erreur GetUser");
                            return false;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Erreur Register");
                        return false;
                    }

                 }
                 catch (HttpRequestException e)
                 {
                    Console.WriteLine(e.Message);
                    return false;
                 }
             }
        }

        // requête pour modifier son mot de passe
        public async Task<bool> EditPassword(string oldPassword, string newPassword)
        {

            try
            {
                httpClient = new HttpClient();
                HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("PATCH"), "https://td-api.julienmialon.com/auth/me/password");
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Barrel.Current.Get<LoginResult>(key: "Login").AccessToken);
                UpdatePasswordRequest pass = new UpdatePasswordRequest(oldPassword, newPassword);

                string data = JsonConvert.SerializeObject(pass);
                request.Content = new StringContent(data, Encoding.UTF8, "application/json");


                var response = await httpClient.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    if (await GetUser())
                    {
                        return true;
                    }
                }
                else
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        bool tryRef = await RefreshToken();
                        if (tryRef)
                        {
                            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Barrel.Current.Get<LoginResult>(key: "Login").AccessToken);
                            response = await httpClient.SendAsync(request);
                            if (response.IsSuccessStatusCode)
                            {
                                if (await GetUser())
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
                return false;
            }
            catch (Exception e)
            {
                return false;
            }
        }


        // requête pour ajouter un lieu
        public async Task<bool> AddPlace(string title, string description, int imageId, double latitude, double longitude)
        {

            try
            {
                httpClient = new HttpClient();
                HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("PATCH"), "https://td-api.julienmialon.com/places");
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Barrel.Current.Get<LoginResult>(key: "Login").AccessToken);
                CreatePlaceRequest place = new CreatePlaceRequest(title, description, imageId, latitude, longitude);

                string data = JsonConvert.SerializeObject(place);
                request.Content = new StringContent(data, Encoding.UTF8, "application/json");


                var response = await httpClient.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    if (await GetUser())
                    {
                        return true;
                    }
                }
                else
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        bool tryRef = await RefreshToken();
                        if (tryRef)
                        {
                            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Barrel.Current.Get<LoginResult>(key: "Login").AccessToken);
                            response = await httpClient.SendAsync(request);
                            if (response.IsSuccessStatusCode)
                            {
                                if (await GetUser())
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
                return false;
            }
            catch (Exception e)
            {
                return false;
            }

        }

        // requêt pour récupérer les Place
        public async Task<PlaceItem> GetPlace(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "https://td-api.julienmialon.com/places/"+id);
                    HttpResponseMessage response = await client.SendAsync(request);
                    var jsonString = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        var contentResponse = await response.Content.ReadAsStringAsync();
                        Response<PlaceItem> res = JsonConvert.DeserializeObject<Response<PlaceItem>>(contentResponse);                        Barrel.Current.Add(key: "Place", data: res.Data, expireIn: TimeSpan.FromDays(1));
                        return res.Data;
                    }
                    else
                    {
                        Console.WriteLine("Code error");
                        return null;
                    }

                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine(e.Message);
                    return null;
                }
            }
            
        }


        // On modifie les informations de l'utilisateur
        public async Task<bool> EditUser(string firstName, string lastName, int? imageId)
        {

            try
            {
                httpClient = new HttpClient();
                HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("PATCH"), "https://td-api.julienmialon.com/me");
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Barrel.Current.Get<LoginResult>(key: "Login").AccessToken);
                UpdateProfileRequest temp = new UpdateProfileRequest(firstName, lastName, imageId);

                string data = JsonConvert.SerializeObject(temp);
                request.Content = new StringContent(data, Encoding.UTF8, "application/json");


                var response = await httpClient.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    if (await GetUser())
                    {
                        return true;
                    }
                }
                else
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        bool tryRef = await RefreshToken();
                        if (tryRef)
                        {
                            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Barrel.Current.Get<LoginResult>(key: "Login").AccessToken);
                            response = await httpClient.SendAsync(request);
                            if (response.IsSuccessStatusCode)
                            {
                                if (await GetUser())
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
                return false;
            }
            catch (Exception e)
            {
                return false;
            }
           
        }

        public async Task<PlacesList> GetListPlaces()
        {

            /*if (Barrel.Current.IsExpired("PlacesList"))
            {
                return Barrel.Current.Get<PlacesList>("PlacesList");
            }*/
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "https://td-api.julienmialon.com/places");
                    HttpResponseMessage response = await client.SendAsync(request);
                    var jsonString = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        var contentResponse = await response.Content.ReadAsStringAsync();
                        Response<List<PlaceItemSummary>> res = JsonConvert.DeserializeObject<Response<List<PlaceItemSummary>>>(contentResponse);
                        PlacesList list = new PlacesList(res.Data);
                        Barrel.Current.Add(key: "PlacesList", data: list, expireIn: TimeSpan.FromDays(1));
                        return new PlacesList(res.Data);
                    }
                    else
                    {
                        Console.WriteLine("Code error");
                        return null;
                    }

                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine(e.Message);
                    return null;
                }
            }
        }

        // authentification de l'utilisateur et récupération des Tokens
        public async Task<bool> Authentification(string email, string password)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://td-api.julienmialon.com/auth/login");
                    LoginRequest register = new LoginRequest(email, password);
                    string data = JsonConvert.SerializeObject(register);
                    request.Content = new StringContent(data, Encoding.UTF8, "application/json"); 
                    HttpResponseMessage response = await client.SendAsync(request);

                    if (response.IsSuccessStatusCode)
                    {

                        Console.WriteLine("Connexion réussie code 200");
                        var contentResponse = await response.Content.ReadAsStringAsync();
                        Response<LoginResult> res = JsonConvert.DeserializeObject<Response<LoginResult>>(contentResponse);

                        // on stocke les données de l'Authentification, pour les utilisées dans les autres requêtes
                        Barrel.Current.Add(key: "Login", data: res.Data, expireIn: TimeSpan.FromDays(1));
                        if (await GetUser())
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Erreur connexion");
                        return false;
                    }
                    
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine(e.Message);
                    return false;
                }
            } 
        }

        // Ajout d'un commentaire
        public async Task<bool> SubmitComment(string comment, int place)
        {
            try
            {
                httpClient = new HttpClient();
                HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("PATCH"), "https://td-api.julienmialon.com/places/" + place + "/comments");
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Barrel.Current.Get<LoginResult>(key: "Login").AccessToken);
                CreateCommentRequest commentR= new CreateCommentRequest(comment);

                string data = JsonConvert.SerializeObject(commentR);
                request.Content = new StringContent(data, Encoding.UTF8, "application/json");


                var response = await httpClient.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    if (await GetUser())
                    {
                        return true;
                    }
                }
                else
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        bool tryRef = await RefreshToken();
                        if (tryRef)
                        {
                            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Barrel.Current.Get<LoginResult>(key: "Login").AccessToken);
                            response = await httpClient.SendAsync(request);
                            if (response.IsSuccessStatusCode)
                            {
                                if (await GetUser())
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
                return false;
            }
            catch (Exception e)
            {
                return false;
            }

        }

        // Obtenir les informations de l'utilisateur
        public async Task<bool> GetUser()
        {

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "https://td-api.julienmialon.com/me");
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer",Barrel.Current.Get<LoginResult>("Login").AccessToken);
                    HttpResponseMessage response = await client.SendAsync(request);

                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("GetUser réussi code 200");
                        var contentResponse = await response.Content.ReadAsStringAsync();
                        Response<UserItem> res = JsonConvert.DeserializeObject<Response<UserItem>>(contentResponse);
                        Barrel.Current.Add(key: "User", data: res.Data, expireIn: TimeSpan.FromDays(1));
                        App.SESSION_USER = res.Data;
                        Console.WriteLine("GetUser réussi code 200 objet");
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("Erreur GetUSER");
                        return false;
                    }

                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine(e.Message);
                    return false;
                }
            }
        }

        // on met à jour le token grâce au refresh_token
        public async Task<bool> RefreshToken()
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://td-api.julienmialon.com/auth/refresh");
                    RefreshRequest refreshToken = new RefreshRequest(Barrel.Current.Get<LoginResult>(key: "Login").RefreshToken);
                    string data = JsonConvert.SerializeObject(refreshToken);
                    request.Content = new StringContent(data, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.SendAsync(request);

                    if (response.IsSuccessStatusCode)
                    {
                        var contentResponse = await response.Content.ReadAsStringAsync();
                        Response<LoginResult> res = JsonConvert.DeserializeObject<Response<LoginResult>>(contentResponse);
                        Barrel.Current.Add(key: "Login", data: res.Data, expireIn: TimeSpan.FromDays(1));

                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine(e.Message);
                    return false;
                }
            }
        }


        // récupération d'une image
        public async Task<bool> GetImage(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://td-api.julienmialon.com/images/"+id);
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Barrel.Current.Get<LoginResult>("Login").AccessToken);
                    HttpResponseMessage response = await client.SendAsync(request);

                    if (response.IsSuccessStatusCode)
                    {
                        var contentResponse = await response.Content.ReadAsStringAsync();
                        Response<ImageItem> res = JsonConvert.DeserializeObject<Response<ImageItem>>(contentResponse);
                        

                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine(e.Message);
                    return false;
                }
            }
        }


        public async Task<bool> TestImage(int id)
        {
            try
            {
                HttpClient HttpClient = new HttpClient();
                var response = await HttpClient.GetAsync("https://td-api.julienmialon.com/images/" + id);
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                return false;
            }
            return false;
        }

        public async Task<int?> LoadPicture()
        {
            try
            {
                var uri = new Uri(string.Format("https://td-api.julienmialon.com/images", string.Empty));
                MediaFile file;
                
                file = await App.MEDIA_SERVICE.ChooseImage();
               
                httpClient = new HttpClient();
                byte[] imageData = File.ReadAllBytes(file.Path);
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, uri);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", App.SESSION_LOGIN.AccessToken);

                //request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Barrel.Current.Get<LoginResult>(key: "Login").AccessToken);
                MultipartFormDataContent requestContent = new MultipartFormDataContent();
                var imageContent = new ByteArrayContent(imageData);
                imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
                requestContent.Add(imageContent, "file", "file.jpg");
                request.Content = requestContent;
                HttpResponseMessage response = await httpClient.SendAsync(request);
                string result = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    Response<ImageItem> res = JsonConvert.DeserializeObject<Response<ImageItem>>(result);
                    return res.Data.Id;
                }
                else
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        bool tryRef = await RefreshToken();
                        if (tryRef)
                        {
                            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.SESSION_LOGIN.AccessToken);

                            requestContent = new MultipartFormDataContent();
                            imageContent = new ByteArrayContent(imageData);
                            imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
                            requestContent.Add(imageContent, "file", "file.jpg");
                            request.Content = requestContent;

                            response = await httpClient.SendAsync(request);
                            result = await response.Content.ReadAsStringAsync();
                            if (response.IsSuccessStatusCode)
                            {
                                Response<ImageItem> res = JsonConvert.DeserializeObject<Response<ImageItem>>(result);
                                return res.Data.Id;
                            }
                        }
                    }
                }
                return null;
            }
            catch (Exception e)
            {
                return null;
            }
        }

    }
}
