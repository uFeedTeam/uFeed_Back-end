using System.Collections.Generic;
using uFeed.BLL.DTO;
using uFeed.BLL.Enums;

namespace uFeed.BLL.Interfaces
{
    public interface IClientProfileService
    {
        IEnumerable<ClientProfileDTO> GetAll();

        ClientProfileDTO Get(int userId);

        void Create(ClientProfileDTO clientProfileDTO);

        void Update(ClientProfileDTO clientProfileDTO);

        void Delete(int userId);

        void AddLogin(int userId, Socials type);

        void RemoveLogin(int userId, Socials type);
    }
}
