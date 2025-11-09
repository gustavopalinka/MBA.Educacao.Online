using MBA.Educacao.Online.Core.Messages;

namespace MBA.Educacao.Online.Core.Mediator;

public class DomainNotification : Event
{
    public DomainNotification(string key, string value)
    {
        Key = key;
        Value = value;
    }

    public string Key { get; }
    public string Value { get; }
}

