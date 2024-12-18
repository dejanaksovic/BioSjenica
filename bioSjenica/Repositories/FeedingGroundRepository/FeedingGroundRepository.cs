using bioSjenica.Utilities;
using bioSjenica.CustomMappers;
using bioSjenica.Data;
using bioSjenica.DTOs;
using bioSjenica.Exceptions;
using Microsoft.EntityFrameworkCore;
using bioSjenica.Models;

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

        try {
          await _sqlContext.SaveChangesAsync();
        }
        catch(Exception e) {
          throw new RequestException("Error saving to DB", errorCodes.INTERNAL_ERROR);

        }
        return _feedingGroundMapper.FeedingGroundToRead(feedingGroundToAdd);
      }
      public async Task<ReadFeedingGroundDTO> Delete(int feedingGroundNumber)
      {
        var feedingGroundToDelete = await _sqlContext.FeedingGorunds.FirstOrDefaultAsync(fg => fg.GroundNumber == feedingGroundNumber);
        if(feedingGroundToDelete is null) throw new RequestException("Feeding ground not found", errorCodes.NOT_FOUND, ErrorDict.CreateDict("GroundNumber", feedingGroundNumber));

        _sqlContext.FeedingGorunds.Remove(feedingGroundToDelete);
        
        try {
          await _sqlContext.SaveChangesAsync();
        }
        catch(Exception e) {
          throw new RequestException("Error savign to DB", errorCodes.INTERNAL_ERROR);
        }

        return _feedingGroundMapper.FeedingGroundToRead(feedingGroundToDelete);
      }
      public async Task<List<ReadFeedingGroundDTO>> Get(int? month)
      {
        // TODO: Handle database errors
        List<FeedingGround>? feedingGrounds; 
        if(month is not null && month != 0) {
          feedingGrounds = await _sqlContext.FeedingGorunds.Where(fg => fg.StartWork >= month && fg.EndWork <= month).ToListAsync();
        }
        else {
          feedingGrounds = await _sqlContext.FeedingGorunds.ToListAsync();
        }

        return _feedingGroundMapper.FeedingToReadList(feedingGrounds);
      }
      public async Task<ReadFeedingGroundDTO> Update(CreateFeedingGroundDTO feedingGroundPayload, int feedingGroundNumber)
      {
        var feedingGroundToUpdate = _sqlContext.FeedingGorunds
                                    .Include(fg => fg.Region)
                                    .Include(fg => fg.Animals)
                                    .FirstOrDefault(fg => fg.GroundNumber == feedingGroundNumber);
        if(feedingGroundToUpdate is null) throw new RequestException("Feeding ground not found", errorCodes.NOT_FOUND);

        var newProps = await _feedingGroundMapper.CreateToFeedingGround(feedingGroundPayload);
        //Update
        feedingGroundToUpdate.GroundNumber = newProps.GroundNumber != 0 ? newProps.GroundNumber : feedingGroundToUpdate.GroundNumber;
        feedingGroundToUpdate.Region ??= newProps.Region;
        feedingGroundToUpdate.StartWork = (newProps.StartWork != 0) ? newProps.StartWork : feedingGroundToUpdate.StartWork;
        feedingGroundToUpdate.EndWork = (newProps.EndWork != 0) ? newProps.EndWork : feedingGroundToUpdate.EndWork;
        feedingGroundToUpdate.Animals = (newProps.Animals != null) ? newProps.Animals : feedingGroundToUpdate.Animals;

        try {
          await _sqlContext.SaveChangesAsync();
        }
        catch(Exception e) {
          throw new RequestException("Error saving to db", errorCodes.INTERNAL_ERROR);
        }
        
        return _feedingGroundMapper.FeedingGroundToRead(feedingGroundToUpdate);
      }
  }
}