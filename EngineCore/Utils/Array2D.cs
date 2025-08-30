namespace EngineCore.Utils;

public sealed class Array2D<T>
{
    private readonly T[] _data;
    public int Width { get; }
    public int Height { get; }

    public Array2D(int width, int height)
    {
        Width = width; Height = height;
        _data = new T[width * height];
    }

    public T this[int x, int y]
    {
        get => _data[y * Width + x];
        set => _data[y * Width + x] = value;
    }

    public bool InBounds(int x, int y) => x >= 0 && x < Width && y < Height;
}