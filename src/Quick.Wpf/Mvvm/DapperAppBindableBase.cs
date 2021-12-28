using Dapper;
using System;

namespace Quick
{
    public abstract class DapperAppBindableBase<TMainDbContext> : QBindableAppBase where TMainDbContext : IDapperDbContext
    {
        private TMainDbContext _dbContext;
        protected virtual IDapperDbContext DbContext => ServiceProvider.LazyGetRequiredService(ref _dbContext);
    }
}
