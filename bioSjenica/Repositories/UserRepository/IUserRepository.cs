using bioSjenica.DTOs;
using bioSjenica.Models;

namespace bioSjenica.Repositories {
  public interface IUserRepository {
    public Task<List<ReadUserDTO>> Get();
    public Task<ReadUserDTO> GetBySsn(string SSN);
    public Task<ReadUserDTO> GetByEmail(string email);
    public Task<ReadUserDTO> Create(User user);
    public Task<ReadUserDTO> Delete(string SSN);
    public Task<ReadUserDTO> Update(User user, string SSN);
  }
}