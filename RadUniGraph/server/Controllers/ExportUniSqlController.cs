using Microsoft.AspNetCore.Mvc;
using UniBlocksGraph.Data;

namespace UniBlocksGraph
{
    public partial class ExportUniSqlController : ExportController
    {
        private readonly UniSqlContext context;

        public ExportUniSqlController(UniSqlContext context)
        {
            this.context = context;
        }

        [HttpGet("/export/UniSql/aservicesubscriptions/csv")]
        public FileStreamResult ExportAServiceSubscriptionsToCSV()
        {
            return ToCSV(ApplyQuery(context.AServiceSubscriptions, Request.Query));
        }

        [HttpGet("/export/UniSql/aservicesubscriptions/excel")]
        public FileStreamResult ExportAServiceSubscriptionsToExcel()
        {
            return ToExcel(ApplyQuery(context.AServiceSubscriptions, Request.Query));
        }

      

        [HttpGet("/export/UniSql/blocks/csv")]
        public FileStreamResult ExportBlocksToCSV()
        {
            return ToCSV(ApplyQuery(context.Blocks, Request.Query));
        }

        [HttpGet("/export/UniSql/blocks/excel")]
        public FileStreamResult ExportBlocksToExcel()
        {
            return ToExcel(ApplyQuery(context.Blocks, Request.Query));
        }

        [HttpGet("/export/UniSql/blocksubscriptions/csv")]
        public FileStreamResult ExportBlockSubscriptionsToCSV()
        {
            return ToCSV(ApplyQuery(context.BlockSubscriptions, Request.Query));
        }

        [HttpGet("/export/UniSql/blocksubscriptions/excel")]
        public FileStreamResult ExportBlockSubscriptionsToExcel()
        {
            return ToExcel(ApplyQuery(context.BlockSubscriptions, Request.Query));
        }

        [HttpGet("/export/UniSql/blockusers/csv")]
        public FileStreamResult ExportBlockUsersToCSV()
        {
            return ToCSV(ApplyQuery(context.BlockUsers, Request.Query));
        }

        [HttpGet("/export/UniSql/blockusers/excel")]
        public FileStreamResult ExportBlockUsersToExcel()
        {
            return ToExcel(ApplyQuery(context.BlockUsers, Request.Query));
        }

        [HttpGet("/export/UniSql/invoices/csv")]
        public FileStreamResult ExportInvoicesToCSV()
        {
            return ToCSV(ApplyQuery(context.Invoices, Request.Query));
        }

        [HttpGet("/export/UniSql/invoices/excel")]
        public FileStreamResult ExportInvoicesToExcel()
        {
            return ToExcel(ApplyQuery(context.Invoices, Request.Query));
        }

        [HttpGet("/export/UniSql/messages/csv")]
        public FileStreamResult ExportMessagesToCSV()
        {
            return ToCSV(ApplyQuery(context.Messages, Request.Query));
        }

        [HttpGet("/export/UniSql/messages/excel")]
        public FileStreamResult ExportMessagesToExcel()
        {
            return ToExcel(ApplyQuery(context.Messages, Request.Query));
        }

        [HttpGet("/export/UniSql/services/csv")]
        public FileStreamResult ExportServicesToCSV()
        {
            return ToCSV(ApplyQuery(context.Services, Request.Query));
        }

        [HttpGet("/export/UniSql/services/excel")]
        public FileStreamResult ExportServicesToExcel()
        {
            return ToExcel(ApplyQuery(context.Services, Request.Query));
        }

        [HttpGet("/export/UniSql/subscriptions/csv")]
        public FileStreamResult ExportSubscriptionsToCSV()
        {
            return ToCSV(ApplyQuery(context.Subscriptions, Request.Query));
        }

        [HttpGet("/export/UniSql/subscriptions/excel")]
        public FileStreamResult ExportSubscriptionsToExcel()
        {
            return ToExcel(ApplyQuery(context.Subscriptions, Request.Query));
        }

        [HttpGet("/export/UniSql/transactions/csv")]
        public FileStreamResult ExportTransactionsToCSV()
        {
            return ToCSV(ApplyQuery(context.Transactions, Request.Query));
        }

        [HttpGet("/export/UniSql/transactions/excel")]
        public FileStreamResult ExportTransactionsToExcel()
        {
            return ToExcel(ApplyQuery(context.Transactions, Request.Query));
        }

        [HttpGet("/export/UniSql/users/csv")]
        public FileStreamResult ExportUsersToCSV()
        {
            return ToCSV(ApplyQuery(context.Users, Request.Query));
        }

        [HttpGet("/export/UniSql/users/excel")]
        public FileStreamResult ExportUsersToExcel()
        {
            return ToExcel(ApplyQuery(context.Users, Request.Query));
        }

        [HttpGet("/export/UniSql/usermessages/csv")]
        public FileStreamResult ExportUserMessagesToCSV()
        {
            return ToCSV(ApplyQuery(context.UserMessages, Request.Query));
        }

        [HttpGet("/export/UniSql/usermessages/excel")]
        public FileStreamResult ExportUserMessagesToExcel()
        {
            return ToExcel(ApplyQuery(context.UserMessages, Request.Query));
        }
    }
}
