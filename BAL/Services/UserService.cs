using AutoMapper;
using localMarketingSystem.BAL.Interfaces;
using localMarketingSystem.DAL.Entities;
using localMarketingSystem.DAL.Interfaces;
using localMarketingSystem.DTOs;
using localMarketingSystem.Helpers;
using Microsoft.AspNetCore.Identity;

namespace localMarketingSystem.BAL.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _UserRepository;
        private readonly IMapper _mapper;
        public UserService(IUserRepository UserRepository, IMapper mapper)
        {
            _UserRepository = UserRepository;
            _mapper = mapper;
        }
        public async Task<int> InsertNewUser(CreateUserDTO createUserDTO)
        {
            byte[] passwordHash, passwordSalt;
            PasswordHelper.CreatePasswordHash(createUserDTO.Password, out passwordHash, out passwordSalt);
            MUser user = new MUser
            {
                UserId = createUserDTO.UserId,
                UserName = createUserDTO.UserName,
                Email = createUserDTO.Email,
                MobileNo = createUserDTO.MobileNo,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Role = createUserDTO.Role,
                Status = createUserDTO.Status,
                CreatedAt = DateTime.Now
            };

            if (_UserRepository.Add(user))
            {
                _UserRepository.SaveChangesManaged();
                return user.UserId;
            }
            return 0;
        }
        public async Task<MUser> UserDeatilsByEmail(string email)
        {
            MUser userDetails = await _UserRepository.GetSingleAysnc(entity => entity.Email == email);
            return userDetails;
        }
        public async Task<List<UserListDTO>> UserByStatus(bool statusId)
        {
            List<UserListDTO> userListDTO = (List<UserListDTO>)await _UserRepository.GetSelectedColumnByConditionAsync(entity => entity.Status == statusId, en => new UserListDTO
            {
                UserId = en.UserId,
                UserName = en.UserName,
                MobileNo = en.MobileNo,
            });
            return userListDTO;
        }

        public async Task<bool> AssignUser(int userId, string role)
        {
            MUser usersHasRole = new MUser
            {
                Role = role,
                UserId = userId
            };
            if (_UserRepository.Add(usersHasRole))
            {
                _UserRepository.SaveChangesManaged();
                return true;
            }
            return false;
        }
        public async Task<string> GetRoleByUserId(int userId)
        {
            string roleDetails = await _UserRepository.GetSingleSelectedColumnByConditionAsync(entity => entity.UserId == userId, entity => entity.Role);
            return roleDetails;
        }
        public async Task<List<UserListDTO>> getAllUserDetails()
        {
            var allUserDetails = await _UserRepository.GetAllAsync();
            var getAllUsers = _mapper.Map<List<UserListDTO>>(allUserDetails);

            return getAllUsers;
        }
        public async Task<int> updateUserDetails(UpdateUserDTO updateUserDTO)
        {
            ArgumentNullException.ThrowIfNull(updateUserDTO, $"The argument of this name {nameof(updateUserDTO)} is null");
            var existingUser = await _UserRepository.GetDetailsAsync(user => user.UserId == updateUserDTO.UserId, true);
            if (existingUser == null)
            {
                throw new Exception($"No student details has found with this ID: {updateUserDTO.UserId}");
            }
            else
            {
                var userUpdate = _mapper.Map<MUser>(updateUserDTO);
                userUpdate.UpdatedAt = DateTime.Now;
                if (_UserRepository.Update(userUpdate))
                {
                    _UserRepository.SaveChangesManaged();
                    return userUpdate.UserId;
                }
                return 0;
            }
        }
    }
}
