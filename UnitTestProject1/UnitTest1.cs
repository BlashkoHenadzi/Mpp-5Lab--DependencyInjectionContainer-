using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mpp_5Lab__DependencyInjectionContainer_;

namespace UnitTestProject1
{
    interface InterfaceTest1 { };
    class ClassTest1:InterfaceTest1
    {

    }
    [TestClass]
    public class UnitTest1
    {
        DependenciesConfiguration configuration;
        [TestInitialize]
        public void Initialize()
        {
            configuration = new DependenciesConfiguration();
        }
        [TestMethod]
        public void TestMethod1()
        {
            configuration.Register<InterfaceTest1, ClassTest1>(Lifetime.Singleton);
            DependencyProvider DIprovider = new DependencyProvider(configuration);
            var interface1 = DIprovider.Resolve<InterfaceTest1>();
            Assert.AreEqual(typeof(ClassTest1), interface1.GetType());
        }
    }
}
