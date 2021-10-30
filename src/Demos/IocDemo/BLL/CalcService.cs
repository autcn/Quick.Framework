using Quick;

namespace IocDemo.BLL
{
    //标记SingletonDependency(单例)或TransientDependency(短暂)后，程序启动时自动注入它和它的子类(如果存在)
    [SingletonDependency]
    public class CalcService
    {
        public int Add(int a, int b)
        {
            return a + b;
        }
    }
}
