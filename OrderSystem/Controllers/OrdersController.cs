using Microsoft.AspNetCore.Mvc;
using MediatR;
using OrderSystem.Domain;
using OrderSystem.Domain.Enums;
using OrderSystem.Application.Orders.Commands;
using OrderSystem.Infrastructure.Services;
using System.Threading.Tasks;

namespace OrderSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly TelegramBotService _telegramBot;

    public OrdersController(IMediator mediator, TelegramBotService telegramBot)
    {
        _mediator = mediator;
        _telegramBot = telegramBot;
    }

    [HttpPost("{id}/status")]
    public async Task<IActionResult> ChangeStatus(int id, [FromBody] OrderStatus newStatus)
    {
        var command = new ChangeOrderStatusCommand { OrderId = id, NewStatus = newStatus };
        var result = await _mediator.Send(command);

        if (!result)
        {
            return BadRequest("Cannot change status of a cancelled order.");
        }

        // Відправляємо сповіщення менеджеру в Telegram
        await _telegramBot.SendNotificationAsync($"Order #{id} status changed to {newStatus}");

        return Ok($"Status updated to {newStatus}");
    }
}