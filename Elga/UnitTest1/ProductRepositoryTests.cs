using FashionApp.DAL;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace UnitTest1
{
	[TestClass]
	public class ProductRepositoryTests
	{
        private IUnitOfWork _unitOfWork;
        public ProductRepositoryTests()
        {
            var appConfig = new Dictionary<string, string> {
                {"ConnectionStrings:AppDbContextConnection","Data Source=DESKTOP-47SPOUM\\SQLEXPRESS;Initial Catalog=FashionDB;Integrated Security=True"}
            };
            IConfiguration config = new ConfigurationBuilder()
                                .AddInMemoryCollection(appConfig)
                                .Build();
            var serviceCollection = new ServiceCollection()
                .AddSingleton<IConfiguration>(config)
                ;
            FashionApp.DAL.Startup.RegisterServices(serviceCollection, config);
            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
            _unitOfWork = serviceProvider.GetService(typeof(IUnitOfWork)) as IUnitOfWork;
        }

        [TestMethod]
        public void Test_001_ShouldListDataOK()
        {
            var products = _unitOfWork.ProductsRepository.GetAll();
            Assert.IsNotNull(products, "Nuk u lexuan dot produktet");
            Assert.IsTrue(products.Count() > 0, "Nuk ka asnje produkt");
        }

        [TestMethod]
        public void Test_002_CanFilterDataOK()
        {
            var products = _unitOfWork.ProductsRepository.Filter(1, "").Result;
            Assert.IsNotNull(products, "Nuk u lexuan dot produktet");
            Assert.IsTrue(products.Count > 0, "Nuk ka asnje produkt");
        }

        [TestMethod]
        public void Test_003_CanGetByIdOK()
        {
            var products = _unitOfWork.ProductsRepository.GetById(9);
            Assert.IsNotNull(products, "Nuk u lexua dot produkti");
        }
    }
}