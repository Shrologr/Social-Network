using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Social_Network.BLL.DTO;
using Social_Network.BLL.Infrastructure;
using Social_Network.BLL.Interfaces;
using Social_Network.BLL.Services;
using Social_Network.DAL.EF;
using Social_Network.DAL.Implementation;
using Social_Network.DAL.Interfaces;


namespace Social_Network.Tests
{
    [TestClass]
    public class BusinessLayerTest
    {
        [TestMethod]
        public void UserCreateTest()
        {
            try
            {
                using (IUnitOfWork uow = new EFUnitOfWork("name=SocialNetworkDatabaseContext"))
                {
                    Console.WriteLine("Test start...");
                    NetworkUsers user = new NetworkUsers()
                    {
                        Name = "Ivan",
                        Surname = "Ivanov",
                        Birthday = new DateTime(1994, 12, 3),
                        Gender = "Male",
                        Mail = "",
                        URL = "",
                        Photo_ID = 0,
                        User_Password = ""
                    };
                    uow.NetworkUsers.Create(user);
                    Console.WriteLine("User created...");
                    uow.Save();
                    Console.WriteLine("User saved...");
                    Assert.IsNotNull(uow.NetworkUsers.Find(s => s.Name == "Ivan"));

                    uow.Dispose();
                }
            }
            catch(Exception ex)
            {
                System.IO.File.WriteAllText("file.txt",ex.Message);
                Console.WriteLine(ex.Message);
                Assert.Fail();
            }
        }
    }
}
