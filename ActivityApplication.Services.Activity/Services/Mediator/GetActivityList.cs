using ActivityApplication.DataAccess.DbContext;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

namespace ActivityApplication.Services.Activity.Services.Mediator;

public class GetActivityList
{
    public class Query : IRequest<List<DataAccess.Activities.Activity>>
    {
    }

    public class Handler : IRequestHandler<Query, List<DataAccess.Activities.Activity>>
    {
        private readonly ApplicationDbContext _dbContext;

        public Handler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<DataAccess.Activities.Activity?>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await _dbContext.Activities
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
    }
}