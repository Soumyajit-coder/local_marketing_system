using localMarketingSystem.DAL.Entities;
using localMarketingSystem.DTOs;

namespace localMarketingSystem.BAL.Interfaces
{
    public interface IUserService
    {
        public Task<int> InsertNewUser(CreateUserDTO createUserDTO);
        public Task<int> updateUserDetails(UpdateUserDTO updateUserDTO);
        public Task<List<UserListDTO>> getAllUserDetails();
        public Task<MUser> UserDeatilsByEmail(string email);
        public Task<List<UserListDTO>> UserByStatus(bool statusId);
        public Task<bool> AssignUser(int userId, string role);
        public Task<string> GetRoleByUserId(int userId);
    }
}
