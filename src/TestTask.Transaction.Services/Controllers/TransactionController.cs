using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TestTask.Transaction.BL.Services;
using TestTask.Transaction.Common.DTOs;
using TestTask.Transaction.Services.Filters;
using TestTask.Transaction.Common.Helpers;
using Swashbuckle.Swagger;


namespace TestTask.Transaction.Services.Controllers
{
    /// <summary>
    /// Main controller.
    /// Contains all the necessary queries described in the "Test case.pdf"
    /// </summary>
    [AuthJWT]
    [Route("[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }


        /// <summary>
        /// Deletes a record in the table by the passed TransactionId
        /// </summary>
        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> DeleteById([FromQuery] long transactionId)
        {
            await _transactionService.DeleteByIdAsync(transactionId);
            return Ok(transactionId);
        }


        /// <summary>
        /// Updates the value of record's status in the table to the specified status by the passed TransactionId
        /// </summary>
        [HttpPut]
        [Route("UpdateStatus")]
        public async Task<IActionResult> UpdateStatus([FromBody] UpdateStatusDTO updateStatusDTO)
        {
            await _transactionService.UpdateStatusAsync(updateStatusDTO);
            return NoContent();
        }

        /// <summary>
        /// Merges an existing table with a passed table
        /// </summary>
        /// <remarks>
        /// Pass the table in JSON format.
        /// </remarks>
        [HttpPut]
        [Route("Merge")]
        public async Task<IActionResult> Merge([FromBody] TransactionDTO[] transactionDTOs)
        {
            await _transactionService.MergeAsync(transactionDTOs);
            return NoContent();
        }

        /// <summary>
        /// Merges an existing table with a passed in the file table
        /// </summary>
        /// <remarks>
        /// Pass the table in CSV-file. <para>   </para>
        /// Needed CSV-headers:
        /// <code>TransactionId,Status,Type,ClientName,Amount</code>
        /// </remarks>
        [HttpPost]
        [Route("MergeByFile")]
        public async Task<IActionResult> MergeByFile(IFormFile uploadedFile)
        {
            await _transactionService.MergeByFileAsync(uploadedFile);
            return NoContent();
        }

        /// <summary>
        /// Returns filtered records from the table
        /// </summary>
        /// <remarks>
        /// Specify values to retrieve records with corresponding values for these fields (if necessary).
        /// </remarks>
        [HttpGet]
        [Route("GetFiltered")]
        public async Task<IActionResult> GetFiltered([FromQuery] string status, string type, string clientName)
        {
            var result = await _transactionService.GetFilteredAsync(status,type, clientName);
            return Ok(result);
        }

        /// <summary>
        /// Downloads XL-file of filtered records from table
        /// </summary>
        /// <remarks>
        /// Specify values to retrieve records with corresponding values for these fields (if necessary). <para>   </para>
        /// Set <code>true</code> for each column from the table to be imported into the file.
        /// </remarks>
        [HttpGet]
        [Route("DownloadXL")]
        public async Task<FileResult> DownloadFilteredExcelFile([FromQuery] bool isIdColumn, bool isStatusColumn, bool isTypeColumn,
                                                              bool isClientNameColumn, bool isAmountColumn,
                                                              string status, string type, string clientName)
        {
            var result = await _transactionService.DownloadFilteredExcelFileAsync(
                new TransactionFilterConfig(isIdColumn, isStatusColumn, isTypeColumn,
                                            isClientNameColumn, isAmountColumn,
                                            status, type, clientName));

            return File(result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Filtered_Transactions.xlsx");
        }  
    }
}
