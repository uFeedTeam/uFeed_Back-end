using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using uFeed.BLL.DTO;
using uFeed.BLL.Infrastructure.Exceptions;
using uFeed.BLL.Interfaces;
using uFeed.DAL.Entities;
using uFeed.DAL.Interfaces;

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

        public ClientProfileDTO Get(int id)
        {
            var clientProfile = _unitOfWork.ClientProfiles.Get(id);
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
            var clientProfile = _unitOfWork.ClientProfiles.Get(clientProfileDTO.Id);

            if (clientProfile == null)
            {
                throw new EntityNotFoundException("Client profile doesn't exist", "ClientProfile");
            }

            Mapper.Map(clientProfileDTO, clientProfile);

            AssignCategoriesToClient(clientProfile, clientProfileDTO);

            _unitOfWork.ClientProfiles.Update(clientProfile);
            _unitOfWork.Save();
        }

        public void Delete(int id)
        {
            if (_unitOfWork.ClientProfiles.Get(id) == null)
            {
                throw new EntityNotFoundException("Cannot find client profile", "ClientProfile");
            }

            _unitOfWork.ClientProfiles.Delete(id);
            _unitOfWork.Save();
        }

        private void AssignCategoriesToClient(ClientProfile destinationClientProfile, ClientProfileDTO sourceClientProfileDTO)
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
