using AutoMapper;
using eCommerce.Core.DTO;
using eCommerce.Core.Entities;
using eCommerce.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Infrastructure.Repository
{

    public class UsersService : IUsersService
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IMapper _mapper;

        public UsersService(IUsersRepository usersRepository,IMapper mapper) 
        {
            _usersRepository=usersRepository;
            _mapper=mapper;
        }

        public async Task<UserDTO?> GetUserByUserID(int userID)
        {
            ApplicationUser? user = await _usersRepository.GetUserByUserID(userID);
            if (user == null) { return null; }
            UserDTO? userDTO = _mapper.Map<UserDTO>(user);
            return userDTO;
        }

        public async Task<AuthenticationResponse?> Login(LoginRequest loginRequest)
        {
            ApplicationUser? user = await _usersRepository.GetUserByEmailAndPassword(loginRequest.Email, loginRequest.Password);

            if (user == null)
            {
                return null;
            }

            //return new AuthenticationResponse(user.UserID, user.Email, user.PersonName, user.Gender, "token", Success: true);
            return _mapper.Map<AuthenticationResponse>(user);
        }

        public Task<AuthenticationResponse?> Register(RegisterRequest registerRequest)
        {
            throw new NotImplementedException();
        }
    }
}
