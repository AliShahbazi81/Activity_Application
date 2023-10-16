using ActivityApplication.DataAccess.DbContext;
using ActivityApplication.Services.Activity.DTO;
using MediatR;

namespace ActivityApplication.Services.Activity.Services.Mediator;

public class CreateActivity
{
    public class Command : IRequest
    {
        public DataAccess.Entities.Activities.Activity Activity { get; set; }
    }

    public class Handler : IRequestHandler<Command>
    {
        private readonly ApplicationDbContext _dbContext;

        public Handler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            _dbContext.Activities.Add(request.Activity);

            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}