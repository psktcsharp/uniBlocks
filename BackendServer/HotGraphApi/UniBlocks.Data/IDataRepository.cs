using HotChocolate;
using HotGraphApi.UniBlocks.Data.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;

namespace HotGraphApi.UniBlocks.Data
{
    public interface IDataRepository 
    {
       
        //fake data storage 
        ConcurrentStack<MsgTest> AllMsgs { get; }
        MsgTest AddMsg(MsgTest msgTest);
        IObservable<MsgTest> Msgs();

        //Services 
        IEnumerable<AService> AllServices { get; }
        IEnumerable<AService> GetAllServices([Service]UniBlocksDBContext uniBlocks);
    }

    public class DataRepository : IDataRepository,IDisposable
    {
        //private IServiceScopeFactory Services { get; }
        public UniBlocksDBContext _uniBlocksDBContext;
        public DataRepository([Service]UniBlocksDBContext dbContext) 
        {
            _uniBlocksDBContext = dbContext;
            AllMsgs = new ConcurrentStack<MsgTest>();

            //Services = services;
           

        }
        private readonly ISubject<MsgTest> _msgsStream = new ReplaySubject<MsgTest>(1);
        private readonly ISubject<AService> _srvcStream = new ReplaySubject<AService>(1);
        public ConcurrentStack<MsgTest> AllMsgs { get; }
           
               
        
        

        public IEnumerable<AService> AllServices { get; set; }
             
       

        

        public MsgTest AddMsg(MsgTest msgTest)
        {
            AllMsgs.Push(msgTest);
            return msgTest;
        }

        public IObservable<MsgTest> Msgs()
        {
           return _msgsStream
                .Select(message =>
                {
                    return message;
                })
                .AsObservable();
        }
      

        public IEnumerable<AService> GetAllServices([Service]UniBlocksDBContext dbContext)
        {
          using (var db = dbContext)
            {
                AllServices = dbContext.Services.ToList<AService>();
            }
            return AllServices;
            //return _srvcStream
            //     .Select(aservice =>
            //     {
            //         return aservice;
            //     })
            //     .AsObservable();

        }

        public void AddError(Exception exception)
        {
            _msgsStream.OnError(exception);
        }

        public void Dispose()
        {
            ((IDisposable)_uniBlocksDBContext).Dispose();
        }
    }
}
