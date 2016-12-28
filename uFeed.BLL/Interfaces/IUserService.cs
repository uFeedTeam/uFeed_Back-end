using System.Collections.Generic;
using uFeed.BLL.DTO;
using uFeed.BLL.Enums;

namespace uFeed.BLL.Interfaces
{
    public interface IUserService
    {
        IEnumerable<UserDTO> GetAll();

        UserDTO Get(int userId);

        UserDTO GetByEmail(string email);

        void Update(UserDTO userDTO);

        void Delete(int userId);

        void AddLogin(int userId, Socials type);

        void RemoveLogin(int userId, Socials type);

        UserDTO Login(string email, string password);

        void Register(string email, string name, string password, byte[] photo = null);
    }
}
