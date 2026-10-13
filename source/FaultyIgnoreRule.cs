using System;
using System.IO;
using Shoko.Abstractions.Video;

namespace Shoko.Plugin.FaultyTest;

/// <summary>
/// An export the server creates while handing the plugins' parts to its
/// services, which cannot be created in the <c>export</c> mode. It ignores
/// nothing.
/// </summary>
public class FaultyIgnoreRule : IManagedFolderIgnoreRule
{
    /// <summary>
    /// Creates the rule, failing in the <c>export</c> mode.
    /// </summary>
    /// <exception cref="InvalidOperationException">The mode is <c>export</c>.</exception>
    public FaultyIgnoreRule()
        => Plugin.FailIn("export");

    /// <inheritdoc/>
    public string Name => "Faulty Test";

    /// <inheritdoc/>
    public bool ShouldIgnore(IManagedFolder folder, FileSystemInfo fileSystemInfo)
        => false;
}
