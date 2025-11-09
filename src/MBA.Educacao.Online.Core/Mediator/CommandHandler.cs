using FluentValidation.Results;
using System.Threading.Tasks;

namespace MBA.Educacao.Online.Core.Mediator;

public abstract class CommandHandler
{
    protected CommandHandler(IMediatorHandler mediatorHandler)
    {
        MediatorHandler = mediatorHandler;
    }

    protected IMediatorHandler MediatorHandler { get; }

    protected async Task NotificarErros(ValidationResult validationResult)
    {
        foreach (var error in validationResult.Errors)
        {
            await NotificarErro(string.Empty, error.ErrorMessage);
        }
    }

    protected async Task NotificarErro(string key, string mensagem)
    {
        await MediatorHandler.PublicarNotificacao(new DomainNotification(key, mensagem));
    }
}

