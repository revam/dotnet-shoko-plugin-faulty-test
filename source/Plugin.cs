using System;
using Microsoft.Extensions.DependencyInjection;
using Shoko.Abstractions.Core;
using Shoko.Abstractions.Plugin;

namespace Shoko.Plugin.FaultyTest;

/// <summary>
/// A plugin that fails on purpose, at the start-up step named by the
/// <c>FAULTY_PLUGIN_MODE</c> environment variable, for testing how the server
/// handles a plugin that fails. Without the variable it does nothing.
/// </summary>
public class Plugin : IPlugin, IPluginServiceRegistration
{
    /// <summary>
    /// The environment variable naming the step to fail at.
    /// </summary>
    public const string ModeVariable = "FAULTY_PLUGIN_MODE";

    /// <summary>
    /// Creates the plugin, failing in the <c>constructor</c> mode.
    /// </summary>
    /// <exception cref="InvalidOperationException">The mode is <c>constructor</c>.</exception>
    public Plugin()
        => FailIn("constructor");

    /// <inheritdoc/>
    public Guid ID { get; private init; } = new("fa017000-0000-4000-8000-000000000001");

    /// <inheritdoc/>
    public string Name { get; private init; } = "Faulty Test";

    /// <inheritdoc/>
    public string Description { get; private init; } = """
        Fails on purpose at the start-up step named by FAULTY_PLUGIN_MODE, for testing how the server handles a failing plugin.
    """;

    /// <summary>
    /// Registers the hosted service of the <c>hosted</c> mode, failing in the
    /// <c>register</c> mode.
    /// </summary>
    /// <param name="serviceCollection">The service collection.</param>
    /// <param name="applicationPaths">The application paths.</param>
    /// <exception cref="InvalidOperationException">The mode is <c>register</c>.</exception>
    public static void RegisterServices(IServiceCollection serviceCollection, IApplicationPaths applicationPaths)
    {
        FailIn("register");
        if (IsMode("hosted"))
            serviceCollection.AddHostedService<FaultyHostedService>();
    }

    /// <inheritdoc/>
    /// <exception cref="InvalidOperationException">The mode is <c>setup</c>.</exception>
    public void Setup(IServiceProvider serviceProvider)
        => FailIn("setup");

    /// <inheritdoc/>
    /// <exception cref="InvalidOperationException">The mode is <c>ready</c>.</exception>
    public void Ready()
        => FailIn("ready");

    /// <summary>
    /// Whether the given mode is the one asked for.
    /// </summary>
    /// <param name="mode">The mode to check for.</param>
    /// <returns><c>true</c> when <c>FAULTY_PLUGIN_MODE</c> names it, ignoring case.</returns>
    internal static bool IsMode(string mode)
        => string.Equals(Environment.GetEnvironmentVariable(ModeVariable), mode, StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Throws when the given mode is the one asked for.
    /// </summary>
    /// <param name="mode">The mode to fail in.</param>
    /// <exception cref="InvalidOperationException">The mode is the one asked for.</exception>
    internal static void FailIn(string mode)
    {
        if (IsMode(mode))
            throw new InvalidOperationException($"The faulty test plugin failed on purpose ({mode}).");
    }
}
