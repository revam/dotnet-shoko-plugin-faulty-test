using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Shoko.Plugin.FaultyTest;

/// <summary>
/// A hosted service that throws as the web host starts, registered only in
/// the <c>hosted</c> mode. Nothing can catch this one in time: the server
/// exits, and the next start leaves the plugin out and says why.
/// </summary>
public class FaultyHostedService : IHostedService
{
    /// <inheritdoc/>
    /// <exception cref="InvalidOperationException">Always.</exception>
    public Task StartAsync(CancellationToken cancellationToken)
        => throw new InvalidOperationException("The faulty test plugin failed on purpose (hosted).");

    /// <inheritdoc/>
    public Task StopAsync(CancellationToken cancellationToken)
        => Task.CompletedTask;
}
