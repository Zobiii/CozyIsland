namespace Game.Scenes;

public abstract class BaseScene
{
    private BaseScene? _next;

    public abstract void onEnter();
    public abstract void Update(float dt);
    public abstract void Draw();
    public abstract void onExit();

    protected void SwitchTo(BaseScene next) => _next = next;
    public BaseScene? NextScene() => _next;
}