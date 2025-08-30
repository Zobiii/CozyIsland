using Raylib_cs;
using IslandEngine.Generation;
using IslandEngine.Tiles;
using Game.Scenes;

namespace Game.Scenes;

public class LoadingScene : BaseScene
{
    private bool _done;

    public override void onEnter()
    {
        _done = false;
    }

    public override void Update(float dt)
    {
        if (_done) return;

        int seed = (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        var generator = new IslandGenerator(seed);
        var island = generator.Generate(width: 100, height: 120, scale: 0.015f);

        SwitchTo(new WorldScene(island));
        _done = true;
    }

    public override void Draw()
    {
        Raylib.DrawText("Generating island...", 40, 40, 32, Color.Gold);
    }

    public override void onExit()
    {

    }
}