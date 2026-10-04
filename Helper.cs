namespace bookmarkr;

static class Helper
{
    public static void ShowErrorMessage(string[] messages)
    {
        // save previous color and change current to red
        var color = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Red;

        DisplayMessages(messages);
        Console.ForegroundColor = color;
    }

    public static void ShowWarningMessage(string[] messages)
    {
        // save previous color and change current to yellow
        var color = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Yellow;

        DisplayMessages(messages);
        Console.ForegroundColor = color;
    }

    public static void ShowSuccessMessage(string[] messages)
    {
        // save previous color and change current to green
        var color = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Green;

        DisplayMessages(messages);
        Console.ForegroundColor = color;
    }

    private static void DisplayMessages(string[] messages)
    {
        foreach (var message in messages)
        {
            Console.WriteLine(message);
        }
    }
}