using bioSjenica.DTOs;
using bioSjenica.Models;

namespace bioSjenica.CustomMappers {
    public class UserMapper : IUserMapper
    {
        public ReadUserDTO UserToRead(User user)
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

        public List<ReadUserDTO> UserToReadList(List<User> users) {
          var usersToReturn = new List<ReadUserDTO>();

          foreach(var user in users) {
            usersToReturn.Add(this.UserToRead(user));
          }

          return usersToReturn;
        }
    }
}