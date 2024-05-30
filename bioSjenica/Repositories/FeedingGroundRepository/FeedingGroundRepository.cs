using System.Runtime.CompilerServices;
using bioSjenica.CustomMappers;
using bioSjenica.Data;
using bioSjenica.DTOs;
using Microsoft.EntityFrameworkCore;

namespace bioSjenica.Repositories {
  public class FeedingGroundRepository : IFeedingGroundRepository
  {
      private readonly ILogger<FeedingGroundRepository> _logger;
      private readonly SqlContext _sqlContext;
      private readonly IFeedingGroundsMapper _feedingGroundMapper;
      public FeedingGroundRepository(SqlContext context, ILogger<FeedingGroundRepository> logger, IFeedingGroundsMapper feedingGroundsMapper) {
        _logger = logger;
        _sqlContext = context;
        _feedingGroundMapper = feedingGroundsMapper;
      }
      public async Task<ReadFeedingGroundDTO> Create(CreateFeedingGroundDTO feedingGroundPayload)
      {
        //TODO: HANDLE DATABASE ERRORS
        var feedingGroundToAdd = await _feedingGroundMapper.CreateToFeedingGround(feedingGroundPayload);
        _sqlContext.FeedingGorunds.Add(feedingGroundToAdd);
        await _sqlContext.SaveChangesAsync();

        return await _feedingGroundMapper.FeedingGroundToRead(feedingGroundToAdd);
      }
      public async Task<ReadFeedingGroundDTO> Delete(int feedingGroundNumber)
      {
        // TODO: Handle database errors
        var feedingGroundToDelete = await _sqlContext.FeedingGorunds.FirstOrDefaultAsync(fg => fg.GroundNumber == feedingGroundNumber);
        if(feedingGroundToDelete is null) {
          // TODO: Handle not found feeding ground;
          _logger.LogError("Feeding ground not found");
          throw new NotImplementedException();
        }
        _sqlContext.Remove(feedingGroundToDelete);
        await _sqlContext.SaveChangesAsync();

        return await _feedingGroundMapper.FeedingGroundToRead(feedingGroundToDelete);
      }
      public async Task<List<ReadFeedingGroundDTO>> Get()
      {
        // TODO: Handle database errors
        List<ReadFeedingGroundDTO> feedingGroundsToReturn = new List<ReadFeedingGroundDTO>();
        var feedingGrounds = await _sqlContext.FeedingGorunds.ToListAsync();
        foreach(var feedingGround in feedingGrounds) {
          feedingGroundsToReturn.Add(await _feedingGroundMapper.FeedingGroundToRead(feedingGround));
        }
        return feedingGroundsToReturn;
      }
      public async Task<ReadFeedingGroundDTO> Update(CreateFeedingGroundDTO feedingGroundPayload, int feedingGroundNumber)
      {
        //TODO: Handle database errors
        var feedingGroundToUpdate = _sqlContext.FeedingGorunds
                                    .Include(fg => fg.Region)
                                    .Include(fg => fg.Animals)
                                    .FirstOrDefault(fg => fg.GroundNumber == feedingGroundNumber);
        var newProps = await _feedingGroundMapper.CreateToFeedingGround(feedingGroundPayload);
        //Update
        feedingGroundToUpdate.GroundNumber = newProps.GroundNumber != 0 ? newProps.GroundNumber : feedingGroundToUpdate.GroundNumber;
        feedingGroundToUpdate.Region ??= newProps.Region;
        feedingGroundToUpdate.StartWork = (newProps.StartWork != DateTime.MinValue) ? newProps.StartWork : feedingGroundToUpdate.StartWork;
        feedingGroundToUpdate.EndWork = (newProps.EndWork != DateTime.MinValue) ? newProps.EndWork : feedingGroundToUpdate.EndWork;
        feedingGroundToUpdate.Animals = (newProps.Animals != null) ? newProps.Animals : feedingGroundToUpdate.Animals;

        await _sqlContext.SaveChangesAsync();
        return await _feedingGroundMapper.FeedingGroundToRead(feedingGroundToUpdate);
      }
  }
}