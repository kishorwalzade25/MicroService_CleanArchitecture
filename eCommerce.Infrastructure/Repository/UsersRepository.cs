using eCommerce.Core.Entities;
using eCommerce.Core.Repository;
using eCommerce.Infrastructure.eCommerceDbContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Infrastructure.Repository
{
    public class UsersRepository : IUsersRepository
    {
        private readonly eCommDbContext _context;

        public UsersRepository(eCommDbContext context) 
        {
            _context=context;
        }


        public async Task<ApplicationUser?> AddUser(ApplicationUser user)
        {
            await _context.ApplicationUsers.AddAsync(user);
            await _context.SaveChangesAsync();
            if(user == null) 
            {
                return null;
            }
            return user;
        }

        public async Task<ApplicationUser?> GetUserByEmailAndPassword(string? email, string? password)
        {
            var user =await _context.ApplicationUsers.Where(u => u.Email == email && u.Password == password).FirstOrDefaultAsync();
            if (user == null) { return null; }
            return user;
        }

        public async Task<ApplicationUser?> GetUserByUserID(int? userID)
        {
            var user = await _context.ApplicationUsers.FirstOrDefaultAsync(x=>x.UserID == userID);
            if (user == null) { return null; }
            return user;
        }
    }
}
