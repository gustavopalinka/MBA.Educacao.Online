using System.Collections.Generic;
using System.Linq;

namespace MBA.Educacao.Online.Core.Mediator;

public class ResponseResult
{
    public object? Data { get; set; }
    public List<string> Errors { get; set; } = new();
    public bool IsValid => !Errors.Any();

    public void AddError(string message)
    {
        Errors.Add(message);
    }
}

