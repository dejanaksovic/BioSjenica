using bioSjenica.DTOs;
using bioSjenica.Models;

namespace bioSjenica.CustomMappers {
  public interface IFeedingGroundsMapper {
    public Task<FeedingGround> CreateToFeedingGround(CreateFeedingGroundDTO DTO);
    public ReadFeedingGroundDTO FeedingGroundToRead(FeedingGround feedingGround);
    public List<ReadFeedingGroundDTO> FeedingToReadList(List<FeedingGround> feedingGrounds);
  }
}