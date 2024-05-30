using bioSjenica.DTOs;

namespace bioSjenica.Repositories {
  public interface IFeedingGroundRepository {
    public Task<ReadFeedingGroundDTO> Create(CreateFeedingGroundDTO feedingGroundPayload);
    public Task<List<ReadFeedingGroundDTO>> Get();
    public Task<ReadFeedingGroundDTO> Update(CreateFeedingGroundDTO feedingGroundPayload, int feedingGroundNumber);
    public Task<ReadFeedingGroundDTO> Delete(int feedingGroundNumber);
  }
}