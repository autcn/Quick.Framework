namespace IocDemo.BLL
{
    public abstract class TestServiceBase
    {
        public string Name { protected set; get; }
        public abstract string Print();
    }
}
