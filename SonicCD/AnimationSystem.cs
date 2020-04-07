// Decompiled with JetBrains decompiler
// Type: Retro_Engine.AnimationSystem
// Assembly: Sonic CD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D35AF46A-1892-4F52-B201-E664C9200079
// Assembly location: E:\wamwo\Downloads\Sonic CD Mod Via Rebel\Sonic CD Mod Via Rebel\Sonic CD.dll

namespace RetroEngine
{
  public static class AnimationSystem
  {
    public static SpriteFrame[] animationFrames = new SpriteFrame[4096];
    public static int animationFramesNo = 0;
    public static SpriteAnimation[] animationList = new SpriteAnimation[1024];
    public static int animationListNo = 0;
    public static AnimationFileList[] animationFile = new AnimationFileList[256];
    public static int animationFileNo = 0;
    public static CollisionBox[] collisionBoxList = new CollisionBox[32];
    public static int collisionBoxNo = 0;

    static AnimationSystem()
    {
      for (int index = 0; index < AnimationSystem.animationFrames.Length; ++index)
        AnimationSystem.animationFrames[index] = new SpriteFrame();
      for (int index = 0; index < AnimationSystem.animationList.Length; ++index)
        AnimationSystem.animationList[index] = new SpriteAnimation();
      for (int index = 0; index < AnimationSystem.animationFile.Length; ++index)
        AnimationSystem.animationFile[index] = new AnimationFileList();
      for (int index = 0; index < AnimationSystem.collisionBoxList.Length; ++index)
        AnimationSystem.collisionBoxList[index] = new CollisionBox();
    }

    public static void LoadAnimationFile(char[] filePath)
    {
      char[] fileName = new char[32];
      byte[] numArray = new byte[24];
      FileData fData = new FileData();
      if (!FileIO.LoadFile(filePath, fData))
        return;
      byte num1 = FileIO.ReadByte();
      for (int index = 0; index < 24; ++index)
        numArray[index] = (byte) 0;
      for (int index1 = 0; index1 < (int) num1; ++index1)
      {
        byte num2 = FileIO.ReadByte();
        int index2 = 0;
        if (num2 > (byte) 0)
        {
          for (; (int) num2 > index2; ++index2)
            fileName[index2] = (char) FileIO.ReadByte();
          fileName[index2] = char.MinValue;
          FileIO.GetFileInfo(fData);
          FileIO.CloseFile();
          numArray[index1] = GraphicsSystem.AddGraphicsFile(fileName);
          FileIO.SetFileInfo(fData);
        }
      }
      byte num3 = FileIO.ReadByte();
      AnimationSystem.animationFile[AnimationSystem.animationFileNo].numAnimations = (int) num3;
      AnimationSystem.animationFile[AnimationSystem.animationFileNo].aniListOffset = AnimationSystem.animationListNo;
      for (int index1 = 0; index1 < AnimationSystem.animationFile[AnimationSystem.animationFileNo].numAnimations; ++index1)
      {
        byte num2 = FileIO.ReadByte();
        int index2;
        for (index2 = 0; index2 < (int) num2; ++index2)
          AnimationSystem.animationList[AnimationSystem.animationListNo].name[index2] = (char) FileIO.ReadByte();
        AnimationSystem.animationList[AnimationSystem.animationListNo].name[index2] = char.MinValue;
        byte num4 = FileIO.ReadByte();
        AnimationSystem.animationList[AnimationSystem.animationListNo].numFrames = num4;
        byte num5 = FileIO.ReadByte();
        AnimationSystem.animationList[AnimationSystem.animationListNo].animationSpeed = num5;
        byte num6 = FileIO.ReadByte();
        AnimationSystem.animationList[AnimationSystem.animationListNo].loopPosition = num6;
        byte num7 = FileIO.ReadByte();
        AnimationSystem.animationList[AnimationSystem.animationListNo].rotationFlag = num7;
        AnimationSystem.animationList[AnimationSystem.animationListNo].frameListOffset = AnimationSystem.animationFramesNo;
        for (int index3 = 0; index3 < (int) AnimationSystem.animationList[AnimationSystem.animationListNo].numFrames; ++index3)
        {
          byte num8 = FileIO.ReadByte();
          AnimationSystem.animationFrames[AnimationSystem.animationFramesNo].surfaceNum = numArray[(int) num8];
          byte num9 = FileIO.ReadByte();
          AnimationSystem.animationFrames[AnimationSystem.animationFramesNo].collisionBox = num9;
          byte num10 = FileIO.ReadByte();
          AnimationSystem.animationFrames[AnimationSystem.animationFramesNo].left = (int) num10;
          byte num11 = FileIO.ReadByte();
          AnimationSystem.animationFrames[AnimationSystem.animationFramesNo].top = (int) num11;
          byte num12 = FileIO.ReadByte();
          AnimationSystem.animationFrames[AnimationSystem.animationFramesNo].xSize = (int) num12;
          byte num13 = FileIO.ReadByte();
          AnimationSystem.animationFrames[AnimationSystem.animationFramesNo].ySize = (int) num13;
          sbyte num14 = (sbyte) FileIO.ReadByte();
          AnimationSystem.animationFrames[AnimationSystem.animationFramesNo].xPivot = (int) num14;
          sbyte num15 = (sbyte) FileIO.ReadByte();
          AnimationSystem.animationFrames[AnimationSystem.animationFramesNo].yPivot = (int) num15;
          ++AnimationSystem.animationFramesNo;
        }
        if (AnimationSystem.animationList[AnimationSystem.animationListNo].rotationFlag == (byte) 3)
          AnimationSystem.animationList[AnimationSystem.animationListNo].numFrames >>= 1;
        ++AnimationSystem.animationListNo;
      }
      byte num16 = FileIO.ReadByte();
      AnimationSystem.animationFile[AnimationSystem.animationFileNo].cbListOffset = AnimationSystem.collisionBoxNo;
      for (int index1 = 0; index1 < (int) num16; ++index1)
      {
        for (int index2 = 0; index2 < 8; ++index2)
        {
          AnimationSystem.collisionBoxList[AnimationSystem.collisionBoxNo].left[index2] = (sbyte) FileIO.ReadByte();
          AnimationSystem.collisionBoxList[AnimationSystem.collisionBoxNo].top[index2] = (sbyte) FileIO.ReadByte();
          AnimationSystem.collisionBoxList[AnimationSystem.collisionBoxNo].right[index2] = (sbyte) FileIO.ReadByte();
          AnimationSystem.collisionBoxList[AnimationSystem.collisionBoxNo].bottom[index2] = (sbyte) FileIO.ReadByte();
        }
        ++AnimationSystem.collisionBoxNo;
      }
      FileIO.CloseFile();
    }

    public static AnimationFileList AddAnimationFile(char[] fileName)
    {
      int index = 0;
      char[] charArray = "Data/Animations/".ToCharArray();
      char[] strA = new char[64];
      FileIO.StrCopy(ref strA, ref charArray);
      FileIO.StrAdd(ref strA, ref fileName);
      for (; index < 256; ++index)
      {
        if (FileIO.StringLength(ref AnimationSystem.animationFile[index].fileName) > 0)
        {
          if (FileIO.StringComp(ref AnimationSystem.animationFile[index].fileName, ref fileName))
            return AnimationSystem.animationFile[index];
        }
        else
        {
          FileIO.StrCopy(ref AnimationSystem.animationFile[index].fileName, ref fileName);
          AnimationSystem.LoadAnimationFile(strA);
          ++AnimationSystem.animationFileNo;
          return AnimationSystem.animationFile[index];
        }
      }
      return (AnimationFileList) null;
    }

    public static AnimationFileList GetDefaultAnimationRef()
    {
      return AnimationSystem.animationFile[0];
    }

    public static void ClearAnimationData()
    {
      char[] charArray = "".ToCharArray();
      for (int index = 0; index < 4096; ++index)
      {
        AnimationSystem.animationFrames[index].left = 0;
        AnimationSystem.animationFrames[index].top = 0;
        AnimationSystem.animationFrames[index].xSize = 0;
        AnimationSystem.animationFrames[index].ySize = 0;
        AnimationSystem.animationFrames[index].xPivot = 0;
        AnimationSystem.animationFrames[index].yPivot = 0;
        AnimationSystem.animationFrames[index].surfaceNum = (byte) 0;
        AnimationSystem.animationFrames[index].collisionBox = (byte) 0;
      }
      for (int index1 = 0; index1 < 32; ++index1)
      {
        for (int index2 = 0; index2 < 8; ++index2)
        {
          AnimationSystem.collisionBoxList[index1].left[index2] = (sbyte) 0;
          AnimationSystem.collisionBoxList[index1].top[index2] = (sbyte) 0;
          AnimationSystem.collisionBoxList[index1].right[index2] = (sbyte) 0;
          AnimationSystem.collisionBoxList[index1].bottom[index2] = (sbyte) 0;
        }
      }
      for (int index = 0; index < 256; ++index)
        FileIO.StrCopy(ref AnimationSystem.animationFile[index].fileName, ref charArray);
      AnimationSystem.animationFramesNo = 0;
      AnimationSystem.animationListNo = 0;
      AnimationSystem.animationFileNo = 0;
      AnimationSystem.collisionBoxNo = 0;
      AnimationSystem.animationFile[0].numAnimations = 0;
      AnimationSystem.animationList[0].frameListOffset = AnimationSystem.animationFramesNo;
      AnimationSystem.animationFile[0].aniListOffset = AnimationSystem.animationListNo;
      AnimationSystem.animationFile[0].cbListOffset = AnimationSystem.collisionBoxNo;
    }

    public static void ProcessObjectAnimation(
      SpriteAnimation animationRef,
      ObjectEntity currentObject)
    {
      if (currentObject.animationSpeed > 0)
      {
        if (currentObject.animationSpeed > 240)
          currentObject.animationSpeed = 240;
        currentObject.animationTimer += currentObject.animationSpeed;
      }
      else
        currentObject.animationTimer += (int) animationRef.animationSpeed;
      if ((int) currentObject.animation != (int) currentObject.prevAnimation)
      {
        currentObject.prevAnimation = currentObject.animation;
        currentObject.frame = (byte) 0;
        currentObject.animationTimer = 0;
        currentObject.animationSpeed = 0;
      }
      if (currentObject.animationTimer > 239)
      {
        currentObject.animationTimer -= 240;
        ++currentObject.frame;
      }
      if ((int) currentObject.frame < (int) animationRef.numFrames)
        return;
      currentObject.frame = animationRef.loopPosition;
    }

    public static void DrawObjectAnimation(
      SpriteAnimation animationRef,
      ObjectEntity currentObject,
      int xPos,
      int yPos)
    {
      switch (animationRef.rotationFlag)
      {
        case 0:
          SpriteFrame animationFrame1 = AnimationSystem.animationFrames[animationRef.frameListOffset + (int) currentObject.frame];
          switch (currentObject.direction)
          {
            case 0:
              GraphicsSystem.DrawSpriteFlipped(xPos + animationFrame1.xPivot, yPos + animationFrame1.yPivot, animationFrame1.xSize, animationFrame1.ySize, animationFrame1.left, animationFrame1.top, (int) currentObject.direction, (int) animationFrame1.surfaceNum);
              return;
            case 1:
              GraphicsSystem.DrawSpriteFlipped(xPos - animationFrame1.xSize - animationFrame1.xPivot, yPos + animationFrame1.yPivot, animationFrame1.xSize, animationFrame1.ySize, animationFrame1.left, animationFrame1.top, (int) currentObject.direction, (int) animationFrame1.surfaceNum);
              return;
            case 2:
              GraphicsSystem.DrawSpriteFlipped(xPos + animationFrame1.xPivot, yPos - animationFrame1.ySize - animationFrame1.yPivot, animationFrame1.xSize, animationFrame1.ySize, animationFrame1.left, animationFrame1.top, (int) currentObject.direction, (int) animationFrame1.surfaceNum);
              return;
            case 3:
              GraphicsSystem.DrawSpriteFlipped(xPos - animationFrame1.xSize - animationFrame1.xPivot, yPos - animationFrame1.ySize - animationFrame1.yPivot, animationFrame1.xSize, animationFrame1.ySize, animationFrame1.left, animationFrame1.top, (int) currentObject.direction, (int) animationFrame1.surfaceNum);
              return;
            default:
              return;
          }
        case 1:
          SpriteFrame animationFrame2 = AnimationSystem.animationFrames[animationRef.frameListOffset + (int) currentObject.frame];
          GraphicsSystem.DrawRotatedSprite(currentObject.direction, xPos, yPos, -animationFrame2.xPivot, -animationFrame2.yPivot, animationFrame2.left, animationFrame2.top, animationFrame2.xSize, animationFrame2.ySize, currentObject.rotation, (int) animationFrame2.surfaceNum);
          break;
        case 2:
          SpriteFrame animationFrame3 = AnimationSystem.animationFrames[animationRef.frameListOffset + (int) currentObject.frame];
          int rotAngle1 = currentObject.rotation >= 256 ? 512 - (532 - currentObject.rotation >> 6 << 6) : currentObject.rotation + 20 >> 6 << 6;
          GraphicsSystem.DrawRotatedSprite(currentObject.direction, xPos, yPos, -animationFrame3.xPivot, -animationFrame3.yPivot, animationFrame3.left, animationFrame3.top, animationFrame3.xSize, animationFrame3.ySize, rotAngle1, (int) animationFrame3.surfaceNum);
          break;
        case 3:
          int rotAngle2 = currentObject.rotation >= 256 ? 8 - (532 - currentObject.rotation >> 6) : currentObject.rotation + 20 >> 6;
          int frame = (int) currentObject.frame;
          switch (rotAngle2)
          {
            case 0:
            case 8:
              rotAngle2 = 0;
              break;
            case 1:
              frame += (int) animationRef.numFrames;
              rotAngle2 = currentObject.direction != (byte) 0 ? 0 : 128;
              break;
            case 2:
              rotAngle2 = 128;
              break;
            case 3:
              frame += (int) animationRef.numFrames;
              rotAngle2 = currentObject.direction != (byte) 0 ? 128 : 256;
              break;
            case 4:
              rotAngle2 = 256;
              break;
            case 5:
              frame += (int) animationRef.numFrames;
              rotAngle2 = currentObject.direction != (byte) 0 ? 256 : 384;
              break;
            case 6:
              rotAngle2 = 384;
              break;
            case 7:
              frame += (int) animationRef.numFrames;
              rotAngle2 = currentObject.direction != (byte) 0 ? 384 : 0;
              break;
          }
          SpriteFrame animationFrame4 = AnimationSystem.animationFrames[animationRef.frameListOffset + frame];
          GraphicsSystem.DrawRotatedSprite(currentObject.direction, xPos, yPos, -animationFrame4.xPivot, -animationFrame4.yPivot, animationFrame4.left, animationFrame4.top, animationFrame4.xSize, animationFrame4.ySize, rotAngle2, (int) animationFrame4.surfaceNum);
          break;
      }
    }
  }
}
