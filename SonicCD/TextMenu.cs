// Decompiled with JetBrains decompiler
// Type: Retro_Engine.TextMenu
// Assembly: Sonic CD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D35AF46A-1892-4F52-B201-E664C9200079
// Assembly location: E:\wamwo\Downloads\Sonic CD Mod Via Rebel\Sonic CD Mod Via Rebel\Sonic CD.dll

namespace RetroEngine
{
  public class TextMenu
  {
    public char[] textData = new char[10240];
    public int[] entryStart = new int[512];
    public int[] entrySize = new int[512];
    public byte[] entryHighlight = new byte[512];
    public int textDataPos;
    public int selection1;
    public int selection2;
    public ushort numRows;
    public ushort numVisibleRows;
    public ushort visibleRowOffset;
    public byte alignment;
    public byte numSelections;
    public sbyte timer;
  }
}
