using DapperDemo.Model;
using Newtonsoft.Json;
using Quick;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Dapper;
using System.Data;
using System.Threading;

namespace DapperDemo.ViewModel
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
            Mapper.Map(DbContext.GetList<Student>(), Students);
        }

        public void GetBlogs()
        {
            var blogs = BlogDbContext.GetList<Blog>();
            string json = JsonConvert.SerializeObject(blogs, Formatting.Indented);
            Blog blog = blogs.First();
            MsgBox.Show(json);
        }

        public async void DapperRaw()
        {
            var posts = await BlogDbContext.DbConnection.QueryAsync<Post>("SELECT * FROM Post WHERE BlogId = 57221353207590913");
            string json = JsonConvert.SerializeObject(posts, Formatting.Indented);
            MsgBox.Show(json);
        }

        public void TestTransaction()
        {
            Blog blog = BlogDbContext.GetList<Blog>().FirstOrDefault();
            try
            {
                BlogDbContext.Transaction(t =>
                {
                    blog.Url = "new url";
                    BlogDbContext.Update(blog);

                    //故意插入重复Id，导致第二次失败，测试事务回滚
                    Blog newBlog = new Blog();
                    newBlog.Id = blog.Id;
                    BlogDbContext.Insert(newBlog);
                });
            }
            catch (Exception ex)
            {
                MsgBox.Show(ex.Message);
            }

            Blog perBlog = BlogDbContext.Get<Blog>(blog.Id);
            MsgBox.Show($"当前url为：{perBlog.Url}，new url被回滚！");
        }

        public void SetCulture(string cultureName)
        {
            Localization.SetCulture(cultureName);
        }
    }
}
