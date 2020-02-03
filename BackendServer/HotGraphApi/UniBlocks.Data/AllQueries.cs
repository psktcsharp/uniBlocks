﻿using HotChocolate;
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
        public List<AspNetUser> GetUsersWithSubs(
          [Service]UniBlocksDBContext uniBlocks
          )
        {
            //due to a bug in ef core this will solve loading subs with user 
            //load both lists
            var userList = uniBlocks.AspNetUsers.ToList();
            var subs = uniBlocks.Subscriptions.Include("User").ToList();
            //loop through every use and detache from the context to avoid reader is open bug
            foreach (var user in userList)
            {
                uniBlocks.Entry(user).State = EntityState.Detached;
                //loop through the subs and add to the user.subs list based on matching id
                foreach (var sub in subs)
                {
                    uniBlocks.Entry(sub).State = EntityState.Detached;
                    if (user.Id == sub.UserId)
                    {
                        user.Subscriptions.Add(sub);
                    }
                }
            }
            return userList.ToList();  
        }
        // User Queries
        //--= get all users with messages
        public List<AspNetUser> GetUsersWithMessages(
         [Service]UniBlocksDBContext uniBlocks
         )
        {
            return uniBlocks.AspNetUsers
               .Include(user => user.UserMessages).ThenInclude(userMessage => userMessage.Message).ToList();
        }
            //--= get User by id
            public AspNetUser GetUser(int userId,[Service]UniBlocksDBContext uniBlocks)
        {
            return uniBlocks.AspNetUsers.Find(userId);
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
        public Subscription GetSubscription(int subscriptionId, [Service]UniBlocksDBContext uniBlocks)
        {
            return uniBlocks.Subscriptions.Find(subscriptionId);
        }


    }
}
