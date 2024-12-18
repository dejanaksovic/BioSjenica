using bioSjenica.CustomMappers;
using bioSjenica.Data;
using bioSjenica.DTOs;
using bioSjenica.Exceptions;
using bioSjenica.Models;
using Microsoft.EntityFrameworkCore;
using bioSjenica.Utilities;

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

            try {
              await _sqlContext.SaveChangesAsync();
            }
            catch(Exception e) {
              throw new RequestException("Database error", errorCodes.INTERNAL_ERROR);
            }
            return _userMapper.UserToRead(user);
        }

        public async Task<ReadUserDTO> Delete(string SSN)
        {
          var userToDelete = _sqlContext.Users.FirstOrDefault(u => u.SSN == SSN);
          if(userToDelete is null) throw new RequestException("User not found", errorCodes.NOT_FOUND);

          _sqlContext.Users.Remove(userToDelete);
          
          try {
            await _sqlContext.SaveChangesAsync();
          }
          catch(Exception e) {
            throw new RequestException("Saving to db", errorCodes.INTERNAL_ERROR);
          }  

          return _userMapper.UserToRead(userToDelete);     
        }
        public async Task<List<ReadUserDTO>> Get()
        {
            List<User> users = await _sqlContext.Users.ToListAsync();

            return _userMapper.UserToReadList(users);
        }

        public async Task<ReadUserDTO> GetBySsn(string SSN) {
          var user = await _sqlContext.Users.FirstOrDefaultAsync(u => u.SSN == SSN);
          if(user is null) throw new RequestException("User not found", errorCodes.NOT_FOUND, ErrorDict.CreateDict("SSN", SSN));
          
          return _userMapper.UserToRead(user);
        }

        public async Task<ReadUserDTO> GetByEmail(string email) {
          var user = await _sqlContext.Users.FirstOrDefaultAsync(u => u.Email.ToString() == email);
          if(user is null) throw new RequestException("User not found", errorCodes.NOT_FOUND, ErrorDict.CreateDict("Email", email));

          return _userMapper.UserToRead(user);
        }

        public async Task<ReadUserDTO> Update(User userPayload, string SSN)
        {
            var userToUpdate = _sqlContext.Users.FirstOrDefault(u => u.SSN == SSN);
            if(userToUpdate is null) throw new RequestException("User not found", errorCodes.NOT_FOUND, ErrorDict.CreateDict("SSN", SSN));

            userToUpdate.SSN ??= userPayload.SSN;
            userToUpdate.FirstName ??= userPayload.FirstName;
            userToUpdate.LastName ??= userPayload.LastName;
            userToUpdate.Address ??= userPayload.Address;
            userToUpdate.PayGrade = userPayload.PayGrade != 0 ? userPayload.PayGrade : userToUpdate.PayGrade;
            userToUpdate.Role ??= userPayload.Role;

            try {
              await _sqlContext.SaveChangesAsync();
            }
            catch(Exception e) {
              throw new RequestException("Database error", errorCodes.INTERNAL_ERROR);
            }
            
            return _userMapper.UserToRead(userToUpdate);
        }
    }
}