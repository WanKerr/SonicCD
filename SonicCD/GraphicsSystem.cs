// Decompiled with JetBrains decompiler
// Type: Retro_Engine.GraphicsSystem
// Assembly: Sonic CD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D35AF46A-1892-4F52-B201-E664C9200079
// Assembly location: E:\wamwo\Downloads\Sonic CD Mod Via Rebel\Sonic CD Mod Via Rebel\Sonic CD.dll

using Microsoft.Xna.Framework;
using System;

namespace RetroEngine
{
  public static class GraphicsSystem
  {
    public static bool render3DEnabled = false;
    public static byte fadeMode = 0;
    public static byte fadeR = 0;
    public static byte fadeG = 0;
    public static byte fadeB = 0;
    public static byte fadeA = 0;
    public static byte paletteMode = 0;
    public static byte colourMode = 0;
    public static ushort[] texBuffer = new ushort[1048576];
    public static byte texBufferMode = 0;
    public static byte[] tileGfx = new byte[262144];
    public static byte[] graphicData = new byte[2097152];
    public static GfxSurfaceDesc[] gfxSurface = new GfxSurfaceDesc[24];
    public static DrawVertex[] gfxPolyList = new DrawVertex[8192];
    public static DrawVertex3D[] polyList3D = new DrawVertex3D[6404];
    public static short[] gfxPolyListIndex = new short[49152];
    public static ushort gfxVertexSize = 0;
    public static ushort gfxVertexSizeOpaque = 0;
    public static ushort gfxIndexSize = 0;
    public static ushort gfxIndexSizeOpaque = 0;
    public static ushort vertexSize3D = 0;
    public static ushort indexSize3D = 0;
    public static float[] tileUVArray = new float[4096];
    public static Vector3 floor3DPos = new Vector3();
    public static ushort[] blendLookupTable = new ushort[8192];
    public static ushort[] subtractiveLookupTable = new ushort[8192];
    public static PaletteEntry[] tilePalette = new PaletteEntry[256];
    public static ushort[,] tilePalette16_Data = new ushort[8, 256];
    public static int texPaletteNum = 0;
    public static int waterDrawPos = 320;
    public static bool videoPlaying = false;
    public const int NUM_SPRITESHEETS = 24;
    public const int GRAPHIC_DATASIZE = 2097152;
    public const int VERTEX_LIMIT = 8192;
    public const int INDEX_LIMIT = 49152;
    public static uint gfxDataPosition;
    public static float floor3DAngle;
    public static int currentVideoFrame;

    static GraphicsSystem()
    {
      for (int index = 0; index < gfxSurface.Length; ++index)
                gfxSurface[index] = new GfxSurfaceDesc();
      for (int index = 0; index < gfxPolyList.Length; ++index)
                gfxPolyList[index] = new DrawVertex();
      for (int index = 0; index < polyList3D.Length; ++index)
                polyList3D[index] = new DrawVertex3D();
      for (int index = 0; index < tilePalette.Length; ++index)
                tilePalette[index] = new PaletteEntry();
    }

    public static void SetScreenRenderSize(int gfxWidth, int gfxPitch)
    {
      GlobalAppDefinitions.SCREEN_XSIZE = gfxWidth;
      GlobalAppDefinitions.SCREEN_CENTER = GlobalAppDefinitions.SCREEN_XSIZE / 2;
      GlobalAppDefinitions.SCREEN_SCROLL_LEFT = GlobalAppDefinitions.SCREEN_CENTER - 8;
      GlobalAppDefinitions.SCREEN_SCROLL_RIGHT = GlobalAppDefinitions.SCREEN_CENTER + 8;
      GlobalAppDefinitions.OBJECT_BORDER_X1 = 128;
      GlobalAppDefinitions.OBJECT_BORDER_X2 = GlobalAppDefinitions.SCREEN_XSIZE + 128;
    }

    public static ushort RGB_16BIT5551(byte r, byte g, byte b, byte a)
    {
      return (ushort) (((int) a << 15) + ((int) r >> 3 << 10) + ((int) g >> 3 << 5) + ((int) b >> 3));
    }

    public static void LoadPalette(
      char[] fileName,
      int paletteNum,
      int destPoint,
      int startPoint,
      int endPoint)
    {
      char[] strA = new char[64];
      char[] charArray = "Data/Palettes/".ToCharArray();
      FileIO.StrCopy(ref strA, ref charArray);
      FileIO.StrAdd(ref strA, ref fileName);
      FileData fData = new FileData();
      byte[] byteP = new byte[3];
      if (!FileIO.LoadFile(strA, fData))
        return;
      FileIO.SetFilePosition((uint) (startPoint * 3));
      if (paletteNum < 0 || paletteNum > 7)
        paletteNum = 0;
      if (paletteNum == 0)
      {
        for (int index = startPoint; index < endPoint; ++index)
        {
          FileIO.ReadByteArray(ref byteP, 3);
                    tilePalette16_Data[0, destPoint] = RGB_16BIT5551(byteP[0], byteP[1], byteP[2], (byte) 1);
                    tilePalette[destPoint].red = byteP[0];
                    tilePalette[destPoint].green = byteP[1];
                    tilePalette[destPoint].blue = byteP[2];
          ++destPoint;
        }
                tilePalette16_Data[0, 0] = RGB_16BIT5551(byteP[0], byteP[1], byteP[2], (byte) 0);
      }
      else
      {
        for (int index = startPoint; index < endPoint; ++index)
        {
          FileIO.ReadByteArray(ref byteP, 3);
                    tilePalette16_Data[paletteNum, destPoint] = RGB_16BIT5551(byteP[0], byteP[1], byteP[2], (byte) 1);
          ++destPoint;
        }
                tilePalette16_Data[paletteNum, 0] = RGB_16BIT5551(byteP[0], byteP[1], byteP[2], (byte) 0);
      }
      FileIO.CloseFile();
    }

    public static byte AddGraphicsFile(char[] fileName)
    {
      byte num = 0;
      char[] fileName1 = new char[64];
      char[] charArray = "Data/Sprites/".ToCharArray();
      FileIO.StrCopy(ref fileName1, ref charArray);
      FileIO.StrAdd(ref fileName1, ref fileName);
      for (; num < (byte) 24; ++num)
      {
        if (FileIO.StringLength(ref gfxSurface[(int) num].fileName) > 0)
        {
          if (FileIO.StringComp(ref gfxSurface[(int) num].fileName, ref fileName1))
            return num;
        }
        else
        {
          int index = FileIO.StringLength(ref fileName1) - 1;
          switch (fileName1[index])
          {
            case 'f':
                            LoadGIFFile(fileName1, (int) num);
              break;
            case 'p':
                            LoadBMPFile(fileName1, (int) num);
              break;
          }
          return num;
        }
      }
      return 0;
    }

    public static void RemoveGraphicsFile(char[] fileName, int surfaceNum)
    {
      if (surfaceNum < 0)
      {
        for (uint index = 0; index < 24U; ++index)
        {
          if (FileIO.StringLength(ref gfxSurface[index].fileName) > 0 && FileIO.StringComp(ref gfxSurface[index].fileName, ref fileName))
            surfaceNum = (int) index;
        }
      }
      if (surfaceNum < 0 || FileIO.StringLength(ref gfxSurface[surfaceNum].fileName) == 0)
        return;
      FileIO.StrClear(ref gfxSurface[surfaceNum].fileName);
      uint dataStart = gfxSurface[surfaceNum].dataStart;
      uint num = (uint) ((ulong)gfxSurface[surfaceNum].dataStart + (ulong) (gfxSurface[surfaceNum].width * gfxSurface[surfaceNum].height));
      for (uint index = 2097152U - num; index > 0U; --index)
      {
                graphicData[dataStart] = graphicData[num];
        ++dataStart;
        ++num;
      }
            gfxDataPosition -= (uint) (gfxSurface[surfaceNum].width * gfxSurface[surfaceNum].height);
      for (uint index = 0; index < 24U; ++index)
      {
        if (gfxSurface[index].dataStart > gfxSurface[surfaceNum].dataStart)
                    gfxSurface[index].dataStart -= (uint) (gfxSurface[surfaceNum].width * gfxSurface[surfaceNum].height);
      }
    }

    public static void ClearGraphicsData()
    {
      for (int index = 0; index < 24; ++index)
        FileIO.StrClear(ref gfxSurface[index].fileName);
            gfxDataPosition = 0U;
    }

    public static bool CheckSurfaceSize(int size)
    {
      for (int index = 2; index < 2048; index <<= 1)
      {
        if (index == size)
          return true;
      }
      return false;
    }

    public static void SetupPolygonLists()
    {
      int index1 = 0;
      for (int index2 = 0; index2 < 8192; ++index2)
      {
                gfxPolyListIndex[index1] = (short) (index2 << 2);
        int index3 = index1 + 1;
                gfxPolyListIndex[index3] = (short) ((index2 << 2) + 1);
        int index4 = index3 + 1;
                gfxPolyListIndex[index4] = (short) ((index2 << 2) + 2);
        int index5 = index4 + 1;
                gfxPolyListIndex[index5] = (short) ((index2 << 2) + 1);
        int index6 = index5 + 1;
                gfxPolyListIndex[index6] = (short) ((index2 << 2) + 3);
        int index7 = index6 + 1;
                gfxPolyListIndex[index7] = (short) ((index2 << 2) + 2);
        index1 = index7 + 1;
      }
      for (int index2 = 0; index2 < 8192; ++index2)
      {
                gfxPolyList[index2].color.R = byte.MaxValue;
                gfxPolyList[index2].color.G = byte.MaxValue;
                gfxPolyList[index2].color.B = byte.MaxValue;
                gfxPolyList[index2].color.A = byte.MaxValue;
      }
      for (int index2 = 0; index2 < 6404; ++index2)
      {
                polyList3D[index2].color.R = byte.MaxValue;
                polyList3D[index2].color.G = byte.MaxValue;
                polyList3D[index2].color.B = byte.MaxValue;
                polyList3D[index2].color.A = byte.MaxValue;
      }
    }

    public static void UpdateTextureBufferWithTiles()
    {
      int num1 = 0;
      if (texBufferMode == (byte) 0)
      {
        for (int index1 = 0; index1 < 512; index1 += 16)
        {
          for (int index2 = 0; index2 < 512; index2 += 16)
          {
            int index3 = num1 << 8;
            ++num1;
            int index4 = index2 + (index1 << 10);
            for (int index5 = 0; index5 < 16; ++index5)
            {
              for (int index6 = 0; index6 < 16; ++index6)
              {
                                texBuffer[index4] = tileGfx[index3] <= (byte) 0 ? (ushort) 0 : tilePalette16_Data[texPaletteNum, (int)tileGfx[index3]];
                ++index4;
                ++index3;
              }
              index4 += 1008;
            }
          }
        }
      }
      else
      {
        for (int index1 = 0; index1 < 504; index1 += 18)
        {
          for (int index2 = 0; index2 < 504; index2 += 18)
          {
            int index3 = num1 << 8;
            ++num1;
            if (num1 == 783)
              num1 = 1023;
            int index4 = index2 + (index1 << 10);
                        texBuffer[index4] = tileGfx[index3] <= (byte) 0 ? (ushort) 0 : tilePalette16_Data[texPaletteNum, (int)tileGfx[index3]];
            int index5 = index4 + 1;
            for (int index6 = 0; index6 < 15; ++index6)
            {
                            texBuffer[index5] = tileGfx[index3] <= (byte) 0 ? (ushort) 0 : tilePalette16_Data[texPaletteNum, (int)tileGfx[index3]];
              ++index5;
              ++index3;
            }
            int index7;
            if (tileGfx[index3] > (byte) 0)
            {
                            texBuffer[index5] = tilePalette16_Data[texPaletteNum, (int)tileGfx[index3]];
              index7 = index5 + 1;
                            texBuffer[index7] = tilePalette16_Data[texPaletteNum, (int)tileGfx[index3]];
            }
            else
            {
                            texBuffer[index5] = (ushort) 0;
              index7 = index5 + 1;
                            texBuffer[index7] = (ushort) 0;
            }
            int num2 = index7 + 1;
            int index8 = index3 - 15;
            int index9 = num2 + 1006;
            for (int index6 = 0; index6 < 16; ++index6)
            {
                            texBuffer[index9] = tileGfx[index8] <= (byte) 0 ? (ushort) 0 : tilePalette16_Data[texPaletteNum, (int)tileGfx[index8]];
              int index10 = index9 + 1;
              for (int index11 = 0; index11 < 15; ++index11)
              {
                                texBuffer[index10] = tileGfx[index8] <= (byte) 0 ? (ushort) 0 : tilePalette16_Data[texPaletteNum, (int)tileGfx[index8]];
                ++index10;
                ++index8;
              }
              int index12;
              if (tileGfx[index8] > (byte) 0)
              {
                                texBuffer[index10] = tilePalette16_Data[texPaletteNum, (int)tileGfx[index8]];
                index12 = index10 + 1;
                                texBuffer[index12] = tilePalette16_Data[texPaletteNum, (int)tileGfx[index8]];
              }
              else
              {
                                texBuffer[index10] = (ushort) 0;
                index12 = index10 + 1;
                                texBuffer[index12] = (ushort) 0;
              }
              int num3 = index12 + 1;
              ++index8;
              index9 = num3 + 1006;
            }
            int index13 = index8 - 16;
                        texBuffer[index9] = tileGfx[index13] <= (byte) 0 ? (ushort) 0 : tilePalette16_Data[texPaletteNum, (int)tileGfx[index13]];
            int index14 = index9 + 1;
            for (int index6 = 0; index6 < 15; ++index6)
            {
                            texBuffer[index14] = tileGfx[index13] <= (byte) 0 ? (ushort) 0 : tilePalette16_Data[texPaletteNum, (int)tileGfx[index13]];
              ++index14;
              ++index13;
            }
            int index15;
            if (tileGfx[index13] > (byte) 0)
            {
                            texBuffer[index14] = tilePalette16_Data[texPaletteNum, (int)tileGfx[index13]];
              index15 = index14 + 1;
                            texBuffer[index15] = tilePalette16_Data[texPaletteNum, (int)tileGfx[index13]];
            }
            else
            {
                            texBuffer[index14] = (ushort) 0;
              index15 = index14 + 1;
                            texBuffer[index15] = (ushort) 0;
            }
            int num4 = index15 + 1 + 1006;
          }
        }
      }
      int index16 = 0;
      for (int index1 = 0; index1 < 16; ++index1)
      {
        for (int index2 = 0; index2 < 16; ++index2)
        {
                    texBuffer[index16] = RGB_16BIT5551(byte.MaxValue, byte.MaxValue, byte.MaxValue, (byte) 1);
          ++index16;
        }
        index16 += 1008;
      }
    }

    public static void UpdateTextureBufferWithSortedSprites()
    {
      byte num1 = 0;
      byte[] numArray = new byte[24];
      bool flag = true;
      for (int index = 0; index < 24; ++index)
                gfxSurface[index].texStartX = -1;
      for (int index1 = 0; index1 < 24; ++index1)
      {
        int num2 = 0;
        sbyte num3 = -1;
        for (int index2 = 0; index2 < 24; ++index2)
        {
          if (FileIO.StringLength(ref gfxSurface[index2].fileName) > 0 && gfxSurface[index2].texStartX == -1)
          {
            if (CheckSurfaceSize(gfxSurface[index2].width) && CheckSurfaceSize(gfxSurface[index2].height))
            {
              if (gfxSurface[index2].width + gfxSurface[index2].height > num2)
              {
                num2 = gfxSurface[index2].width + gfxSurface[index2].height;
                num3 = (sbyte) index2;
              }
            }
            else
                            gfxSurface[index2].texStartX = 0;
          }
        }
        if (num3 == (sbyte) -1)
        {
          index1 = 24;
        }
        else
        {
                    gfxSurface[(int) num3].texStartX = 0;
          numArray[(int) num1] = (byte) num3;
          ++num1;
        }
      }
      for (int index = 0; index < 24; ++index)
                gfxSurface[index].texStartX = -1;
      for (int index1 = 0; index1 < (int) num1; ++index1)
      {
        sbyte num2 = (sbyte) numArray[index1];
                gfxSurface[(int) num2].texStartX = 0;
                gfxSurface[(int) num2].texStartY = 0;
        int num3 = 0;
        while (num3 == 0)
        {
          num3 = 1;
          if (gfxSurface[(int) num2].height == 1024)
            flag = false;
          if (flag)
          {
            if (gfxSurface[(int) num2].texStartX < 512 && gfxSurface[(int) num2].texStartY < 512)
            {
              num3 = 0;
                            gfxSurface[(int) num2].texStartX += gfxSurface[(int) num2].width;
              if (gfxSurface[(int) num2].texStartX + gfxSurface[(int) num2].width > 1024)
              {
                                gfxSurface[(int) num2].texStartX = 0;
                                gfxSurface[(int) num2].texStartY += gfxSurface[(int) num2].height;
              }
            }
            else
            {
              for (int index2 = 0; index2 < 24; ++index2)
              {
                if (gfxSurface[index2].texStartX > -1 && index2 != (int) num2 && (gfxSurface[(int) num2].texStartX < gfxSurface[index2].texStartX + gfxSurface[index2].width && gfxSurface[(int) num2].texStartX >= gfxSurface[index2].texStartX) && gfxSurface[(int) num2].texStartY < gfxSurface[index2].texStartY + gfxSurface[index2].height)
                {
                  num3 = 0;
                                    gfxSurface[(int) num2].texStartX += gfxSurface[(int) num2].width;
                  if (gfxSurface[(int) num2].texStartX + gfxSurface[(int) num2].width > 1024)
                  {
                                        gfxSurface[(int) num2].texStartX = 0;
                                        gfxSurface[(int) num2].texStartY += gfxSurface[(int) num2].height;
                  }
                  index2 = 24;
                }
              }
            }
          }
          else if (gfxSurface[(int) num2].width < 1024)
          {
            if (gfxSurface[(int) num2].texStartX < 16 && gfxSurface[(int) num2].texStartY < 16)
            {
              num3 = 0;
                            gfxSurface[(int) num2].texStartX += gfxSurface[(int) num2].width;
              if (gfxSurface[(int) num2].texStartX + gfxSurface[(int) num2].width > 1024)
              {
                                gfxSurface[(int) num2].texStartX = 0;
                                gfxSurface[(int) num2].texStartY += gfxSurface[(int) num2].height;
              }
            }
            else
            {
              for (int index2 = 0; index2 < 24; ++index2)
              {
                if (gfxSurface[index2].texStartX > -1 && index2 != (int) num2 && (gfxSurface[(int) num2].texStartX < gfxSurface[index2].texStartX + gfxSurface[index2].width && gfxSurface[(int) num2].texStartX >= gfxSurface[index2].texStartX) && gfxSurface[(int) num2].texStartY < gfxSurface[index2].texStartY + gfxSurface[index2].height)
                {
                  num3 = 0;
                                    gfxSurface[(int) num2].texStartX += gfxSurface[(int) num2].width;
                  if (gfxSurface[(int) num2].texStartX + gfxSurface[(int) num2].width > 1024)
                  {
                                        gfxSurface[(int) num2].texStartX = 0;
                                        gfxSurface[(int) num2].texStartY += gfxSurface[(int) num2].height;
                  }
                  index2 = 24;
                }
              }
            }
          }
        }
        if (gfxSurface[(int) num2].texStartY + gfxSurface[(int) num2].height <= 1024)
        {
          int dataStart = (int)gfxSurface[(int) num2].dataStart;
          int index2 = gfxSurface[(int) num2].texStartX + (gfxSurface[(int) num2].texStartY << 10);
          for (int index3 = 0; index3 < gfxSurface[(int) num2].height; ++index3)
          {
            for (int index4 = 0; index4 < gfxSurface[(int) num2].width; ++index4)
            {
                            texBuffer[index2] = graphicData[dataStart] <= (byte) 0 ? (ushort) 0 : tilePalette16_Data[texPaletteNum, (int)graphicData[dataStart]];
              ++index2;
              ++dataStart;
            }
            index2 += 1024 - gfxSurface[(int) num2].width;
          }
        }
      }
    }

    public static void UpdateTextureBufferWithSprites()
    {
      for (int index1 = 0; index1 < 24; ++index1)
      {
        if (gfxSurface[index1].texStartY + gfxSurface[index1].height <= 1024 && gfxSurface[index1].texStartX > -1)
        {
          int dataStart = (int)gfxSurface[index1].dataStart;
          int index2 = gfxSurface[index1].texStartX + (gfxSurface[index1].texStartY << 10);
          for (int index3 = 0; index3 < gfxSurface[index1].height; ++index3)
          {
            for (int index4 = 0; index4 < gfxSurface[index1].width; ++index4)
            {
                            texBuffer[index2] = graphicData[dataStart] <= (byte) 0 ? (ushort) 0 : tilePalette16_Data[texPaletteNum, (int)graphicData[dataStart]];
              ++index2;
              ++dataStart;
            }
            index2 += 1024 - gfxSurface[index1].width;
          }
        }
      }
    }

    public static void LoadBMPFile(char[] fileName, int surfaceNum)
    {
      FileData fData = new FileData();
      if (!FileIO.LoadFile(fileName, fData))
        return;
      FileIO.StrCopy(ref gfxSurface[surfaceNum].fileName, ref fileName);
      FileIO.SetFilePosition(18U);
      byte num1 = FileIO.ReadByte();
            gfxSurface[surfaceNum].width = (int) num1;
      byte num2 = FileIO.ReadByte();
            gfxSurface[surfaceNum].width += (int) num2 << 8;
      byte num3 = FileIO.ReadByte();
            gfxSurface[surfaceNum].width += (int) num3 << 16;
      byte num4 = FileIO.ReadByte();
            gfxSurface[surfaceNum].width += (int) num4 << 24;
      byte num5 = FileIO.ReadByte();
            gfxSurface[surfaceNum].height = (int) num5;
      byte num6 = FileIO.ReadByte();
            gfxSurface[surfaceNum].height += (int) num6 << 8;
      byte num7 = FileIO.ReadByte();
            gfxSurface[surfaceNum].height += (int) num7 << 16;
      byte num8 = FileIO.ReadByte();
            gfxSurface[surfaceNum].height += (int) num8 << 24;
      FileIO.SetFilePosition((uint) ((ulong) fData.fileSize - (ulong) (gfxSurface[surfaceNum].width * gfxSurface[surfaceNum].height)));
            gfxSurface[surfaceNum].dataStart = gfxDataPosition;
      int index1 = (int)gfxSurface[surfaceNum].dataStart + gfxSurface[surfaceNum].width * (gfxSurface[surfaceNum].height - 1);
      for (int index2 = 0; index2 < gfxSurface[surfaceNum].height; ++index2)
      {
        for (int index3 = 0; index3 < gfxSurface[surfaceNum].width; ++index3)
        {
          byte num9 = FileIO.ReadByte();
                    graphicData[index1] = num9;
          ++index1;
        }
        index1 -= gfxSurface[surfaceNum].width << 1;
      }
            gfxDataPosition += (uint) (gfxSurface[surfaceNum].width * gfxSurface[surfaceNum].height);
      if (gfxDataPosition >= 4194304U)
                gfxDataPosition = 0U;
      FileIO.CloseFile();
    }

    public static void LoadGIFFile(char[] fileName, int surfaceNum)
    {
      FileData fData = new FileData();
      byte[] byteP = new byte[3];
      bool interlaced = false;
      if (!FileIO.LoadFile(fileName, fData))
        return;
      FileIO.StrCopy(ref gfxSurface[surfaceNum].fileName, ref fileName);
      FileIO.SetFilePosition(6U);
      byteP[0] = FileIO.ReadByte();
      int num1 = (int) byteP[0];
      byteP[0] = FileIO.ReadByte();
      int width = num1 + ((int) byteP[0] << 8);
      byteP[0] = FileIO.ReadByte();
      int num2 = (int) byteP[0];
      byteP[0] = FileIO.ReadByte();
      int height = num2 + ((int) byteP[0] << 8);
      byteP[0] = FileIO.ReadByte();
      byteP[0] = FileIO.ReadByte();
      byteP[0] = FileIO.ReadByte();
      for (int index = 0; index < 256; ++index)
        FileIO.ReadByteArray(ref byteP, 3);
      byteP[0] = FileIO.ReadByte();
      while (byteP[0] != (byte) 44)
        byteP[0] = FileIO.ReadByte();
      if (byteP[0] == (byte) 44)
      {
        FileIO.ReadByteArray(ref byteP, 2);
        FileIO.ReadByteArray(ref byteP, 2);
        FileIO.ReadByteArray(ref byteP, 2);
        FileIO.ReadByteArray(ref byteP, 2);
        byteP[0] = FileIO.ReadByte();
        if (((int) byteP[0] & 64) >> 6 == 1)
          interlaced = true;
        if (((int) byteP[0] & 128) >> 7 == 1)
        {
          for (int index = 128; index < 256; ++index)
            FileIO.ReadByteArray(ref byteP, 3);
        }
                gfxSurface[surfaceNum].width = width;
                gfxSurface[surfaceNum].height = height;
                gfxSurface[surfaceNum].dataStart = gfxDataPosition;
                gfxDataPosition += (uint) (gfxSurface[surfaceNum].width * gfxSurface[surfaceNum].height);
        if (gfxDataPosition >= 4194304U)
                    gfxDataPosition = 0U;
        else
          GifLoader.ReadGifPictureData(width, height, interlaced, ref graphicData, (int)gfxSurface[surfaceNum].dataStart);
      }
      FileIO.CloseFile();
    }

    public static void LoadStageGIFFile(int zNumber)
    {
      FileData fData = new FileData();
      byte[] byteP = new byte[3];
      bool interlaced = false;
      if (!FileIO.LoadStageFile("16x16Tiles.gif".ToCharArray(), zNumber, fData))
        return;
      FileIO.SetFilePosition(6U);
      byteP[0] = FileIO.ReadByte();
      int num1 = (int) byteP[0];
      byteP[0] = FileIO.ReadByte();
      int width = num1 + ((int) byteP[0] << 8);
      byteP[0] = FileIO.ReadByte();
      int num2 = (int) byteP[0];
      byteP[0] = FileIO.ReadByte();
      int height = num2 + ((int) byteP[0] << 8);
      byteP[0] = FileIO.ReadByte();
      byteP[0] = FileIO.ReadByte();
      byteP[0] = FileIO.ReadByte();
      for (int index = 128; index < 256; ++index)
        FileIO.ReadByteArray(ref byteP, 3);
      for (int index = 128; index < 256; ++index)
      {
        FileIO.ReadByteArray(ref byteP, 3);
                tilePalette[index].red = byteP[0];
                tilePalette[index].green = byteP[1];
                tilePalette[index].blue = byteP[2];
                tilePalette16_Data[texPaletteNum, index] = RGB_16BIT5551(byteP[0], byteP[1], byteP[2], (byte) 1);
      }
      byteP[0] = FileIO.ReadByte();
      if (byteP[0] == (byte) 44)
      {
        FileIO.ReadByteArray(ref byteP, 2);
        FileIO.ReadByteArray(ref byteP, 2);
        FileIO.ReadByteArray(ref byteP, 2);
        FileIO.ReadByteArray(ref byteP, 2);
        byteP[0] = FileIO.ReadByte();
        if (((int) byteP[0] & 64) >> 6 == 1)
          interlaced = true;
        if (((int) byteP[0] & 128) >> 7 == 1)
        {
          for (int index = 128; index < 256; ++index)
            FileIO.ReadByteArray(ref byteP, 3);
        }
        GifLoader.ReadGifPictureData(width, height, interlaced, ref tileGfx, 0);
        byteP[0] = tileGfx[0];
        for (int index = 0; index < 262144; ++index)
        {
          if ((int)tileGfx[index] == (int) byteP[0])
                        tileGfx[index] = (byte) 0;
        }
      }
      FileIO.CloseFile();
    }

    public static void Copy16x16Tile(int tDest, int tSource)
    {
      tSource <<= 2;
      tDest <<= 2;
            tileUVArray[tDest] = tileUVArray[tSource];
            tileUVArray[tDest + 1] = tileUVArray[tSource + 1];
            tileUVArray[tDest + 2] = tileUVArray[tSource + 2];
            tileUVArray[tDest + 3] = tileUVArray[tSource + 3];
    }

    public static void ClearScreen(byte clearColour)
    {
            gfxPolyList[(int)gfxVertexSize].position.X = 0.0f;
            gfxPolyList[(int)gfxVertexSize].position.Y = 0.0f;
            gfxPolyList[(int)gfxVertexSize].color.R = tilePalette[(int) clearColour].red;
            gfxPolyList[(int)gfxVertexSize].color.G = tilePalette[(int) clearColour].green;
            gfxPolyList[(int)gfxVertexSize].color.B = tilePalette[(int) clearColour].blue;
            gfxPolyList[(int)gfxVertexSize].color.A = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].texCoord.X = 0.0f;
            gfxPolyList[(int)gfxVertexSize].texCoord.Y = 0.0f;
      ++gfxVertexSize;
            gfxPolyList[(int)gfxVertexSize].position.X = (float) (GlobalAppDefinitions.SCREEN_XSIZE << 4);
            gfxPolyList[(int)gfxVertexSize].position.Y = 0.0f;
            gfxPolyList[(int)gfxVertexSize].color.R = tilePalette[(int) clearColour].red;
            gfxPolyList[(int)gfxVertexSize].color.G = tilePalette[(int) clearColour].green;
            gfxPolyList[(int)gfxVertexSize].color.B = tilePalette[(int) clearColour].blue;
            gfxPolyList[(int)gfxVertexSize].color.A = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].texCoord.X = 0.0f;
            gfxPolyList[(int)gfxVertexSize].texCoord.Y = 0.0f;
      ++gfxVertexSize;
            gfxPolyList[(int)gfxVertexSize].position.X = 0.0f;
            gfxPolyList[(int)gfxVertexSize].position.Y = 3840f;
            gfxPolyList[(int)gfxVertexSize].color.R = tilePalette[(int) clearColour].red;
            gfxPolyList[(int)gfxVertexSize].color.G = tilePalette[(int) clearColour].green;
            gfxPolyList[(int)gfxVertexSize].color.B = tilePalette[(int) clearColour].blue;
            gfxPolyList[(int)gfxVertexSize].color.A = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].texCoord.X = 0.0f;
            gfxPolyList[(int)gfxVertexSize].texCoord.Y = 0.0f;
      ++gfxVertexSize;
            gfxPolyList[(int)gfxVertexSize].position.X = (float) (GlobalAppDefinitions.SCREEN_XSIZE << 4);
            gfxPolyList[(int)gfxVertexSize].position.Y = 3840f;
            gfxPolyList[(int)gfxVertexSize].color.R = tilePalette[(int) clearColour].red;
            gfxPolyList[(int)gfxVertexSize].color.G = tilePalette[(int) clearColour].green;
            gfxPolyList[(int)gfxVertexSize].color.B = tilePalette[(int) clearColour].blue;
            gfxPolyList[(int)gfxVertexSize].color.A = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].texCoord.X = 0.0f;
            gfxPolyList[(int)gfxVertexSize].texCoord.Y = 0.0f;
      ++gfxVertexSize;
            gfxIndexSize += (ushort) 2;
    }

    public static void DrawSprite(
      int xPos,
      int yPos,
      int xSize,
      int ySize,
      int xBegin,
      int yBegin,
      int surfaceNum)
    {
      if (gfxSurface[surfaceNum].texStartX <= -1 || gfxVertexSize >= (ushort) 8192 || (xPos <= -512 || xPos >= 872) || (yPos <= -512 || yPos >= 752))
        return;
            gfxPolyList[(int)gfxVertexSize].position.X = (float) (xPos << 4);
            gfxPolyList[(int)gfxVertexSize].position.Y = (float) (yPos << 4);
            gfxPolyList[(int)gfxVertexSize].color.R = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].color.G = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].color.B = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].color.A = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].texCoord.X = (float) (gfxSurface[surfaceNum].texStartX + xBegin) * 0.0009765625f;
            gfxPolyList[(int)gfxVertexSize].texCoord.Y = (float) (gfxSurface[surfaceNum].texStartY + yBegin) * 0.0009765625f;
      ++gfxVertexSize;
            gfxPolyList[(int)gfxVertexSize].position.X = (float) (xPos + xSize << 4);
            gfxPolyList[(int)gfxVertexSize].position.Y = (float) (yPos << 4);
            gfxPolyList[(int)gfxVertexSize].color.R = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].color.G = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].color.B = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].color.A = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].texCoord.X = (float) (gfxSurface[surfaceNum].texStartX + xBegin + xSize) * 0.0009765625f;
            gfxPolyList[(int)gfxVertexSize].texCoord.Y = gfxPolyList[(int)gfxVertexSize - 1].texCoord.Y;
      ++gfxVertexSize;
            gfxPolyList[(int)gfxVertexSize].position.X = (float) (xPos << 4);
            gfxPolyList[(int)gfxVertexSize].position.Y = (float) (yPos + ySize << 4);
            gfxPolyList[(int)gfxVertexSize].color.R = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].color.G = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].color.B = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].color.A = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].texCoord.X = gfxPolyList[(int)gfxVertexSize - 2].texCoord.X;
            gfxPolyList[(int)gfxVertexSize].texCoord.Y = (float) (gfxSurface[surfaceNum].texStartY + yBegin + ySize) * 0.0009765625f;
      ++gfxVertexSize;
            gfxPolyList[(int)gfxVertexSize].position.X = gfxPolyList[(int)gfxVertexSize - 2].position.X;
            gfxPolyList[(int)gfxVertexSize].position.Y = gfxPolyList[(int)gfxVertexSize - 1].position.Y;
            gfxPolyList[(int)gfxVertexSize].color.R = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].color.G = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].color.B = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].color.A = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].texCoord.X = gfxPolyList[(int)gfxVertexSize - 2].texCoord.X;
            gfxPolyList[(int)gfxVertexSize].texCoord.Y = gfxPolyList[(int)gfxVertexSize - 1].texCoord.Y;
      ++gfxVertexSize;
            gfxIndexSize += (ushort) 2;
    }

    public static void DrawSpriteFlipped(
      int xPos,
      int yPos,
      int xSize,
      int ySize,
      int xBegin,
      int yBegin,
      int direction,
      int surfaceNum)
    {
      if (gfxSurface[surfaceNum].texStartX <= -1 || gfxVertexSize >= (ushort) 8192 || (xPos <= -512 || xPos >= 872) || (yPos <= -512 || yPos >= 752))
        return;
      switch (direction)
      {
        case 0:
                    gfxPolyList[(int)gfxVertexSize].position.X = (float) (xPos << 4);
                    gfxPolyList[(int)gfxVertexSize].position.Y = (float) (yPos << 4);
                    gfxPolyList[(int)gfxVertexSize].color.R = byte.MaxValue;
                    gfxPolyList[(int)gfxVertexSize].color.G = byte.MaxValue;
                    gfxPolyList[(int)gfxVertexSize].color.B = byte.MaxValue;
                    gfxPolyList[(int)gfxVertexSize].color.A = byte.MaxValue;
                    gfxPolyList[(int)gfxVertexSize].texCoord.X = (float) (gfxSurface[surfaceNum].texStartX + xBegin) * 0.0009765625f;
                    gfxPolyList[(int)gfxVertexSize].texCoord.Y = (float) (gfxSurface[surfaceNum].texStartY + yBegin) * 0.0009765625f;
          ++gfxVertexSize;
                    gfxPolyList[(int)gfxVertexSize].position.X = (float) (xPos + xSize << 4);
                    gfxPolyList[(int)gfxVertexSize].position.Y = (float) (yPos << 4);
                    gfxPolyList[(int)gfxVertexSize].color.R = byte.MaxValue;
                    gfxPolyList[(int)gfxVertexSize].color.G = byte.MaxValue;
                    gfxPolyList[(int)gfxVertexSize].color.B = byte.MaxValue;
                    gfxPolyList[(int)gfxVertexSize].color.A = byte.MaxValue;
                    gfxPolyList[(int)gfxVertexSize].texCoord.X = (float) (gfxSurface[surfaceNum].texStartX + xBegin + xSize) * 0.0009765625f;
                    gfxPolyList[(int)gfxVertexSize].texCoord.Y = gfxPolyList[(int)gfxVertexSize - 1].texCoord.Y;
          ++gfxVertexSize;
                    gfxPolyList[(int)gfxVertexSize].position.X = (float) (xPos << 4);
                    gfxPolyList[(int)gfxVertexSize].position.Y = (float) (yPos + ySize << 4);
                    gfxPolyList[(int)gfxVertexSize].color.R = byte.MaxValue;
                    gfxPolyList[(int)gfxVertexSize].color.G = byte.MaxValue;
                    gfxPolyList[(int)gfxVertexSize].color.B = byte.MaxValue;
                    gfxPolyList[(int)gfxVertexSize].color.A = byte.MaxValue;
                    gfxPolyList[(int)gfxVertexSize].texCoord.X = gfxPolyList[(int)gfxVertexSize - 2].texCoord.X;
                    gfxPolyList[(int)gfxVertexSize].texCoord.Y = (float) (gfxSurface[surfaceNum].texStartY + yBegin + ySize) * 0.0009765625f;
          ++gfxVertexSize;
                    gfxPolyList[(int)gfxVertexSize].position.X = gfxPolyList[(int)gfxVertexSize - 2].position.X;
                    gfxPolyList[(int)gfxVertexSize].position.Y = gfxPolyList[(int)gfxVertexSize - 1].position.Y;
                    gfxPolyList[(int)gfxVertexSize].color.R = byte.MaxValue;
                    gfxPolyList[(int)gfxVertexSize].color.G = byte.MaxValue;
                    gfxPolyList[(int)gfxVertexSize].color.B = byte.MaxValue;
                    gfxPolyList[(int)gfxVertexSize].color.A = byte.MaxValue;
                    gfxPolyList[(int)gfxVertexSize].texCoord.X = gfxPolyList[(int)gfxVertexSize - 2].texCoord.X;
                    gfxPolyList[(int)gfxVertexSize].texCoord.Y = gfxPolyList[(int)gfxVertexSize - 1].texCoord.Y;
          ++gfxVertexSize;
          break;
        case 1:
                    gfxPolyList[(int)gfxVertexSize].position.X = (float) (xPos << 4);
                    gfxPolyList[(int)gfxVertexSize].position.Y = (float) (yPos << 4);
                    gfxPolyList[(int)gfxVertexSize].color.R = byte.MaxValue;
                    gfxPolyList[(int)gfxVertexSize].color.G = byte.MaxValue;
                    gfxPolyList[(int)gfxVertexSize].color.B = byte.MaxValue;
                    gfxPolyList[(int)gfxVertexSize].color.A = byte.MaxValue;
                    gfxPolyList[(int)gfxVertexSize].texCoord.X = (float) (gfxSurface[surfaceNum].texStartX + xBegin + xSize) * 0.0009765625f;
                    gfxPolyList[(int)gfxVertexSize].texCoord.Y = (float) (gfxSurface[surfaceNum].texStartY + yBegin) * 0.0009765625f;
          ++gfxVertexSize;
                    gfxPolyList[(int)gfxVertexSize].position.X = (float) (xPos + xSize << 4);
                    gfxPolyList[(int)gfxVertexSize].position.Y = (float) (yPos << 4);
                    gfxPolyList[(int)gfxVertexSize].color.R = byte.MaxValue;
                    gfxPolyList[(int)gfxVertexSize].color.G = byte.MaxValue;
                    gfxPolyList[(int)gfxVertexSize].color.B = byte.MaxValue;
                    gfxPolyList[(int)gfxVertexSize].color.A = byte.MaxValue;
                    gfxPolyList[(int)gfxVertexSize].texCoord.X = (float) (gfxSurface[surfaceNum].texStartX + xBegin) * 0.0009765625f;
                    gfxPolyList[(int)gfxVertexSize].texCoord.Y = gfxPolyList[(int)gfxVertexSize - 1].texCoord.Y;
          ++gfxVertexSize;
                    gfxPolyList[(int)gfxVertexSize].position.X = (float) (xPos << 4);
                    gfxPolyList[(int)gfxVertexSize].position.Y = (float) (yPos + ySize << 4);
                    gfxPolyList[(int)gfxVertexSize].color.R = byte.MaxValue;
                    gfxPolyList[(int)gfxVertexSize].color.G = byte.MaxValue;
                    gfxPolyList[(int)gfxVertexSize].color.B = byte.MaxValue;
                    gfxPolyList[(int)gfxVertexSize].color.A = byte.MaxValue;
                    gfxPolyList[(int)gfxVertexSize].texCoord.X = gfxPolyList[(int)gfxVertexSize - 2].texCoord.X;
                    gfxPolyList[(int)gfxVertexSize].texCoord.Y = (float) (gfxSurface[surfaceNum].texStartY + yBegin + ySize) * 0.0009765625f;
          ++gfxVertexSize;
                    gfxPolyList[(int)gfxVertexSize].position.X = gfxPolyList[(int)gfxVertexSize - 2].position.X;
                    gfxPolyList[(int)gfxVertexSize].position.Y = gfxPolyList[(int)gfxVertexSize - 1].position.Y;
                    gfxPolyList[(int)gfxVertexSize].color.R = byte.MaxValue;
                    gfxPolyList[(int)gfxVertexSize].color.G = byte.MaxValue;
                    gfxPolyList[(int)gfxVertexSize].color.B = byte.MaxValue;
                    gfxPolyList[(int)gfxVertexSize].color.A = byte.MaxValue;
                    gfxPolyList[(int)gfxVertexSize].texCoord.X = gfxPolyList[(int)gfxVertexSize - 2].texCoord.X;
                    gfxPolyList[(int)gfxVertexSize].texCoord.Y = gfxPolyList[(int)gfxVertexSize - 1].texCoord.Y;
          ++gfxVertexSize;
          break;
        case 2:
                    gfxPolyList[(int)gfxVertexSize].position.X = (float) (xPos << 4);
                    gfxPolyList[(int)gfxVertexSize].position.Y = (float) (yPos << 4);
                    gfxPolyList[(int)gfxVertexSize].color.R = byte.MaxValue;
                    gfxPolyList[(int)gfxVertexSize].color.G = byte.MaxValue;
                    gfxPolyList[(int)gfxVertexSize].color.B = byte.MaxValue;
                    gfxPolyList[(int)gfxVertexSize].color.A = byte.MaxValue;
                    gfxPolyList[(int)gfxVertexSize].texCoord.X = (float) (gfxSurface[surfaceNum].texStartX + xBegin) * 0.0009765625f;
                    gfxPolyList[(int)gfxVertexSize].texCoord.Y = (float) (gfxSurface[surfaceNum].texStartY + yBegin + ySize) * 0.0009765625f;
          ++gfxVertexSize;
                    gfxPolyList[(int)gfxVertexSize].position.X = (float) (xPos + xSize << 4);
                    gfxPolyList[(int)gfxVertexSize].position.Y = (float) (yPos << 4);
                    gfxPolyList[(int)gfxVertexSize].color.R = byte.MaxValue;
                    gfxPolyList[(int)gfxVertexSize].color.G = byte.MaxValue;
                    gfxPolyList[(int)gfxVertexSize].color.B = byte.MaxValue;
                    gfxPolyList[(int)gfxVertexSize].color.A = byte.MaxValue;
                    gfxPolyList[(int)gfxVertexSize].texCoord.X = (float) (gfxSurface[surfaceNum].texStartX + xBegin + xSize) * 0.0009765625f;
                    gfxPolyList[(int)gfxVertexSize].texCoord.Y = gfxPolyList[(int)gfxVertexSize - 1].texCoord.Y;
          ++gfxVertexSize;
                    gfxPolyList[(int)gfxVertexSize].position.X = (float) (xPos << 4);
                    gfxPolyList[(int)gfxVertexSize].position.Y = (float) (yPos + ySize << 4);
                    gfxPolyList[(int)gfxVertexSize].color.R = byte.MaxValue;
                    gfxPolyList[(int)gfxVertexSize].color.G = byte.MaxValue;
                    gfxPolyList[(int)gfxVertexSize].color.B = byte.MaxValue;
                    gfxPolyList[(int)gfxVertexSize].color.A = byte.MaxValue;
                    gfxPolyList[(int)gfxVertexSize].texCoord.X = gfxPolyList[(int)gfxVertexSize - 2].texCoord.X;
                    gfxPolyList[(int)gfxVertexSize].texCoord.Y = (float) (gfxSurface[surfaceNum].texStartY + yBegin) * 0.0009765625f;
          ++gfxVertexSize;
                    gfxPolyList[(int)gfxVertexSize].position.X = gfxPolyList[(int)gfxVertexSize - 2].position.X;
                    gfxPolyList[(int)gfxVertexSize].position.Y = gfxPolyList[(int)gfxVertexSize - 1].position.Y;
                    gfxPolyList[(int)gfxVertexSize].color.R = byte.MaxValue;
                    gfxPolyList[(int)gfxVertexSize].color.G = byte.MaxValue;
                    gfxPolyList[(int)gfxVertexSize].color.B = byte.MaxValue;
                    gfxPolyList[(int)gfxVertexSize].color.A = byte.MaxValue;
                    gfxPolyList[(int)gfxVertexSize].texCoord.X = gfxPolyList[(int)gfxVertexSize - 2].texCoord.X;
                    gfxPolyList[(int)gfxVertexSize].texCoord.Y = gfxPolyList[(int)gfxVertexSize - 1].texCoord.Y;
          ++gfxVertexSize;
          break;
        case 3:
                    gfxPolyList[(int)gfxVertexSize].position.X = (float) (xPos << 4);
                    gfxPolyList[(int)gfxVertexSize].position.Y = (float) (yPos << 4);
                    gfxPolyList[(int)gfxVertexSize].color.R = byte.MaxValue;
                    gfxPolyList[(int)gfxVertexSize].color.G = byte.MaxValue;
                    gfxPolyList[(int)gfxVertexSize].color.B = byte.MaxValue;
                    gfxPolyList[(int)gfxVertexSize].color.A = byte.MaxValue;
                    gfxPolyList[(int)gfxVertexSize].texCoord.X = (float) (gfxSurface[surfaceNum].texStartX + xBegin + xSize) * 0.0009765625f;
                    gfxPolyList[(int)gfxVertexSize].texCoord.Y = (float) (gfxSurface[surfaceNum].texStartY + yBegin + ySize) * 0.0009765625f;
          ++gfxVertexSize;
                    gfxPolyList[(int)gfxVertexSize].position.X = (float) (xPos + xSize << 4);
                    gfxPolyList[(int)gfxVertexSize].position.Y = (float) (yPos << 4);
                    gfxPolyList[(int)gfxVertexSize].color.R = byte.MaxValue;
                    gfxPolyList[(int)gfxVertexSize].color.G = byte.MaxValue;
                    gfxPolyList[(int)gfxVertexSize].color.B = byte.MaxValue;
                    gfxPolyList[(int)gfxVertexSize].color.A = byte.MaxValue;
                    gfxPolyList[(int)gfxVertexSize].texCoord.X = (float) (gfxSurface[surfaceNum].texStartX + xBegin) * 0.0009765625f;
                    gfxPolyList[(int)gfxVertexSize].texCoord.Y = gfxPolyList[(int)gfxVertexSize - 1].texCoord.Y;
          ++gfxVertexSize;
                    gfxPolyList[(int)gfxVertexSize].position.X = (float) (xPos << 4);
                    gfxPolyList[(int)gfxVertexSize].position.Y = (float) (yPos + ySize << 4);
                    gfxPolyList[(int)gfxVertexSize].color.R = byte.MaxValue;
                    gfxPolyList[(int)gfxVertexSize].color.G = byte.MaxValue;
                    gfxPolyList[(int)gfxVertexSize].color.B = byte.MaxValue;
                    gfxPolyList[(int)gfxVertexSize].color.A = byte.MaxValue;
                    gfxPolyList[(int)gfxVertexSize].texCoord.X = gfxPolyList[(int)gfxVertexSize - 2].texCoord.X;
                    gfxPolyList[(int)gfxVertexSize].texCoord.Y = (float) (gfxSurface[surfaceNum].texStartY + yBegin) * 0.0009765625f;
          ++gfxVertexSize;
                    gfxPolyList[(int)gfxVertexSize].position.X = gfxPolyList[(int)gfxVertexSize - 2].position.X;
                    gfxPolyList[(int)gfxVertexSize].position.Y = gfxPolyList[(int)gfxVertexSize - 1].position.Y;
                    gfxPolyList[(int)gfxVertexSize].color.R = byte.MaxValue;
                    gfxPolyList[(int)gfxVertexSize].color.G = byte.MaxValue;
                    gfxPolyList[(int)gfxVertexSize].color.B = byte.MaxValue;
                    gfxPolyList[(int)gfxVertexSize].color.A = byte.MaxValue;
                    gfxPolyList[(int)gfxVertexSize].texCoord.X = gfxPolyList[(int)gfxVertexSize - 2].texCoord.X;
                    gfxPolyList[(int)gfxVertexSize].texCoord.Y = gfxPolyList[(int)gfxVertexSize - 1].texCoord.Y;
          ++gfxVertexSize;
          break;
      }
            gfxIndexSize += (ushort) 2;
    }

    public static void DrawBlendedSprite(
      int xPos,
      int yPos,
      int xSize,
      int ySize,
      int xBegin,
      int yBegin,
      int surfaceNum)
    {
      if (gfxSurface[surfaceNum].texStartX <= -1 || gfxVertexSize >= (ushort) 8192 || (xPos <= -512 || xPos >= 872) || (yPos <= -512 || yPos >= 752))
        return;
            gfxPolyList[(int)gfxVertexSize].position.X = (float) (xPos << 4);
            gfxPolyList[(int)gfxVertexSize].position.Y = (float) (yPos << 4);
            gfxPolyList[(int)gfxVertexSize].color.R = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].color.G = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].color.B = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].color.A = (byte) 128;
            gfxPolyList[(int)gfxVertexSize].texCoord.X = (float) (gfxSurface[surfaceNum].texStartX + xBegin) * 0.0009765625f;
            gfxPolyList[(int)gfxVertexSize].texCoord.Y = (float) (gfxSurface[surfaceNum].texStartY + yBegin) * 0.0009765625f;
      ++gfxVertexSize;
            gfxPolyList[(int)gfxVertexSize].position.X = (float) (xPos + xSize << 4);
            gfxPolyList[(int)gfxVertexSize].position.Y = (float) (yPos << 4);
            gfxPolyList[(int)gfxVertexSize].color.R = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].color.G = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].color.B = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].color.A = (byte) 128;
            gfxPolyList[(int)gfxVertexSize].texCoord.X = (float) (gfxSurface[surfaceNum].texStartX + xBegin + xSize) * 0.0009765625f;
            gfxPolyList[(int)gfxVertexSize].texCoord.Y = gfxPolyList[(int)gfxVertexSize - 1].texCoord.Y;
      ++gfxVertexSize;
            gfxPolyList[(int)gfxVertexSize].position.X = (float) (xPos << 4);
            gfxPolyList[(int)gfxVertexSize].position.Y = (float) (yPos + ySize << 4);
            gfxPolyList[(int)gfxVertexSize].color.R = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].color.G = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].color.B = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].color.A = (byte) 128;
            gfxPolyList[(int)gfxVertexSize].texCoord.X = gfxPolyList[(int)gfxVertexSize - 2].texCoord.X;
            gfxPolyList[(int)gfxVertexSize].texCoord.Y = (float) (gfxSurface[surfaceNum].texStartY + yBegin + ySize) * 0.0009765625f;
      ++gfxVertexSize;
            gfxPolyList[(int)gfxVertexSize].position.X = gfxPolyList[(int)gfxVertexSize - 2].position.X;
            gfxPolyList[(int)gfxVertexSize].position.Y = gfxPolyList[(int)gfxVertexSize - 1].position.Y;
            gfxPolyList[(int)gfxVertexSize].color.R = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].color.G = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].color.B = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].color.A = (byte) 128;
            gfxPolyList[(int)gfxVertexSize].texCoord.X = gfxPolyList[(int)gfxVertexSize - 2].texCoord.X;
            gfxPolyList[(int)gfxVertexSize].texCoord.Y = gfxPolyList[(int)gfxVertexSize - 1].texCoord.Y;
      ++gfxVertexSize;
            gfxIndexSize += (ushort) 2;
    }

    public static void DrawAlphaBlendedSprite(
      int xPos,
      int yPos,
      int xSize,
      int ySize,
      int xBegin,
      int yBegin,
      int alpha,
      int surfaceNum)
    {
      if (gfxSurface[surfaceNum].texStartX <= -1 || gfxVertexSize >= (ushort) 8192 || (xPos <= -512 || xPos >= 872) || (yPos <= -512 || yPos >= 752))
        return;
            gfxPolyList[(int)gfxVertexSize].position.X = (float) (xPos << 4);
            gfxPolyList[(int)gfxVertexSize].position.Y = (float) (yPos << 4);
            gfxPolyList[(int)gfxVertexSize].color.R = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].color.G = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].color.B = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].color.A = (byte) alpha;
            gfxPolyList[(int)gfxVertexSize].texCoord.X = (float) (gfxSurface[surfaceNum].texStartX + xBegin) * 0.0009765625f;
            gfxPolyList[(int)gfxVertexSize].texCoord.Y = (float) (gfxSurface[surfaceNum].texStartY + yBegin) * 0.0009765625f;
      ++gfxVertexSize;
            gfxPolyList[(int)gfxVertexSize].position.X = (float) (xPos + xSize << 4);
            gfxPolyList[(int)gfxVertexSize].position.Y = (float) (yPos << 4);
            gfxPolyList[(int)gfxVertexSize].color.R = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].color.G = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].color.B = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].color.A = (byte) alpha;
            gfxPolyList[(int)gfxVertexSize].texCoord.X = (float) (gfxSurface[surfaceNum].texStartX + xBegin + xSize) * 0.0009765625f;
            gfxPolyList[(int)gfxVertexSize].texCoord.Y = gfxPolyList[(int)gfxVertexSize - 1].texCoord.Y;
      ++gfxVertexSize;
            gfxPolyList[(int)gfxVertexSize].position.X = (float) (xPos << 4);
            gfxPolyList[(int)gfxVertexSize].position.Y = (float) (yPos + ySize << 4);
            gfxPolyList[(int)gfxVertexSize].color.R = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].color.G = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].color.B = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].color.A = (byte) alpha;
            gfxPolyList[(int)gfxVertexSize].texCoord.X = gfxPolyList[(int)gfxVertexSize - 2].texCoord.X;
            gfxPolyList[(int)gfxVertexSize].texCoord.Y = (float) (gfxSurface[surfaceNum].texStartY + yBegin + ySize) * 0.0009765625f;
      ++gfxVertexSize;
            gfxPolyList[gfxVertexSize].position.X = gfxPolyList[(int)gfxVertexSize - 2].position.X;
            gfxPolyList[(int)gfxVertexSize].position.Y = gfxPolyList[(int)gfxVertexSize - 1].position.Y;
            gfxPolyList[(int)gfxVertexSize].color.R = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].color.G = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].color.B = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].color.A = (byte) alpha;
            gfxPolyList[(int)gfxVertexSize].texCoord.X = gfxPolyList[(int)gfxVertexSize - 2].texCoord.X;
            gfxPolyList[(int)gfxVertexSize].texCoord.Y = gfxPolyList[(int)gfxVertexSize - 1].texCoord.Y;
      ++gfxVertexSize;
            gfxIndexSize += (ushort) 2;
    }

    public static void DrawAdditiveBlendedSprite(
      int xPos,
      int yPos,
      int xSize,
      int ySize,
      int xBegin,
      int yBegin,
      int alpha,
      int surfaceNum)
    {
      if (gfxSurface[surfaceNum].texStartX <= -1 || gfxVertexSize >= (ushort) 8192 || (xPos <= -512 || xPos >= 872) || (yPos <= -512 || yPos >= 752))
        return;
            gfxPolyList[(int)gfxVertexSize].position.X = (float) (xPos << 4);
            gfxPolyList[(int)gfxVertexSize].position.Y = (float) (yPos << 4);
            gfxPolyList[(int)gfxVertexSize].color.R = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].color.G = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].color.B = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].color.A = (byte) alpha;
            gfxPolyList[(int)gfxVertexSize].texCoord.X = (float) (gfxSurface[surfaceNum].texStartX + xBegin) * 0.0009765625f;
            gfxPolyList[(int)gfxVertexSize].texCoord.Y = (float) (gfxSurface[surfaceNum].texStartY + yBegin) * 0.0009765625f;
      ++gfxVertexSize;
            gfxPolyList[(int)gfxVertexSize].position.X = (float) (xPos + xSize << 4);
            gfxPolyList[(int)gfxVertexSize].position.Y = (float) (yPos << 4);
            gfxPolyList[(int)gfxVertexSize].color.R = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].color.G = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].color.B = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].color.A = (byte) alpha;
            gfxPolyList[(int)gfxVertexSize].texCoord.X = (float) (gfxSurface[surfaceNum].texStartX + xBegin + xSize) * 0.0009765625f;
            gfxPolyList[(int)gfxVertexSize].texCoord.Y = gfxPolyList[(int)gfxVertexSize - 1].texCoord.Y;
      ++gfxVertexSize;
            gfxPolyList[(int)gfxVertexSize].position.X = (float) (xPos << 4);
            gfxPolyList[(int)gfxVertexSize].position.Y = (float) (yPos + ySize << 4);
            gfxPolyList[(int)gfxVertexSize].color.R = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].color.G = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].color.B = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].color.A = (byte) alpha;
            gfxPolyList[(int)gfxVertexSize].texCoord.X = gfxPolyList[(int)gfxVertexSize - 2].texCoord.X;
            gfxPolyList[(int)gfxVertexSize].texCoord.Y = (float) (gfxSurface[surfaceNum].texStartY + yBegin + ySize) * 0.0009765625f;
      ++gfxVertexSize;
            gfxPolyList[(int)gfxVertexSize].position.X = gfxPolyList[(int)gfxVertexSize - 2].position.X;
            gfxPolyList[(int)gfxVertexSize].position.Y = gfxPolyList[(int)gfxVertexSize - 1].position.Y;
            gfxPolyList[(int)gfxVertexSize].color.R = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].color.G = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].color.B = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].color.A = (byte) alpha;
            gfxPolyList[(int)gfxVertexSize].texCoord.X = gfxPolyList[(int)gfxVertexSize - 2].texCoord.X;
            gfxPolyList[(int)gfxVertexSize].texCoord.Y = gfxPolyList[(int)gfxVertexSize - 1].texCoord.Y;
      ++gfxVertexSize;
            gfxIndexSize += (ushort) 2;
    }

    public static void DrawSubtractiveBlendedSprite(
      int xPos,
      int yPos,
      int xSize,
      int ySize,
      int xBegin,
      int yBegin,
      int alpha,
      int surfaceNum)
    {
      if (gfxSurface[surfaceNum].texStartX <= -1 || gfxVertexSize >= (ushort) 8192 || (xPos <= -512 || xPos >= 872) || (yPos <= -512 || yPos >= 752))
        return;
            gfxPolyList[(int)gfxVertexSize].position.X = (float) (xPos << 4);
            gfxPolyList[(int)gfxVertexSize].position.Y = (float) (yPos << 4);
            gfxPolyList[(int)gfxVertexSize].color.R = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].color.G = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].color.B = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].color.A = (byte) alpha;
            gfxPolyList[(int)gfxVertexSize].texCoord.X = (float) (gfxSurface[surfaceNum].texStartX + xBegin) * 0.0009765625f;
            gfxPolyList[(int)gfxVertexSize].texCoord.Y = (float) (gfxSurface[surfaceNum].texStartY + yBegin) * 0.0009765625f;
      ++gfxVertexSize;
            gfxPolyList[(int)gfxVertexSize].position.X = (float) (xPos + xSize << 4);
            gfxPolyList[(int)gfxVertexSize].position.Y = (float) (yPos << 4);
            gfxPolyList[(int)gfxVertexSize].color.R = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].color.G = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].color.B = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].color.A = (byte) alpha;
            gfxPolyList[(int)gfxVertexSize].texCoord.X = (float) (gfxSurface[surfaceNum].texStartX + xBegin + xSize) * 0.0009765625f;
            gfxPolyList[(int)gfxVertexSize].texCoord.Y = gfxPolyList[(int)gfxVertexSize - 1].texCoord.Y;
      ++gfxVertexSize;
            gfxPolyList[(int)gfxVertexSize].position.X = (float) (xPos << 4);
            gfxPolyList[(int)gfxVertexSize].position.Y = (float) (yPos + ySize << 4);
            gfxPolyList[(int)gfxVertexSize].color.R = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].color.G = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].color.B = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].color.A = (byte) alpha;
            gfxPolyList[(int)gfxVertexSize].texCoord.X = gfxPolyList[(int)gfxVertexSize - 2].texCoord.X;
            gfxPolyList[(int)gfxVertexSize].texCoord.Y = (float) (gfxSurface[surfaceNum].texStartY + yBegin + ySize) * 0.0009765625f;
      ++gfxVertexSize;
            gfxPolyList[(int)gfxVertexSize].position.X = gfxPolyList[(int)gfxVertexSize - 2].position.X;
            gfxPolyList[(int)gfxVertexSize].position.Y = gfxPolyList[(int)gfxVertexSize - 1].position.Y;
            gfxPolyList[(int)gfxVertexSize].color.R = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].color.G = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].color.B = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].color.A = (byte) alpha;
            gfxPolyList[(int)gfxVertexSize].texCoord.X = gfxPolyList[(int)gfxVertexSize - 2].texCoord.X;
            gfxPolyList[(int)gfxVertexSize].texCoord.Y = gfxPolyList[(int)gfxVertexSize - 1].texCoord.Y;
      ++gfxVertexSize;
            gfxIndexSize += (ushort) 2;
    }

    public static void DrawRectangle(
      int xPos,
      int yPos,
      int xSize,
      int ySize,
      int r,
      int g,
      int b,
      int alpha)
    {
      if (alpha > (int) byte.MaxValue)
        alpha = (int) byte.MaxValue;
      if (gfxVertexSize >= (ushort) 8192)
        return;
            gfxPolyList[(int)gfxVertexSize].position.X = (float) (xPos << 4);
            gfxPolyList[(int)gfxVertexSize].position.Y = (float) (yPos << 4);
            gfxPolyList[(int)gfxVertexSize].color.R = (byte) r;
            gfxPolyList[(int)gfxVertexSize].color.G = (byte) g;
            gfxPolyList[(int)gfxVertexSize].color.B = (byte) b;
            gfxPolyList[(int)gfxVertexSize].color.A = (byte) alpha;
            gfxPolyList[(int)gfxVertexSize].texCoord.X = 0.0f;
            gfxPolyList[(int)gfxVertexSize].texCoord.Y = 0.0f;
      ++gfxVertexSize;
            gfxPolyList[(int)gfxVertexSize].position.X = (float) (xPos + xSize << 4);
            gfxPolyList[(int)gfxVertexSize].position.Y = (float) (yPos << 4);
            gfxPolyList[(int)gfxVertexSize].color.R = (byte) r;
            gfxPolyList[(int)gfxVertexSize].color.G = (byte) g;
            gfxPolyList[(int)gfxVertexSize].color.B = (byte) b;
            gfxPolyList[(int)gfxVertexSize].color.A = (byte) alpha;
            gfxPolyList[(int)gfxVertexSize].texCoord.X = 0.01f;
            gfxPolyList[(int)gfxVertexSize].texCoord.Y = gfxPolyList[(int)gfxVertexSize - 1].texCoord.Y;
      ++gfxVertexSize;
            gfxPolyList[(int)gfxVertexSize].position.X = (float) (xPos << 4);
            gfxPolyList[(int)gfxVertexSize].position.Y = (float) (yPos + ySize << 4);
            gfxPolyList[(int)gfxVertexSize].color.R = (byte) r;
            gfxPolyList[(int)gfxVertexSize].color.G = (byte) g;
            gfxPolyList[(int)gfxVertexSize].color.B = (byte) b;
            gfxPolyList[(int)gfxVertexSize].color.A = (byte) alpha;
            gfxPolyList[(int)gfxVertexSize].texCoord.X = 0.0f;
            gfxPolyList[(int)gfxVertexSize].texCoord.Y = 0.01f;
      ++gfxVertexSize;
            gfxPolyList[(int)gfxVertexSize].position.X = gfxPolyList[(int)gfxVertexSize - 2].position.X;
            gfxPolyList[(int)gfxVertexSize].position.Y = gfxPolyList[(int)gfxVertexSize - 1].position.Y;
            gfxPolyList[(int)gfxVertexSize].color.R = (byte) r;
            gfxPolyList[(int)gfxVertexSize].color.G = (byte) g;
            gfxPolyList[(int)gfxVertexSize].color.B = (byte) b;
            gfxPolyList[(int)gfxVertexSize].color.A = (byte) alpha;
            gfxPolyList[(int)gfxVertexSize].texCoord.X = 0.01f;
            gfxPolyList[(int)gfxVertexSize].texCoord.Y = 0.01f;
      ++gfxVertexSize;
            gfxIndexSize += (ushort) 2;
    }

    public static void DrawTintRectangle(int xPos, int yPos, int xSize, int ySize)
    {
    }

    public static void DrawTintSpriteMask(
      int xPos,
      int yPos,
      int xSize,
      int ySize,
      int xBegin,
      int yBegin,
      int tableNo,
      int surfaceNum)
    {
    }

    public static void DrawScaledTintMask(
      byte direction,
      int xPos,
      int yPos,
      int xPivot,
      int yPivot,
      int xScale,
      int yScale,
      int xSize,
      int ySize,
      int xBegin,
      int yBegin,
      int surfaceNum)
    {
    }

    public static void DrawScaledSprite(
      byte direction,
      int xPos,
      int yPos,
      int xPivot,
      int yPivot,
      int xScale,
      int yScale,
      int xSize,
      int ySize,
      int xBegin,
      int yBegin,
      int surfaceNum)
    {
      if (gfxVertexSize >= (ushort) 8192 || xPos <= -512 || (xPos >= 872 || yPos <= -512) || yPos >= 752)
        return;
      xScale <<= 2;
      yScale <<= 2;
      xPos -= xPivot * xScale >> 11;
      xScale = xSize * xScale >> 11;
      yPos -= yPivot * yScale >> 11;
      yScale = ySize * yScale >> 11;
      if (gfxSurface[surfaceNum].texStartX <= -1)
        return;
            gfxPolyList[(int)gfxVertexSize].position.X = (float) (xPos << 4);
            gfxPolyList[(int)gfxVertexSize].position.Y = (float) (yPos << 4);
            gfxPolyList[(int)gfxVertexSize].color.R = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].color.G = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].color.B = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].color.A = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].texCoord.X = (float) (gfxSurface[surfaceNum].texStartX + xBegin) * 0.0009765625f;
            gfxPolyList[(int)gfxVertexSize].texCoord.Y = (float) (gfxSurface[surfaceNum].texStartY + yBegin) * 0.0009765625f;
      ++gfxVertexSize;
            gfxPolyList[(int)gfxVertexSize].position.X = (float) (xPos + xScale << 4);
            gfxPolyList[(int)gfxVertexSize].position.Y = (float) (yPos << 4);
            gfxPolyList[(int)gfxVertexSize].color.R = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].color.G = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].color.B = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].color.A = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].texCoord.X = (float) (gfxSurface[surfaceNum].texStartX + xBegin + xSize) * 0.0009765625f;
            gfxPolyList[(int)gfxVertexSize].texCoord.Y = gfxPolyList[(int)gfxVertexSize - 1].texCoord.Y;
      ++gfxVertexSize;
            gfxPolyList[(int)gfxVertexSize].position.X = (float) (xPos << 4);
            gfxPolyList[(int)gfxVertexSize].position.Y = (float) (yPos + yScale << 4);
            gfxPolyList[(int)gfxVertexSize].color.R = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].color.G = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].color.B = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].color.A = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].texCoord.X = gfxPolyList[(int)gfxVertexSize - 2].texCoord.X;
            gfxPolyList[(int)gfxVertexSize].texCoord.Y = (float) (gfxSurface[surfaceNum].texStartY + yBegin + ySize) * 0.0009765625f;
      ++gfxVertexSize;
            gfxPolyList[(int)gfxVertexSize].position.X = gfxPolyList[(int)gfxVertexSize - 2].position.X;
            gfxPolyList[(int)gfxVertexSize].position.Y = gfxPolyList[(int)gfxVertexSize - 1].position.Y;
            gfxPolyList[(int)gfxVertexSize].color.R = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].color.G = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].color.B = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].color.A = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].texCoord.X = gfxPolyList[(int)gfxVertexSize - 2].texCoord.X;
            gfxPolyList[(int)gfxVertexSize].texCoord.Y = gfxPolyList[(int)gfxVertexSize - 1].texCoord.Y;
      ++gfxVertexSize;
            gfxIndexSize += (ushort) 2;
    }

    public static void DrawScaledChar(
      byte direction,
      int xPos,
      int yPos,
      int xPivot,
      int yPivot,
      int xScale,
      int yScale,
      int xSize,
      int ySize,
      int xBegin,
      int yBegin,
      int surfaceNum)
    {
      if (gfxVertexSize >= (ushort) 8192 || xPos <= -8192 || (xPos >= 13951 || yPos <= -1024) || yPos >= 4864)
        return;
      xPos -= xPivot * xScale >> 5;
      xScale = xSize * xScale >> 5;
      yPos -= yPivot * yScale >> 5;
      yScale = ySize * yScale >> 5;
      if (gfxSurface[surfaceNum].texStartX <= -1 || gfxVertexSize >= (ushort) 4096)
        return;
            gfxPolyList[(int)gfxVertexSize].position.X = (float) xPos;
            gfxPolyList[(int)gfxVertexSize].position.Y = (float) yPos;
            gfxPolyList[(int)gfxVertexSize].color.R = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].color.G = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].color.B = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].color.A = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].texCoord.X = (float) (gfxSurface[surfaceNum].texStartX + xBegin) * 0.0009765625f;
            gfxPolyList[(int)gfxVertexSize].texCoord.Y = (float) (gfxSurface[surfaceNum].texStartY + yBegin) * 0.0009765625f;
      ++gfxVertexSize;
            gfxPolyList[(int)gfxVertexSize].position.X = (float) (xPos + xScale);
            gfxPolyList[(int)gfxVertexSize].position.Y = (float) yPos;
            gfxPolyList[(int)gfxVertexSize].color.R = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].color.G = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].color.B = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].color.A = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].texCoord.X = (float) (gfxSurface[surfaceNum].texStartX + xBegin + xSize) * 0.0009765625f;
            gfxPolyList[(int)gfxVertexSize].texCoord.Y = gfxPolyList[(int)gfxVertexSize - 1].texCoord.Y;
      ++gfxVertexSize;
            gfxPolyList[(int)gfxVertexSize].position.X = (float) xPos;
            gfxPolyList[(int)gfxVertexSize].position.Y = (float) (yPos + yScale);
            gfxPolyList[(int)gfxVertexSize].color.R = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].color.G = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].color.B = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].color.A = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].texCoord.X = gfxPolyList[(int)gfxVertexSize - 2].texCoord.X;
            gfxPolyList[(int)gfxVertexSize].texCoord.Y = (float) (gfxSurface[surfaceNum].texStartY + yBegin + ySize) * 0.0009765625f;
      ++gfxVertexSize;
            gfxPolyList[(int)gfxVertexSize].position.X = gfxPolyList[(int)gfxVertexSize - 2].position.X;
            gfxPolyList[(int)gfxVertexSize].position.Y = gfxPolyList[(int)gfxVertexSize - 1].position.Y;
            gfxPolyList[(int)gfxVertexSize].color.R = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].color.G = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].color.B = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].color.A = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].texCoord.X = gfxPolyList[(int)gfxVertexSize - 2].texCoord.X;
            gfxPolyList[(int)gfxVertexSize].texCoord.Y = gfxPolyList[(int)gfxVertexSize - 1].texCoord.Y;
      ++gfxVertexSize;
            gfxIndexSize += (ushort) 2;
    }

    public static void DrawRotatedSprite(
      byte direction,
      int xPos,
      int yPos,
      int xPivot,
      int yPivot,
      int xBegin,
      int yBegin,
      int xSize,
      int ySize,
      int rotAngle,
      int surfaceNum)
    {
      xPos <<= 4;
      yPos <<= 4;
      rotAngle -= rotAngle >> 9 << 9;
      if (rotAngle < 0)
        rotAngle += 512;
      if (rotAngle != 0)
        rotAngle = 512 - rotAngle;
      int num1 = GlobalAppDefinitions.SinValue512[rotAngle];
      int num2 = GlobalAppDefinitions.CosValue512[rotAngle];
      if (gfxSurface[surfaceNum].texStartX <= -1 || gfxVertexSize >= (ushort) 8192 || (xPos <= -8192 || xPos >= 13952) || (yPos <= -8192 || yPos >= 12032))
        return;
      if (direction == (byte) 0)
      {
        int num3 = -xPivot;
        int num4 = -yPivot;
                gfxPolyList[(int)gfxVertexSize].position.X = (float) (xPos + (num3 * num2 + num4 * num1 >> 5));
                gfxPolyList[(int)gfxVertexSize].position.Y = (float) (yPos + (num4 * num2 - num3 * num1 >> 5));
                gfxPolyList[(int)gfxVertexSize].color.R = byte.MaxValue;
                gfxPolyList[(int)gfxVertexSize].color.G = byte.MaxValue;
                gfxPolyList[(int)gfxVertexSize].color.B = byte.MaxValue;
                gfxPolyList[(int)gfxVertexSize].color.A = byte.MaxValue;
                gfxPolyList[(int)gfxVertexSize].texCoord.X = (float) (gfxSurface[surfaceNum].texStartX + xBegin) * 0.0009765625f;
                gfxPolyList[(int)gfxVertexSize].texCoord.Y = (float) (gfxSurface[surfaceNum].texStartY + yBegin) * 0.0009765625f;
        ++gfxVertexSize;
        int num5 = xSize - xPivot;
        int num6 = -yPivot;
                gfxPolyList[(int)gfxVertexSize].position.X = (float) (xPos + (num5 * num2 + num6 * num1 >> 5));
                gfxPolyList[(int)gfxVertexSize].position.Y = (float) (yPos + (num6 * num2 - num5 * num1 >> 5));
                gfxPolyList[(int)gfxVertexSize].color.R = byte.MaxValue;
                gfxPolyList[(int)gfxVertexSize].color.G = byte.MaxValue;
                gfxPolyList[(int)gfxVertexSize].color.B = byte.MaxValue;
                gfxPolyList[(int)gfxVertexSize].color.A = byte.MaxValue;
                gfxPolyList[(int)gfxVertexSize].texCoord.X = (float) (gfxSurface[surfaceNum].texStartX + xBegin + xSize) * 0.0009765625f;
                gfxPolyList[(int)gfxVertexSize].texCoord.Y = gfxPolyList[(int)gfxVertexSize - 1].texCoord.Y;
        ++gfxVertexSize;
        int num7 = -xPivot;
        int num8 = ySize - yPivot;
                gfxPolyList[(int)gfxVertexSize].position.X = (float) (xPos + (num7 * num2 + num8 * num1 >> 5));
                gfxPolyList[(int)gfxVertexSize].position.Y = (float) (yPos + (num8 * num2 - num7 * num1 >> 5));
                gfxPolyList[(int)gfxVertexSize].color.R = byte.MaxValue;
                gfxPolyList[(int)gfxVertexSize].color.G = byte.MaxValue;
                gfxPolyList[(int)gfxVertexSize].color.B = byte.MaxValue;
                gfxPolyList[(int)gfxVertexSize].color.A = byte.MaxValue;
                gfxPolyList[(int)gfxVertexSize].texCoord.X = gfxPolyList[(int)gfxVertexSize - 2].texCoord.X;
                gfxPolyList[(int)gfxVertexSize].texCoord.Y = (float) (gfxSurface[surfaceNum].texStartY + yBegin + ySize) * 0.0009765625f;
        ++gfxVertexSize;
        int num9 = xSize - xPivot;
        int num10 = ySize - yPivot;
                gfxPolyList[(int)gfxVertexSize].position.X = (float) (xPos + (num9 * num2 + num10 * num1 >> 5));
                gfxPolyList[(int)gfxVertexSize].position.Y = (float) (yPos + (num10 * num2 - num9 * num1 >> 5));
                gfxPolyList[(int)gfxVertexSize].color.R = byte.MaxValue;
                gfxPolyList[(int)gfxVertexSize].color.G = byte.MaxValue;
                gfxPolyList[(int)gfxVertexSize].color.B = byte.MaxValue;
                gfxPolyList[(int)gfxVertexSize].color.A = byte.MaxValue;
                gfxPolyList[(int)gfxVertexSize].texCoord.X = gfxPolyList[(int)gfxVertexSize - 2].texCoord.X;
                gfxPolyList[(int)gfxVertexSize].texCoord.Y = gfxPolyList[(int)gfxVertexSize - 1].texCoord.Y;
        ++gfxVertexSize;
                gfxIndexSize += (ushort) 2;
      }
      else
      {
        int num3 = xPivot;
        int num4 = -yPivot;
                gfxPolyList[(int)gfxVertexSize].position.X = (float) (xPos + (num3 * num2 + num4 * num1 >> 5));
                gfxPolyList[(int)gfxVertexSize].position.Y = (float) (yPos + (num4 * num2 - num3 * num1 >> 5));
                gfxPolyList[(int)gfxVertexSize].color.R = byte.MaxValue;
                gfxPolyList[(int)gfxVertexSize].color.G = byte.MaxValue;
                gfxPolyList[(int)gfxVertexSize].color.B = byte.MaxValue;
                gfxPolyList[(int)gfxVertexSize].color.A = byte.MaxValue;
                gfxPolyList[(int)gfxVertexSize].texCoord.X = (float) (gfxSurface[surfaceNum].texStartX + xBegin) * 0.0009765625f;
                gfxPolyList[(int)gfxVertexSize].texCoord.Y = (float) (gfxSurface[surfaceNum].texStartY + yBegin) * 0.0009765625f;
        ++gfxVertexSize;
        int num5 = xPivot - xSize;
        int num6 = -yPivot;
                gfxPolyList[(int)gfxVertexSize].position.X = (float) (xPos + (num5 * num2 + num6 * num1 >> 5));
                gfxPolyList[(int)gfxVertexSize].position.Y = (float) (yPos + (num6 * num2 - num5 * num1 >> 5));
                gfxPolyList[(int)gfxVertexSize].color.R = byte.MaxValue;
                gfxPolyList[(int)gfxVertexSize].color.G = byte.MaxValue;
                gfxPolyList[(int)gfxVertexSize].color.B = byte.MaxValue;
                gfxPolyList[(int)gfxVertexSize].color.A = byte.MaxValue;
                gfxPolyList[(int)gfxVertexSize].texCoord.X = (float) (gfxSurface[surfaceNum].texStartX + xBegin + xSize) * 0.0009765625f;
                gfxPolyList[(int)gfxVertexSize].texCoord.Y = gfxPolyList[(int)gfxVertexSize - 1].texCoord.Y;
        ++gfxVertexSize;
        int num7 = xPivot;
        int num8 = ySize - yPivot;
                gfxPolyList[(int)gfxVertexSize].position.X = (float) (xPos + (num7 * num2 + num8 * num1 >> 5));
                gfxPolyList[(int)gfxVertexSize].position.Y = (float) (yPos + (num8 * num2 - num7 * num1 >> 5));
                gfxPolyList[(int)gfxVertexSize].color.R = byte.MaxValue;
                gfxPolyList[(int)gfxVertexSize].color.G = byte.MaxValue;
                gfxPolyList[(int)gfxVertexSize].color.B = byte.MaxValue;
                gfxPolyList[(int)gfxVertexSize].color.A = byte.MaxValue;
                gfxPolyList[(int)gfxVertexSize].texCoord.X = gfxPolyList[(int)gfxVertexSize - 2].texCoord.X;
                gfxPolyList[(int)gfxVertexSize].texCoord.Y = (float) (gfxSurface[surfaceNum].texStartY + yBegin + ySize) * 0.0009765625f;
        ++gfxVertexSize;
        int num9 = xPivot - xSize;
        int num10 = ySize - yPivot;
                gfxPolyList[(int)gfxVertexSize].position.X = (float) (xPos + (num9 * num2 + num10 * num1 >> 5));
                gfxPolyList[(int)gfxVertexSize].position.Y = (float) (yPos + (num10 * num2 - num9 * num1 >> 5));
                gfxPolyList[(int)gfxVertexSize].color.R = byte.MaxValue;
                gfxPolyList[(int)gfxVertexSize].color.G = byte.MaxValue;
                gfxPolyList[(int)gfxVertexSize].color.B = byte.MaxValue;
                gfxPolyList[(int)gfxVertexSize].color.A = byte.MaxValue;
                gfxPolyList[(int)gfxVertexSize].texCoord.X = gfxPolyList[(int)gfxVertexSize - 2].texCoord.X;
                gfxPolyList[(int)gfxVertexSize].texCoord.Y = gfxPolyList[(int)gfxVertexSize - 1].texCoord.Y;
        ++gfxVertexSize;
                gfxIndexSize += (ushort) 2;
      }
    }

    public static void DrawRotoZoomSprite(
      byte direction,
      int xPos,
      int yPos,
      int xPivot,
      int yPivot,
      int xBegin,
      int yBegin,
      int xSize,
      int ySize,
      int rotAngle,
      int scale,
      int surfaceNum)
    {
      xPos <<= 4;
      yPos <<= 4;
      rotAngle -= rotAngle >> 9 << 9;
      if (rotAngle < 0)
        rotAngle += 512;
      if (rotAngle != 0)
        rotAngle = 512 - rotAngle;
      int num1 = GlobalAppDefinitions.SinValue512[rotAngle] * scale >> 9;
      int num2 = GlobalAppDefinitions.CosValue512[rotAngle] * scale >> 9;
      if (gfxSurface[surfaceNum].texStartX <= -1 || gfxVertexSize >= (ushort) 8192 || (xPos <= -8192 || xPos >= 13952) || (yPos <= -8192 || yPos >= 12032))
        return;
      if (direction == (byte) 0)
      {
        int num3 = -xPivot;
        int num4 = -yPivot;
                gfxPolyList[(int)gfxVertexSize].position.X = (float) (xPos + (num3 * num2 + num4 * num1 >> 5));
                gfxPolyList[(int)gfxVertexSize].position.Y = (float) (yPos + (num4 * num2 - num3 * num1 >> 5));
                gfxPolyList[(int)gfxVertexSize].color.R = byte.MaxValue;
                gfxPolyList[(int)gfxVertexSize].color.G = byte.MaxValue;
                gfxPolyList[(int)gfxVertexSize].color.B = byte.MaxValue;
                gfxPolyList[(int)gfxVertexSize].color.A = byte.MaxValue;
                gfxPolyList[(int)gfxVertexSize].texCoord.X = (float) (gfxSurface[surfaceNum].texStartX + xBegin) * 0.0009765625f;
                gfxPolyList[(int)gfxVertexSize].texCoord.Y = (float) (gfxSurface[surfaceNum].texStartY + yBegin) * 0.0009765625f;
        ++gfxVertexSize;
        int num5 = xSize - xPivot;
        int num6 = -yPivot;
                gfxPolyList[(int)gfxVertexSize].position.X = (float) (xPos + (num5 * num2 + num6 * num1 >> 5));
                gfxPolyList[(int)gfxVertexSize].position.Y = (float) (yPos + (num6 * num2 - num5 * num1 >> 5));
                gfxPolyList[(int)gfxVertexSize].color.R = byte.MaxValue;
                gfxPolyList[(int)gfxVertexSize].color.G = byte.MaxValue;
                gfxPolyList[(int)gfxVertexSize].color.B = byte.MaxValue;
                gfxPolyList[(int)gfxVertexSize].color.A = byte.MaxValue;
                gfxPolyList[(int)gfxVertexSize].texCoord.X = (float) (gfxSurface[surfaceNum].texStartX + xBegin + xSize) * 0.0009765625f;
                gfxPolyList[(int)gfxVertexSize].texCoord.Y = gfxPolyList[(int)gfxVertexSize - 1].texCoord.Y;
        ++gfxVertexSize;
        int num7 = -xPivot;
        int num8 = ySize - yPivot;
                gfxPolyList[(int)gfxVertexSize].position.X = (float) (xPos + (num7 * num2 + num8 * num1 >> 5));
                gfxPolyList[(int)gfxVertexSize].position.Y = (float) (yPos + (num8 * num2 - num7 * num1 >> 5));
                gfxPolyList[(int)gfxVertexSize].color.R = byte.MaxValue;
                gfxPolyList[(int)gfxVertexSize].color.G = byte.MaxValue;
                gfxPolyList[(int)gfxVertexSize].color.B = byte.MaxValue;
                gfxPolyList[(int)gfxVertexSize].color.A = byte.MaxValue;
                gfxPolyList[(int)gfxVertexSize].texCoord.X = gfxPolyList[(int)gfxVertexSize - 2].texCoord.X;
                gfxPolyList[(int)gfxVertexSize].texCoord.Y = (float) (gfxSurface[surfaceNum].texStartY + yBegin + ySize) * 0.0009765625f;
        ++gfxVertexSize;
        int num9 = xSize - xPivot;
        int num10 = ySize - yPivot;
                gfxPolyList[(int)gfxVertexSize].position.X = (float) (xPos + (num9 * num2 + num10 * num1 >> 5));
                gfxPolyList[(int)gfxVertexSize].position.Y = (float) (yPos + (num10 * num2 - num9 * num1 >> 5));
                gfxPolyList[(int)gfxVertexSize].color.R = byte.MaxValue;
                gfxPolyList[(int)gfxVertexSize].color.G = byte.MaxValue;
                gfxPolyList[(int)gfxVertexSize].color.B = byte.MaxValue;
                gfxPolyList[(int)gfxVertexSize].color.A = byte.MaxValue;
                gfxPolyList[(int)gfxVertexSize].texCoord.X = gfxPolyList[(int)gfxVertexSize - 2].texCoord.X;
                gfxPolyList[(int)gfxVertexSize].texCoord.Y = gfxPolyList[(int)gfxVertexSize - 1].texCoord.Y;
        ++gfxVertexSize;
                gfxIndexSize += (ushort) 2;
      }
      else
      {
        int num3 = xPivot;
        int num4 = -yPivot;
                gfxPolyList[(int)gfxVertexSize].position.X = (float) (xPos + (num3 * num2 + num4 * num1 >> 5));
                gfxPolyList[(int)gfxVertexSize].position.Y = (float) (yPos + (num4 * num2 - num3 * num1 >> 5));
                gfxPolyList[(int)gfxVertexSize].color.R = byte.MaxValue;
                gfxPolyList[(int)gfxVertexSize].color.G = byte.MaxValue;
                gfxPolyList[(int)gfxVertexSize].color.B = byte.MaxValue;
                gfxPolyList[(int)gfxVertexSize].color.A = byte.MaxValue;
                gfxPolyList[(int)gfxVertexSize].texCoord.X = (float) (gfxSurface[surfaceNum].texStartX + xBegin) * 0.0009765625f;
                gfxPolyList[(int)gfxVertexSize].texCoord.Y = (float) (gfxSurface[surfaceNum].texStartY + yBegin) * 0.0009765625f;
        ++gfxVertexSize;
        int num5 = xPivot - xSize;
        int num6 = -yPivot;
                gfxPolyList[(int)gfxVertexSize].position.X = (float) (xPos + (num5 * num2 + num6 * num1 >> 5));
                gfxPolyList[(int)gfxVertexSize].position.Y = (float) (yPos + (num6 * num2 - num5 * num1 >> 5));
                gfxPolyList[(int)gfxVertexSize].color.R = byte.MaxValue;
                gfxPolyList[(int)gfxVertexSize].color.G = byte.MaxValue;
                gfxPolyList[(int)gfxVertexSize].color.B = byte.MaxValue;
                gfxPolyList[(int)gfxVertexSize].color.A = byte.MaxValue;
                gfxPolyList[(int)gfxVertexSize].texCoord.X = (float) (gfxSurface[surfaceNum].texStartX + xBegin + xSize) * 0.0009765625f;
                gfxPolyList[(int)gfxVertexSize].texCoord.Y = gfxPolyList[(int)gfxVertexSize - 1].texCoord.Y;
        ++gfxVertexSize;
        int num7 = xPivot;
        int num8 = ySize - yPivot;
                gfxPolyList[(int)gfxVertexSize].position.X = (float) (xPos + (num7 * num2 + num8 * num1 >> 5));
                gfxPolyList[(int)gfxVertexSize].position.Y = (float) (yPos + (num8 * num2 - num7 * num1 >> 5));
                gfxPolyList[(int)gfxVertexSize].color.R = byte.MaxValue;
                gfxPolyList[(int)gfxVertexSize].color.G = byte.MaxValue;
                gfxPolyList[(int)gfxVertexSize].color.B = byte.MaxValue;
                gfxPolyList[(int)gfxVertexSize].color.A = byte.MaxValue;
                gfxPolyList[(int)gfxVertexSize].texCoord.X = gfxPolyList[(int)gfxVertexSize - 2].texCoord.X;
                gfxPolyList[(int)gfxVertexSize].texCoord.Y = (float) (gfxSurface[surfaceNum].texStartY + yBegin + ySize) * 0.0009765625f;
        ++gfxVertexSize;
        int num9 = xPivot - xSize;
        int num10 = ySize - yPivot;
                gfxPolyList[(int)gfxVertexSize].position.X = (float) (xPos + (num9 * num2 + num10 * num1 >> 5));
                gfxPolyList[(int)gfxVertexSize].position.Y = (float) (yPos + (num10 * num2 - num9 * num1 >> 5));
                gfxPolyList[(int)gfxVertexSize].color.R = byte.MaxValue;
                gfxPolyList[(int)gfxVertexSize].color.G = byte.MaxValue;
                gfxPolyList[(int)gfxVertexSize].color.B = byte.MaxValue;
                gfxPolyList[(int)gfxVertexSize].color.A = byte.MaxValue;
                gfxPolyList[(int)gfxVertexSize].texCoord.X = gfxPolyList[(int)gfxVertexSize - 2].texCoord.X;
                gfxPolyList[(int)gfxVertexSize].texCoord.Y = gfxPolyList[(int)gfxVertexSize - 1].texCoord.Y;
        ++gfxVertexSize;
                gfxIndexSize += (ushort) 2;
      }
    }

    public static void DrawQuad(Quad2D face, int rgbVal)
    {
      if (gfxVertexSize >= (ushort) 8192)
        return;
            gfxPolyList[(int)gfxVertexSize].position.X = (float) (face.vertex[0].x << 4);
            gfxPolyList[(int)gfxVertexSize].position.Y = (float) (face.vertex[0].y << 4);
            gfxPolyList[(int)gfxVertexSize].color.R = (byte) (rgbVal >> 16 & (int) byte.MaxValue);
            gfxPolyList[(int)gfxVertexSize].color.G = (byte) (rgbVal >> 8 & (int) byte.MaxValue);
            gfxPolyList[(int)gfxVertexSize].color.B = (byte) (rgbVal & (int) byte.MaxValue);
      rgbVal = (rgbVal & 2130706432) >> 23;
            gfxPolyList[(int)gfxVertexSize].color.A = rgbVal <= 253 ? (byte) rgbVal : byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].texCoord.X = 0.01f;
            gfxPolyList[(int)gfxVertexSize].texCoord.Y = 0.01f;
      ++gfxVertexSize;
            gfxPolyList[(int)gfxVertexSize].position.X = (float) (face.vertex[1].x << 4);
            gfxPolyList[(int)gfxVertexSize].position.Y = (float) (face.vertex[1].y << 4);
            gfxPolyList[(int)gfxVertexSize].color.R = gfxPolyList[(int)gfxVertexSize - 1].color.R;
            gfxPolyList[(int)gfxVertexSize].color.G = gfxPolyList[(int)gfxVertexSize - 1].color.G;
            gfxPolyList[(int)gfxVertexSize].color.B = gfxPolyList[(int)gfxVertexSize - 1].color.B;
            gfxPolyList[(int)gfxVertexSize].color.A = gfxPolyList[(int)gfxVertexSize - 1].color.A;
            gfxPolyList[(int)gfxVertexSize].texCoord.X = 0.01f;
            gfxPolyList[(int)gfxVertexSize].texCoord.Y = 0.01f;
      ++gfxVertexSize;
            gfxPolyList[(int)gfxVertexSize].position.X = (float) (face.vertex[2].x << 4);
            gfxPolyList[(int)gfxVertexSize].position.Y = (float) (face.vertex[2].y << 4);
            gfxPolyList[(int)gfxVertexSize].color.R = gfxPolyList[(int)gfxVertexSize - 1].color.R;
            gfxPolyList[(int)gfxVertexSize].color.G = gfxPolyList[(int)gfxVertexSize - 1].color.G;
            gfxPolyList[(int)gfxVertexSize].color.B = gfxPolyList[(int)gfxVertexSize - 1].color.B;
            gfxPolyList[(int)gfxVertexSize].color.A = gfxPolyList[(int)gfxVertexSize - 1].color.A;
            gfxPolyList[(int)gfxVertexSize].texCoord.X = 0.01f;
            gfxPolyList[(int)gfxVertexSize].texCoord.Y = 0.01f;
      ++gfxVertexSize;
            gfxPolyList[(int)gfxVertexSize].position.X = (float) (face.vertex[3].x << 4);
            gfxPolyList[(int)gfxVertexSize].position.Y = (float) (face.vertex[3].y << 4);
            gfxPolyList[(int)gfxVertexSize].color.R = gfxPolyList[(int)gfxVertexSize - 1].color.R;
            gfxPolyList[(int)gfxVertexSize].color.G = gfxPolyList[(int)gfxVertexSize - 1].color.G;
            gfxPolyList[(int)gfxVertexSize].color.B = gfxPolyList[(int)gfxVertexSize - 1].color.B;
            gfxPolyList[(int)gfxVertexSize].color.A = gfxPolyList[(int)gfxVertexSize - 1].color.A;
            gfxPolyList[(int)gfxVertexSize].texCoord.X = 0.01f;
            gfxPolyList[(int)gfxVertexSize].texCoord.Y = 0.01f;
      ++gfxVertexSize;
            gfxIndexSize += (ushort) 2;
    }

    public static void DrawTexturedQuad(Quad2D face, int surfaceNum)
    {
      if (gfxVertexSize >= (ushort) 8192)
        return;
            gfxPolyList[(int)gfxVertexSize].position.X = (float) (face.vertex[0].x << 4);
            gfxPolyList[(int)gfxVertexSize].position.Y = (float) (face.vertex[0].y << 4);
            gfxPolyList[(int)gfxVertexSize].color.R = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].color.G = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].color.B = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].color.A = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].texCoord.X = (float) (gfxSurface[surfaceNum].texStartX + face.vertex[0].u) * 0.0009765625f;
            gfxPolyList[(int)gfxVertexSize].texCoord.Y = (float) (gfxSurface[surfaceNum].texStartY + face.vertex[0].v) * 0.0009765625f;
      ++gfxVertexSize;
            gfxPolyList[(int)gfxVertexSize].position.X = (float) (face.vertex[1].x << 4);
            gfxPolyList[(int)gfxVertexSize].position.Y = (float) (face.vertex[1].y << 4);
            gfxPolyList[(int)gfxVertexSize].color.R = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].color.G = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].color.B = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].color.A = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].texCoord.X = (float) (gfxSurface[surfaceNum].texStartX + face.vertex[1].u) * 0.0009765625f;
            gfxPolyList[(int)gfxVertexSize].texCoord.Y = (float) (gfxSurface[surfaceNum].texStartY + face.vertex[1].v) * 0.0009765625f;
      ++gfxVertexSize;
            gfxPolyList[(int)gfxVertexSize].position.X = (float) (face.vertex[2].x << 4);
            gfxPolyList[(int)gfxVertexSize].position.Y = (float) (face.vertex[2].y << 4);
            gfxPolyList[(int)gfxVertexSize].color.R = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].color.G = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].color.B = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].color.A = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].texCoord.X = (float) (gfxSurface[surfaceNum].texStartX + face.vertex[2].u) * 0.0009765625f;
            gfxPolyList[(int)gfxVertexSize].texCoord.Y = (float) (gfxSurface[surfaceNum].texStartY + face.vertex[2].v) * 0.0009765625f;
      ++gfxVertexSize;
            gfxPolyList[(int)gfxVertexSize].position.X = (float) (face.vertex[3].x << 4);
            gfxPolyList[(int)gfxVertexSize].position.Y = (float) (face.vertex[3].y << 4);
            gfxPolyList[(int)gfxVertexSize].color.R = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].color.G = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].color.B = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].color.A = byte.MaxValue;
            gfxPolyList[(int)gfxVertexSize].texCoord.X = (float) (gfxSurface[surfaceNum].texStartX + face.vertex[3].u) * 0.0009765625f;
            gfxPolyList[(int)gfxVertexSize].texCoord.Y = (float) (gfxSurface[surfaceNum].texStartY + face.vertex[3].v) * 0.0009765625f;
      ++gfxVertexSize;
            gfxIndexSize += (ushort) 2;
    }

    public static void SetPaletteEntry(byte entryPos, byte cR, byte cG, byte cB)
    {
            tilePalette16_Data[texPaletteNum, (int) entryPos] = entryPos <= (byte) 0 ? RGB_16BIT5551(cR, cG, cB, (byte) 0) : RGB_16BIT5551(cR, cG, cB, (byte) 1);
            tilePalette[(int) entryPos].red = cR;
            tilePalette[(int) entryPos].green = cG;
            tilePalette[(int) entryPos].blue = cB;
    }

    public static void SetFade(byte clrR, byte clrG, byte clrB, ushort clrA)
    {
            fadeMode = (byte) 1;
      if (clrA > (ushort) byte.MaxValue)
        clrA = (ushort) byte.MaxValue;
            fadeR = clrR;
            fadeG = clrG;
            fadeB = clrB;
            fadeA = (byte) clrA;
    }

    public static void SetLimitedFade(
      byte paletteNum,
      byte clrR,
      byte clrG,
      byte clrB,
      ushort clrA,
      int fStart,
      int fEnd)
    {
      byte[] numArray = new byte[3];
            paletteMode = paletteNum;
      if (paletteNum >= (byte) 8)
        return;
      if (clrA > (ushort) byte.MaxValue)
        clrA = (ushort) byte.MaxValue;
      if (fEnd < 256)
        ++fEnd;
      for (int index = fStart; index < fEnd; ++index)
      {
        numArray[0] = (byte) ((int)tilePalette[index].red * ((int) byte.MaxValue - (int) clrA) + (int) clrA * (int) clrR >> 8);
        numArray[1] = (byte) ((int)tilePalette[index].green * ((int) byte.MaxValue - (int) clrA) + (int) clrA * (int) clrG >> 8);
        numArray[2] = (byte) ((int)tilePalette[index].blue * ((int) byte.MaxValue - (int) clrA) + (int) clrA * (int) clrB >> 8);
                tilePalette16_Data[0, index] = RGB_16BIT5551(numArray[0], numArray[1], numArray[2], (byte) 1);
      }
            tilePalette16_Data[0, 0] = RGB_16BIT5551(numArray[0], numArray[1], numArray[2], (byte) 0);
    }

    public static void SetActivePalette(byte paletteNum, int minY, int maxY)
    {
      if (paletteNum >= (byte) 8)
        return;
            texPaletteNum = (int) paletteNum;
    }

    public static void CopyPalette(byte paletteSource, byte paletteDest)
    {
      if (paletteSource >= (byte) 8 || paletteDest >= (byte) 8)
        return;
      for (int index = 0; index < 256; ++index)
                tilePalette16_Data[(int) paletteDest, index] = tilePalette16_Data[(int) paletteSource, index];
    }

    public static void RotatePalette(byte pStart, byte pEnd, byte pDirection)
    {
      switch (pDirection)
      {
        case 0:
          ushort num1 = tilePalette16_Data[texPaletteNum, (int) pStart];
          for (byte index = pStart; (int) index < (int) pEnd; ++index)
                        tilePalette16_Data[texPaletteNum, (int) index] = tilePalette16_Data[texPaletteNum, (int) index + 1];
                    tilePalette16_Data[texPaletteNum, (int) pEnd] = num1;
          break;
        case 1:
          ushort num2 = tilePalette16_Data[texPaletteNum, (int) pEnd];
          for (byte index = pEnd; (int) index > (int) pStart; --index)
                        tilePalette16_Data[texPaletteNum, (int) index] = tilePalette16_Data[texPaletteNum, (int) index - 1];
                    tilePalette16_Data[texPaletteNum, (int) pStart] = num2;
          break;
      }
    }

    public static void GenerateBlendLookupTable()
    {
      int index1 = 0;
      for (int index2 = 0; index2 < 256; ++index2)
      {
        for (int index3 = 0; index3 < 32; ++index3)
        {
                    blendLookupTable[index1] = (ushort) (index3 * index2 >> 8);
                    subtractiveLookupTable[index1] = (ushort) ((31 - index3) * index2 >> 8);
          ++index1;
        }
      }
    }
  }
}
