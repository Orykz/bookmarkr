namespace bookmarkr;

public class BookmarkService
{
    // private readonly List<Bookmark> _bookmarks = [];
    // testing
    private readonly List<Bookmark> _bookmarks = [
        new Bookmark {
            Name = "test",
            Url = "test.com",
            Category = "Books"
        },
        new Bookmark {
            Name = "lenovo",
            Url = "lenovo.com",
            Category = "Read Later"
        }
    ];

    public void AddLink(string name, string url, string category)
    {
        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(url))
        {
            Helper.ShowErrorMessage([
                "Invalid syntax for the add command. The expected syntax is: ",
                "bookmarkr link add --name <name> --url <url>"
            ]);
            return;
        }

        if (_bookmarks.Any(b => b.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
        {
            Helper.ShowErrorMessage([
                $"A link with the name '{name}' already exists. command is canceled.",
                "To update the existing link, use the command: bookmarkr link update <name> <url>"
            ]);
            return;
        }

        _bookmarks.Add(new Bookmark
        {
            Name = name,
            Url = url,
            Category = category
        });
        Helper.ShowSuccessMessage(["Bookmark successfully added."]);
    }

    public void RemoveLink(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            Helper.ShowErrorMessage([
                "Invalid syntax for the remove command. The expected syntax is: ",
                "bookmarkr link remove --name <name>"
            ]);
            return;
        }

        var bookmark = _bookmarks.SingleOrDefault(b => b.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        if (bookmark == null)
        {
            Helper.ShowErrorMessage([
                $"Bookmark '{name}' does not exist."
            ]);
            return;
        }

        _bookmarks.Remove(bookmark!);
        Helper.ShowSuccessMessage([$"Bookmark '{name}' successfully removed."]);
    }

    public void UpdateLink(string name, string url, string? category)
    {
        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(url))
        {
            Helper.ShowErrorMessage([
                "Invalid syntax for the update command. The expected syntax is: ",
                "bookmarkr link update --name <name> --url <url>"
            ]);
            return;
        }
        
        var bookmark = _bookmarks.SingleOrDefault(b => b.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        if (bookmark == null)
        {
            Helper.ShowErrorMessage([
                $"Bookmark '{name}' does not exist."
            ]);
            return;
        }

        bookmark.Url = url;
        if (category != null)
        {
            bookmark.Category = category;
        }

        Helper.ShowSuccessMessage([$"Bookmark '{name}' successfully updated.", $"URL: {bookmark.Url}, Category: {bookmark.Category}"]);
    }

    public List<Bookmark> GetAllBookmarks()
    {
        return _bookmarks;
    }
}