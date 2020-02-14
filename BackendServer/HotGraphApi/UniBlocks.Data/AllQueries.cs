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
        //--= get all users with subs
        public List<User> GetUsersWithSubs(
          [Service]UniBlocksDBContext uniBlocks
          )
        {
            //due to a bug in ef core this will solve loading subs with user 
            //load both lists
            var userList = uniBlocks.Users.ToList();
            var subs = uniBlocks.Subscriptions.Include("User").ToList();
            //loop through every use and detache from the context to avoid reader is open bug
            foreach (var user in userList)
            {
                uniBlocks.Entry(user).State = EntityState.Detached;
                //loop through the subs and add to the user.subs list based on matching id
                foreach (var sub in subs)
                {
                    uniBlocks.Entry(sub).State = EntityState.Detached;
                    if (user.UserId == sub.UserId)
                    {
                        user.Subscriptions.Add(sub);
                    }
                }
            }
            return userList.ToList();  
        }
        // User Queries
        //--= get all users with messages
        public List<User> GetUsersWithMessages(
         [Service]UniBlocksDBContext uniBlocks
         )
        {
            return uniBlocks.Users
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
            var blockList = uniBlocks.Blocks.Include(b => b.BlockSubscriptions).ThenInclude(bs => bs.Subscription).ThenInclude(sub => sub.User)
                .ToList();
            //get user info for each sub to attach to the list

            return blockList;
        }
        //--= get block by id with it's subscriptions
        public Block GetBlock(int blockId, [Service]UniBlocksDBContext uniBlocks)
        {

            return uniBlocks.Blocks.Include(b => b.BlockSubscriptions).ThenInclude(bs => bs.Subscription).ThenInclude(sub => sub.User).
                Where(b => b.BlockId == blockId).First();
        }
        //--= get block by id with it's services
        public Block GetBlockWithServices(int blockId, [Service]UniBlocksDBContext uniBlocks)
        {

            return uniBlocks.Blocks.Include(b => b.BlockSubscriptions).ThenInclude(bs => bs.Subscription).ThenInclude(sub => sub.AServiceSubscriptions).
                ThenInclude(ss => ss.Service).
                Where(b => b.BlockId == blockId).First();
        }
        //--= get block by name with it's subscriptions
        public Block GetBlockByName(string blockName, [Service]UniBlocksDBContext uniBlocks)
        {

            return uniBlocks.Blocks.Include(b => b.BlockSubscriptions).ThenInclude(bs => bs.Subscription).ThenInclude(sub => sub.User).
                Where(b => b.BlockName == blockName).First();
        }
        //--= get block by id with it's services
        public Block GetBlockServices(int blockId, [Service]UniBlocksDBContext uniBlocks)
        {

            return uniBlocks.Blocks.Where(b => b.BlockId == blockId).
                Include(b => b.BlockSubscriptions)
                .ThenInclude(bs => bs.Subscription)
                .ThenInclude(sub => sub.AServiceSubscriptions)
                .ThenInclude(ss => ss.Service)
                .AsQueryable().First();
               
                
        }
        //--= get all blocks managed by an admin
        public List<Block> GetBlocksForAdmin(int adminUserId,[Service]UniBlocksDBContext uniBlocks)
        {
          
            var blocks = new List<Block>();
            var buListToCheck = uniBlocks.BlockUsers.Where(bu => bu.UserId == adminUserId).ToList();
            var blocksdb = uniBlocks.BlockUsers.Include(bu => bu.Block)
                .ThenInclude(b => b.BlockSubscriptions)
                .ThenInclude(bu => bu.Subscription)
                .ThenInclude(sub => sub.User)
                .Select(selector => selector.Block)
                .AsQueryable().ToList();

            foreach (var item in blocksdb)
            {
                foreach (var bulistitem in buListToCheck)
                {
                    if(item.BlockId == bulistitem.BlockId)
                    {
                        blocks.Add(item);
                    }
                }
            }

            return blocks.Distinct().ToList();


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
        //-- get service by name
        public AService GetServiceByName(string aServiceName, [Service]UniBlocksDBContext uniBlocks)
        {
            return uniBlocks.Services.Where(serv => serv.ServiceName == aServiceName).First();
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
        public Subscription GetSubscription(int subscriptionId, [Service]UniBlocksDBContext uniBlocks)
        {
            return uniBlocks.Subscriptions.Find(subscriptionId);
        }
        //-- get subscription by name
        public Subscription GetSubscriptionByName(string subscriptionName, [Service]UniBlocksDBContext uniBlocks)
        {
            return uniBlocks.Subscriptions.Where(sub => sub.SubscriptionName == subscriptionName).First();
        }
        // invoice Queries
        //- get all invoices with transaction
        public List<Invoice> GetATransactions(
            [Service]UniBlocksDBContext uniBlocks)
        {
            return uniBlocks.Invoices
                .Include(inv => inv.AServiceSubscription)
                .Include(inv => inv.Transaction)
                .ToList();
                
               
        }
    }
}
