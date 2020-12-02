using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace TestTask.Transaction.DB.Schema
{
    public class DataContext : DbContext
    {
        readonly string _connectionString = "Server=VLAD-NOUT-HP;Initial Catalog=TestTask.Transaction.Dev;Integrated Security=True;MultipleActiveResultSets=True";

        public DataContext() { }

        public DataContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DataContext(DbContextOptions<DataContext> options) : base(options) { }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(new SqlConnection(_connectionString));
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
                
            builder.Entity<Entities.Transaction>(rest =>
            {
                rest.HasKey(x => x.TransactionId);

                rest.Property(x => x.Status)
                    .IsRequired(true);
            });

        }
    }
}
