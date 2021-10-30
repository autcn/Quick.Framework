using Dapper;
using DapperDemo.Database;
using Quick;
using System;

namespace DapperDemo.ViewModel
{
    //直接继承主库，DbContext为主库
    public class DemoAppBindableBase : DapperAppBindableBase<StudentDapperDbContext>
    {
        //添加其他库
        private BlogDapperDbContext _blogDbContext;
        public IDapperDbContext BlogDbContext => ServiceProvider.LazyGetRequiredService(ref _blogDbContext);
    }
}
