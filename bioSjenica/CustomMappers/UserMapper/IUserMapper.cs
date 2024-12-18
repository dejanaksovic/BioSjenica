using bioSjenica.DTOs;
using bioSjenica.Models;

namespace bioSjenica.CustomMappers {
  public interface IUserMapper {
    public ReadUserDTO UserToRead(User user);
    public List<ReadUserDTO> UserToReadList(List<User> users);
    }
}
