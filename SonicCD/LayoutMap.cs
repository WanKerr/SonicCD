// Decompiled with JetBrains decompiler
// Type: Retro_Engine.LayoutMap
// Assembly: Sonic CD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D35AF46A-1892-4F52-B201-E664C9200079
// Assembly location: E:\wamwo\Downloads\Sonic CD Mod Via Rebel\Sonic CD Mod Via Rebel\Sonic CD.dll

namespace RetroEngine
{
  public class LayoutMap
  {
    public ushort[] tileMap = new ushort[65536];
    public byte[] lineScrollRef = new byte[32768];
    public int parallaxFactor;
    public int scrollSpeed;
    public int scrollPosition;
    public int angle;
    public int xPos;
    public int yPos;
    public int zPos;
    public int deformationPos;
    public int deformationPosW;
    public byte type;
    public byte xSize;
    public byte ySize;
  }
}
