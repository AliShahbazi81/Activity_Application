using ActivityApplication.DataAccess.DbContext;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ActivityApplication.Services.Activity.Services.Mediator;

public class GetActivity
{
    public class Query : IRequest<DataAccess.Entities.Activities.Activity>
    {
        public Guid Id { get; set; }
    }

    public class Handler : IRequestHandler<Query, DataAccess.Entities.Activities.Activity>
    {
        private readonly ApplicationDbContext _dbContext;

        public Handler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<DataAccess.Entities.Activities.Activity?> Handle(Query request, CancellationToken cancellationToken)
        {
            return await _dbContext.Activities
                .FindAsync(request.Id);
        }
    }
}