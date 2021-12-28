using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Quick
{
    public abstract class EfEditBindableBase<TMainDbContext> : QEditBindableBase where TMainDbContext : DbContext
    {
        #region DbContext

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
        #endregion


        public bool SaveToDatabase { get; set; } = true;

        public override Task<int> SubmitAsync()
        {
            if (!SaveToDatabase)
            {
                return base.SubmitAsync();
            }
            Type vmType = this.GetType();
            AutoMapAttribute attr = vmType.GetCustomAttribute<AutoMapAttribute>();
            if (attr == null)
            {
                throw new Exception("未定义AutoMapAttribute，无法获取数据库实体类型");
            }
            Type modelType = attr.SourceType;

            //DbSet dbSet = DbContext.Set(modelType);
            bool isEntity = attr.SourceType.HasInterface<IEntity>();
            DbContext dbContext = CreateDbContext();
            if (IsEditMode)
            {
                if (!isEntity)
                {
                    throw new Exception("实体未实现IQEntity接口，无法找到Id，无法更新到数据库！");
                }
                object tempModel = Mapper.Map(this, vmType, attr.SourceType);
                long id = (tempModel as IEntity).Id;
                return dbContext.FindAsync(modelType, id).AsTask().ContinueWith(task =>
                {
                    if (task.Result != null)
                    {
                        Mapper.Map(this, task.Result, vmType, modelType);
                        return dbContext.SaveChanges();
                    }
                    throw new Exception($"实体{attr.SourceType.Name}，Id: {id}不存在，无法保存到数据库！");
                });
            }
            else
            {
                object model = Mapper.Map(this, vmType, attr.SourceType);
                if (model is IEntity entity)
                {
                    entity.Id = IdGenerator.New();
                }
                dbContext.Add(model);
                Mapper.Map(model, this, attr.SourceType, vmType);
            }

            return dbContext.SaveChangesAsync();
        }

        public override Task<int> DeleteAsync()
        {
            if (!SaveToDatabase)
            {
                return base.DeleteAsync();
            }
            Type vmType = this.GetType();
            AutoMapAttribute attr = vmType.GetCustomAttribute<AutoMapAttribute>();
            if (attr == null)
            {
                throw new Exception("未定义AutoMapAttribute，无法获取数据库实体类型");
            }
            Type modelType = attr.SourceType;
            bool isEntity = attr.SourceType.HasInterface<IEntity>();
            if (!isEntity)
            {
                throw new Exception("实体未实现IQEntity接口，无法找到Id，无法从数据库删除！");
            }
            object tempModel = Mapper.Map(this, vmType, attr.SourceType);
            long id = (tempModel as IEntity).Id;

            DbContext dbContext = CreateDbContext();
            return dbContext.FindAsync(modelType, id).AsTask().ContinueWith(task =>
            {
                if (task.Result != null)
                {
                    dbContext.Remove(task.Result);
                    return dbContext.SaveChanges();
                }
                throw new Exception($"实体{attr.SourceType.Name}，Id: {id}不存在，无法从数据库删除！");
            });
        }
    }


}
