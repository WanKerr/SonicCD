// Decompiled with JetBrains decompiler
// Type: Retro_Engine.GifLoader
// Assembly: Sonic CD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D35AF46A-1892-4F52-B201-E664C9200079
// Assembly location: E:\wamwo\Downloads\Sonic CD Mod Via Rebel\Sonic CD Mod Via Rebel\Sonic CD.dll

namespace RetroEngine
{
  public static class GifLoader
  {
    private static GifDecoder gifDecoder = new GifDecoder();
    private static int[] codeMasks = new int[13]
    {
      0,
      1,
      3,
      7,
      15,
      31,
      63,
      (int) sbyte.MaxValue,
      (int) byte.MaxValue,
      511,
      1023,
      2047,
      4095
    };
    public const int LOADING_IMAGE = 0;
    public const int LOAD_COMPLETE = 1;
    public const int LZ_MAX_CODE = 4095;
    public const int LZ_BITS = 12;
    public const int FLUSH_OUTPUT = 4096;
    public const int FIRST_CODE = 4097;
    public const int NO_SUCH_CODE = 4098;
    public const int HT_SIZE = 8192;
    public const int HT_KEY_MASK = 8191;

    public static void InitGifDecoder()
    {
      int num = (int) FileIO.ReadByte();
      GifLoader.gifDecoder.fileState = 0;
      GifLoader.gifDecoder.position = 0;
      GifLoader.gifDecoder.bufferSize = 0;
      GifLoader.gifDecoder.buffer[0] = (byte) 0;
      GifLoader.gifDecoder.depth = num;
      GifLoader.gifDecoder.clearCode = 1 << num;
      GifLoader.gifDecoder.eofCode = GifLoader.gifDecoder.clearCode + 1;
      GifLoader.gifDecoder.runningCode = GifLoader.gifDecoder.eofCode + 1;
      GifLoader.gifDecoder.runningBits = num + 1;
      GifLoader.gifDecoder.maxCodePlusOne = 1 << GifLoader.gifDecoder.runningBits;
      GifLoader.gifDecoder.stackPtr = 0;
      GifLoader.gifDecoder.prevCode = 4098;
      GifLoader.gifDecoder.shiftState = 0;
      GifLoader.gifDecoder.shiftData = 0U;
      for (int index = 0; index <= 4095; ++index)
        GifLoader.gifDecoder.prefix[index] = 4098U;
    }

    public static void ReadGifPictureData(
      int width,
      int height,
      bool interlaced,
      ref byte[] gfxData,
      int offset)
    {
      int[] numArray1 = new int[4]{ 0, 4, 2, 1 };
      int[] numArray2 = new int[4]{ 8, 8, 4, 2 };
      GifLoader.InitGifDecoder();
      if (interlaced)
      {
        for (int index1 = 0; index1 < 4; ++index1)
        {
          for (int index2 = numArray1[index1]; index2 < height; index2 += numArray2[index1])
            GifLoader.ReadGifLine(ref gfxData, width, index2 * width + offset);
        }
      }
      else
      {
        for (int index = 0; index < height; ++index)
          GifLoader.ReadGifLine(ref gfxData, width, index * width + offset);
      }
    }

    public static void ReadGifLine(ref byte[] line, int length, int offset)
    {
      int num1 = 0;
      int stackPtr = GifLoader.gifDecoder.stackPtr;
      int eofCode = GifLoader.gifDecoder.eofCode;
      int clearCode = GifLoader.gifDecoder.clearCode;
      int code1 = GifLoader.gifDecoder.prevCode;
      if (stackPtr != 0)
      {
        for (; stackPtr != 0 && num1 < length; ++num1)
          line[offset++] = GifLoader.gifDecoder.stack[--stackPtr];
      }
      while (num1 < length)
      {
        int code2 = GifLoader.ReadGifCode();
        if (code2 == eofCode)
        {
          if (num1 != length - 1 | GifLoader.gifDecoder.pixelCount != 0U)
            return;
          ++num1;
        }
        else if (code2 == clearCode)
        {
          for (int index = 0; index <= 4095; ++index)
            GifLoader.gifDecoder.prefix[index] = 4098U;
          GifLoader.gifDecoder.runningCode = GifLoader.gifDecoder.eofCode + 1;
          GifLoader.gifDecoder.runningBits = GifLoader.gifDecoder.depth + 1;
          GifLoader.gifDecoder.maxCodePlusOne = 1 << GifLoader.gifDecoder.runningBits;
          code1 = GifLoader.gifDecoder.prevCode = 4098;
        }
        else
        {
          if (code2 < clearCode)
          {
            line[offset] = (byte) code2;
            ++offset;
            ++num1;
          }
          else
          {
            if (code2 < 0 | code2 > 4095)
              return;
            int index;
            if (GifLoader.gifDecoder.prefix[code2] == 4098U)
            {
              if (code2 != GifLoader.gifDecoder.runningCode - 2)
                return;
              index = code1;
              GifLoader.gifDecoder.suffix[GifLoader.gifDecoder.runningCode - 2] = GifLoader.gifDecoder.stack[stackPtr++] = GifLoader.TracePrefix(ref GifLoader.gifDecoder.prefix, code1, clearCode);
            }
            else
              index = code2;
            int num2;
            for (num2 = 0; num2++ <= 4095 && index > clearCode && index <= 4095; index = (int) GifLoader.gifDecoder.prefix[index])
              GifLoader.gifDecoder.stack[stackPtr++] = GifLoader.gifDecoder.suffix[index];
            if (num2 >= 4095 | index > 4095)
              return;
            for (GifLoader.gifDecoder.stack[stackPtr++] = (byte) index; stackPtr != 0 && num1 < length; ++num1)
              line[offset++] = GifLoader.gifDecoder.stack[--stackPtr];
          }
          if (code1 != 4098)
          {
            if (GifLoader.gifDecoder.runningCode < 2 | GifLoader.gifDecoder.runningCode > 4097)
              return;
            GifLoader.gifDecoder.prefix[GifLoader.gifDecoder.runningCode - 2] = (uint) code1;
            GifLoader.gifDecoder.suffix[GifLoader.gifDecoder.runningCode - 2] = code2 != GifLoader.gifDecoder.runningCode - 2 ? GifLoader.TracePrefix(ref GifLoader.gifDecoder.prefix, code2, clearCode) : GifLoader.TracePrefix(ref GifLoader.gifDecoder.prefix, code1, clearCode);
          }
          code1 = code2;
        }
      }
      GifLoader.gifDecoder.prevCode = code1;
      GifLoader.gifDecoder.stackPtr = stackPtr;
    }

    private static int ReadGifCode()
    {
      for (; GifLoader.gifDecoder.shiftState < GifLoader.gifDecoder.runningBits; GifLoader.gifDecoder.shiftState += 8)
      {
        byte num = GifLoader.ReadGifByte();
        GifLoader.gifDecoder.shiftData |= (uint) num << GifLoader.gifDecoder.shiftState;
      }
      int num1 = (int) ((long) GifLoader.gifDecoder.shiftData & (long) GifLoader.codeMasks[GifLoader.gifDecoder.runningBits]);
      GifLoader.gifDecoder.shiftData >>= GifLoader.gifDecoder.runningBits;
      GifLoader.gifDecoder.shiftState -= GifLoader.gifDecoder.runningBits;
      if (++GifLoader.gifDecoder.runningCode > GifLoader.gifDecoder.maxCodePlusOne && GifLoader.gifDecoder.runningBits < 12)
      {
        GifLoader.gifDecoder.maxCodePlusOne <<= 1;
        ++GifLoader.gifDecoder.runningBits;
      }
      return num1;
    }

    private static byte ReadGifByte()
    {
      char minValue = char.MinValue;
      if (GifLoader.gifDecoder.fileState == 1)
        return (byte) minValue;
      byte num1;
      if (GifLoader.gifDecoder.position == GifLoader.gifDecoder.bufferSize)
      {
        byte num2 = FileIO.ReadByte();
        GifLoader.gifDecoder.bufferSize = (int) num2;
        if (GifLoader.gifDecoder.bufferSize == 0)
        {
          GifLoader.gifDecoder.fileState = 1;
          return (byte) minValue;
        }
        FileIO.ReadByteArray(ref GifLoader.gifDecoder.buffer, GifLoader.gifDecoder.bufferSize);
        num1 = GifLoader.gifDecoder.buffer[0];
        GifLoader.gifDecoder.position = 1;
      }
      else
        num1 = GifLoader.gifDecoder.buffer[GifLoader.gifDecoder.position++];
      return num1;
    }

    private static byte TracePrefix(ref uint[] prefix, int code, int clearCode)
    {
      int num = 0;
      while (code > clearCode && num++ <= 4095)
        code = (int) prefix[code];
      return (byte) code;
    }
  }
}
