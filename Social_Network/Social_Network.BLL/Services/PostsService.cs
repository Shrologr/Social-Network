using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Social_Network.BLL.Interfaces;
using Social_Network.BLL.DTO;
using Social_Network.BLL.Infrastructure;
using Social_Network.DAL.Interfaces;
using Social_Network.DAL.EF;
using AutoMapper;

namespace Social_Network.BLL.Services
{
    public class PostsService: IPostsService
    {
        private IUnitOfWork Database { get; set; }
        public PostsService(IUnitOfWork uow)
        {
            Database = uow;
        }
        public IEnumerable<PostsDTO> GetAllPosts()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Posts, PostsDTO>());
            var mapper = config.CreateMapper();
            return mapper.Map<IEnumerable<Posts>, IEnumerable<PostsDTO>>(Database.Posts.GetAll());
        }

        public IEnumerable<PostsDTO> GetUserPosts(int? id)
        {
            if (id == null)
            {
                throw new ValidationException("Id is null", "");
            }
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Posts, PostsDTO>());
            var mapper = config.CreateMapper();
            return mapper.Map<IEnumerable<Posts>, IEnumerable<PostsDTO>>(Database.Posts.Find(s=>s.User_ID == id.Value)).Reverse();
        }

        public PostsDTO GetPost(int? id)
        {
            if (id == null)
            {
                throw new ValidationException("Id is null", "");
            }
            var post = Database.Posts.Get(id.Value);
            if (post == null)
            {
                throw new ValidationException("No post with this id", "");
            }
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Posts, PostsDTO>());
            var mapper = config.CreateMapper();
            return mapper.Map<Posts, PostsDTO>(post);
        }

        public void CreatePost(PostsDTO post)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<PostsDTO, Posts>());
            var mapper = config.CreateMapper();
            var newPost = mapper.Map<PostsDTO, Posts>(post);
            Database.Posts.Create(newPost);
            Database.Save();
        }

        public IEnumerable<NetworkUsersDTO> GetPostLikes(int? id)
        {
            if (id == null)
            {
                throw new ValidationException("Id is null","");
            }
            var post = Database.Posts.Get(id.Value);
            if (post == null)
            {
                throw new ValidationException("No post with such id", "");
            }
            var config = new MapperConfiguration(cfg => cfg.CreateMap<NetworkUsers, NetworkUsersDTO>());
            var mapper = config.CreateMapper();
            return mapper.Map<IEnumerable<NetworkUsers>, IEnumerable<NetworkUsersDTO>>(post.NetworkUsers).Reverse();
        }

        public void ChangePostLike(int? userId, int? postId)
        {
            if (postId == null)
            {
                throw new ValidationException("PostId is null", "");
            }
            var post = Database.Posts.Get(postId.Value);
            if (post == null)
            {
                throw new ValidationException("No post with such id", "");
            }
            if (userId == null)
            {
                throw new ValidationException("UserId is null", "");
            }
            var user = Database.NetworkUsers.Get(userId.Value);
            if (user == null)
            {
                throw new ValidationException("No user with such id", "");
            }
            if (post.NetworkUsers.Contains(user))
            {
                post.NetworkUsers.Remove(user);
                user.Posts.Remove(post);
            }
            else 
            {
                post.NetworkUsers.Add(user);
                user.Posts.Add(post); 
            }
            Database.Save();
        }

        public void DeletePost(int? id)
        {
            if (id == null)
            {
                throw new ValidationException("Id is null", "");
            }
            if (Database.Posts.Get(id.Value) == null)
            {
                throw new ValidationException("Such post does not exist", "");
            }
            var users = Database.Posts.Get(id.Value).NetworkUsers;
            users.ToList().ForEach(
                s => 
                {
                    var item = s.Posts.FirstOrDefault(d => d.ID == id.Value);
                    if (item != null)
                    {
                        s.Posts.Remove(item);
                    }
                }
                );
            Database.Posts.Delete(id.Value);
            Database.Save();
        }

        public void Dispose()
        {
            Database.Save();
        }
    }
}