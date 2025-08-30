using Game.Scenes;

namespace Game;

public static class Program
{
    public static void Main(string[] args)
    {
        using var game = new GameApp();
        game.Run();
    }
}