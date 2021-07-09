using Insurance.BL.Model;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.BL.Auth
{
    public interface IUsersBL
    {
        Task<GlobalResponseDTO> Authenticate(string username, string password, HttpResponse response);
        string ReIssuetoken(string claimID, string roleID, HttpResponse response);
        Task<bool> ForgeryDetected(string token, int userID);
        Task<GlobalResponseDTO> ForgotPassword(string Username);
        Task<GlobalResponseDTO> ForgotUser(string email);
        Task<GlobalResponseDTO> CreateUser(UserCreationDTO userCreationDTO);
        Task<GlobalResponseDTO> ResetPassword(string guid, string password);
        Task<GlobalResponseDTO> ConfirmRegistration(string guid);
        Task<GlobalResponseDTO> HeartBeat(string claimID, string roleID, HttpResponse response);
    }
}
