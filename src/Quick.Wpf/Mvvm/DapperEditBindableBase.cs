using AutoMapper;
using Dapper;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Quick
{
    public abstract class DapperEditBindableBase<TMainDbContext> : QEditBindableBase where TMainDbContext : IDapperDbContext
    {
        private TMainDbContext _dbContext;
        protected virtual IDapperDbContext DbContext => ServiceProvider.LazyGetRequiredService(ref _dbContext);

        public override Task<int> SubmitAsync()
        {
            Type vmType = this.GetType();
            AutoMapAttribute attr = vmType.GetCustomAttribute<AutoMapAttribute>();
            if (attr == null)
            {
                throw new Exception("未定义AutoMapAttribute，无法获取数据库实体类型");
            }
            Type modelType = attr.SourceType;
            bool isEntity = attr.SourceType.HasInterface<IEntity>();
            if (IsEditMode)
            {
                if (!isEntity)
                {
                    throw new Exception("实体未实现IEntity接口，无法找到Id，无法更新到数据库！");
                }
                object tempModel = Mapper.Map(this, vmType, attr.SourceType);
                long id = (tempModel as IEntity).Id;
                Mapper.Map(this, tempModel, vmType, modelType);
                return Task.Run(() => DbContext.Update(tempModel));
            }
            else
            {
                object model = Mapper.Map(this, vmType, attr.SourceType);
                if (model is IEntity entity)
                {
                    entity.Id = IdGenerator.New();
                }
                Mapper.Map(model, this, attr.SourceType, vmType);
                return Task.Run(() => DbContext.Insert(model));
            }

        }

        public override Task<int> DeleteAsync()
        {
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
                throw new Exception("实体未实现IEntity接口，无法找到Id，无法从数据库删除！");
            }
            object tempModel = Mapper.Map(this, vmType, attr.SourceType);
            long id = (tempModel as IEntity).Id;
            return Task.Run(() => DbContext.Delete(new CrudIdTypeBuilder(attr.SourceType, tempModel)));
        }
    }
}
