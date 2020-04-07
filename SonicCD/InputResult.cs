// Decompiled with JetBrains decompiler
// Type: Retro_Engine.InputResult
// Assembly: Sonic CD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D35AF46A-1892-4F52-B201-E664C9200079
// Assembly location: E:\wamwo\Downloads\Sonic CD Mod Via Rebel\Sonic CD Mod Via Rebel\Sonic CD.dll

namespace RetroEngine
{
  public class InputResult
  {
    public byte[] touchDown = new byte[4];
    public int[] touchX = new int[4];
    public int[] touchY = new int[4];
    public int[] touchID = new int[4];
    public byte up;
    public byte down;
    public byte left;
    public byte right;
    public byte buttonA;
    public byte buttonB;
    public byte buttonC;
    public byte start;
    public int touches;
  }
}
