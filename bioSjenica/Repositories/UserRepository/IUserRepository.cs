using bioSjenica.Models;

namespace bioSjenica.Repositories {
  public interface IUserRepository {
    public Task<List<User>> Get();
    public Task<User> Create(User user);
    public Task<User> Delete(string SSN);
    public Task<User> Update(User user, string SSN);
  }
}