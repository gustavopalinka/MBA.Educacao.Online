using MBA.Educacao.Online.Core.Mediator;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace MBA.Educacao.Online.API.Controllers;

[ApiController]
public abstract class MainController : ControllerBase
{
    private readonly DomainNotificationHandler _notifications;

    protected MainController(DomainNotificationHandler notifications,
                             IMediatorHandler mediatorHandler)
    {
        _notifications = notifications;
        MediatorHandler = mediatorHandler;
    }

    protected IMediatorHandler MediatorHandler { get; }

    protected bool OperacaoValida() => !_notifications.HasNotifications();

    protected ActionResult CustomResponse(object? result = null)
    {
        if (OperacaoValida())
        {
            if (result == null)
            {
                return Ok();
            }

            return Ok(result);
        }

        var response = new ResponseResult
        {
            Errors = _notifications
                .GetNotifications()
                .Select(n => n.Value)
                .ToList()
        };

        return BadRequest(response);
    }

    protected async Task NotificarErroModelInvalido()
    {
        foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
        {
            var errorMessage = error.Exception == null ? error.ErrorMessage : error.Exception.Message;
            await MediatorHandler.PublicarNotificacao(new DomainNotification(string.Empty, errorMessage));
        }
    }
}

