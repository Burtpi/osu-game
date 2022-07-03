using OpenTK;

namespace osu__Game;

internal static class cProgram
{
    private static void Main()
    {
        var window = new GameWindow(1600, 900);
        var osuGame = new cOsuGame(window);
        window.Run(1.0 / 144.0);
    }
}