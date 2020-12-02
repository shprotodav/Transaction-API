using ClosedXML.Excel;
using CsvHelper;
using Dapper;
using DapperParameters;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using TestTask.Transaction.Common.DTOs;
using TestTask.Transaction.Common.Helpers;
using TestTask.Transaction.Common.IRepositories;
using TestTask.Transaction.DAL.Helpers;
using TestTask.Transaction.DAL.Queries;

namespace TestTask.Transaction.DAL.Repositories.Main
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly IConnectionFactory _connectionFactory;
        private readonly IHelperRepository _helperRepository;

        public TransactionRepository(IConnectionFactory connectionFactory,
                         IHelperRepository helperRepository)
        {
            _connectionFactory = connectionFactory;
            _helperRepository = helperRepository;
        }

        public async Task<TransactionDTO[]> GetFilteredAsync(string status, string type, string clientName)
        {
            using (var connection = _connectionFactory.GetOpenedConnection())
            {
                var rules = new List<QueryRule>();
                if (!string.IsNullOrEmpty(status)) rules.Add(new QueryRule { Value = status, Key = "[Status]" });
                if (!string.IsNullOrEmpty(type)) rules.Add(new QueryRule { Value = type, Key = "[Type]" });
                if (!string.IsNullOrEmpty(clientName)) rules.Add(new QueryRule { Value = clientName, Key = "[ClientName]" });

                var whereCondition = "";

                if (rules.Count > 0) { 
                    whereCondition += "WHERE " + SQLConditionBuilder.Build(rules.ToArray(), x => $" {x.Key}='{x.Value}'", " AND ");
                }

                return await connection.QueryArrayAsync<TransactionDTO>($@"SELECT [TransactionId]
                  ,[Status]
                  ,[Type]
                  ,[ClientName]
                  ,[Amount]
              FROM [dbo].[Transaction] {whereCondition}");
            }
        }

        public async Task<byte[]> DownloadFilteredExcelFileAsync(TransactionFilterConfig transactionFilterConfig)
        {
            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Transactions");
            transactionFilterConfig.XlLetterGenerate();

            TransactionDTO[] transactionDTOs = await GetFilteredAsync(transactionFilterConfig.StatusColumn.Value, transactionFilterConfig.TypeColumn.Value,
                                                                      transactionFilterConfig.ClientNameColumn.Value);

            if (!string.IsNullOrEmpty(transactionFilterConfig.IdColumn.XlLetter)) worksheet.Cell(1, transactionFilterConfig.IdColumn.XlLetter).Value = "TransactionId";
            if (!string.IsNullOrEmpty(transactionFilterConfig.StatusColumn.XlLetter)) worksheet.Cell(1, transactionFilterConfig.StatusColumn.XlLetter).Value = "Status";
            if (!string.IsNullOrEmpty(transactionFilterConfig.TypeColumn.XlLetter)) worksheet.Cell(1, transactionFilterConfig.TypeColumn.XlLetter).Value = "Type";
            if (!string.IsNullOrEmpty(transactionFilterConfig.ClientNameColumn.XlLetter)) worksheet.Cell(1, transactionFilterConfig.ClientNameColumn.XlLetter).Value = "ClientName";
            if (!string.IsNullOrEmpty(transactionFilterConfig.AmountColumn.XlLetter)) worksheet.Cell(1, transactionFilterConfig.AmountColumn.XlLetter).Value = "Amount";

            int sheetRow = 2;
            foreach (TransactionDTO transactionDTO in transactionDTOs)
            {
                if (!string.IsNullOrEmpty(transactionFilterConfig.IdColumn.XlLetter)) worksheet.Cell(sheetRow, transactionFilterConfig.IdColumn.XlLetter).Value = transactionDTO.TransactionId;
                if (!string.IsNullOrEmpty(transactionFilterConfig.StatusColumn.XlLetter)) worksheet.Cell(sheetRow, transactionFilterConfig.StatusColumn.XlLetter).Value = transactionDTO.Status;
                if (!string.IsNullOrEmpty(transactionFilterConfig.TypeColumn.XlLetter)) worksheet.Cell(sheetRow, transactionFilterConfig.TypeColumn.XlLetter).Value = transactionDTO.Type;
                if (!string.IsNullOrEmpty(transactionFilterConfig.ClientNameColumn.XlLetter)) worksheet.Cell(sheetRow, transactionFilterConfig.ClientNameColumn.XlLetter).Value = transactionDTO.ClientName;
                if (!string.IsNullOrEmpty(transactionFilterConfig.AmountColumn.XlLetter)) worksheet.Cell(sheetRow, transactionFilterConfig.AmountColumn.XlLetter).Value = transactionDTO.Amount;
                sheetRow++;
            }

            worksheet.Columns().AdjustToContents();

            using (MemoryStream stream = new MemoryStream())
            {
                workbook.SaveAs(stream);
                return stream.ToArray();
            }
        }

        public async Task UpdateStatusAsync(UpdateStatusDTO updateStatusDTO)
        {
            using (var connection = _connectionFactory.GetOpenedConnection())
            {
                await connection.ExecuteAsync(TransactionQueries.UpdateStatus, updateStatusDTO);
            }
        }

        public async Task DeleteByIdAsync(long transactionId)
        {
            using (var connection = _connectionFactory.GetOpenedConnection())
            {
                await connection.ExecuteAsync(TransactionQueries.DeleteById, new { transactionId});
            }
        }

        public async Task MergeAsync(TransactionDTO[] transactionDTOs)
        {            
            using (var connection = _connectionFactory.GetOpenedConnection())
            {
                var parameters = new DynamicParameters();
                parameters.AddTable("@ChildTransaction", "ChildItemsType", transactionDTOs);

                await connection.ExecuteAsync(
                    "[dbo].[MergeTransaction]", parameters,
                        commandType: CommandType.StoredProcedure 
                );
            }       
        }

        public async Task MergeByCsvAsync(IFormFile uploadedFile)
        {
            List<TransactionDTO> transactionDTOsList = new List<TransactionDTO>();
            IEnumerable<TransactionUnformatted> transactionUnformattedEnum;

            Encoding enc8 = Encoding.UTF8;
            var stream = new MemoryStream((int)uploadedFile.Length);
            uploadedFile.CopyTo(stream);           

            var bytes = stream.ToArray();
            string noo = enc8.GetString(bytes);            

            using (TextReader streamReader = new StringReader(noo))
            {
                using (CsvReader csvReader = new CsvReader(streamReader, CultureInfo.CurrentCulture))
                {
                    csvReader.Configuration.Delimiter = ",";
                    transactionUnformattedEnum = csvReader.GetRecords<TransactionUnformatted>();

                    foreach (TransactionUnformatted transactionUnformatted in transactionUnformattedEnum)
                        transactionDTOsList.Add(transactionUnformatted.ToDTO());
                }
            }

            await MergeAsync(transactionDTOsList.ToArray());
        }

        public async Task<TransactionDTO[]> GetAllAsync()
        {
            using (var connection = _connectionFactory.GetOpenedConnection())
            {
                return await connection.QueryArrayAsync<TransactionDTO>(TransactionQueries.GetAll);
            }
        }

        public async Task<bool> IsExistAsync(long transactionId)
        {
            using (var connection = _connectionFactory.GetOpenedConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<bool>(TransactionQueries.IsExist, new { transactionId }); // Last parameter ?
            }
        }
    }

}
