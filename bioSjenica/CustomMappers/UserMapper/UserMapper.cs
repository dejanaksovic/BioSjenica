using bioSjenica.DTOs;
using bioSjenica.Models;

namespace bioSjenica.CustomMappers {
    public class UserMapper : IUserMapper
    {
        public async Task<ReadUserDTO> UserToRead(User user)
        {
          return new ReadUserDTO() {
            SSN = user.SSN,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Address = user.Address,
            Role = user.Role,
            Email = user.Email,
            PayGrade = user.PayGrade,
          };
        }
    }
}