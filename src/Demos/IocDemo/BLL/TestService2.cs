namespace IocDemo.BLL
{
    public class TestService2 : TestServiceBase
    {
        public TestService2()
        {
            Name = "TestService 2";
        }

        public override string Print()
        {
            return "I'm service 2";
        }
    }
}
