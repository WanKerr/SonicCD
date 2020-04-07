// Decompiled with JetBrains decompiler
// Type: Retro_Engine.LineScrollParallax
// Assembly: Sonic CD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D35AF46A-1892-4F52-B201-E664C9200079
// Assembly location: E:\wamwo\Downloads\Sonic CD Mod Via Rebel\Sonic CD Mod Via Rebel\Sonic CD.dll

namespace RetroEngine
{
  public class LineScrollParallax
  {
    public int[] parallaxFactor = new int[256];
    public int[] scrollSpeed = new int[256];
    public int[] scrollPosition = new int[256];
    public int[] linePos = new int[256];
    public byte[] deformationEnabled = new byte[256];
    public byte numEntries;
  }
}
