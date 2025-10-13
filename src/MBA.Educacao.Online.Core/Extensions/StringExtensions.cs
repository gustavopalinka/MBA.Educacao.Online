namespace MBA.Educacao.Online.Core.Extensions;

public static class StringExtensions
{
    public static Guid ToGuid(this string value)
    {
        return Guid.TryParse(value, out var guid) ? guid : Guid.Empty;
    }
}