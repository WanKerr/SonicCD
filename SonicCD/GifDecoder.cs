// Decompiled with JetBrains decompiler
// Type: Retro_Engine.GifDecoder
// Assembly: Sonic CD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D35AF46A-1892-4F52-B201-E664C9200079
// Assembly location: E:\wamwo\Downloads\Sonic CD Mod Via Rebel\Sonic CD Mod Via Rebel\Sonic CD.dll

namespace RetroEngine
{
  public class GifDecoder
  {
    public byte[] buffer = new byte[256];
    public byte[] stack = new byte[4096];
    public byte[] suffix = new byte[4096];
    public uint[] prefix = new uint[4096];
    public int depth;
    public int clearCode;
    public int eofCode;
    public int runningCode;
    public int runningBits;
    public int prevCode;
    public int currentCode;
    public int maxCodePlusOne;
    public int stackPtr;
    public int shiftState;
    public int fileState;
    public int position;
    public int bufferSize;
    public uint shiftData;
    public uint pixelCount;
  }
}
