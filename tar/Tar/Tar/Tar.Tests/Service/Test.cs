namespace Tar.Tests.Service
{
    public class Test : Tar.Service.Service<ITest, Test>, ITest
    {
        #region ITest Members
        public void Test1()
        {
        }
        #endregion
    }
}