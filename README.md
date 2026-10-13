# Shoko Faulty Test Plugin

A plugin that fails on purpose, for testing how Shoko Server handles a plugin
that fails while the server is starting. It is never released: there is no
manifest and no release workflow.

Set `FAULTY_PLUGIN_MODE` in the server's environment to choose where it fails.
Without the variable the plugin loads and does nothing.

| Mode | Where it throws | What the server should do |
|---|---|---|
| `register` | `RegisterServices` | Fail the plugin, halt the start with the web server up |
| `constructor` | The plugin's constructor | The same |
| `export` | The constructor of an export (`FaultyIgnoreRule`) | The same, naming the export |
| `setup` | `Setup` | The same |
| `ready` | `Ready` | The same |
| `hosted` | A hosted service, as the web host starts | Exit; the next start leaves the plugin out and comes up in failure mode saying why; the start after tries it again |

In failure mode `GET /api/v3/Init/Status` reports `Failed` and names the plugin,
and `GET /api/v3/Plugin?failed=Only` lists it with its `FailureReason`.

The plugin cannot stand in for one built against an older contract; for that,
install a release built for an older `Shoko.Abstractions`, such as Offline
Importer v1.1.1.

## Building

```
dotnet build source -c Release
```

Copy `source/bin/Release/net10.0/Shoko.Plugin.FaultyTest.dll` into a folder of
its own under the server's `plugins` folder. To build against a Shoko checkout
instead of the published package, pass `-p:ShokoRepositoryPath=/path/to/Shoko`.
