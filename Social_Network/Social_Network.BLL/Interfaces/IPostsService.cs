using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Social_Network.BLL.DTO;

namespace Social_Network.BLL.Interfaces
{
    public interface IPostsService
    {
        IEnumerable<PostsDTO> GetAllPosts();
        IEnumerable<PostsDTO> GetUserPosts(int? id);
        PostsDTO GetPost(int? id);
        void CreatePost(PostsDTO post);
        IEnumerable<NetworkUsersDTO> GetPostLikes(int? id);
        void ChangePostLike(int? userId, int? postId);
        void DeletePost(int? id);
        void Dispose();
    }
}