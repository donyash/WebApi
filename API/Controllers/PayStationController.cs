using API.Repositories;
using Microsoft.AspNetCore.Mvc;
using API.Models;
using System.Linq;
using System;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using System.Net;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace API.Controllers
{
    [Route("v1/[controller]")]
    [EnableCors("AllowSpecificOrigin")]
    public class PayStationController: Controller
    {
        private readonly IPayStationRepo payStationRepo;

        public PayStationController(IPayStationRepo payStationRepo)
        {
            this.payStationRepo = payStationRepo;
        }

        [HttpGet()]
        [DisableCors]
        //[Authorize]
        [Route("GetAllPaymentStations")]
        public async Task<IActionResult> GetAllPaymentStations()
        {
            try
            {
                var myTask = Task.Run(() => GetAllStations());
                List<PaymentStationRecord> stations = await myTask;

                if (!stations.Any())
                    return new NoContentResult();

                return new OkObjectResult(stations);
            }
            catch (Exception ex)
            {
                //LogException (ex);
                return StatusCode(500);
            }
        }


        [HttpGet()]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]    //THIS IS THE PROBLEM LINE
        [Route("GetPaymentStationById")]
        ///v1/PayStation/GetPaymentStationById?id=0000069
        public async Task<IActionResult> GetPaymentStationById(string id)
        {
            try
            {
                var myTask = Task.Run(() => GetAllStations());
                List<PaymentStationRecord> stations = await myTask;

                var byId = stations
                    .Where(x => x.PayStationId.Equals(id))
                    .Take(1).ToList();

                if (!byId.Any())
                    return new NoContentResult();

               // PaymentStationRecord record = byId.First();
                return new OkObjectResult(byId);
            }
            catch (Exception ex)
            {
                //LogException (ex);
                return StatusCode(500);
            }
        }


        //just a test for CI - will run in stage only then remove.
        [HttpGet()]
        [DisableCors]
        [Route("Testing")]
        public async Task<IActionResult> Testing()
        {
            try
            {
                var myTask = Task.Run(() => GetAllStations());
                List<PaymentStationRecord> stations = await myTask;

                if (!stations.Any())
                    return new NoContentResult();

                return new OkObjectResult(stations);
            }
            catch (Exception ex)
            {
                //LogException (ex);
                return StatusCode(500);
            }
        }



        private async Task<List<PaymentStationRecord>> GetAllStations()
        {
            return await payStationRepo.Retrieve();
        }

    }
}
