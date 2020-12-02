using Microsoft.EntityFrameworkCore.Migrations;

namespace TestTask.Transaction.DB.Schema.Migrations
{
    public partial class StoredProcedure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($@"CREATE TYPE [dbo].[ChildItemsType] AS TABLE(
                [TransactionId] int NOT NULL,
                [Status] nvarchar(max) NOT NULL,
                [Type] nvarchar(max) NULL,
                [ClientName] nvarchar(max) NULL,
                [Amount] decimal(18,2) NOT NULL
                )");
            migrationBuilder.Sql($@"CREATE PROCEDURE [dbo].[MergeTransaction]
                @ChildTransaction ChildItemsType READONLY
                AS
                BEGIN
                SET IDENTITY_INSERT [dbo].[Transaction] ON

                  MERGE [dbo].[Transaction] AS t
                  USING (SELECT [TransactionId], [Type], [ClientName], [Status], [Amount] FROM @ChildTransaction) AS s
                  ON t.[TransactionId] = s.[TransactionId]
                  WHEN MATCHED THEN
                    UPDATE SET t.[Status] = s.[Status], t.[Type] = s.[Type], t.[ClientName] = s.[ClientName], t.[Amount] = s.[Amount]
                  WHEN NOT MATCHED BY TARGET THEN
                    INSERT ([TransactionId], [Status], [Type], [ClientName], [Amount]) 
                      VALUES (s.[TransactionId], s.[Status], s.[Type], s.[ClientName], s.[Amount]);
                    SET IDENTITY_INSERT [dbo].[Transaction] OFF
                END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP PROCEDURE [dbo].[MergeTransaction]");
            migrationBuilder.Sql(@"DROP TYPE [dbo].[ChildItemsType]");

        }
    }
}
