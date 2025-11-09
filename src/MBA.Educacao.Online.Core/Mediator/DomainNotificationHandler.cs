using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MBA.Educacao.Online.Core.Mediator;

public class DomainNotificationHandler : INotificationHandler<DomainNotification>, IDisposable
{
    private readonly List<DomainNotification> _notifications = new();

    public Task Handle(DomainNotification notification, CancellationToken cancellationToken)
    {
        _notifications.Add(notification);
        return Task.CompletedTask;
    }

    public virtual IReadOnlyCollection<DomainNotification> GetNotifications() => _notifications.AsReadOnly();

    public virtual bool HasNotifications() => _notifications.Any();

    public void Dispose() => _notifications.Clear();
}

