using bioSjenica.DTOs;
using bioSjenica.Models;

namespace bioSjenica.CustomMappers {
  public interface IUserMapper {
    public Task<ReadUserDTO> UserToRead(User user);
  }
}