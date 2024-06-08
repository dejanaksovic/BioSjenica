using bioSjenica.CustomMappers;
using bioSjenica.Data;
using bioSjenica.DTOs;
using bioSjenica.Exceptions;
using bioSjenica.Models;
using Microsoft.EntityFrameworkCore;

namespace bioSjenica.Repositories {
    public class UserRepository : IUserRepository
    {
      private readonly ILogger<UserRepository> _logger;
      private readonly SqlContext _sqlContext;
      private readonly IUserMapper _userMapper;
      public UserRepository(ILogger<UserRepository> logger, SqlContext sqlContext, IUserMapper userMapper)
      {
        _logger = logger;
        _sqlContext = sqlContext;
        _userMapper = userMapper;
      }
        public async Task<ReadUserDTO> Create(User user)
        {
            _sqlContext.Users.Add(user);
            await _sqlContext.SaveChangesAsync();
            return await _userMapper.UserToRead(user);
        }

        public async Task<ReadUserDTO> Delete(string SSN)
        {
          var userToDelete = _sqlContext.Users.FirstOrDefault(u => u.SSN == SSN);
          if(userToDelete is null) {
            throw new NotFoundException("User");
          }
          _sqlContext.Users.Remove(userToDelete);
          await _sqlContext.SaveChangesAsync();
          return await _userMapper.UserToRead(userToDelete);     
        }
        public async Task<List<ReadUserDTO>> Get()
        {
            List<User> users = await _sqlContext.Users.ToListAsync();

            List<ReadUserDTO> usersToReturn = [];
            foreach (User user in users) {
              usersToReturn.Add(await _userMapper.UserToRead(user));
            }
            return usersToReturn;
        }

        public async Task<ReadUserDTO> GetBySsn(string SSN) {
          var user = await _sqlContext.Users.FirstOrDefaultAsync(u => u.SSN == SSN);
          if(user is null)
            throw new NotImplementedException();
          return await _userMapper.UserToRead(user);
        }

        public async Task<ReadUserDTO> GetByEmail(string email) {
          var user = await _sqlContext.Users.FirstOrDefaultAsync(u => u.Email.ToString() == email);
          if(user is null)
            throw new NotImplementedException();
          return await _userMapper.UserToRead(user);
        }

        public async Task<ReadUserDTO> Update(User userPayload, string SSN)
        {
            var userToUpdate = _sqlContext.Users.FirstOrDefault(u => u.SSN == SSN);
            if(userToUpdate is null) {
              throw new NotFoundException("User");
            }
            userToUpdate.SSN ??= userPayload.SSN;
            userToUpdate.FirstName ??= userPayload.FirstName;
            userToUpdate.LastName ??= userPayload.LastName;
            userToUpdate.Address ??= userPayload.Address;
            userToUpdate.PayGrade = userPayload.PayGrade != 0 ? userPayload.PayGrade : userToUpdate.PayGrade;
            userToUpdate.Role ??= userPayload.Role;

            await _sqlContext.SaveChangesAsync();
            return await _userMapper.UserToRead(userToUpdate);
        }
    }
}