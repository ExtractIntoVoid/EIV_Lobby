using Serilog;
using Serilog.Core;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace LobbyLib.Managers;

/// <summary>
/// Provides a better logging.
/// </summary>
public static class LogManager
{
    /// <summary>
    /// Gets the serilog logger.
    /// </summary>
    public static Serilog.ILogger? Logger { get; private set; }

    /// <summary>
    /// Gets the main level switch.
    /// </summary>
    public static LoggingLevelSwitch LevelSwitch { get; private set; } = new(Serilog.Events.LogEventLevel.Verbose);

    /// <summary>
    /// Gets the unity level switch.
    /// </summary>
    public static LoggingLevelSwitch ConsoleLevelSwitch { get; private set; } = new(Serilog.Events.LogEventLevel.Verbose);

    /// <summary>
    /// Gets the file level switch.
    /// </summary>
    public static LoggingLevelSwitch FileLevelSwitch { get; private set; } = new(Serilog.Events.LogEventLevel.Verbose);

    /// <summary>
    /// Create new <see cref="Logger"/>.
    /// </summary>
    public static void Create()
    {
        Create(GetConfiguration());
    }

    /// <summary>
    /// Creates new  <see cref="Logger"/>.
    /// </summary>
    /// <param name="config">The config to use.</param>
    public static void Create([NotNull] LoggerConfiguration config)
    {
        Logger = config.CreateLogger();
        Logger.Information("--- START ----");
        Log.Logger = Logger;
    }

    /// <summary>
    /// Gets the default configuration.
    /// </summary>
    /// <returns>The default configuration.</returns>
    public static LoggerConfiguration GetConfiguration()
    {
        return new LoggerConfiguration()
            .MinimumLevel.ControlledBy(LevelSwitch)
            .WriteTo.File(
                "logs/lobby_log_.txt",
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
                levelSwitch: FileLevelSwitch,
                formatProvider: CultureInfo.InvariantCulture,
                rollingInterval: RollingInterval.Hour)
            .WriteTo.Console(levelSwitch: ConsoleLevelSwitch, formatProvider: CultureInfo.InvariantCulture)
            ;
    }

    /// <summary>
    /// Close the <see cref="Logger"/>.
    /// </summary>
    public static void Close()
    {
        if (Logger == null)
        {
            return;
        }

        Logger.Information("--- STOP -----");
        Log.CloseAndFlush();
        Logger = null;
    }
}
