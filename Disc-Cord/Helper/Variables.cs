using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Disc_Cord.Helper
{
    public static class Variables
    {
        public static int PageSize { get; set; }

        // Static constructor
        static Variables()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            PageSize = configuration.GetValue<int>("PageSize");
        }
    }
}
