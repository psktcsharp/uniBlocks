﻿using GraphQL.Types;
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
        public class createBlockInput
        {
            public string BlockName { get; set; }
            public string Location { get; set; }
        }
        public int CreateBlock(
        createBlockInput input,
        [Service]UniBlocksDBContext uniBlocks)
        {
            uniBlocks.Database.EnsureCreated();
            uniBlocks.Add(new Block() { BlockName = input.BlockName, location = input.BlockName });
            var insertBlockResult = uniBlocks.SaveChangesAsync().Result;
            return insertBlockResult;
        }
        //Message Mutations
        public class createMessageInput
        {
            public createMessageInput()
            {
                ToList = new List<int>();
            }
            public string content { get; set; }
            public int senderId { get; set; }
            public List<int> ToList { get; set; }
        }
        public int CreateMessage(
       createMessageInput input,
       [Service]UniBlocksDBContext uniBlocks)
        {
            //uniBlocks.Database.EnsureDeleted();
            uniBlocks.Database.EnsureCreated();
            //get sender entity based on id
            var sender = uniBlocks.Users.Find(input.senderId);
            //get the list of users that the message will be sent to
            var ToList = new List<User>();
            foreach (var id in input.ToList)
            {
                ToList.Add(uniBlocks.Users.Find(id));
            }
           
            var msgToSend = new Message();
            msgToSend.content = input.content;
            msgToSend.Sender = sender;
            //save message to database to get an id and be able to usermessages into it
            uniBlocks.SaveChangesAsync();

            //save the usermessges
            foreach (var user in ToList)
            {
                msgToSend.UserMessages.Add(new UserMessages() { Message = msgToSend, User = user });
            }
            uniBlocks.Update(msgToSend);
            var insertMsgResult = uniBlocks.SaveChangesAsync().Result;
            return insertMsgResult;
        }
        //Transaction Mutations
        public class createTransactionInput
        {
         
            public int ServiceId { get; set; }
            public int SubscriptionId { get; set; }
            public decimal Amount { get; set; }
            public string TransactionType { get; set; }
        }
        public async Task<int> CreateTransaction(
         createTransactionInput input,
         [Service]UniBlocksDBContext uniBlocks)
        {
            // uniBlocks.Database.EnsureDeleted();
            uniBlocks.Database.EnsureCreated();
            //create transaction
            var transactionToSave = new ATransaction();
            transactionToSave.Amount = input.Amount;
            transactionToSave.TransactionType = input.TransactionType;
            //save to database to get id
            uniBlocks.Add(transactionToSave);
           await uniBlocks.SaveChangesAsync();
            //get AServiceSubscription entity to add while creating the invoice
            var aServiceSubscription = uniBlocks.AServiceSubscriptions.Where(ss => ss.SubscriptionId == input.SubscriptionId & ss.ServiceId == input.ServiceId).FirstOrDefault();
            //create invoice
            var invoiceToSave = new Invoice();
            invoiceToSave.AServiceSubscription = aServiceSubscription;
            invoiceToSave.Transaction = transactionToSave;
            //save to database 
            uniBlocks.Add(invoiceToSave);
            await uniBlocks.SaveChangesAsync();
           
            //effect the balance with the amount ; todo
            //get the subscription
            uniBlocks.Subscriptions.Find(aServiceSubscription.SubscriptionId);
            //get the balance

            var balanceToEffect = uniBlocks.Subscriptions.Include("Balance").Where(sub => sub.SubscriptionId == aServiceSubscription.SubscriptionId).First().Balance;
            balanceToEffect.value += transactionToSave.Amount;
            //save the new balance
            var saveBalanceResult = await uniBlocks.SaveChangesAsync();
            return saveBalanceResult;
        }

        //User Mutations
        public int CreateUser(
             User input,
             [Service]UniBlocksDBContext uniBlocks)
            {
                uniBlocks.Database.EnsureDeleted();
                uniBlocks.Database.EnsureCreated();
                uniBlocks.Add(input);
                var insertUserResult = uniBlocks.SaveChangesAsync().Result;
                return insertUserResult;
            }
    }
}
