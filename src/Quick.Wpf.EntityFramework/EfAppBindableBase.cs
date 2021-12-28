using Microsoft.EntityFrameworkCore;
using System;

namespace Quick
{
    public abstract class EfAppBindableBase<TMainDbContext> : QBindableAppBase where TMainDbContext : DbContext
    {
        protected TMainDbContext CreateDbContext()
        {
            return ServiceProvider.GetService<TMainDbContext>();
        }

        protected TOtherDbContext CreateDbContext<TOtherDbContext>() where TOtherDbContext : DbContext
        {
            return ServiceProvider.GetService<TOtherDbContext>();
        }

        protected void Transaction(Action<TMainDbContext> action)
        {
            Exception exception = null;
            using (var dbContext = CreateDbContext())
            {
                try
                {
                    action?.Invoke(dbContext);
                    dbContext.SaveChanges();
                }
                catch (Exception ex)
                {
                    exception = ex;
                }
            }
            if (exception != null)
            {
                throw exception;
            }
        }

        protected void Transaction<TOtherDbContext>(Action<TOtherDbContext> action) where TOtherDbContext : DbContext
        {
            Exception exception = null;
            using (var dbContext = CreateDbContext<TOtherDbContext>())
            {
                try
                {
                    action?.Invoke(dbContext);
                    dbContext.SaveChanges();
                }
                catch (Exception ex)
                {
                    exception = ex;
                }
            }
            if (exception != null)
            {
                throw exception;
            }
        }
    }


}
