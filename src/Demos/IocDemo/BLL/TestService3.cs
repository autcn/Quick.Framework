namespace IocDemo.BLL
{
    public class TestService3 : TestServiceBase
    {
        public TestService3()
        {
            Name = "TestService 3";
        }

        public override string Print()
        {
            return "I'm service 3";
        }
    }
}
