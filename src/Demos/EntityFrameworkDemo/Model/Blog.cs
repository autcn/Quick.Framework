using System.Collections.Generic;

namespace EntityFrameworkDemo.Model
{
    public class Blog
    {
        public long Id { get; set; }
        public string Url { get; set; }
        public List<Post> Posts { get; set; }
    }
}
