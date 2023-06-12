﻿using SilvagenumData;
using SilvagenumUI;

internal class Program
{
    private static void Main()
    {
        IRepo activeRepo = new DummyRepo();
        var userInterface = new ConsoleUI(activeRepo);
        userInterface.Run();
    }
}