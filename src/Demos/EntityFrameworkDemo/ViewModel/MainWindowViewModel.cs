using EntityFrameworkDemo.Model;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Quick;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace EntityFrameworkDemo.ViewModel
{
    /*操作步骤：
     * 1.在配置文件appsettings.json中添加两个连接字符串，起名字
     * 2.添加两个DapperDbContext，标记其使用的连接字符串，声明为单例(根据项目实际情况也可声明为临时，主要取决于是否并发)，并实现相应的接口，见StudentDapperDbContext和BlogDapperDbContext
     * 3.在任何需要数据库访问的地方，注入上述的DapperDbContext即可
     * 4.要全自动支持自动添加，修改，删除的表类，应该从QDapperEditBindableBase继承，见StudentItem.cs
     */
    public class MainWindowViewModel : DemoAppBindableBase, IInitializable
    {
        public MainWindowViewModel()
        {
            Students = new ObservableCollection<StudentItem>();
        }
        public ObservableCollection<StudentItem> Students { get; set; }

        public void Initialize()
        {
            Mapper.Map(CreateDbContext().AsNoTracking().Students.ToList(), Students);
        }

        public void GetBlogs()
        {
            string strJson = null;
            using (var dbContext = CreateDbContext().AsNoTracking())
            {
                var query = dbContext.Blogs.AsQueryable();
                query = query.Include(p => p.Posts);
                List<Blog> blogs = query.ToList();
                strJson = JsonConvert.SerializeObject(blogs, Formatting.Indented);
            }
            MsgBox.Show(strJson);
        }


        public void TestFailed()
        {
            try
            {
                Transaction(dbContext =>
                {
                    Blog blog = dbContext.Blogs.First();
                    blog.Url = "http://1111111";

                    Blog newBlog = new Blog()
                    {
                        Id = blog.Id,
                        Url = "aaa"
                    };
                    dbContext.Blogs.Add(newBlog);
                });
            }
            catch (Exception ex)
            {
                MsgBox.Show(ex.Message);
            }
        }

        public void TestSuccess()
        {
            try
            {
                Transaction(dbContext =>
                {
                    Blog blog = dbContext.Blogs.First();
                    blog.Url = "http://22222";

                    Blog newBlog = new Blog()
                    {
                        Id = IdGenerator.New(),
                        Url = "bbb"
                    };
                    dbContext.Blogs.Add(newBlog);
                });
            }
            catch (Exception ex)
            {
                MsgBox.Show(ex.Message);
            }
        }

        public void SetCulture(string cultureName)
        {
            Localization.SetCulture(cultureName);
        }
    }
}
