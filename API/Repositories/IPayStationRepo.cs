using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using API.Models;
using System.Threading.Tasks;

namespace API.Repositories
{
    public interface IPayStationRepo
    {
        Task<List<PaymentStationRecord>> Retrieve();
    }
}