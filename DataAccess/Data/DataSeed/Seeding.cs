using DataAccess.Data.Contexts;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DataAccess.Data.DataSeed
{
    public class Seeding
    {
        public static bool SeedData(GymSystemDbContext dbContext)
        {
            try
            {
                var HasPlans = dbContext.Plans.Any();
                var HasCategories = dbContext.Categories.Any();
                if (HasPlans && HasCategories) return false;

                if (!HasPlans)
                {
                    var Plans = LoodDataFromJsonFiles<Plan>("plans.json");
                    if (Plans.Any())
                    {
                        dbContext.Plans.AddRange(Plans);
                    }
                }
                if (!HasCategories)
                {
                    var Categories = LoodDataFromJsonFiles<Category>("categories.json");
                    if (Categories.Any())
                        dbContext.Categories.AddRange(Categories);
                }
                return dbContext.SaveChanges() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
        private static List<T> LoodDataFromJsonFiles<T>(string FileName)
        {
            var FilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", FileName);
            if (!File.Exists(FilePath))
                throw new FileNotFoundException();

            string Data = File.ReadAllText(FilePath);
            var Options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            };

            return JsonSerializer.Deserialize<List<T>>(Data, Options) ?? [];
        }
    }
}
