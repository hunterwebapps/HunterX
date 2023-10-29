using Serilog;

namespace HunterX.Trader.Common.Logging;

public static class Logger
{
    public static void Information(string messageTemplate, params object[]? propertyValues)
    {
        Log.Information(messageTemplate, propertyValues);
    }

    public static void Warning(string messageTemplate, params object[]? propertyValues)
    {
        Log.Warning(messageTemplate, propertyValues);
    }

    public static void Error(string messageTemplate, params object[]? propertyValues)
    {
        Log.Error(messageTemplate, propertyValues);
    }

    public static void Error(Exception exception, string messageTemplate, params object[]? propertyValues)
    {
        Log.Error(exception, messageTemplate, propertyValues);
    }
}
