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
    public class BusinessLayerTest
    {
        [TestMethod]
        public void CreateDeleteTest()
        {
            IUserPhotoService photoService = null;
            IUserService userService = null;
            IPostsService postService = null;
            IUnitOfWork uow;
            try
            {
                uow = new EFUnitOfWork("name=SocialNetworkDatabaseContext");
                photoService = new UserPhotoService(uow);
                postService = new PostsService(uow);
                userService = new UserService(uow, postService, photoService);
                Console.WriteLine("Test start...");
                NetworkUsersDTO user = new NetworkUsersDTO()
                {
                    Name = "Ivan",
                    Surname = "Ivanov",
                    Birthday = new DateTime(1994, 12, 3),
                    Gender = "Male",
                    Mail = "someMail@mail.ru",
                    URL = "",
                    User_Password = "petuh2"
                };
                userService.CreateUser(user);
                var someUser = userService.GetUser("someMail@mail.ru", "petuh2");
                PostsDTO post1 = new PostsDTO()
                {
                    Image = System.IO.File.ReadAllBytes("D:\\plus.png"),
                    Text = "Ecolog - ebanoe dno",
                    User_ID = someUser.ID,
                    Poster_ID = someUser.ID,
                    Date = DateTime.Now
                };
                postService.CreatePost(post1);
                var somePost = postService.GetAllPosts().First();
                postService.ChangePostLike(someUser.ID, somePost.ID);

                var like = postService.GetPostLikes(somePost.ID).First();
                Assert.IsTrue(someUser.ID == like.ID);

                postService.ChangePostLike(someUser.ID, somePost.ID);                
                Assert.IsFalse(postService.GetPostLikes(somePost.ID).Any());

                userService.DeleteUser(someUser.ID);
                Assert.IsTrue(userService.GetAllUsers().Count() == 0);
                Assert.IsTrue(postService.GetAllPosts().Count() == 0);
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
