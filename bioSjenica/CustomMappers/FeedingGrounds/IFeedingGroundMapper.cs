using bioSjenica.DTOs;
using bioSjenica.Models;

namespace bioSjenica.CustomMappers {
  public interface IFeedingGroundsMapper {
    public Task<FeedingGround> CreateToFeedingGround(CreateFeedingGroundDTO DTO);
    public Task<ReadFeedingGroundDTO> FeedingGroundToRead(FeedingGround feedingGround);
  }
}