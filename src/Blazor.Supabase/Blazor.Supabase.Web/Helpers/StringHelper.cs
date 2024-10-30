using FluentResults;

namespace BlazorSupabase.Web.Helpers;

public static class StringHelper
{
    public static string ToStringError(this List<IError> errors)
    {
        if (errors == null || errors.Count == 0)
        {
            return string.Empty;
        }

        return string.Join(',', errors.Select(e => e.Message));
    }
}
