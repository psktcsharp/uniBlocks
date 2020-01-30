using GraphQL.Types;
using HotChocolate;
using HotChocolate.Types;
using HotGraphApi.UniBlocks.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotGraphApi.UniBlocks.Data
{
    [ExtendObjectType(Name = "Mutation")]
    public class AllMutations 
    {
        //Services Mutations
        public async Task<string> CreateService(
          AService input,
          [Service]UniBlocksDBContext uniBlocks)
        {
             uniBlocks.Database.EnsureCreated();
           var insertService =  await uniBlocks.CreateService(input);
           return insertService.ServiceName;
        }
        //Subscriptions Mutations
        public int CreateSubscription(
         Subscription input,
         [Service]UniBlocksDBContext uniBlocks)
        {
           // uniBlocks.Database.EnsureDeleted();
            uniBlocks.Database.EnsureCreated();
            input.Balance = new Balance();
            uniBlocks.Add(input.Balance);
            uniBlocks.Add(input);
          //  uniBlocks.Subscriptions.Add(input);
            var insertSubscription = uniBlocks.SaveChangesAsync().Result;
            return insertSubscription;
        }
        //ServiceSubscriptions Mutations
        public class updateSubInput
        {
            public int subId { get; set; }
            public int servId { get; set; }
        }
        public int UpdateSubscription(
         updateSubInput input,
         [Service]UniBlocksDBContext uniBlocks)
        {
            uniBlocks.Database.EnsureCreated();
            //get the sub entity to update
            var subToUpdate = uniBlocks.Subscriptions.Find(input.subId);
            //get the service entity
            var serviceToAdd = uniBlocks.Services.Find(input.servId);
            //add the service by creating a new entry in the joint table AServiceSubscription
            subToUpdate.AServiceSubscriptions.Add(new AServiceSubscription(){ Service = serviceToAdd, Subscription = subToUpdate });
            var insertServSubResult= uniBlocks.SaveChangesAsync().Result;
            return insertServSubResult;
        }
        //Block Mutations
        public class createBlockbInput
        {
            public string BlockName { get; set; }
            public string Location { get; set; }
        }
        public int CreateBlock(
        createBlockbInput input,
        [Service]UniBlocksDBContext uniBlocks)
        {
            uniBlocks.Database.EnsureCreated();
            uniBlocks.Add(new Block() { BlockName = input.BlockName, location = input.BlockName });
            var insertBlockResult = uniBlocks.SaveChangesAsync().Result;
            return insertBlockResult;
        }

        //Block Mutations
        //public class createBlockbInput
        //{
        //    public string BlockName { get; set; }
        //    public string Location { get; set; }
        //}
        public int CreateTransaction(
        ATransaction input,
        [Service]UniBlocksDBContext uniBlocks)
        {
           // uniBlocks.Database.EnsureDeleted();
            uniBlocks.Database.EnsureCreated();
            uniBlocks.Add(input);
            var insertBlockResult = uniBlocks.SaveChangesAsync().Result;
            return insertBlockResult;
        }

    }  
}
