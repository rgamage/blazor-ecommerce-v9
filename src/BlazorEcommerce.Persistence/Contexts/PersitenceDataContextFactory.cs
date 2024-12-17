using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using BlazorEcommerce.Shared.Constant;

namespace BlazorEcommerce.Persistence.Contexts
{
    /// <summary>
    /// this is for the sole purpose of generating migrations at design time
    /// </summary>
    public class PersistenceDataContextFactory : IDesignTimeDbContextFactory<PersistenceDataContext>
    {
        public PersistenceDataContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<PersistenceDataContext>();
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=blazorecommerce;Trusted_Connection=True;MultipleActiveResultSets=true",
                            opts => opts.MigrationsHistoryTable(tableName: "__EFMigrationsHistory", schema: Constants.PersistenceDbSchema));

            return new PersistenceDataContext(optionsBuilder.Options);
        }
    }
}