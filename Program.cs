using System.CommandLine;


namespace bookmarkr;

class Program
{
    static async Task<int> Main(string[] args)
    {
        // if (args == null || args.Length == 0)
        // {
        //     Helper.ShowErrorMessage([
        //         "You have not entered any command.", 
        //         "Use `bookmarkr --help` to view valid commands"
        //     ]);
        // }

        // var service = new BookmarkService();

        // switch (args[0].ToLower())
        // {
        //     case "link":
        //         ManageLinks(args, service);
        //         break;
        //     default:
        //         Helper.ShowErrorMessage([
        //             "Unkown Command."
        //         ]);
        //         break;
        // }
        var service = new BookmarkService();

        // initializing the command objects
        var rootCommand = new RootCommand("Bookmarkr is a bookmark manager provided as a CLI application.");
        var linkCommand = new Command("link", "Manage bookmark links");
        var addLinkCommand = new Command("add", "Add a new bookmark link");
        var removeLinkCommand = new Command("remove", "Remove an existing bookmark");
        var updateLinkCommand = new Command("update", "Update the url of an existing bookmark");

        // initializing the option objects
        var nameOption = new Option<string>("--name", ["-n"])
        {
            Required = true,
            Description = "The name of the bookmark"
        };
        var urlOption = new Option<string>("--url", ["-u"])
        {
            Required = true,
            Description = "The URL of the bookmark"
        };

        var listOption = new Option<bool>("--list", ["-l"])
        {

            Description = "Lists all of the bookmarks"
        };

        // linking the options to the commands
        addLinkCommand.Options.Add(nameOption);
        addLinkCommand.Options.Add(urlOption);
        removeLinkCommand.Options.Add(nameOption);
        updateLinkCommand.Options.Add(nameOption);
        updateLinkCommand.Options.Add(urlOption);
        linkCommand.Options.Add(listOption);

        // linking the commands to each other
        linkCommand.Subcommands.Add(addLinkCommand);
        linkCommand.Subcommands.Add(removeLinkCommand);
        linkCommand.Subcommands.Add(updateLinkCommand);
        rootCommand.Subcommands.Add(linkCommand);

        // handling the function call of commands
        addLinkCommand.SetAction(parseResult =>
        {
            var name = parseResult.GetValue(nameOption);
            var url = parseResult.GetValue(urlOption);
            OnHandleAddLinkCommand(name, url, service);
        });
        removeLinkCommand.SetAction(parseResult =>
        {
            var name = parseResult.GetValue(nameOption);
            OnHandleRemoveLinkCommand(name, service);
        });
        updateLinkCommand.SetAction(parseResult =>
        {
            var name = parseResult.GetValue(nameOption);
            var url = parseResult.GetValue(urlOption);
            OnHandleUpdateLinkCommand(name, url, service);
        });
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

    private static void OnHandleUpdateLinkCommand(string name, string url, BookmarkService service)
    {
        service.UpdateLink(name, url);
    }

    private static void OnHandleRemoveLinkCommand(string name, BookmarkService service)
    {
        service.RemoveLink(name);
    }

    private static void OnHandleAddLinkCommand(string name, string url, BookmarkService service)
    {
        service.AddLink(name, url);
    }

    private static void OnHandleRootCommand(ParseResult result)
    {
        Console.WriteLine("Hello from the rootcommand");
    }

    private static void ManageLinks(string[] args, BookmarkService service)
    {
        if (args.Length < 2)
        {
            Helper.ShowErrorMessage([
                "Insufficient number of parameters. The expected syntax is: ",
                "bookmarkr link <subcommand> <parameters>"
            ]);
        }

        switch (args[1].ToLower())
        {
            case "add":
                service.AddLink(args[2], args[3]);
                break;
            default:
                Helper.ShowErrorMessage([
                    "Insufficient number of parameters. The expected syntax is: ",
                    "bookmarkr link <subcommand> <parameters>"
                ]);
                break;
        }
    }
}
