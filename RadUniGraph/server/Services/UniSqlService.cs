using Radzen;
using System;
using System.Web;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Components;
using UniBlocksGraph.Data;
using UniBlocksGraph.Models.UniSql;
using UniBlocksGraph.Models;

namespace UniBlocksGraph
{
    public partial class UniSqlService
    {
        private readonly UniSqlContext context;
        private readonly NavigationManager navigationManager;
        private readonly SecurityService securityService;
        //  private readonly AccountController accountController;

        public UniSqlService(UniSqlContext context, NavigationManager navigationManager, SecurityService securityService)
        {
            this.context = context;
            this.navigationManager = navigationManager;
            this.securityService = securityService;
            // this.accountController = accountController;

            // seedDataBase();
        }

        //public async Task<string> seedDataBase()
        //{
        //    try
        //    {
        //        //create users
        //       await accountController.Register("buylibya@gmail.com", "aA@654321");
        //       // await accountController.Register("buylibya@ammar.ly", "aA@654321");
        //        return "Sucess";
        //    }
        //    catch (Exception error)
        //    {

        //        return error.InnerException.Message.ToString();
        //    }
        //}
        public async Task ExportAServiceSubscriptionsToExcel(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/unisql/aservicesubscriptions/excel") : "export/unisql/aservicesubscriptions/excel", true);
        }
        public void ensureCreated()
        {
            context.Database.EnsureCreated();
        }
        public async Task ExportAServiceSubscriptionsToCSV(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/unisql/aservicesubscriptions/csv") : "export/unisql/aservicesubscriptions/csv", true);
        }

        partial void OnAServiceSubscriptionsRead(ref IQueryable<Models.UniSql.AServiceSubscription> items);

        public async Task<IQueryable<Models.UniSql.AServiceSubscription>> GetAServiceSubscriptions(Query query = null)
        {
            var items = context.AServiceSubscriptions.AsQueryable();

            items = items.Include(i => i.Service);

            items = items.Include(i => i.Subscription);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Filter))
                {
                    items = items.Where(query.Filter);
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach (var p in propertiesToExpand)
                    {
                        items = items.Include(p);
                    }
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnAServiceSubscriptionsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnAServiceSubscriptionCreated(Models.UniSql.AServiceSubscription item);

        public async Task<Models.UniSql.AServiceSubscription> CreateAServiceSubscription(Models.UniSql.AServiceSubscription aServiceSubscription)
        {
            OnAServiceSubscriptionCreated(aServiceSubscription);

            context.AServiceSubscriptions.Add(aServiceSubscription);
            context.SaveChanges();

            return aServiceSubscription;
        }
        public async Task ExportBalancesToExcel(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/unisql/balances/excel") : "export/unisql/balances/excel", true);
        }

        public async Task ExportBalancesToCSV(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/unisql/balances/csv") : "export/unisql/balances/csv", true);
        }

  


        public async Task ExportBlocksToExcel(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/unisql/blocks/excel") : "export/unisql/blocks/excel", true);
        }

        public async Task ExportBlocksToCSV(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/unisql/blocks/csv") : "export/unisql/blocks/csv", true);
        }

        partial void OnBlocksRead(ref IQueryable<Models.UniSql.Block> items);

        public async Task<IQueryable<Models.UniSql.Block>> GetBlocks(Query query = null)
        {
            //get logged in use from db
            var loggedInUser = context.Users.Where(u => u.AspNetId == securityService.User.Id).First();
            //temp blocks list for end result
            var temp = new List<Block>();
            //get all blockssubs from database , get all subs via that .. make sure you are only getting subs 
            //with a user id that matches the user id inside the sub
            var deBlocksSubs = context.BlockSubscriptions.Include(bsub => bsub.Subscription).Include(bsub => bsub.Block)
                .Where(bsub => bsub.Subscription.UserId == loggedInUser.UserId).AsQueryable();

            //get blocks if he is an admin only without having a sub
            var blocksByAdmin = context.Blocks.Include(b => b.BlockUsers).AsQueryable();
            //extract all blocks that matches the logged in user id     
            foreach (var block in blocksByAdmin)
            {
                foreach (var blockuser in block.BlockUsers)
                {
                    if (blockuser.UserId == loggedInUser.UserId)
                    {
                        temp.Add(block);
                    }
                }
            }
            //exract all blocks   
            foreach (var bsub in deBlocksSubs)
            {
              
                //Console.WriteLine(subsCount);
                temp.Add(bsub.Block);
            }

            //subs count 
            foreach (var block in temp)
            {
                //subs and service count
                var subsCount = context.BlockSubscriptions.Where(bu => bu.BlockId == block.BlockId).Select(selector => selector.SubscriptionId).Count();
                var serviceCount = context.BlockSubscriptions.Where(b => b.BlockId == block.BlockId)
                   .Include(b => b.Subscription)
                   .ThenInclude(s => s.AServiceSubscriptions)
                   .Count();
         
                block.SubsCount = subsCount;
                block.ServicesCount = serviceCount;
            }

            //clean from dublicated blocks
            var items = temp.Distinct().AsQueryable();
            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Filter))
                {
                    items = items.Where(query.Filter);
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach (var p in propertiesToExpand)
                    {
                        items = items.Include(p);
                    }
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnBlocksRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnBlockCreated(Models.UniSql.Block item);

        public async Task<Models.UniSql.Block> CreateBlock(Models.UniSql.Block block)
        {
            OnBlockCreated(block);

            context.Blocks.Add(block);
            context.SaveChanges();

            return block;
        }
        public async Task ExportBlockSubscriptionsToExcel(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/unisql/blocksubscriptions/excel") : "export/unisql/blocksubscriptions/excel", true);
        }

        public async Task ExportBlockSubscriptionsToCSV(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/unisql/blocksubscriptions/csv") : "export/unisql/blocksubscriptions/csv", true);
        }

        partial void OnBlockSubscriptionsRead(ref IQueryable<Models.UniSql.BlockSubscription> items);

        public async Task<IQueryable<Models.UniSql.BlockSubscription>> GetBlockSubscriptions(Query query = null)
        {
            var items = context.BlockSubscriptions.AsQueryable();

            items = items.Include(i => i.Block);

            items = items.Include(i => i.Subscription);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Filter))
                {
                    items = items.Where(query.Filter);
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach (var p in propertiesToExpand)
                    {
                        items = items.Include(p);
                    }
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnBlockSubscriptionsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnBlockSubscriptionCreated(Models.UniSql.BlockSubscription item);

        public async Task<Models.UniSql.BlockSubscription> CreateBlockSubscription(Models.UniSql.BlockSubscription blockSubscription)
        {
            OnBlockSubscriptionCreated(blockSubscription);

            context.BlockSubscriptions.Add(blockSubscription);
            context.SaveChanges();

            return blockSubscription;
        }
        public async Task ExportBlockUsersToExcel(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/unisql/blockusers/excel") : "export/unisql/blockusers/excel", true);
        }

        public async Task ExportBlockUsersToCSV(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/unisql/blockusers/csv") : "export/unisql/blockusers/csv", true);
        }

        partial void OnBlockUsersRead(ref IQueryable<Models.UniSql.BlockUser> items);

        public async Task<IQueryable<Models.UniSql.BlockUser>> GetBlockUsers(Query query = null)
        {
            var items = context.BlockUsers.AsQueryable();

            items = items.Include(i => i.Block);

            items = items.Include(i => i.User);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Filter))
                {
                    items = items.Where(query.Filter);
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach (var p in propertiesToExpand)
                    {
                        items = items.Include(p);
                    }
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnBlockUsersRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnBlockUserCreated(Models.UniSql.BlockUser item);

        public async Task<Models.UniSql.BlockUser> CreateBlockUser(Models.UniSql.BlockUser blockUser)
        {
            OnBlockUserCreated(blockUser);

            context.BlockUsers.Add(blockUser);
            context.SaveChanges();

            return blockUser;
        }
        public async Task ExportInvoicesToExcel(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/unisql/invoices/excel") : "export/unisql/invoices/excel", true);
        }

        public async Task ExportInvoicesToCSV(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/unisql/invoices/csv") : "export/unisql/invoices/csv", true);
        }

        partial void OnInvoicesRead(ref IQueryable<Models.UniSql.Invoice> items);

        public async Task<IQueryable<Models.UniSql.Invoice>> GetInvoices(Query query = null)
        {
            var items = context.Invoices.AsQueryable();

            items = items.Include(i => i.AServiceSubscription);

            items = items.Include(i => i.Transaction);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Filter))
                {
                    items = items.Where(query.Filter);
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach (var p in propertiesToExpand)
                    {
                        items = items.Include(p);
                    }
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnInvoicesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnInvoiceCreated(Models.UniSql.Invoice item);

        public async Task<Models.UniSql.Invoice> CreateInvoice(Models.UniSql.Invoice invoice)
        {
            OnInvoiceCreated(invoice);

            context.Invoices.Add(invoice);
            context.SaveChanges();

            return invoice;
        }
        public async Task ExportMessagesToExcel(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/unisql/messages/excel") : "export/unisql/messages/excel", true);
        }

        public async Task ExportMessagesToCSV(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/unisql/messages/csv") : "export/unisql/messages/csv", true);
        }

        partial void OnMessagesRead(ref IQueryable<Models.UniSql.Message> items);

        public async Task<IQueryable<Models.UniSql.Message>> GetMessages(Query query = null)
        {
            var items = context.Messages.AsQueryable();

            items = items.Include(i => i.User);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Filter))
                {
                    items = items.Where(query.Filter);
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach (var p in propertiesToExpand)
                    {
                        items = items.Include(p);
                    }
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnMessagesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnMessageCreated(Models.UniSql.Message item);

        public async Task<Models.UniSql.Message> CreateMessage(Models.UniSql.Message message)
        {
            OnMessageCreated(message);

            context.Messages.Add(message);
            context.SaveChanges();

            return message;
        }
        public async Task ExportServicesToExcel(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/unisql/services/excel") : "export/unisql/services/excel", true);
        }

        public async Task ExportServicesToCSV(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/unisql/services/csv") : "export/unisql/services/csv", true);
        }

        partial void OnServicesRead(ref IQueryable<Models.UniSql.Service> items);

        public async Task<IQueryable<Models.UniSql.Service>> GetServices(Query query = null)
        {
            var items = context.Services.AsQueryable();

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Filter))
                {
                    // items = items.Where(query.Filter);

                    var tempList = items.Include(serv => serv.AServiceSubscriptions)
                            .ThenInclude(ss => ss.Subscription)
                            .ThenInclude(sub => sub.BlockSubscriptions).AsQueryable();

                    var toAddList = new List<Service>();

                    foreach (var item in tempList)
                    {
                        foreach (var ss in item.AServiceSubscriptions)
                        {
                            var sub = ss.Subscription;
                            var blocksub = sub.BlockSubscriptions;
                            foreach (var bs in blocksub)
                            {
                                if (bs.BlockId.ToString() == query.Filter)
                                {
                                    toAddList.Add(item);
                                }


                            }
                            //if( ss. == int.Parse(query.Filter))
                            //{
                            //    toAddList.Add(item);
                            //}

                        }
                    }
                    items = toAddList.AsQueryable();

                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach (var p in propertiesToExpand)
                    {
                        items = items.Include(p);
                    }
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnServicesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnServiceCreated(Models.UniSql.Service item);

        public async Task<Models.UniSql.Service> CreateService(Models.UniSql.Service service)
        {
            OnServiceCreated(service);

            context.Services.Add(service);
            context.SaveChanges();

            return service;
        }
        public async Task ExportSubscriptionsToExcel(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/unisql/subscriptions/excel") : "export/unisql/subscriptions/excel", true);
        }

        public async Task ExportSubscriptionsToCSV(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/unisql/subscriptions/csv") : "export/unisql/subscriptions/csv", true);
        }

        partial void OnSubscriptionsRead(ref IQueryable<Models.UniSql.Subscription> items);

        public async Task<IQueryable<Models.UniSql.Subscription>> GetSubscriptions(Query query = null)
        {
            var items = context.Subscriptions.Include(sub => sub.BlockSubscriptions).AsQueryable();

       

            items = items.Include(i => i.User);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Filter))
                {
                    //items = items.Where(query.Filter);
                    items = context.BlockSubscriptions.Include(b => b.Subscription).Include(b => b.Block)
                        .Where(query.Filter)
                        .Select(selector => selector.Subscription).AsQueryable();

                    //testing getting admins
                    var blockUsers = context.BlockUsers.Include(bu => bu.Block);
                    if (blockUsers == null)
                    {
                        Console.WriteLine(blockUsers.First().Block.SubsCount);
                    }
                    var temp = new List<Subscription>();
                    foreach (var sub in items)
                    {
                        var user = context.Users.Find(sub.UserId);
                        sub.User = user;
                        temp.Add(sub);

                    }
                    items = temp.AsQueryable();
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach (var p in propertiesToExpand)
                    {
                        items = items.Include(p);
                    }
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnSubscriptionsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnSubscriptionCreated(Models.UniSql.Subscription item);

        public async Task<Models.UniSql.Subscription> CreateSubscription(Models.UniSql.Subscription subscription)
        {
            OnSubscriptionCreated(subscription);

            context.Subscriptions.Add(subscription);
            context.SaveChanges();

            return subscription;
        }
        public async Task ExportTransactionsToExcel(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/unisql/transactions/excel") : "export/unisql/transactions/excel", true);
        }

        public async Task ExportTransactionsToCSV(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/unisql/transactions/csv") : "export/unisql/transactions/csv", true);
        }

        partial void OnTransactionsRead(ref IQueryable<Models.UniSql.Transaction> items);

        public async Task<IQueryable<Models.UniSql.Transaction>> GetTransactions(Query query = null)
        {
            var items = context.Transactions.AsQueryable();

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Filter))
                {
                    items = items.Where(query.Filter);
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach (var p in propertiesToExpand)
                    {
                        items = items.Include(p);
                    }
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnTransactionsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnTransactionCreated(Models.UniSql.Transaction item);

        public async Task<Models.UniSql.Transaction> CreateTransaction(Models.UniSql.Transaction transaction)
        {
            OnTransactionCreated(transaction);

            context.Transactions.Add(transaction);
            context.SaveChanges();

            return transaction;
        }
        public async Task ExportUsersToExcel(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/unisql/users/excel") : "export/unisql/users/excel", true);
        }

        public async Task ExportUsersToCSV(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/unisql/users/csv") : "export/unisql/users/csv", true);
        }

        partial void OnUsersRead(ref IQueryable<Models.UniSql.User> items);

        public async Task<IQueryable<Models.UniSql.User>> GetUsers(Query query = null)
        {
            var items = context.Users.AsQueryable();

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Filter))
                {
                    items = items.Where(query.Filter);
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach (var p in propertiesToExpand)
                    {
                        items = items.Include(p);
                    }
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnUsersRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnUserCreated(Models.UniSql.User item);

        public async Task<Models.UniSql.User> CreateUser(Models.UniSql.User user)
        {
            OnUserCreated(user);

            context.Users.Add(user);
            context.SaveChanges();

            return user;
        }
        public async Task ExportUserMessagesToExcel(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/unisql/usermessages/excel") : "export/unisql/usermessages/excel", true);
        }

        public async Task ExportUserMessagesToCSV(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/unisql/usermessages/csv") : "export/unisql/usermessages/csv", true);
        }

        partial void OnUserMessagesRead(ref IQueryable<Models.UniSql.UserMessage> items);

        public async Task<IQueryable<Models.UniSql.UserMessage>> GetUserMessages(Query query = null)
        {
            var items = context.UserMessages.AsQueryable();

            items = items.Include(i => i.User);

            items = items.Include(i => i.Message);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Filter))
                {
                    items = items.Where(query.Filter);
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach (var p in propertiesToExpand)
                    {
                        items = items.Include(p);
                    }
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnUserMessagesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnUserMessageCreated(Models.UniSql.UserMessage item);

        public async Task<Models.UniSql.UserMessage> CreateUserMessage(Models.UniSql.UserMessage userMessage)
        {
            OnUserMessageCreated(userMessage);

            context.UserMessages.Add(userMessage);
            context.SaveChanges();

            return userMessage;
        }

        partial void OnAServiceSubscriptionDeleted(Models.UniSql.AServiceSubscription item);

        public async Task<Models.UniSql.AServiceSubscription> DeleteAServiceSubscription(int? serviceId, int? subscriptionId)
        {
            var item = context.AServiceSubscriptions
                              .Where(i => i.ServiceId == serviceId && i.SubscriptionId == subscriptionId)
                              .Include(i => i.Invoices)
                              .FirstOrDefault();

            OnAServiceSubscriptionDeleted(item);

            context.AServiceSubscriptions.Remove(item);
            context.SaveChanges();

            return item;
        }

        partial void OnAServiceSubscriptionGet(Models.UniSql.AServiceSubscription item);

        public async Task<Models.UniSql.AServiceSubscription> GetAServiceSubscriptionByServiceIdAndSubscriptionId(int? serviceId, int? subscriptionId)
        {
            var items = context.AServiceSubscriptions
                              .AsNoTracking()
                              .Where(i => i.ServiceId == serviceId && i.SubscriptionId == subscriptionId);

            items = items.Include(i => i.Service);

            items = items.Include(i => i.Subscription);

            var item = items.FirstOrDefault();

            OnAServiceSubscriptionGet(item);

            return await Task.FromResult(item);
        }

        public async Task<Models.UniSql.AServiceSubscription> CancelAServiceSubscriptionChanges(Models.UniSql.AServiceSubscription item)
        {
            var entity = context.Entry(item);
            entity.CurrentValues.SetValues(entity.OriginalValues);
            entity.State = EntityState.Unchanged;

            return item;
        }

        partial void OnAServiceSubscriptionUpdated(Models.UniSql.AServiceSubscription item);

        public async Task<Models.UniSql.AServiceSubscription> UpdateAServiceSubscription(int? serviceId, int? subscriptionId, Models.UniSql.AServiceSubscription aServiceSubscription)
        {
            OnAServiceSubscriptionUpdated(aServiceSubscription);

            var item = context.AServiceSubscriptions
                              .Where(i => i.ServiceId == serviceId && i.SubscriptionId == subscriptionId)
                              .First();
            var entry = context.Entry(item);
            entry.CurrentValues.SetValues(aServiceSubscription);
            entry.State = EntityState.Modified;
            context.SaveChanges();

            return aServiceSubscription;
        }

       

        partial void OnBlockDeleted(Models.UniSql.Block item);

        public async Task<Models.UniSql.Block> DeleteBlock(int? blockId)
        {
            var item = context.Blocks
                              .Where(i => i.BlockId == blockId)
                              .Include(i => i.BlockUsers)
                              .Include(i => i.BlockSubscriptions)
                              .FirstOrDefault();

            OnBlockDeleted(item);

            context.Blocks.Remove(item);
            context.SaveChanges();

            return item;
        }

        partial void OnBlockGet(Models.UniSql.Block item);

        public async Task<Models.UniSql.Block> GetBlockByBlockId(int? blockId)
        {
            var items = context.Blocks
                              .AsNoTracking()
                              .Where(i => i.BlockId == blockId);

            var item = items.FirstOrDefault();

            OnBlockGet(item);

            return await Task.FromResult(item);
        }

        public async Task<Models.UniSql.Block> CancelBlockChanges(Models.UniSql.Block item)
        {
            var entity = context.Entry(item);
            entity.CurrentValues.SetValues(entity.OriginalValues);
            entity.State = EntityState.Unchanged;

            return item;
        }

        partial void OnBlockUpdated(Models.UniSql.Block item);

        public async Task<Models.UniSql.Block> UpdateBlock(int? blockId, Models.UniSql.Block block)
        {
            OnBlockUpdated(block);

            var item = context.Blocks
                              .Where(i => i.BlockId == blockId)
                              .First();
            var entry = context.Entry(item);
            entry.CurrentValues.SetValues(block);
            entry.State = EntityState.Modified;
            context.SaveChanges();

            return block;
        }

        partial void OnBlockSubscriptionDeleted(Models.UniSql.BlockSubscription item);

        public async Task<Models.UniSql.BlockSubscription> DeleteBlockSubscription(int? blockId, int? subscriptionId)
        {
            var item = context.BlockSubscriptions
                              .Where(i => i.BlockId == blockId && i.SubscriptionId == subscriptionId)
                              .FirstOrDefault();

            OnBlockSubscriptionDeleted(item);

            context.BlockSubscriptions.Remove(item);
            context.SaveChanges();

            return item;
        }

        partial void OnBlockSubscriptionGet(Models.UniSql.BlockSubscription item);

        public async Task<Models.UniSql.BlockSubscription> GetBlockSubscriptionByBlockIdAndSubscriptionId(int? blockId, int? subscriptionId)
        {
            var items = context.BlockSubscriptions
                              .AsNoTracking()
                              .Where(i => i.BlockId == blockId && i.SubscriptionId == subscriptionId);

            items = items.Include(i => i.Block);

            items = items.Include(i => i.Subscription);

            var item = items.FirstOrDefault();

            OnBlockSubscriptionGet(item);

            return await Task.FromResult(item);
        }

        public async Task<Models.UniSql.BlockSubscription> CancelBlockSubscriptionChanges(Models.UniSql.BlockSubscription item)
        {
            var entity = context.Entry(item);
            entity.CurrentValues.SetValues(entity.OriginalValues);
            entity.State = EntityState.Unchanged;

            return item;
        }

        partial void OnBlockSubscriptionUpdated(Models.UniSql.BlockSubscription item);

        public async Task<Models.UniSql.BlockSubscription> UpdateBlockSubscription(int? blockId, int? subscriptionId, Models.UniSql.BlockSubscription blockSubscription)
        {
            OnBlockSubscriptionUpdated(blockSubscription);

            var item = context.BlockSubscriptions
                              .Where(i => i.BlockId == blockId && i.SubscriptionId == subscriptionId)
                              .First();
            var entry = context.Entry(item);
            entry.CurrentValues.SetValues(blockSubscription);
            entry.State = EntityState.Modified;
            context.SaveChanges();

            return blockSubscription;
        }

        partial void OnBlockUserDeleted(Models.UniSql.BlockUser item);

        public async Task<Models.UniSql.BlockUser> DeleteBlockUser(int? blockId, int? userId)
        {
            var item = context.BlockUsers
                              .Where(i => i.BlockId == blockId && i.UserId == userId)
                              .FirstOrDefault();

            OnBlockUserDeleted(item);

            context.BlockUsers.Remove(item);
            context.SaveChanges();

            return item;
        }

        partial void OnBlockUserGet(Models.UniSql.BlockUser item);

        public async Task<Models.UniSql.BlockUser> GetBlockUserByBlockIdAndUserId(int? blockId, int? userId)
        {
            var items = context.BlockUsers
                              .AsNoTracking()
                              .Where(i => i.BlockId == blockId && i.UserId == userId);

            items = items.Include(i => i.Block);

            items = items.Include(i => i.User);

            var item = items.FirstOrDefault();

            OnBlockUserGet(item);

            return await Task.FromResult(item);
        }

        public async Task<Models.UniSql.BlockUser> CancelBlockUserChanges(Models.UniSql.BlockUser item)
        {
            var entity = context.Entry(item);
            entity.CurrentValues.SetValues(entity.OriginalValues);
            entity.State = EntityState.Unchanged;

            return item;
        }

        partial void OnBlockUserUpdated(Models.UniSql.BlockUser item);

        public async Task<Models.UniSql.BlockUser> UpdateBlockUser(int? blockId, int? userId, Models.UniSql.BlockUser blockUser)
        {
            OnBlockUserUpdated(blockUser);

            var item = context.BlockUsers
                              .Where(i => i.BlockId == blockId && i.UserId == userId)
                              .First();
            var entry = context.Entry(item);
            entry.CurrentValues.SetValues(blockUser);
            entry.State = EntityState.Modified;
            context.SaveChanges();

            return blockUser;
        }

        partial void OnInvoiceDeleted(Models.UniSql.Invoice item);

        public async Task<Models.UniSql.Invoice> DeleteInvoice(int? invoiceId)
        {
            var item = context.Invoices
                              .Where(i => i.InvoiceId == invoiceId)
                              .FirstOrDefault();

            OnInvoiceDeleted(item);

            context.Invoices.Remove(item);
            context.SaveChanges();

            return item;
        }

        partial void OnInvoiceGet(Models.UniSql.Invoice item);

        public async Task<Models.UniSql.Invoice> GetInvoiceByInvoiceId(int? invoiceId)
        {
            var items = context.Invoices
                              .AsNoTracking()
                              .Where(i => i.InvoiceId == invoiceId);

            items = items.Include(i => i.AServiceSubscription);

            items = items.Include(i => i.Transaction);

            var item = items.FirstOrDefault();

            OnInvoiceGet(item);

            return await Task.FromResult(item);
        }

        public async Task<Models.UniSql.Invoice> CancelInvoiceChanges(Models.UniSql.Invoice item)
        {
            var entity = context.Entry(item);
            entity.CurrentValues.SetValues(entity.OriginalValues);
            entity.State = EntityState.Unchanged;

            return item;
        }

        partial void OnInvoiceUpdated(Models.UniSql.Invoice item);

        public async Task<Models.UniSql.Invoice> UpdateInvoice(int? invoiceId, Models.UniSql.Invoice invoice)
        {
            OnInvoiceUpdated(invoice);

            var item = context.Invoices
                              .Where(i => i.InvoiceId == invoiceId)
                              .First();
            var entry = context.Entry(item);
            entry.CurrentValues.SetValues(invoice);
            entry.State = EntityState.Modified;
            context.SaveChanges();

            return invoice;
        }

        partial void OnMessageDeleted(Models.UniSql.Message item);

        public async Task<Models.UniSql.Message> DeleteMessage(int? messageId)
        {
            var item = context.Messages
                              .Where(i => i.MessageId == messageId)
                              .Include(i => i.UserMessages)
                              .FirstOrDefault();

            OnMessageDeleted(item);

            context.Messages.Remove(item);
            context.SaveChanges();

            return item;
        }

        partial void OnMessageGet(Models.UniSql.Message item);

        public async Task<Models.UniSql.Message> GetMessageByMessageId(int? messageId)
        {
            var items = context.Messages
                              .AsNoTracking()
                              .Where(i => i.MessageId == messageId);

            items = items.Include(i => i.User);

            var item = items.FirstOrDefault();

            OnMessageGet(item);

            return await Task.FromResult(item);
        }

        public async Task<Models.UniSql.Message> CancelMessageChanges(Models.UniSql.Message item)
        {
            var entity = context.Entry(item);
            entity.CurrentValues.SetValues(entity.OriginalValues);
            entity.State = EntityState.Unchanged;

            return item;
        }

        partial void OnMessageUpdated(Models.UniSql.Message item);

        public async Task<Models.UniSql.Message> UpdateMessage(int? messageId, Models.UniSql.Message message)
        {
            OnMessageUpdated(message);

            var item = context.Messages
                              .Where(i => i.MessageId == messageId)
                              .First();
            var entry = context.Entry(item);
            entry.CurrentValues.SetValues(message);
            entry.State = EntityState.Modified;
            context.SaveChanges();

            return message;
        }

        partial void OnServiceDeleted(Models.UniSql.Service item);

        public async Task<Models.UniSql.Service> DeleteService(int? aServiceId)
        {
            var item = context.Services
                              .Where(i => i.AServiceId == aServiceId)
                              .Include(i => i.AServiceSubscriptions)
                              .FirstOrDefault();

            OnServiceDeleted(item);

            context.Services.Remove(item);
            context.SaveChanges();

            return item;
        }

        partial void OnServiceGet(Models.UniSql.Service item);

        public async Task<Models.UniSql.Service> GetServiceByAServiceId(int? aServiceId)
        {
            var items = context.Services
                              .AsNoTracking()
                              .Where(i => i.AServiceId == aServiceId);

            var item = items.FirstOrDefault();

            OnServiceGet(item);

            return await Task.FromResult(item);
        }

        public async Task<Models.UniSql.Service> CancelServiceChanges(Models.UniSql.Service item)
        {
            var entity = context.Entry(item);
            entity.CurrentValues.SetValues(entity.OriginalValues);
            entity.State = EntityState.Unchanged;

            return item;
        }

        partial void OnServiceUpdated(Models.UniSql.Service item);

        public async Task<Models.UniSql.Service> UpdateService(int? aServiceId, Models.UniSql.Service service)
        {
            OnServiceUpdated(service);

            var item = context.Services
                              .Where(i => i.AServiceId == aServiceId)
                              .First();
            var entry = context.Entry(item);
            entry.CurrentValues.SetValues(service);
            entry.State = EntityState.Modified;
            context.SaveChanges();

            return service;
        }

        partial void OnSubscriptionDeleted(Models.UniSql.Subscription item);

        public async Task<Models.UniSql.Subscription> DeleteSubscription(int? subscriptionId)
        {
            var item = context.Subscriptions
                              .Where(i => i.SubscriptionId == subscriptionId)
                              .Include(i => i.AServiceSubscriptions)
                              .Include(i => i.BlockSubscriptions)
                              .FirstOrDefault();

            OnSubscriptionDeleted(item);

            context.Subscriptions.Remove(item);
            context.SaveChanges();

            return item;
        }

        partial void OnSubscriptionGet(Models.UniSql.Subscription item);

        public async Task<Models.UniSql.Subscription> GetSubscriptionBySubscriptionId(int? subscriptionId)
        {
            var items = context.Subscriptions
                              .AsNoTracking()
                              .Where(i => i.SubscriptionId == subscriptionId);

       

            items = items.Include(i => i.User);

            var item = items.FirstOrDefault();

            OnSubscriptionGet(item);

            return await Task.FromResult(item);
        }

        public async Task<Models.UniSql.Subscription> CancelSubscriptionChanges(Models.UniSql.Subscription item)
        {
            var entity = context.Entry(item);
            entity.CurrentValues.SetValues(entity.OriginalValues);
            entity.State = EntityState.Unchanged;

            return item;
        }


        partial void OnSubscriptionUpdated(Models.UniSql.Subscription item);

        public async Task<Models.UniSql.Subscription> UpdateSubscription(int? subscriptionId, Models.UniSql.Subscription subscription)
        {
            OnSubscriptionUpdated(subscription);

            var item = context.Subscriptions
                              .Where(i => i.SubscriptionId == subscriptionId)
                              .First();
            var entry = context.Entry(item);
            entry.CurrentValues.SetValues(subscription);
            entry.State = EntityState.Modified;
            context.SaveChanges();

            return subscription;
        }

        partial void OnTransactionDeleted(Models.UniSql.Transaction item);

        public async Task<Models.UniSql.Transaction> DeleteTransaction(int? aTransactionId)
        {
            var item = context.Transactions
                              .Where(i => i.ATransactionId == aTransactionId)
                              .Include(i => i.Invoices)
                              .FirstOrDefault();

            OnTransactionDeleted(item);

            context.Transactions.Remove(item);
            context.SaveChanges();

            return item;
        }

        partial void OnTransactionGet(Models.UniSql.Transaction item);

        public async Task<Models.UniSql.Transaction> GetTransactionByATransactionId(int? aTransactionId)
        {
            var items = context.Transactions
                              .AsNoTracking()
                              .Where(i => i.ATransactionId == aTransactionId);

            var item = items.FirstOrDefault();

            OnTransactionGet(item);

            return await Task.FromResult(item);
        }

        public async Task<Models.UniSql.Transaction> CancelTransactionChanges(Models.UniSql.Transaction item)
        {
            var entity = context.Entry(item);
            entity.CurrentValues.SetValues(entity.OriginalValues);
            entity.State = EntityState.Unchanged;

            return item;
        }

        partial void OnTransactionUpdated(Models.UniSql.Transaction item);

        public async Task<Models.UniSql.Transaction> UpdateTransaction(int? aTransactionId, Models.UniSql.Transaction transaction)
        {
            OnTransactionUpdated(transaction);

            var item = context.Transactions
                              .Where(i => i.ATransactionId == aTransactionId)
                              .First();
            var entry = context.Entry(item);
            entry.CurrentValues.SetValues(transaction);
            entry.State = EntityState.Modified;
            context.SaveChanges();

            return transaction;
        }

        partial void OnUserDeleted(Models.UniSql.User item);

        public async Task<Models.UniSql.User> DeleteUser(int? userId)
        {
            var item = context.Users
                              .Where(i => i.UserId == userId)
                              .Include(i => i.BlockUsers)
                              .Include(i => i.Messages)
                              .Include(i => i.Subscriptions)
                              .Include(i => i.UserMessages)
                              .FirstOrDefault();

            OnUserDeleted(item);

            context.Users.Remove(item);
            context.SaveChanges();

            return item;
        }

        partial void OnUserGet(Models.UniSql.User item);

        public async Task<Models.UniSql.User> GetUserByUserId(int? userId)
        {
            var items = context.Users
                              .AsNoTracking()
                              .Where(i => i.UserId == userId);

            var item = items.FirstOrDefault();

            OnUserGet(item);

            return await Task.FromResult(item);
        }

        public async Task<Models.UniSql.User> CancelUserChanges(Models.UniSql.User item)
        {
            var entity = context.Entry(item);
            entity.CurrentValues.SetValues(entity.OriginalValues);
            entity.State = EntityState.Unchanged;

            return item;
        }

        partial void OnUserUpdated(Models.UniSql.User item);

        public async Task<Models.UniSql.User> UpdateUser(int? userId, Models.UniSql.User user)
        {
            OnUserUpdated(user);

            var item = context.Users
                              .Where(i => i.UserId == userId)
                              .First();
            var entry = context.Entry(item);
            entry.CurrentValues.SetValues(user);
            entry.State = EntityState.Modified;
            context.SaveChanges();

            return user;
        }

        partial void OnUserMessageDeleted(Models.UniSql.UserMessage item);

        public async Task<Models.UniSql.UserMessage> DeleteUserMessage(int? userId, int? messageId)
        {
            var item = context.UserMessages
                              .Where(i => i.UserId == userId && i.MessageId == messageId)
                              .FirstOrDefault();

            OnUserMessageDeleted(item);

            context.UserMessages.Remove(item);
            context.SaveChanges();

            return item;
        }

        partial void OnUserMessageGet(Models.UniSql.UserMessage item);

        public async Task<Models.UniSql.UserMessage> GetUserMessageByUserIdAndMessageId(int? userId, int? messageId)
        {
            var items = context.UserMessages
                              .AsNoTracking()
                              .Where(i => i.UserId == userId && i.MessageId == messageId);

            items = items.Include(i => i.User);

            items = items.Include(i => i.Message);

            var item = items.FirstOrDefault();

            OnUserMessageGet(item);

            return await Task.FromResult(item);
        }

        public async Task<Models.UniSql.UserMessage> CancelUserMessageChanges(Models.UniSql.UserMessage item)
        {
            var entity = context.Entry(item);
            entity.CurrentValues.SetValues(entity.OriginalValues);
            entity.State = EntityState.Unchanged;

            return item;
        }

        partial void OnUserMessageUpdated(Models.UniSql.UserMessage item);

        public async Task<Models.UniSql.UserMessage> UpdateUserMessage(int? userId, int? messageId, Models.UniSql.UserMessage userMessage)
        {
            OnUserMessageUpdated(userMessage);

            var item = context.UserMessages
                              .Where(i => i.UserId == userId && i.MessageId == messageId)
                              .First();
            var entry = context.Entry(item);
            entry.CurrentValues.SetValues(userMessage);
            entry.State = EntityState.Modified;
            context.SaveChanges();

            return userMessage;
        }

        //custom invoice call
        public async Task<IQueryable<Models.UniSql.Invoice>> GetInvoicesCustom()
        {
            //get logged in use from db
            var loggedInUser = context.Users.Where(u => u.AspNetId == securityService.User.Id).First();
            var invoices = context.Invoices
                .Include(invo => invo.Transaction)
                .Include(invo => invo.AServiceSubscription)
                .ThenInclude(ss => ss.Subscription)
                .ThenInclude(sub => sub.User)
                .Where(invoice => invoice.AServiceSubscription.Subscription.UserId == loggedInUser.UserId)
                .AsQueryable();

            //set service name
            foreach (var invoice in invoices)
            {
                var service = context.Services.Find(invoice.AServiceSubscriptionServiceId);
                invoice.AServiceSubscription.Service = service;
            }
            return invoices;
        }
        //create custom transaction
        public async Task<int> CreateCustomTransaction(int serviceId,int subId,decimal amount)
        {
            
            //create transaction
            var transactionToSave = new Transaction();
            transactionToSave.Amount = amount;
            if(amount < 0)
            transactionToSave.TransactionType = "withdrawal";
            else { transactionToSave.TransactionType = "deposit"; }
            //save to database to get id
            context.Add(transactionToSave);
            await context.SaveChangesAsync();
            //get AServiceSubscription entity to add while creating the invoice
            var aServiceSubscription = context.AServiceSubscriptions.Where(ss => ss.SubscriptionId == subId & ss.ServiceId == serviceId).FirstOrDefault();
            //create invoice
            var invoiceToSave = new Invoice();
            invoiceToSave.AServiceSubscription = aServiceSubscription;
            invoiceToSave.Transaction = transactionToSave;
            //save to database 
            context.Add(invoiceToSave);
          var saveResult =  await context.SaveChangesAsync();

          
            return saveResult;
        }
    }
}
