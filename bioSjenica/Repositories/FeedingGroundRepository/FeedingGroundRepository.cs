using System.Runtime.CompilerServices;
using bioSjenica.CustomMappers;
using bioSjenica.Data;
using bioSjenica.DTOs;
using bioSjenica.Exceptions;
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
          _logger.LogError("Feeding ground not found");
          throw (RequestException)new NotFoundException("Feeding ground");
        }
        _sqlContext.Remove(feedingGroundToDelete);
        await _sqlContext.SaveChangesAsync();

        return await _feedingGroundMapper.FeedingGroundToRead(feedingGroundToDelete);
      }
      public async Task<List<ReadFeedingGroundDTO>> Get(int? month)
      {
        // TODO: Handle database errors
        List<ReadFeedingGroundDTO> feedingGroundsToReturn = new List<ReadFeedingGroundDTO>();
        var feedingGrounds = await _sqlContext.FeedingGorunds.ToListAsync();
        if(!(month is null) && month != 0) {
          feedingGrounds = feedingGrounds.Where(fg => fg.StartWork >= month && fg.EndWork <= month).ToList();
          if(feedingGrounds.Count() == 0) {
            throw new NotFoundException("Feeding ground");
          }
        }
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
        if(feedingGroundToUpdate is null) {
          throw (RequestException)new NotFoundException("Feeding ground");
        }
        var newProps = await _feedingGroundMapper.CreateToFeedingGround(feedingGroundPayload);
        //Update
        feedingGroundToUpdate.GroundNumber = newProps.GroundNumber != 0 ? newProps.GroundNumber : feedingGroundToUpdate.GroundNumber;
        feedingGroundToUpdate.Region ??= newProps.Region;
        feedingGroundToUpdate.StartWork = (newProps.StartWork != 0) ? newProps.StartWork : feedingGroundToUpdate.StartWork;
        feedingGroundToUpdate.EndWork = (newProps.EndWork != 0) ? newProps.EndWork : feedingGroundToUpdate.EndWork;
        feedingGroundToUpdate.Animals = (newProps.Animals != null) ? newProps.Animals : feedingGroundToUpdate.Animals;

        await _sqlContext.SaveChangesAsync();
        return await _feedingGroundMapper.FeedingGroundToRead(feedingGroundToUpdate);
      }
  }
}