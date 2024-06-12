using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Product.Data.Contexts;

namespace Product.Data.Util
{
    public static class InitDB
    {
        public static void RunMigration(ProductContext context, IConfiguration configuration)
        {
            bool runMigrations = Convert.ToBoolean(configuration["RunMigrations"]);
            if (runMigrations is true)
            {
                Console.WriteLine("--> Attempting to apply migrations...");
                try
                {
                    Console.WriteLine(context.Database.GetDbConnection().ConnectionString);

                    if (context.Database.GetPendingMigrations().Any())
                    {
                        context.Database.Migrate();
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not run migrations: {ex.Message}");
                }
            }
        }
    }
}
