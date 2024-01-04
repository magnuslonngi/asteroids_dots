using Unity.Entities;

public struct WarpComponent : IComponentData {
    public float spriteHeigth;
    public float spriteWidth;
    public int pixelsPerUnit;
    public int spriteDivision;
}
