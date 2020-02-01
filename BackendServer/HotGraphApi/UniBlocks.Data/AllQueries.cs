using HotChocolate;
using HotChocolate.Types;
using HotGraphApi.UniBlocks.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotGraphApi.UniBlocks.Data
{
    [ExtendObjectType(Name = "Query")]
    public class AllQueries 
    {
        // User Queries
        //--= get all users with messages 
        public List<User> GetUsers(
          [Service]UniBlocksDBContext uniBlocks
          )
        {
            return  uniBlocks.Users
                .Include(user => user.UserMessages).ThenInclude(userMessage => userMessage.Message).ToList();
        }
        //--= get User by id
        public User GetUser(int userId,[Service]UniBlocksDBContext uniBlocks)
        {
            return uniBlocks.Users.Find(userId);
        }
        // Block Queries
        //--= get all blocks 
        public List<Block> GetBlocks(
          [Service]UniBlocksDBContext uniBlocks
          )
        {
            return uniBlocks.Blocks
               .ToList();
        }
        //--= get block by id with it's subscriptions
        public Block GetBlock(int blockId, [Service]UniBlocksDBContext uniBlocks)
        {
            //return uniBlocks.Blocks.Include(b => b.).
            //    Where(b => b.BlockId == blockId).First();
            return new Block();
        }
        // Service Queries
        //--= get all services with the subscriptions
        public List<AService> GetServices(
          [Service]UniBlocksDBContext uniBlocks
          )
        {
            return uniBlocks.Services.Include(s => s.AServiceSubscriptions).ThenInclude(ss => ss.Subscription)
               .ToList();
        }
        //--= get service by id
        public AService GetService(int serviceId, [Service]UniBlocksDBContext uniBlocks)
        {
            return uniBlocks.Services.Include(s => s.AServiceSubscriptions).ThenInclude(ss => ss.Subscription).
                Where(s => s.AServiceId == serviceId).First();
        }
        // Subscriptions Queries
        //--= get all subscriptions with the user
        public List<Subscription> GetSubscription(
          [Service]UniBlocksDBContext uniBlocks
          )
        {
            return uniBlocks.Subscriptions.Include(s => s.User)
               .ToList();
        }
        //--= get subscription by id
        public AService GetService(int serviceId, [Service]UniBlocksDBContext uniBlocks)
        {
            return uniBlocks.Services.Find(serviceId);
        }


    }
}
