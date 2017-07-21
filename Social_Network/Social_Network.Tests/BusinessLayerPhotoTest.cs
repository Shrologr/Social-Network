using System;
using System.Linq;
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
    public class BusinessLayerPhotosTest
    {
        [TestMethod]
        public void PhotoCreateCascadeDeleteTest()
        {
            try
            {
                IUserPhotoService service;
                UserPhotosDTO photo;
                {
                    service = new UserPhotoService(new EFUnitOfWork("name=SocialNetworkDatabaseContext"));
                    Console.WriteLine("Test start...");
                    var tempService = new UserService(new EFUnitOfWork("name=SocialNetworkDatabaseContext"));
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
                    tempService.CreateUser(user);
                    photo = new UserPhotosDTO()
                    {
                        Image = System.IO.File.ReadAllBytes("D:\\plus.png"),
                        User_ID = tempService.GetAllUsers().First().ID
                    };
                    service.CreatePhoto(photo);
                    Console.WriteLine("Photo created and saved...");
                    photo = service.GetAllPhotos().First();
                    Assert.IsNotNull(photo);
                    ///
                    user = new NetworkUsersDTO()
                    {
                        Name = "Petro",
                        Surname = "Ivanov",
                        Birthday = new DateTime(1994, 12, 3),
                        Gender = "Male",
                        Mail = "someOtherMail@mail.ru",
                        URL = "",
                        Photo_ID = 0,
                        User_Password = "petuh4"
                    };
                    tempService.CreateUser(user);
                    photo = new UserPhotosDTO()
                    {
                        Image = System.IO.File.ReadAllBytes("D:\\stop.png"),
                        User_ID = tempService.GetAllUsers().Last().ID
                    };
                    service.CreatePhoto(photo);
                    var photos = service.GetUserPhotos(tempService.GetAllUsers().Last().ID);
                    Assert.IsTrue(photos.Count() == 1);
                    foreach (var item in tempService.GetAllUsers())
                    {
                        tempService.DeleteUser(item.ID);
                    }
                    photos = service.GetAllPhotos();
                    Assert.IsTrue(photos.Count() == 0);
                    tempService.Dispose();
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
