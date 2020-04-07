// Decompiled with JetBrains decompiler
// Type: Retro_Engine.Mappings128x128
// Assembly: Sonic CD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D35AF46A-1892-4F52-B201-E664C9200079
// Assembly location: E:\wamwo\Downloads\Sonic CD Mod Via Rebel\Sonic CD Mod Via Rebel\Sonic CD.dll

namespace RetroEngine
{
  public class Mappings128x128
  {
    public int[] gfxDataPos = new int[32768];
    public ushort[] tile16x16 = new ushort[32768];
    public byte[] direction = new byte[32768];
    public byte[] visualPlane = new byte[32768];
    public byte[,] collisionFlag = new byte[2, 32768];
  }
}
