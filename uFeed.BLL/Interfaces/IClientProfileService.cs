using System.Collections.Generic;
using uFeed.BLL.DTO;
using uFeed.BLL.Enums;

namespace uFeed.BLL.Interfaces
{
    public interface IClientProfileService
    {
        IEnumerable<ClientProfileDTO> GetAll();

        ClientProfileDTO Get(int id);

        void Create(ClientProfileDTO clientProfileDTO);

        void Update(ClientProfileDTO clientProfileDTO);

        void Delete(int id);

        void AddLogin(int clientId, Socials type);
    }
}
