using System.Collections.Generic;
using Newtonsoft.Json;
using API.Models;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;

namespace API.Repositories
{
    public class PayStationRepository : IPayStationRepo
    {
        private IHostingEnvironment _environment;

        private readonly List<PaymentStationRecord> stations = new List<PaymentStationRecord>();

        public PayStationRepository(IHostingEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<List<PaymentStationRecord>> Retrieve()
        {
            var filePath = _environment.ContentRootPath;    // (@"~//paystations.json");
            var file = System.IO.Path.Combine(filePath, "\\Repositories\\paystations.json");

            var json = System.IO.File.ReadAllText(filePath + "\\Repositories\\paystations.json");

            var stations = JsonConvert.DeserializeObject<List<PaymentStationRecord>>(json);

            return stations;
        }
    }
}