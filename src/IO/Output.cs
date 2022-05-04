namespace LoadTester.IO;

public static class Output
{
    public static void WriteSpace() => Console.WriteLine(string.Empty);

    public static void WriteDivider() => WriteDiminishedLine("------------------------------");

    public static void WriteDiminishedLine(string message)
    {
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine(message);
        Console.ResetColor();
    }

    public static void WriteInfoLine(string message)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(message);
        Console.ResetColor();
    }

    public static void WriteSuccessLine(string message)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(message);
        Console.ResetColor();
    }

    public static void WriteErrorLine(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(message);
        Console.ResetColor();
    }
}
