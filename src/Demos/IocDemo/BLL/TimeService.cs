using Quick;
using System;

namespace IocDemo.BLL
{
    //当本类继承的接口符合名字规则，XXX : IXXX 时，自动注册接口及本身，如果不符合改名字规则，要自动注册接口，需要用[ExposeServices]手动指定
    [SingletonDependency]
    public class TimeService : ITimeService
    {
        public string GetCurrentTime()
        {
            return DateTime.Now.ToString();
        }
    }
}
