using bioSjenica.DTOs;
using bioSjenica.Models;

namespace bioSjenica.CustomMappers {
    public class UserMapper : IUserMapper
    {
        public ReadUserDTO UserToRead(User user)
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

        public List<ReadUserDTO> UserToReadList(List<User> users) {
          var usersToReturn = new List<ReadUserDTO>();

          foreach(var user in users) {
            usersToReturn.Add(this.UserToRead(user));
          }

          return usersToReturn;
        }
    }
}