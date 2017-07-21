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
    public class BusinessLayerUserTest
    {
        [TestMethod]
        public void UserCreateUpdateDeleteTest()
        {
            try
            {
                IUserService service;
                NetworkUsersDTO userTest;
                {
                    service = new UserService(new EFUnitOfWork("name=SocialNetworkDatabaseContext"));
                    Console.WriteLine("Test start...");
                    NetworkUsersDTO user = new NetworkUsersDTO()
                    {
                        Name = "Ivan",
                        Surname = "Ivanov",
                        Birthday = new DateTime(1994, 12, 3),
                        Gender = "Male",
                        Mail = "someMail@mail.ru",
                        URL = "",
                        Photo_ID = 0,
                        User_Password = "petuh2"
                    };
                    service.CreateUser(user);
                    Console.WriteLine("User created and saved...");
                    userTest = service.GetUser("someMail@mail.ru", "petuh2");
                    Assert.IsNotNull(userTest);
                    service.Dispose();
                }
                userTest.Mail = "Lalka1222@gmail.com";
                ///
                {
                    service = new UserService(new EFUnitOfWork("name=SocialNetworkDatabaseContext"));
                    service.UpdateUser(userTest);
                    userTest = service.GetUser(userTest.ID);
                    Assert.AreEqual(userTest.Mail, "Lalka1222@gmail.com");
                    ///
                    service.DeleteUser(userTest.ID);
                    userTest = service.GetUser("someMail@mail.ru", "petuh2");
                    Assert.IsNull(userTest);
                    service.Dispose();
                }
            }
            catch (Exception ex)
            {
                System.IO.File.WriteAllText("file.txt", ex.Message);
                Console.WriteLine(ex.Message);
                Assert.Fail();
            }
        }
    }
}
