using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using uFeed.BLL.DTO;
using uFeed.BLL.Enums;
using uFeed.BLL.Infrastructure;
using uFeed.BLL.Infrastructure.Exceptions;
using uFeed.BLL.Interfaces;
using uFeed.DAL.Interfaces;
using uFeed.Entities;

namespace uFeed.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<UserDTO> GetAll()
        {
            var users = _unitOfWork.Users.GetAll();
            var usersDto = Mapper.Map<IEnumerable<UserDTO>>(users);

            return usersDto;
        }

        public UserDTO Get(int id)
        {
            var user = _unitOfWork.Users.Get(id);
            var userDto = Mapper.Map<UserDTO>(user);

            return userDto;
        }

        public UserDTO GetByEmail(string email)
        {
            var user = _unitOfWork.Users.Find(x => x.Email.Equals(email, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            var userDto = Mapper.Map<UserDTO>(user);

            return userDto;
        }

        public void Update(UserDTO userDTO)
        {
            var user = _unitOfWork.Users.Get(userDTO.Id);

            if (user == null)
            {
                throw new EntityNotFoundException("User doesn't exist", "User");
            }

            Mapper.Map(userDTO, user);

            AssignCategoriesToClient(user, userDTO);

            _unitOfWork.Users.Update(user);
            _unitOfWork.Save();
        }

        public void Delete(int id)
        {
            var user = _unitOfWork.Users.Get(id);

            if (user == null)
            {
                throw new EntityNotFoundException("Can`t find user with such id", "User");
            }

            _unitOfWork.Users.Delete(user.Id);
            _unitOfWork.Save();
        }

        public void AddLogin(int id, Socials type)
        {
            var clientProfile = _unitOfWork.Users.Get(id);

            if (clientProfile == null)
            {
                throw new EntityNotFoundException("Cannot find client profile", "User");
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

            _unitOfWork.Users.Update(clientProfile);
            _unitOfWork.Save();
        }

        public void RemoveLogin(int id, Socials type)
        {
            var clientProfile = _unitOfWork.Users.Get(id);

            if (clientProfile == null)
            {
                throw new EntityNotFoundException("Cannot find client profile", "User");
            }

            var dalType = (Entities.Enums.Socials)type;

            if (clientProfile.Logins != null && 
                clientProfile.Logins.Select(l => l.LoginType).Contains(dalType))
            {
                _unitOfWork.Logins.Delete(clientProfile.Logins.First(l => l.LoginType.Equals(dalType)).Id);
                _unitOfWork.Save();
            }
        }

        public UserDTO Login(string email, string password)
        {
            var user = _unitOfWork.Users.Find(x => x.Email == email).SingleOrDefault();

            if (user == null)
            {
                throw new EntityNotFoundException("No such user in the databse.", "User");
            }

            if (!CryptoProvider.VerifyMd5Hash(password, user.PasswordHash))
            {
                throw new ArgumentException("Invalid password");
            }

            var userDto = Mapper.Map<User, UserDTO>(user);
            return userDto;
        }

        public void Register(string email, string name, string password, byte[] photo = null)
        {
            var hash = CryptoProvider.GetMd5Hash(password);

            var user = new User
            {
                Name = name,
                Email = email,
                PasswordHash = hash,
                Photo = photo
            };

            _unitOfWork.Users.Create(user);
            _unitOfWork.Save();
        }

        private void AssignCategoriesToClient(User destinationClientProfile, 
            UserDTO sourceUserDTO)
        {
            var categoryIds = sourceUserDTO.Categories.Select(category => category.Id);
            var categories = _unitOfWork.Categories.GetAll()
                .Where(category => categoryIds.Any(categoryId => category.Id == categoryId))
                .ToList();

            if (categories.Count != sourceUserDTO.Categories.Count)
            {
                throw new EntityNotFoundException("Cannot find some categories", "Category");
            }

            destinationClientProfile.Categories = categories;
        }
    }
}
