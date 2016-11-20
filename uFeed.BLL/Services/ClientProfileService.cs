using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using uFeed.BLL.DTO;
using uFeed.BLL.Enums;
using uFeed.BLL.Infrastructure.Exceptions;
using uFeed.BLL.Interfaces;
using uFeed.DAL.Interfaces;
using uFeed.Entities;

namespace uFeed.BLL.Services
{
    public class ClientProfileService : IClientProfileService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ClientProfileService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<ClientProfileDTO> GetAll()
        {
            var clientProfiles = _unitOfWork.ClientProfiles.GetAll();
            var clientProfilesDTO = Mapper.Map<IEnumerable<ClientProfileDTO>>(clientProfiles);

            return clientProfilesDTO;
        }

        public ClientProfileDTO Get(int userId)
        {
            var clientProfile = _unitOfWork.ClientProfiles.GetByUserId(userId);
            var clientProfileDTO = Mapper.Map<ClientProfileDTO>(clientProfile);

            return clientProfileDTO;
        }

        public void Create(ClientProfileDTO clientProfileDTO)
        {
            var clientProfile = Mapper.Map<ClientProfile>(clientProfileDTO);

            _unitOfWork.ClientProfiles.Create(clientProfile);
            _unitOfWork.Save();
        }

        public void Update(ClientProfileDTO clientProfileDTO)
        {
            var clientProfile = _unitOfWork.ClientProfiles.GetByUserId(clientProfileDTO.UserId);

            if (clientProfile == null)
            {
                throw new EntityNotFoundException("Client profile doesn't exist", "ClientProfile");
            }

            Mapper.Map(clientProfileDTO, clientProfile);

            AssignCategoriesToClient(clientProfile, clientProfileDTO);

            _unitOfWork.ClientProfiles.Update(clientProfile);
            _unitOfWork.Save();
        }

        public void Delete(int userId)
        {
            var clientProfile = _unitOfWork.ClientProfiles.GetByUserId(userId);

            if (clientProfile == null)
            {
                throw new EntityNotFoundException("Cannot find client profile", "ClientProfile");
            }

            _unitOfWork.ClientProfiles.Delete(clientProfile.Id);
            _unitOfWork.Save();
        }

        public void AddLogin(int userId, Socials type)
        {
            var clientProfile = _unitOfWork.ClientProfiles.GetByUserId(userId);

            if (clientProfile == null)
            {
                throw new EntityNotFoundException("Cannot find client profile", "ClientProfile");
            }

            var dalType = (Entities.Enums.Socials)type;

            if (clientProfile.Logins == null)
            {
                clientProfile.Logins = new List<Login>();
            }

            if (!clientProfile.Logins.Select(l => l.LoginType).Contains(dalType))
            {
                clientProfile.Logins.Add(new Login { ClientProfileId = clientProfile.Id, LoginType = dalType });
            }

            _unitOfWork.ClientProfiles.Update(clientProfile);
            _unitOfWork.Save();
        }

        public void RemoveLogin(int userId, Socials type)
        {
            var clientProfile = _unitOfWork.ClientProfiles.GetByUserId(userId);

            if (clientProfile == null)
            {
                throw new EntityNotFoundException("Cannot find client profile", "ClientProfile");
            }

            var dalType = (Entities.Enums.Socials)type;

            if (clientProfile.Logins != null && 
                clientProfile.Logins.Select(l => l.LoginType).Contains(dalType))
            {
                _unitOfWork.Logins.Delete(clientProfile.Logins.First(l => l.LoginType.Equals(dalType)).Id);
                _unitOfWork.Save();
            }
        }

        private void AssignCategoriesToClient(ClientProfile destinationClientProfile, 
            ClientProfileDTO sourceClientProfileDTO)
        {
            var categoryIds = sourceClientProfileDTO.Categories.Select(category => category.Id);
            var categories = _unitOfWork.Categories.GetAll()
                .Where(category => categoryIds.Any(categoryId => category.Id == categoryId))
                .ToList();

            if (categories.Count != sourceClientProfileDTO.Categories.Count)
            {
                throw new EntityNotFoundException("Cannot find some categories", "Category");
            }

            destinationClientProfile.Categories = categories;
        }
    }
}
