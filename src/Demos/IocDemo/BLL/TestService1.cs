namespace IocDemo.BLL
{
    public class TestService1 : TestServiceBase
    {
        public TestService1()
        {
            Name = "TestService 1";
        }

        public override string Print()
        {
            return "I'm service 1";
        }
    }
}
