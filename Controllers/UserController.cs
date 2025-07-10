using localMarketingSystem.BAL.Interfaces;
using localMarketingSystem.DTOs;
using localMarketingSystem.Enum;
using localMarketingSystem.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace localMarketingSystem.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("create-user")]
        public async Task<APIResponseHelper<string>> CreateNewUser(CreateUserDTO createUserDTO)
        {
            APIResponseHelper<string> response = new APIResponseHelper<string>();
            try
            {
                string[] roles = { "Admin", "Client" };
                if (!roles.Contains(createUserDTO.Role))
                {
                    response.apiResponseStatus = Enum.APIResponseStatus.Error;
                    response.Message = "Invalid Role";
                    return response;
                }
                int createNewUser = await _userService.InsertNewUser(createUserDTO);
                if (createNewUser != 0)
                {
                    response.apiResponseStatus = Enum.APIResponseStatus.Success;
                    response.Message = "Inserted Succesfully.";
                    return response;
                } else
                {
                    response.apiResponseStatus = Enum.APIResponseStatus.Error;
                    response.Message = "Faild to Insert!";
                    return response;
                }
            } catch (Exception ex) 
            {
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.Message = ex.Message;
                return response;
            }
        }
        [AllowAnonymous]
        [HttpPut("update-user")]
        public async Task<APIResponseHelper<string>> UpdateUserDetails(UpdateUserDTO updateUserDTO)
        {
            APIResponseHelper<string> response = new APIResponseHelper<string>();
            try
            {
                int updateExistingUser = await _userService.updateUserDetails(updateUserDTO);
                if (updateExistingUser != 0)
                {
                    response.apiResponseStatus = Enum.APIResponseStatus.Success;
                    response.Message = "Updated Succesfully.";
                    return response;
                }
                else
                {
                    response.apiResponseStatus = Enum.APIResponseStatus.Error;
                    response.Message = "Faild to Update!";
                    return response;
                }
            } 
            catch (Exception ex) 
            {
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.Message = ex.Message;
                return response;
            }
        }
        [AllowAnonymous]
        [HttpGet("get-all-users")]
        public async Task<APIResponseHelper<List<UserListDTO>>> GetAllUser()
        {
            APIResponseHelper<List<UserListDTO>> response = new APIResponseHelper<List<UserListDTO>>();
            try
            {
                List<UserListDTO> getUsers = await _userService.getAllUserDetails();
                if (getUsers == null)
                {
                    response.apiResponseStatus = Enum.APIResponseStatus.Error;
                    response.Message = "No Data Found!";
                    return response;
                } else 
                {
                    response.result = getUsers;
                    response.apiResponseStatus = Enum.APIResponseStatus.Success;
                    response.Message = "";
                    return response;
                }
            }
            catch (Exception ex) 
            {
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.Message = ex.Message;
                return response;
            }
        }
    }
}
