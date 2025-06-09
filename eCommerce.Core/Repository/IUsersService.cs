using eCommerce.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Core.Repository
{
    public interface IUsersService
    {
        Task<AuthenticationResponse?> Login(LoginRequest loginRequest);
        Task<AuthenticationResponse?> Register(RegisterRequest registerRequest);

        Task<UserDTO?> GetUserByUserID(int userID);
    }
}
