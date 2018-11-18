using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sample03.E3SClient.Entities;
using Sample03.E3SClient;
using System.Configuration;
using System.Linq;
using Moq;

namespace Sample03
{
    [TestClass]
    public class E3SProviderTests
    {
        //[TestMethod]
        //public void WithoutProvider()
        //{
        //    var client = new E3SQueryClient(ConfigurationManager.AppSettings["user"], ConfigurationManager.AppSettings["password"]);
        //    var res = client.SearchFTS<EmployeeEntity>("workstation:(EPRUIZHW0249)", 0, 1);

        //    foreach (var emp in res)
        //    {
        //        Console.WriteLine("{0} {1}", emp.nativename, emp.startworkdate);
        //    }
        //}

        //[TestMethod]
        //public void WithoutProviderNonGeneric()
        //{
        //    var client = new E3SQueryClient(ConfigurationManager.AppSettings["user"], ConfigurationManager.AppSettings["password"]);
        //    var res = client.SearchFTS(typeof(EmployeeEntity), "workstation:(EPRUIZHW0249)", 0, 10);

        //    foreach (var emp in res.OfType<EmployeeEntity>())
        //    {
        //        Console.WriteLine("{0} {1}", emp.nativename, emp.startworkdate);
        //    }
        //}


        [TestMethod]
        public void WithProvider()
        {
            var _mock = new Mock<E3SQueryClient>(ConfigurationManager.AppSettings["user"], ConfigurationManager.AppSettings["password"]);
            var employees = new E3SEntitySet<EmployeeEntity>(ConfigurationManager.AppSettings["user"], ConfigurationManager.AppSettings["password"]);
            _mock.Setup(x => x.SearchFTS(It.IsAny<Type>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Returns(new List<EmployeeEntity>() { new EmployeeEntity("Yana", "05/05/2017") });
            employees.Provider = new E3SLinqProvider(_mock.Object);

            foreach (var emp in employees.Where(e => e.workstation == "EPRUIZHW0249"))
            {
                Console.WriteLine("{0} {1}", emp.nativename, emp.startworkdate);
            }
        }

        [TestMethod]
        public void ReversePredicate()
        {
            var _mock = new Mock<E3SQueryClient>(ConfigurationManager.AppSettings["user"], ConfigurationManager.AppSettings["password"]);
            var employees = new E3SEntitySet<EmployeeEntity>(ConfigurationManager.AppSettings["user"], ConfigurationManager.AppSettings["password"]);
            _mock.Setup(x => x.SearchFTS(It.IsAny<Type>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Returns(new List<EmployeeEntity>() { new EmployeeEntity("Yana", "05/05/2017") });
            employees.Provider = new E3SLinqProvider(_mock.Object);

            foreach (var emp in employees.Where(e => "EPRUIZHW0249" == e.workstation))
            {
                Console.WriteLine("{0} {1}", emp.nativename, emp.startworkdate);
            }
        }

        [TestMethod]
        public void StartsWith()
        {
            var _mock = new Mock<E3SQueryClient>(ConfigurationManager.AppSettings["user"], ConfigurationManager.AppSettings["password"]);
            var employees = new E3SEntitySet<EmployeeEntity>(ConfigurationManager.AppSettings["user"], ConfigurationManager.AppSettings["password"]);
            _mock.Setup(x => x.SearchFTS(It.IsAny<Type>(), "workstation:(EPRUIZHW006*)", It.IsAny<int>(), It.IsAny<int>())).Returns(new List<EmployeeEntity>() { new EmployeeEntity("Vova", "06/05/2017") });
            employees.Provider = new E3SLinqProvider(_mock.Object);

            foreach (var emp in employees.Where(e => e.workstation.StartsWith("EPRUIZHW006")))
            {
                Console.WriteLine("{0} {1}", emp.nativename, emp.startworkdate);
            }
        }

        [TestMethod]
        public void EndsWith()
        {
            var _mock = new Mock<E3SQueryClient>(ConfigurationManager.AppSettings["user"], ConfigurationManager.AppSettings["password"]);
            var employees = new E3SEntitySet<EmployeeEntity>(ConfigurationManager.AppSettings["user"], ConfigurationManager.AppSettings["password"]);
            _mock.Setup(x => x.SearchFTS(It.IsAny<Type>(), "workstation:(*IZHW0060)", It.IsAny<int>(), It.IsAny<int>())).Returns(new List<EmployeeEntity>() { new EmployeeEntity("Petya", "06/05/2017") });
            employees.Provider = new E3SLinqProvider(_mock.Object);

            foreach (var emp in employees.Where(e => e.workstation.EndsWith("IZHW0060")))
            {
                Console.WriteLine("{0} {1}", emp.nativename, emp.startworkdate);
            }
        }

        [TestMethod]
        public void Contains()
        {
            var _mock = new Mock<E3SQueryClient>(ConfigurationManager.AppSettings["user"], ConfigurationManager.AppSettings["password"]);
            var employees = new E3SEntitySet<EmployeeEntity>(ConfigurationManager.AppSettings["user"], ConfigurationManager.AppSettings["password"]);
            _mock.Setup(x => x.SearchFTS(It.IsAny<Type>(), "workstation:(*IZHW006*)", It.IsAny<int>(), It.IsAny<int>())).Returns(new List<EmployeeEntity>() { new EmployeeEntity("Tanya", "06/05/2017") });
            employees.Provider = new E3SLinqProvider(_mock.Object);

            foreach (var emp in employees.Where(e => e.workstation.Contains("IZHW006")))
            {
                Console.WriteLine("{0} {1}", emp.nativename, emp.startworkdate);
            }
        }
    }
}