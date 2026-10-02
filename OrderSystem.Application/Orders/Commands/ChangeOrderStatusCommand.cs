using MediatR;
using OrderSystem.Application.Interfaces;
using OrderSystem.Domain;
using OrderSystem.Domain.Entities;
using OrderSystem.Domain.Enums;
using System.Threading;
using System.Threading.Tasks;

namespace OrderSystem.Application.Orders.Commands;

public class ChangeOrderStatusCommand : IRequest<bool>
{
    public int OrderId { get; set; }
    public OrderStatus NewStatus { get; set; }
}

public class ChangeOrderStatusCommandHandler : IRequestHandler<ChangeOrderStatusCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public ChangeOrderStatusCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(ChangeOrderStatusCommand request, CancellationToken cancellationToken)
    {
        var order = new Order { Id = request.OrderId, Status = OrderStatus.NewOrder };

        if (order.Status == OrderStatus.Cancelled)
        {
            return false;
        }

        order.ChangeStatus(request.NewStatus);

        _context.Update(order);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}