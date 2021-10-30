using Quick;

namespace IocDemo.BLL
{
    //当本类继承的接口不符合名字规则，XXX : IXXX 时，将不会自动注册接口，要自动注入接口，需要用[ExposeServices]手动指定
    [SingletonDependency]
    [ExposeServices(typeof(IScreenService))] //由于名字不符合规则，需要手动指定暴露的接口
    public class ComputerInfoService : IScreenService
    {
        public string GetSize()
        {
            return "1920*1080";
        }
    }
}
