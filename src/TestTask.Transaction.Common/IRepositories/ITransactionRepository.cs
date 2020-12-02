using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using TestTask.Transaction.Common.DTOs;
using TestTask.Transaction.Common.Helpers;
using System.Web.Mvc;
using Microsoft.AspNetCore.Mvc;

namespace TestTask.Transaction.Common.IRepositories
{
    public interface ITransactionRepository
    {
        Task<TransactionDTO[]> GetFilteredAsync(string status, string type, string clientName);
        Task<byte[]> DownloadFilteredExcelFileAsync(TransactionFilterConfig transactionFilterConfig);
        Task UpdateStatusAsync(UpdateStatusDTO updateStatusDTO);
        Task DeleteByIdAsync(long Id);
        Task MergeAsync(TransactionDTO[] transactionDTOs);
        Task<bool> IsExistAsync(long transactionId);
        Task MergeByCsvAsync(IFormFile uploadedFile);

    }
}
