using bioSjenica.Data;
using bioSjenica.Exceptions;
using bioSjenica.Models;
using Microsoft.EntityFrameworkCore;

namespace bioSjenica.Repositories {
    public class UserRepository : IUserRepository
    {
      private readonly ILogger<UserRepository> _logger;
      private readonly SqlContext _sqlContext;
      public UserRepository(ILogger<UserRepository> logger, SqlContext sqlContext)
      {
        _logger = logger;
        _sqlContext = sqlContext;
      }
        public async Task<User> Create(User user)
        {
            _sqlContext.Users.Add(user);
            await _sqlContext.SaveChangesAsync();
            return user;
        }

        public async Task<User> Delete(string SSN)
        {
          var userToDelete = _sqlContext.Users.FirstOrDefault(u => u.SSN == SSN);
          if(userToDelete is null) {
            throw new NotFoundException("User");
          }
          _sqlContext.Users.Remove(userToDelete);
          await _sqlContext.SaveChangesAsync();
          return userToDelete;     
        }
        public async Task<List<User>> Get()
        {
            return await _sqlContext.Users.ToListAsync();
        }

        public async Task<User> Update(User userPayload, string SSN)
        {
            var userToUpdate = _sqlContext.Users.FirstOrDefault(u => u.SSN == SSN);
            if(userToUpdate is null) {
              throw new NotFoundException("User");
            }
            userToUpdate.SSN ??= userPayload.SSN;
            userToUpdate.FistName ??= userPayload.FistName;
            userToUpdate.LastName ??= userPayload.LastName;
            userToUpdate.Address ??= userPayload.Address;
            userToUpdate.PayGrade ??= userPayload.PayGrade;
            userToUpdate.Role ??= userPayload.Role;

            await _sqlContext.SaveChangesAsync();
            return userToUpdate;
        }
    }
}