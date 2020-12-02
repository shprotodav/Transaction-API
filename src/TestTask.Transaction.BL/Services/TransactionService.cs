using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using TestTask.Transaction.BL.Validators;
using TestTask.Transaction.Common.DTOs;
using TestTask.Transaction.Common.Helpers;
using TestTask.Transaction.Common.IRepositories;

namespace TestTask.Transaction.BL.Services
{

    public interface ITransactionService
    {
        Task<TransactionDTO[]> GetFilteredAsync(string status, string type, string clientName);
        Task<byte[]> DownloadFilteredExcelFileAsync(TransactionFilterConfig transactionFilterConfig);
        Task UpdateStatusAsync(UpdateStatusDTO updateStatusDTO);
        Task DeleteByIdAsync(long Id);
        Task MergeAsync(TransactionDTO[] transactionDTOs);
        Task MergeByFileAsync(IFormFile uploadedFile);
    }
    public class TransactionService : ITransactionService
    {

        private readonly ITransactionRepository _transactionRepository;
        private readonly ITransactionValidator _transactionValidator;

        public TransactionService(ITransactionRepository transactionRepository,
                          ITransactionValidator transactionValidator)
        {
            _transactionRepository = transactionRepository;
            _transactionValidator = transactionValidator;
        }

        public async Task<TransactionDTO[]> GetFilteredAsync(string status, string type, string clientName)
        {
            return await _transactionRepository.GetFilteredAsync(status, type, clientName); 
        }

        public async Task<byte[]> DownloadFilteredExcelFileAsync(TransactionFilterConfig transactionFilterConfig)
        {
            return await _transactionRepository.DownloadFilteredExcelFileAsync(transactionFilterConfig);
        }

        public async Task UpdateStatusAsync(UpdateStatusDTO updateStatusDTO)
        {
            await _transactionValidator.CanUpdateAsync(updateStatusDTO.TransactionId);
            await _transactionRepository.UpdateStatusAsync(updateStatusDTO);
        }

        public async Task DeleteByIdAsync(long transactionId)
        {
            await _transactionValidator.IsExistAsync(transactionId);
            await _transactionRepository.DeleteByIdAsync(transactionId);
        }

        public async Task MergeAsync(TransactionDTO[] transactionDTOs)
        {
           await _transactionRepository.MergeAsync(transactionDTOs);
        }

        public async Task MergeByFileAsync(IFormFile uploadedFile)
        {
            await _transactionValidator.IsFileExistAsync(uploadedFile);
            await _transactionRepository.MergeByCsvAsync(uploadedFile);
        }

    }
}
