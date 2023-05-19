using Disc_Cord.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Disc_Cord.DataManager
{
    public class DataManager
    {
        //private readonly ApplicationDbContext _context;
        //public DataManager(ApplicationDbContext context)
        //{
        //    _context = context;
        //}

        public DataManager()
        {

        }

        //public List<Models.Forum> Forums { get; set; }

        //public async Task<List<Models.Forum>> GetAllForumsAsync()
        //{
        //    Forums = await _context.Forum.ToListAsync();

        //    return Forums;
        //}

        //----------------------------------- API CALL -------------------------------------------//
        // ----------------- HERE YOU NEED TO SET YOUR OWN LOCALHOST IN THE URI!

        private static Uri BaseAddress = new Uri("https://localhost:44327/"); // Daniels
        //private static Uri BaseAddress = new Uri("https://localhost:44327/");  // Robins
        //private static Uri BaseAddress = new Uri("https://localhost:44327/");  // Crippa



        //---------------- GET ALL FORUMS
        public static async Task<List<Models.Forum>> GetAllForums()
        {
            List<Models.Forum> forums = new List<Models.Forum>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = BaseAddress;
                HttpResponseMessage response = await client.GetAsync("api/Forums");
                if (response.IsSuccessStatusCode)
                {
                    string responseString = await response.Content.ReadAsStringAsync();
                    forums = JsonSerializer.Deserialize<List<Models.Forum>>(responseString);
                }
                return forums;
            }
        }


        // ------------------------- GET A SPECIFIK FORUM
        public static async Task<Models.Forum> GetForumById(int Id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = BaseAddress;
                HttpResponseMessage response = await client.GetAsync($"api/Forums/{Id}");
                if (response.IsSuccessStatusCode)
                {
                    string responseString = await response.Content.ReadAsStringAsync();
                    Models.Forum forum = JsonSerializer.Deserialize<Models.Forum>(responseString);
                    return forum;
                }
            }

            return null;
        }

        // ---------------- DELETE A FORUM

        public async Task<bool> DeleteForum(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = BaseAddress;
                HttpResponseMessage response = await client.DeleteAsync($"api/Forums/{id}");
                if (response.IsSuccessStatusCode)
                {
                    return true; 
                }
                else
                {
                    return false;
                }
            }
        }

        // ----------------------- UPDATE A FORUM

        public async Task<bool> UpdateForum(Models.Forum forum)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = BaseAddress;
                HttpResponseMessage response = await client.PutAsJsonAsync($"api/Forums/{forum.Id}", forum);
                return response.IsSuccessStatusCode;
            }
        }

        // ----------------- CREATE A FORUM

        public async Task<bool> CreateForum(Models.Forum forum)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = BaseAddress;
                HttpResponseMessage response = await client.PostAsJsonAsync("api/Forums", forum);
                return response.IsSuccessStatusCode;
            }
        }

    }
}
