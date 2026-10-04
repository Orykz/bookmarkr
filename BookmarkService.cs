namespace bookmarkr;

public class BookmarkService
{
    private readonly List<Bookmark> _bookmarks = [];

    public void AddLink(string name, string url)
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
            Url = url
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

    public void UpdateLink(string name, string url)
    {
        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(url))
        {
            Helper.ShowErrorMessage([
                "Invalid syntax for the update command. The expected syntax is: ",
                "bookmarkr link updaste --name <name> --url <url>"
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

        bookmark!.Url = url;
        Helper.ShowSuccessMessage([$"Bookmark '{name}' successfully updated."]);
    }

    public List<Bookmark> GetAllBookmarks()
    {
        return _bookmarks;
    }
}