using AspNetCoreGraph.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreGraph.Services
{
    //writing db connection
    public class AServiceService : IAServiceService
    {
        public AServiceService()
        {

        }
        public Task<List<AService>> getAllServices()
        {
            throw new NotImplementedException();
        }
    }
    public interface IAServiceService
    {
        Task<List<AService>> getAllServices();
    }
}
