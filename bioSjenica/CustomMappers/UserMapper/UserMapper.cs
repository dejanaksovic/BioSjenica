using bioSjenica.DTOs;
using bioSjenica.Models;

namespace bioSjenica.CustomMappers {
    public class UserMapper : IUserMapper
    {
        public async Task<ReadUserDTO> UserToRead(User user)
        {
            return new ReadUserDTO{
              FirstName = user.FirstName,
              LastName = user.LastName,
              SSN = user.SSN,
              Email = user.Email,
              Address = user.Address,
              PayGrade = user.PayGrade,
              Role = user.Role,
            };
        }
    }
}