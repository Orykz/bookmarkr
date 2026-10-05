using System.CommandLine;


namespace bookmarkr;

class Program
{
    // initializing the option objects
    private static readonly Option<string> nameOption = new("--name", ["-n"])
    {
        Required = true,
        Description = "The name of the bookmark"
    };
    private static readonly Option<string> urlOption = new("--url", ["-u"])
    {
        Required = true,
        Description = "The URL of the bookmark"
    };
    private static readonly Option<bool> listOption = new("--list", ["-l"])
    {
        Description = "Lists all of the bookmarks"
    };
    private static readonly Option<string> categoryOption = new("--category", ["-c"])
    {
        Description = "The category which the bookmark is associated"  
    };

    static async Task<int> Main(string[] args)
    {
        var service = new BookmarkService();

        // initializing the command objects
        var rootCommand = new RootCommand("Bookmarkr is a bookmark manager provided as a CLI application.");
        var linkCommand = new Command("link", "Manage bookmark links");
        var addLinkCommand = new Command("add", "Add a new bookmark link");
        var removeLinkCommand = new Command("remove", "Remove an existing bookmark");
        var updateLinkCommand = new Command("update", "Update the url of an existing bookmark");

        // linking the options to the commands
        addLinkCommand.Options.Add(nameOption);
        addLinkCommand.Options.Add(urlOption);
        addLinkCommand.Options.Add(categoryOption);
        removeLinkCommand.Options.Add(nameOption);
        updateLinkCommand.Options.Add(nameOption);
        updateLinkCommand.Options.Add(urlOption);
        updateLinkCommand.Options.Add(categoryOption);
        linkCommand.Options.Add(listOption);

        // linking the commands to each other
        linkCommand.Subcommands.Add(addLinkCommand);
        linkCommand.Subcommands.Add(removeLinkCommand);
        linkCommand.Subcommands.Add(updateLinkCommand);
        rootCommand.Subcommands.Add(linkCommand);

        // handling the function call of commands
        addLinkCommand.SetAction(parseResult => OnHandleAddLinkCommand(parseResult, service));
        removeLinkCommand.SetAction(parseResult => OnHandleRemoveLinkCommand(parseResult, service));
        updateLinkCommand.SetAction(parseResult => OnHandleUpdateLinkCommand(parseResult, service));
        linkCommand.SetAction(parseResult =>
        {
            var isList = parseResult.GetValue(listOption);
            if (isList)
            {
                var bookmarks = service.GetAllBookmarks();
                var toDisplay = bookmarks.Select(b => $"# {b.Name}\n{b.Url}\n").ToArray();
                if (toDisplay == null || toDisplay.Length <= 0)
                {
                    Console.WriteLine("Nothing to display");
                    return;
                }
                Helper.ShowSuccessMessage(toDisplay);
            }
        });
        rootCommand.SetAction(OnHandleRootCommand);

        return rootCommand.Parse(args).Invoke();
    }

    private static void OnHandleUpdateLinkCommand(ParseResult result, BookmarkService service)
    {
        // name and url are required options so can't be null
        var name = result.GetValue(nameOption)!;
        var url = result.GetValue(urlOption)!;
        var category = result.GetValue(categoryOption);
        service.UpdateLink(name, url, category);
    }

    private static void OnHandleRemoveLinkCommand(ParseResult result, BookmarkService service)
    {
        // name is a required option so can't be null
        var name = result.GetValue(nameOption)!;
        service.RemoveLink(name);
    }

    private static void OnHandleAddLinkCommand(ParseResult result, BookmarkService service)
    {
        // name and url are required options so can't be null
        var name = result.GetValue(nameOption)!;
        var url = result.GetValue(urlOption)!;
        var category = result.GetValue(categoryOption);
        service.AddLink(name, url, category);
    }

    private static void OnHandleRootCommand(ParseResult result)
    {
        Console.WriteLine("Hello from the rootcommand");
    }

    // private static void ManageLinks(string[] args, BookmarkService service)
    // {
    //     if (args.Length < 2)
    //     {
    //         Helper.ShowErrorMessage([
    //             "Insufficient number of parameters. The expected syntax is: ",
    //             "bookmarkr link <subcommand> <parameters>"
    //         ]);
    //     }

    //     switch (args[1].ToLower())
    //     {
    //         case "add":
    //             service.AddLink(args[2], args[3]);
    //             break;
    //         default:
    //             Helper.ShowErrorMessage([
    //                 "Insufficient number of parameters. The expected syntax is: ",
    //                 "bookmarkr link <subcommand> <parameters>"
    //             ]);
    //             break;
    //     }
    // }
}
