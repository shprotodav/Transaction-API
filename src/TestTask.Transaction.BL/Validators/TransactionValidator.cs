using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using TestTask.Transaction.Common.DTOs;
using TestTask.Transaction.Common.Exceptions;
using TestTask.Transaction.Common.IRepositories;

namespace TestTask.Transaction.BL.Validators
{
    public interface ITransactionValidator
    {
        Task CanUpdateAsync(long transactionId);
        Task IsExistAsync(long transactionId);
        Task IsFileExistAsync(IFormFile uploadedFile);
    }

    public class TransactionValidator : ITransactionValidator
    {
        private readonly ITransactionRepository _transactionRepository;

        public TransactionValidator(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task CanUpdateAsync(long transactionId)
        {
            var isExistTask = IsExistAsync(transactionId);

            await Task.WhenAll(isExistTask);
        }

        public async Task IsExistAsync(long transactionId)
        {
            if (!await _transactionRepository.IsExistAsync(transactionId))
            {
                throw new BusinessLogicException($"Item already deleted with id: {transactionId}");
            }
        }

        public async Task IsFileExistAsync(IFormFile uploadedFile)
        {
            if (uploadedFile == null || uploadedFile.Length == 0)
                throw new BusinessLogicException($"Invalid file.");

        }
    }
}

