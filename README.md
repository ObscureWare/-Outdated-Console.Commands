# Console.Commands
*![PayPal](https://github.com/ObscureWare/Console.Commands/blob/master/doc/pp64.png) If you find this library useful please consider [donating](https://www.paypal.me/SebastianGruchacz) to support my development.*

*![Nuget](https://github.com/ObscureWare/Console.Commands/blob/master/doc/nugetlogo.png) You can find Nuget [here](https://www.nuget.org/packages/ObscureWare.Console.Commands/)*

Or install from Nuget commandline:

>Install-Package ObscureWare.Console.Commands

## Description
Current version provides developer with:
- declarative syntax for commands and command model. *(Note: This syntax is being still under development.)*
- separation between commands and model
- separation bewteen commands and output (basic stuff only at the moment, coming soon!)
- separation between commands and synatx
- automatically generated help pages (from model annotations)
- syntax and help pages automatically adapt to engine settings
See more details below.

# Samples
Current configuration (while very crude and screaming for refactoring is following):
- Switch literal and value shall be separated. *(OptionArgumentMode = CommandOptionArgumentMode.Separated)*
- All flag literals shall start with '-' or '\'. *(FlagCharacters = new[] {@"\", "-"})* [I would recommend using only one literal.]
- All switch literals shall start with '--' or '-'. *(SwitchCharacters = new[] {@"-", "--"})* [I would recommend using only one literal.]
- Command names ignore character casing *(CommandsSensitivenes = CommandCaseSensitivenes.Insensitive)*
- Setting for free-roaming / switchless arguments is not yet supported - they can actually be placed wverywhere, they will be not consumed by other arguments, including Endonly. *(SwitchlessOptionsMode = SwitchlessOptionsMode.EndOnly)*
- It does not allow to groub flag switch literals together. *(AllowFlagsAsOneArgument = false)*

## Configuring command engine
1. Of required objects - one must provide engine with context object, that implements *ICommandEngineContext* interface and is being shared between commands to manage application's state:
```csharp
public class ConsoleContext : ICommandEngineContext
{
    public bool ShallFinishInteracativeSession { get; set; }

    public string GetCurrentPrompt()
    {
        string dir = Environment.CurrentDirectory;
        if (!dir.EndsWith("\\"))
        {
            dir += "\\";
        }
        return dir;
    }
}
```
2. Also, console instance shall be created. Might as well have colors and settings altered.
```csharp
ConsoleContext context = new ConsoleContext();
```
3. Then configuration of engine parsing options shall be compilled:
```csharp
var options = new CommandParserOptions
    {
        UiCulture = CultureInfo.CreateSpecificCulture("en-US"), // "pl-PL"
        FlagCharacters = new[] {@"\", "-"},
        SwitchCharacters = new[] {@"-", "--"},
        OptionArgumentMode = CommandOptionArgumentMode.Separated,
        OptionArgumentJoinCharacater = ':', // not used because of: CommandOptionArgumentMode.Separated
        AllowFlagsAsOneArgument = false,
        CommandsSensitivenes = CommandCaseSensitivenes.Insensitive,
        SwitchlessOptionsMode = SwitchlessOptionsMode.EndOnly
    };
```

4. Then engine might be build, using fluent syntax:
```csharp
var engine = CommandEngineBuilder.Build()
            .WithCommandsFromAssembly(this.GetType().Assembly)
            .UsingOptions(options)
            .UsingStyles(CommandEngineStyles.DefaultStyles)
            .ConstructForConsole(console);
```

(TODO: more info will in wiki. when completed...)

5. When commands are declared properly (see sections below) no erros shall be reported by the builder and one might start engine:
```csharp
engine.Run(context);
```
It will run, until some command sets *context.ShallFinishInteracativeSession* to TRUE.

## Configuring commands
Sample command might be declared as follwoing (simple implementation of DIR):
```csharp
[CommandModel(typeof(DirCommandModel))]
public class DirCommand : IConsoleCommand
{
    public void Execute(object contextObject, ICommandOutput output, object runtimeModel)
    {
        var model = runtimeModel as DirCommandModel; // necessary to avoid Generic-inheritance troubles...

        // TODO: custom filters normalization?

        switch (model.Mode)
        {
            case DirectoryListMode.CurrentDir:
            {
                this.ListCurrentFolder(contextObject, output, model);
                break;
            }
            case DirectoryListMode.CurrentLocalState:
                break;
            case DirectoryListMode.CurrentRemoteHead:
                break;
            default:
                break;
        }
    }

    private void ListCurrentFolder(object contextObject, ICommandOutput output, DirCommandModel parameters)
    {
        string filter = string.IsNullOrWhiteSpace(parameters.Filter) ? "*.*" : parameters.Filter;
        string basePath = Environment.CurrentDirectory;

        DataTable<string> filesTable = new DataTable<string>(
            new ColumnInfo("Name", ColumnAlignment.Left),
                new ColumnInfo("Size", ColumnAlignment.Right),
                new ColumnInfo("Modified", ColumnAlignment.Right));

            var baseDir = new DirectoryInfo(basePath);
            if (parameters.IncludeFolders)
            {
                var dirs = baseDir.GetDirectories(filter, SearchOption.TopDirectoryOnly);
                foreach (var dirInfo in dirs)
                {
                    filesTable.AddRow(
                        dirInfo.FullName,
                        new []
                            {
                                dirInfo.Name,
                                "<DIR>",
                                Directory.GetLastWriteTime(dirInfo.FullName).ToString(output.UiCulture.DateTimeFormat.ShortDatePattern)
                            });
                }
            }

            var files = baseDir.GetFiles(filter, SearchOption.TopDirectoryOnly);
            foreach (var fileInfo in files)
            {
                filesTable.AddRow(
                    fileInfo.FullName,
                    new []
                        {
                            fileInfo.Name,
                            fileInfo.Length.ToFriendlyXBytesText(output.UiCulture),
                            File.GetLastWriteTime(fileInfo.FullName).ToString(output.UiCulture.DateTimeFormat.ShortDatePattern)
                        });
            }

            output.PrintSimpleTable(filesTable);
        }
    }
```

## Configuring syntax

```csharp
[CommandDescriptorFor(typeof(DirCommand))]
[CommandName("dir")]
[CommandDescription(@"Lists files withing current folder or repository state, depending on selected options.")]
public class DirCommandModel : CommandModel
{
    [CommandOptionName(@"includeFolders")]
    [Mandatory(false)]
    [CommandOptionFlag("d", "D")]
    [CommandDescription("When set, specifies whether directories shall be listed too.")]
    public bool IncludeFolders { get; set; }

    [CommandOptionName(@"mode")]
    [Mandatory(false)]
    [CommandOptionSwitch(typeof(DirectoryListMode), "m", DefaultValue = DirectoryListMode.CurrentDir)]
    [CommandDescription("Specifies which predefined directory location shall be listed.")]
    // TODO: list help for switches. Get from enumeration itself? Allow coloring syntax? Somehow...
    // TODO: more switch types? string?
    // TODO: runtime support switch auto-complete. Sourced through ModelBuilder & Parser
    public DirectoryListMode Mode { get; set; }

    [CommandOptionName(@"filter")]
    [Mandatory(false)]
    [CommandOptionSwitchless(0)]
    [CommandDescription("Specifies filter for enumerated files. Does not apply to folders.")]
    // TODO: runtime support for some values / unnamed values auto-completion? sourced through command itself...
    public string Filter { get; set; }
    
    // TODO: add sorting argument
}
```

## Working with command line
Now, when user requests help to the command, it will be displayed, using current engine settings:
![Help Sample](https://github.com/ObscureWare/Console.Commands/blob/master/doc/help_sample_01.png)
Actually, help switch could be placed anywhere in the command line to work.

It uses simple tabled results (initial, very basic functions) to present results:
![Execution Sample](https://github.com/ObscureWare/Console.Commands/blob/master/doc/help_sample_02.png)

The command will also be listed together with other discovered / configured command, when asked:
![Help Main Sample](https://github.com/ObscureWare/Console.Commands/blob/master/doc/help_sample_03.png)
