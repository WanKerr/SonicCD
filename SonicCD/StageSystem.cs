// Decompiled with JetBrains decompiler
// Type: Retro_Engine.StageSystem
// Assembly: Sonic CD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D35AF46A-1892-4F52-B201-E664C9200079
// Assembly location: E:\wamwo\Downloads\Sonic CD Mod Via Rebel\Sonic CD Mod Via Rebel\Sonic CD.dll

namespace RetroEngine
{
  public static class StageSystem
  {
    public static InputResult gKeyDown = new InputResult();
    public static InputResult gKeyPress = new InputResult();
    public static Mappings128x128 tile128x128 = new Mappings128x128();
    public static LayoutMap[] stageLayouts = new LayoutMap[9];
    public static byte[] activeTileLayers = new byte[4];
    public static CollisionMask16x16[] tileCollisions = new CollisionMask16x16[2];
    public static LineScrollParallax hParallax = new LineScrollParallax();
    public static LineScrollParallax vParallax = new LineScrollParallax();
    public static int[] bgDeformationData0 = new int[576];
    public static int[] bgDeformationData1 = new int[576];
    public static int[] bgDeformationData2 = new int[576];
    public static int[] bgDeformationData3 = new int[576];
    public static int xBoundary1 = 0;
    public static int yBoundary1 = 0;
    public static int newXBoundary1 = 0;
    public static int newYBoundary1 = 0;
    public static byte cameraShift = 0;
    public static byte cameraStyle = 0;
    public static int xScrollOffset = 0;
    public static int yScrollOffset = 0;
    public static int yScrollA = 0;
    public static int yScrollB = 240;
    public static int xScrollA = 0;
    public static int xScrollB = 320;
    public static int xScrollMove = 0;
    public static int yScrollMove = 0;
    public static int screenShakeX = 0;
    public static int screenShakeY = 0;
    public static char[] titleCardText = new char[24];
    public static TextMenu[] gameMenu = new TextMenu[2];
    public static byte debugMode = 0;
    public const int ACTLAYOUT = 0;
    public const int LOADSTAGE = 0;
    public const int PLAYSTAGE = 1;
    public const int STAGEPAUSED = 2;
    public static byte stageMode;
    public static byte pauseEnabled;
    public static int stageListPosition;
    public static byte tLayerMidPoint;
    public static int lastXSize;
    public static int lastYSize;
    public static int xBoundary2;
    public static int yBoundary2;
    public static int newXBoundary2;
    public static int newYBoundary2;
    public static byte cameraEnabled;
    public static sbyte cameraTarget;
    public static int cameraAdjustY;
    public static int waterLevel;
    public static char titleCardWord2;
    public static byte timeEnabled;
    public static byte milliSeconds;
    public static byte seconds;
    public static byte minutes;

    static StageSystem()
    {
      for (int index = 0; index < StageSystem.stageLayouts.Length; ++index)
        StageSystem.stageLayouts[index] = new LayoutMap();
      for (int index = 0; index < StageSystem.tileCollisions.Length; ++index)
        StageSystem.tileCollisions[index] = new CollisionMask16x16();
      for (int index = 0; index < StageSystem.gameMenu.Length; ++index)
        StageSystem.gameMenu[index] = new TextMenu();
    }

    public static void ProcessStage()
    {
      switch (StageSystem.stageMode)
      {
        case 0:
          AudioPlayback.StopMusic();
          GraphicsSystem.fadeMode = (byte) 0;
          GraphicsSystem.paletteMode = (byte) 0;
          GraphicsSystem.SetActivePalette((byte) 0, 0, 256);
          StageSystem.cameraEnabled = (byte) 1;
          StageSystem.cameraTarget = (sbyte) -1;
          StageSystem.cameraAdjustY = 0;
          StageSystem.xScrollOffset = 0;
          StageSystem.yScrollOffset = 0;
          StageSystem.yScrollA = 0;
          StageSystem.yScrollB = 240;
          StageSystem.xScrollA = 0;
          StageSystem.xScrollB = 320;
          StageSystem.xScrollMove = 0;
          StageSystem.yScrollMove = 0;
          StageSystem.screenShakeX = 0;
          StageSystem.screenShakeY = 0;
          Scene3D.numVertices = 0;
          Scene3D.numFaces = 0;
          for (int index = 0; index < 2; ++index)
          {
            PlayerSystem.playerList[index].xPos = 0;
            PlayerSystem.playerList[index].yPos = 0;
            PlayerSystem.playerList[index].xVelocity = 0;
            PlayerSystem.playerList[index].yVelocity = 0;
            PlayerSystem.playerList[index].angle = 0;
            PlayerSystem.playerList[index].visible = (byte) 1;
            PlayerSystem.playerList[index].collisionPlane = (byte) 0;
            PlayerSystem.playerList[index].collisionMode = (byte) 0;
            PlayerSystem.playerList[index].gravity = (byte) 1;
            PlayerSystem.playerList[index].speed = 0;
            PlayerSystem.playerList[index].tileCollisions = (byte) 1;
            PlayerSystem.playerList[index].objectInteraction = (byte) 1;
            PlayerSystem.playerList[index].value[0] = 0;
            PlayerSystem.playerList[index].value[1] = 0;
            PlayerSystem.playerList[index].value[2] = 0;
            PlayerSystem.playerList[index].value[3] = 0;
            PlayerSystem.playerList[index].value[4] = 0;
            PlayerSystem.playerList[index].value[5] = 0;
            PlayerSystem.playerList[index].value[6] = 0;
            PlayerSystem.playerList[index].value[7] = 0;
          }
          StageSystem.pauseEnabled = (byte) 0;
          StageSystem.timeEnabled = (byte) 0;
          StageSystem.milliSeconds = (byte) 0;
          StageSystem.seconds = (byte) 0;
          StageSystem.minutes = (byte) 0;
          GlobalAppDefinitions.frameCounter = (byte) 0;
          StageSystem.ResetBackgroundSettings();
          StageSystem.LoadStageFiles();
          GraphicsSystem.texBufferMode = (byte) 0;
          for (int index = 0; index < 9; ++index)
          {
            if (StageSystem.stageLayouts[index].type == (byte) 4)
              GraphicsSystem.texBufferMode = (byte) 1;
          }
          for (int index = 0; index < (int) StageSystem.hParallax.numEntries; ++index)
          {
            if (StageSystem.hParallax.deformationEnabled[index] == (byte) 1)
              GraphicsSystem.texBufferMode = (byte) 1;
          }
          if (GraphicsSystem.tileGfx[204802] > (byte) 0)
            GraphicsSystem.texBufferMode = (byte) 0;
          if (GraphicsSystem.texBufferMode == (byte) 0)
          {
            for (int index = 0; index < 4096; index += 4)
            {
              GraphicsSystem.tileUVArray[index] = (float) ((index >> 2 & 31) * 16) * 0.0009765625f;
              GraphicsSystem.tileUVArray[index + 1] = (float) ((index >> 2 >> 5) * 16) * 0.0009765625f;
              GraphicsSystem.tileUVArray[index + 2] = GraphicsSystem.tileUVArray[index] + 1f / 64f;
              GraphicsSystem.tileUVArray[index + 3] = GraphicsSystem.tileUVArray[index + 1] + 1f / 64f;
            }
          }
          else
          {
            for (int index = 0; index < 4096; index += 4)
            {
              GraphicsSystem.tileUVArray[index] = (float) ((index >> 2) % 28 * 18 + 1) * 0.0009765625f;
              GraphicsSystem.tileUVArray[index + 1] = (float) ((index >> 2) / 28 * 18 + 1) * 0.0009765625f;
              GraphicsSystem.tileUVArray[index + 2] = GraphicsSystem.tileUVArray[index] + 1f / 64f;
              GraphicsSystem.tileUVArray[index + 3] = GraphicsSystem.tileUVArray[index + 1] + 1f / 64f;
            }
            GraphicsSystem.tileUVArray[4092] = 0.4755859f;
            GraphicsSystem.tileUVArray[4093] = 0.4755859f;
            GraphicsSystem.tileUVArray[4094] = 0.4912109f;
            GraphicsSystem.tileUVArray[4095] = 0.4912109f;
          }
          RenderDevice.UpdateHardwareTextures();
          StageSystem.stageMode = (byte) 1;
          GraphicsSystem.gfxIndexSize = (ushort) 0;
          GraphicsSystem.gfxVertexSize = (ushort) 0;
          GraphicsSystem.gfxIndexSizeOpaque = (ushort) 0;
          GraphicsSystem.gfxVertexSizeOpaque = (ushort) 0;
          StageSystem.stageMode = (byte) 1;
          break;
        case 1:
          if (GraphicsSystem.fadeMode > (byte) 0)
            --GraphicsSystem.fadeMode;
          if (GraphicsSystem.paletteMode > (byte) 0)
          {
            GraphicsSystem.paletteMode = (byte) 0;
            GraphicsSystem.texPaletteNum = 0;
          }
          StageSystem.lastXSize = -1;
          StageSystem.lastYSize = -1;
          InputSystem.CheckKeyDown(StageSystem.gKeyDown, byte.MaxValue);
          InputSystem.CheckKeyPress(StageSystem.gKeyPress, byte.MaxValue);
          if (StageSystem.pauseEnabled == (byte) 1 && StageSystem.gKeyPress.start == (byte) 1)
          {
            StageSystem.stageMode = (byte) 2;
            AudioPlayback.PauseSound();
          }
          if (StageSystem.timeEnabled == (byte) 1)
          {
            ++GlobalAppDefinitions.frameCounter;
            if (GlobalAppDefinitions.frameCounter == (byte) 60)
            {
              GlobalAppDefinitions.frameCounter = (byte) 0;
              ++StageSystem.seconds;
              if (StageSystem.seconds > (byte) 59)
              {
                StageSystem.seconds = (byte) 0;
                ++StageSystem.minutes;
                if (StageSystem.minutes > (byte) 59)
                  StageSystem.minutes = (byte) 0;
              }
            }
            StageSystem.milliSeconds = (byte) ((int) GlobalAppDefinitions.frameCounter * 100 / 60);
          }
          ObjectSystem.ProcessObjects();
          if (StageSystem.cameraTarget > (sbyte) -1)
          {
            if (StageSystem.cameraEnabled == (byte) 1)
            {
              switch (StageSystem.cameraStyle)
              {
                case 0:
                  PlayerSystem.SetPlayerScreenPosition(PlayerSystem.playerList[(int) StageSystem.cameraTarget]);
                  break;
                case 1:
                  PlayerSystem.SetPlayerScreenPositionCDStyle(PlayerSystem.playerList[(int) StageSystem.cameraTarget]);
                  break;
                case 2:
                  PlayerSystem.SetPlayerScreenPositionCDStyle(PlayerSystem.playerList[(int) StageSystem.cameraTarget]);
                  break;
                case 3:
                  PlayerSystem.SetPlayerScreenPositionCDStyle(PlayerSystem.playerList[(int) StageSystem.cameraTarget]);
                  break;
                case 4:
                  PlayerSystem.SetPlayerHLockedScreenPosition(PlayerSystem.playerList[(int) StageSystem.cameraTarget]);
                  break;
              }
            }
            else
              PlayerSystem.SetPlayerLockedScreenPosition(PlayerSystem.playerList[(int) StageSystem.cameraTarget]);
          }
          StageSystem.DrawStageGfx();
          if (GraphicsSystem.fadeMode > (byte) 0)
            GraphicsSystem.DrawRectangle(0, 0, GlobalAppDefinitions.SCREEN_XSIZE, 240, (int) GraphicsSystem.fadeR, (int) GraphicsSystem.fadeG, (int) GraphicsSystem.fadeB, (int) GraphicsSystem.fadeA);
          if (StageSystem.stageMode != (byte) 2)
            break;
          GlobalAppDefinitions.gameMode = (byte) 8;
          break;
        case 2:
          if (GraphicsSystem.fadeMode > (byte) 0)
            --GraphicsSystem.fadeMode;
          if (GraphicsSystem.paletteMode > (byte) 0)
          {
            GraphicsSystem.paletteMode = (byte) 0;
            GraphicsSystem.texPaletteNum = 0;
          }
          StageSystem.lastXSize = -1;
          StageSystem.lastYSize = -1;
          InputSystem.CheckKeyDown(StageSystem.gKeyDown, byte.MaxValue);
          InputSystem.CheckKeyPress(StageSystem.gKeyPress, byte.MaxValue);
          GraphicsSystem.gfxIndexSize = (ushort) 0;
          GraphicsSystem.gfxVertexSize = (ushort) 0;
          GraphicsSystem.gfxIndexSizeOpaque = (ushort) 0;
          GraphicsSystem.gfxVertexSizeOpaque = (ushort) 0;
          ObjectSystem.ProcessPausedObjects();
          ObjectSystem.DrawObjectList(0);
          ObjectSystem.DrawObjectList(1);
          ObjectSystem.DrawObjectList(2);
          ObjectSystem.DrawObjectList(3);
          ObjectSystem.DrawObjectList(4);
          ObjectSystem.DrawObjectList(5);
          ObjectSystem.DrawObjectList(6);
          if (StageSystem.pauseEnabled != (byte) 1 || StageSystem.gKeyPress.start != (byte) 1)
            break;
          StageSystem.stageMode = (byte) 1;
          AudioPlayback.ResumeSound();
          break;
      }
    }

    public static void LoadStageFiles()
    {
      FileData fData = new FileData();
      byte[] byteP = new byte[3];
      char[] charP = new char[64];
      int scriptNum = 1;
      AudioPlayback.StopAllSFX();
      if (!FileIO.CheckCurrentStageFolder(StageSystem.stageListPosition))
      {
        AudioPlayback.ReleaseStageSFX();
        GraphicsSystem.LoadPalette("MasterPalette.act".ToCharArray(), 0, 0, 0, 256);
        ObjectSystem.ClearScriptData();
        for (int index = 16; index > 0; --index)
          GraphicsSystem.RemoveGraphicsFile("".ToCharArray(), index - 1);
        if (FileIO.LoadStageFile("StageConfig.bin".ToCharArray(), StageSystem.stageListPosition, fData))
        {
          byteP[0] = FileIO.ReadByte();
          FileIO.CloseFile();
        }
        if (byteP[0] == (byte) 1 && FileIO.LoadFile("Data/Game/GameConfig.bin".ToCharArray(), fData))
        {
          byteP[0] = FileIO.ReadByte();
          for (int index = 0; index < (int) byteP[0]; ++index)
            byteP[1] = FileIO.ReadByte();
          byteP[0] = FileIO.ReadByte();
          for (int index = 0; index < (int) byteP[0]; ++index)
            byteP[1] = FileIO.ReadByte();
          byteP[0] = FileIO.ReadByte();
          for (int index = 0; index < (int) byteP[0]; ++index)
            byteP[1] = FileIO.ReadByte();
          byteP[0] = FileIO.ReadByte();
          for (int index = 0; index < (int) byteP[0]; ++index)
          {
            byteP[1] = FileIO.ReadByte();
            FileIO.ReadCharArray(ref charP, (int) byteP[1]);
            charP[(int) byteP[1]] = char.MinValue;
            ObjectSystem.SetObjectTypeName(charP, scriptNum + index);
          }
          if (FileIO.useByteCode)
          {
            FileIO.GetFileInfo(fData);
            FileIO.CloseFile();
            ObjectSystem.LoadByteCodeFile(4, scriptNum);
            scriptNum += (int) byteP[0];
            FileIO.SetFileInfo(fData);
          }
          FileIO.CloseFile();
        }
        if (FileIO.LoadStageFile("StageConfig.bin".ToCharArray(), StageSystem.stageListPosition, fData))
        {
          byteP[0] = FileIO.ReadByte();
          for (int index = 96; index < 128; ++index)
          {
            FileIO.ReadByteArray(ref byteP, 3);
            GraphicsSystem.SetPaletteEntry((byte) index, byteP[0], byteP[1], byteP[2]);
          }
          byteP[0] = FileIO.ReadByte();
          for (int index = 0; index < (int) byteP[0]; ++index)
          {
            byteP[1] = FileIO.ReadByte();
            FileIO.ReadCharArray(ref charP, (int) byteP[1]);
            charP[(int) byteP[1]] = char.MinValue;
            ObjectSystem.SetObjectTypeName(charP, index + scriptNum);
          }
          if (FileIO.useByteCode)
          {
            for (int index = 0; index < (int) byteP[0]; ++index)
            {
              byteP[1] = FileIO.ReadByte();
              FileIO.ReadCharArray(ref charP, (int) byteP[1]);
              charP[(int) byteP[1]] = char.MinValue;
            }
            FileIO.GetFileInfo(fData);
            FileIO.CloseFile();
            ObjectSystem.LoadByteCodeFile((int) FileIO.activeStageList, scriptNum);
            FileIO.SetFileInfo(fData);
          }
          byteP[0] = FileIO.ReadByte();
          AudioPlayback.numStageSFX = (int) byteP[0];
          for (int index = 0; index < (int) byteP[0]; ++index)
          {
            byteP[1] = FileIO.ReadByte();
            FileIO.ReadCharArray(ref charP, (int) byteP[1]);
            charP[(int) byteP[1]] = char.MinValue;
            FileIO.GetFileInfo(fData);
            FileIO.CloseFile();
            AudioPlayback.LoadSfx(charP, index + AudioPlayback.numGlobalSFX);
            FileIO.SetFileInfo(fData);
          }
          FileIO.CloseFile();
        }
        GraphicsSystem.LoadStageGIFFile(StageSystem.stageListPosition);
        StageSystem.LoadStageCollisions();
        StageSystem.LoadStageBackground();
      }
      StageSystem.Load128x128Mappings();
      for (int trackNo = 0; trackNo < 16; ++trackNo)
        AudioPlayback.SetMusicTrack("".ToCharArray(), trackNo, (byte) 0, 0U);
      for (int index = 0; index < 1184; ++index)
      {
        ObjectSystem.objectEntityList[index].type = (byte) 0;
        ObjectSystem.objectEntityList[index].direction = (byte) 0;
        ObjectSystem.objectEntityList[index].animation = (byte) 0;
        ObjectSystem.objectEntityList[index].prevAnimation = (byte) 0;
        ObjectSystem.objectEntityList[index].animationSpeed = 0;
        ObjectSystem.objectEntityList[index].animationTimer = 0;
        ObjectSystem.objectEntityList[index].frame = (byte) 0;
        ObjectSystem.objectEntityList[index].priority = (byte) 0;
        ObjectSystem.objectEntityList[index].direction = (byte) 0;
        ObjectSystem.objectEntityList[index].rotation = 0;
        ObjectSystem.objectEntityList[index].state = (byte) 0;
        ObjectSystem.objectEntityList[index].propertyValue = (byte) 0;
        ObjectSystem.objectEntityList[index].xPos = 0;
        ObjectSystem.objectEntityList[index].yPos = 0;
        ObjectSystem.objectEntityList[index].drawOrder = (byte) 3;
        ObjectSystem.objectEntityList[index].scale = 512;
        ObjectSystem.objectEntityList[index].inkEffect = (byte) 0;
        ObjectSystem.objectEntityList[index].value[0] = 0;
        ObjectSystem.objectEntityList[index].value[1] = 0;
        ObjectSystem.objectEntityList[index].value[2] = 0;
        ObjectSystem.objectEntityList[index].value[3] = 0;
        ObjectSystem.objectEntityList[index].value[4] = 0;
        ObjectSystem.objectEntityList[index].value[5] = 0;
        ObjectSystem.objectEntityList[index].value[6] = 0;
        ObjectSystem.objectEntityList[index].value[7] = 0;
      }
      StageSystem.LoadActLayout();
      ObjectSystem.ProcessStartupScripts();
      StageSystem.xScrollA = (PlayerSystem.playerList[0].xPos >> 16) - 160;
      StageSystem.xScrollB = StageSystem.xScrollA + 320;
      StageSystem.yScrollA = (PlayerSystem.playerList[0].yPos >> 16) - 104;
      StageSystem.yScrollB = StageSystem.yScrollA + 240;
    }

    public static void Load128x128Mappings()
    {
      FileData fData = new FileData();
      int index = 0;
      byte[] byteP = new byte[2];
      if (!FileIO.LoadStageFile("128x128Tiles.bin".ToCharArray(), StageSystem.stageListPosition, fData))
        return;
      for (; index < 32768; ++index)
      {
        FileIO.ReadByteArray(ref byteP, 2);
        byteP[0] = (byte) ((uint) byteP[0] - (uint) ((int) byteP[0] >> 6 << 6));
        StageSystem.tile128x128.visualPlane[index] = (byte) ((uint) byteP[0] >> 4);
        byteP[0] = (byte) ((uint) byteP[0] - (uint) ((int) byteP[0] >> 4 << 4));
        StageSystem.tile128x128.direction[index] = (byte) ((uint) byteP[0] >> 2);
        byteP[0] = (byte) ((uint) byteP[0] - (uint) ((int) byteP[0] >> 2 << 2));
        StageSystem.tile128x128.tile16x16[index] = (ushort) (((uint) byteP[0] << 8) + (uint) byteP[1]);
        StageSystem.tile128x128.gfxDataPos[index] = (int) StageSystem.tile128x128.tile16x16[index] << 2;
        byteP[0] = FileIO.ReadByte();
        StageSystem.tile128x128.collisionFlag[0, index] = (byte) ((uint) byteP[0] >> 4);
        StageSystem.tile128x128.collisionFlag[1, index] = (byte) ((uint) byteP[0] - (uint) ((int) byteP[0] >> 4 << 4));
      }
      FileIO.CloseFile();
    }

    public static void LoadStageCollisions()
    {
      FileData fData = new FileData();
      int num1 = 0;
      if (!FileIO.LoadStageFile("CollisionMasks.bin".ToCharArray(), StageSystem.stageListPosition, fData))
        return;
      for (int index1 = 0; index1 < 1024; ++index1)
      {
        for (int index2 = 0; index2 < 2; ++index2)
        {
          byte num2 = FileIO.ReadByte();
          int num3 = (int) num2 >> 4;
          StageSystem.tileCollisions[index2].flags[index1] = (byte) ((uint) num2 & 15U);
          byte num4 = FileIO.ReadByte();
          StageSystem.tileCollisions[index2].angle[index1] = (uint) num4;
          byte num5 = FileIO.ReadByte();
          StageSystem.tileCollisions[index2].angle[index1] += (uint) num5 << 8;
          byte num6 = FileIO.ReadByte();
          StageSystem.tileCollisions[index2].angle[index1] += (uint) num6 << 16;
          byte num7 = FileIO.ReadByte();
          StageSystem.tileCollisions[index2].angle[index1] += (uint) num7 << 24;
          if (num3 == 0)
          {
            for (int index3 = 0; index3 < 16; index3 += 2)
            {
              byte num8 = FileIO.ReadByte();
              StageSystem.tileCollisions[index2].floorMask[num1 + index3] = (sbyte) ((int) num8 >> 4);
              StageSystem.tileCollisions[index2].floorMask[num1 + index3 + 1] = (sbyte) ((int) num8 & 15);
            }
            byte num9 = FileIO.ReadByte();
            byte num10 = 1;
            for (int index3 = 0; index3 < 8; ++index3)
            {
              if (((int) num9 & (int) num10) < 1)
              {
                StageSystem.tileCollisions[index2].floorMask[num1 + index3 + 8] = (sbyte) 64;
                StageSystem.tileCollisions[index2].roofMask[num1 + index3 + 8] = (sbyte) -64;
              }
              else
                StageSystem.tileCollisions[index2].roofMask[num1 + index3 + 8] = (sbyte) 15;
              num10 <<= 1;
            }
            byte num11 = FileIO.ReadByte();
            byte num12 = 1;
            for (int index3 = 0; index3 < 8; ++index3)
            {
              if (((int) num11 & (int) num12) < 1)
              {
                StageSystem.tileCollisions[index2].floorMask[num1 + index3] = (sbyte) 64;
                StageSystem.tileCollisions[index2].roofMask[num1 + index3] = (sbyte) -64;
              }
              else
                StageSystem.tileCollisions[index2].roofMask[num1 + index3] = (sbyte) 15;
              num12 <<= 1;
            }
            for (byte index3 = 0; index3 < (byte) 16; ++index3)
            {
              int num8 = 0;
              while (num8 > -1)
              {
                if (num8 == 16)
                {
                  StageSystem.tileCollisions[index2].leftWallMask[num1 + (int) index3] = (sbyte) 64;
                  num8 = -1;
                }
                else if ((int) index3 >= (int) StageSystem.tileCollisions[index2].floorMask[num1 + num8])
                {
                  StageSystem.tileCollisions[index2].leftWallMask[num1 + (int) index3] = (sbyte) num8;
                  num8 = -1;
                }
                else
                  ++num8;
              }
            }
            for (byte index3 = 0; index3 < (byte) 16; ++index3)
            {
              int num8 = 15;
              while (num8 < 16)
              {
                if (num8 == -1)
                {
                  StageSystem.tileCollisions[index2].rightWallMask[num1 + (int) index3] = (sbyte) -64;
                  num8 = 16;
                }
                else if ((int) index3 >= (int) StageSystem.tileCollisions[index2].floorMask[num1 + num8])
                {
                  StageSystem.tileCollisions[index2].rightWallMask[num1 + (int) index3] = (sbyte) num8;
                  num8 = 16;
                }
                else
                  --num8;
              }
            }
          }
          else
          {
            for (int index3 = 0; index3 < 16; index3 += 2)
            {
              byte num8 = FileIO.ReadByte();
              StageSystem.tileCollisions[index2].roofMask[num1 + index3] = (sbyte) ((int) num8 >> 4);
              StageSystem.tileCollisions[index2].roofMask[num1 + index3 + 1] = (sbyte) ((int) num8 & 15);
            }
            byte num9 = FileIO.ReadByte();
            byte num10 = 1;
            for (int index3 = 0; index3 < 8; ++index3)
            {
              if (((int) num9 & (int) num10) < 1)
              {
                StageSystem.tileCollisions[index2].floorMask[num1 + index3 + 8] = (sbyte) 64;
                StageSystem.tileCollisions[index2].roofMask[num1 + index3 + 8] = (sbyte) -64;
              }
              else
                StageSystem.tileCollisions[index2].floorMask[num1 + index3 + 8] = (sbyte) 0;
              num10 <<= 1;
            }
            byte num11 = FileIO.ReadByte();
            byte num12 = 1;
            for (int index3 = 0; index3 < 8; ++index3)
            {
              if (((int) num11 & (int) num12) < 1)
              {
                StageSystem.tileCollisions[index2].floorMask[num1 + index3] = (sbyte) 64;
                StageSystem.tileCollisions[index2].roofMask[num1 + index3] = (sbyte) -64;
              }
              else
                StageSystem.tileCollisions[index2].floorMask[num1 + index3] = (sbyte) 0;
              num12 <<= 1;
            }
            for (byte index3 = 0; index3 < (byte) 16; ++index3)
            {
              int num8 = 0;
              while (num8 > -1)
              {
                if (num8 == 16)
                {
                  StageSystem.tileCollisions[index2].leftWallMask[num1 + (int) index3] = (sbyte) 64;
                  num8 = -1;
                }
                else if ((int) index3 <= (int) StageSystem.tileCollisions[index2].roofMask[num1 + num8])
                {
                  StageSystem.tileCollisions[index2].leftWallMask[num1 + (int) index3] = (sbyte) num8;
                  num8 = -1;
                }
                else
                  ++num8;
              }
            }
            for (byte index3 = 0; index3 < (byte) 16; ++index3)
            {
              int num8 = 15;
              while (num8 < 16)
              {
                if (num8 == -1)
                {
                  StageSystem.tileCollisions[index2].rightWallMask[num1 + (int) index3] = (sbyte) -64;
                  num8 = 16;
                }
                else if ((int) index3 <= (int) StageSystem.tileCollisions[index2].roofMask[num1 + num8])
                {
                  StageSystem.tileCollisions[index2].rightWallMask[num1 + (int) index3] = (sbyte) num8;
                  num8 = 16;
                }
                else
                  --num8;
              }
            }
          }
        }
        num1 += 16;
      }
      FileIO.CloseFile();
    }

    public static void LoadActLayout()
    {
      FileData fData = new FileData();
      if (!FileIO.LoadActFile(".bin".ToCharArray(), StageSystem.stageListPosition, fData))
        return;
      byte num1 = FileIO.ReadByte();
      int num2 = (int) num1;
      StageSystem.titleCardWord2 = (char) num1;
      int index1;
      for (index1 = 0; index1 < num2; ++index1)
      {
        StageSystem.titleCardText[index1] = (char) FileIO.ReadByte();
        if (StageSystem.titleCardText[index1] == '-')
          StageSystem.titleCardWord2 = (char) (index1 + 1);
      }
      StageSystem.titleCardText[index1] = char.MinValue;
      for (int index2 = 0; index2 < 4; ++index2)
      {
        byte num3 = FileIO.ReadByte();
        StageSystem.activeTileLayers[index2] = num3;
      }
      StageSystem.tLayerMidPoint = FileIO.ReadByte();
      StageSystem.stageLayouts[0].xSize = FileIO.ReadByte();
      StageSystem.stageLayouts[0].ySize = FileIO.ReadByte();
      StageSystem.xBoundary1 = 0;
      StageSystem.newXBoundary1 = 0;
      StageSystem.yBoundary1 = 0;
      StageSystem.newYBoundary1 = 0;
      StageSystem.xBoundary2 = (int) StageSystem.stageLayouts[0].xSize << 7;
      StageSystem.yBoundary2 = (int) StageSystem.stageLayouts[0].ySize << 7;
      StageSystem.waterLevel = StageSystem.yBoundary2 + 128;
      StageSystem.newXBoundary2 = StageSystem.xBoundary2;
      StageSystem.newYBoundary2 = StageSystem.yBoundary2;
      for (int index2 = 0; index2 < 65536; ++index2)
        StageSystem.stageLayouts[0].tileMap[index2] = (ushort) 0;
      for (int index2 = 0; index2 < (int) StageSystem.stageLayouts[0].ySize; ++index2)
      {
        for (int index3 = 0; index3 < (int) StageSystem.stageLayouts[0].xSize; ++index3)
        {
          byte num3 = FileIO.ReadByte();
          StageSystem.stageLayouts[0].tileMap[(index2 << 8) + index3] = (ushort) ((uint) num3 << 8);
          byte num4 = FileIO.ReadByte();
          StageSystem.stageLayouts[0].tileMap[(index2 << 8) + index3] += (ushort) num4;
        }
      }
      int num5 = (int) FileIO.ReadByte();
      for (int index2 = 0; index2 < num5; ++index2)
      {
        for (int index3 = (int) FileIO.ReadByte(); index3 > 0; --index3)
          FileIO.ReadByte();
      }
      int num6 = ((int) FileIO.ReadByte() << 8) + (int) FileIO.ReadByte();
      int index4 = 32;
      for (int index2 = 0; index2 < num6; ++index2)
      {
        byte num3 = FileIO.ReadByte();
        ObjectSystem.objectEntityList[index4].type = num3;
        byte num4 = FileIO.ReadByte();
        ObjectSystem.objectEntityList[index4].propertyValue = num4;
        byte num7 = FileIO.ReadByte();
        ObjectSystem.objectEntityList[index4].xPos = (int) num7 << 8;
        byte num8 = FileIO.ReadByte();
        ObjectSystem.objectEntityList[index4].xPos += (int) num8;
        ObjectSystem.objectEntityList[index4].xPos <<= 16;
        byte num9 = FileIO.ReadByte();
        ObjectSystem.objectEntityList[index4].yPos = (int) num9 << 8;
        byte num10 = FileIO.ReadByte();
        ObjectSystem.objectEntityList[index4].yPos += (int) num10;
        ObjectSystem.objectEntityList[index4].yPos <<= 16;
        ++index4;
      }
      StageSystem.stageLayouts[0].type = (byte) 1;
      FileIO.CloseFile();
    }

    public static void LoadStageBackground()
    {
      FileData fData = new FileData();
      byte[] numArray1 = new byte[3];
      byte[] numArray2 = new byte[2];
      for (int index = 0; index < 9; ++index)
      {
        StageSystem.stageLayouts[index].type = (byte) 0;
        StageSystem.stageLayouts[index].deformationPos = 0;
        StageSystem.stageLayouts[index].deformationPosW = 0;
      }
      for (int index = 0; index < 256; ++index)
      {
        StageSystem.hParallax.scrollPosition[index] = 0;
        StageSystem.vParallax.scrollPosition[index] = 0;
      }
      for (int index = 0; index < 32768; ++index)
        StageSystem.stageLayouts[0].lineScrollRef[index] = (byte) 0;
      if (!FileIO.LoadStageFile("Backgrounds.bin".ToCharArray(), StageSystem.stageListPosition, fData))
        return;
      byte num1 = FileIO.ReadByte();
      byte num2 = FileIO.ReadByte();
      StageSystem.hParallax.numEntries = num2;
      for (int index = 0; index < (int) StageSystem.hParallax.numEntries; ++index)
      {
        byte num3 = FileIO.ReadByte();
        StageSystem.hParallax.parallaxFactor[index] = (int) num3 << 8;
        byte num4 = FileIO.ReadByte();
        StageSystem.hParallax.parallaxFactor[index] += (int) num4;
        byte num5 = FileIO.ReadByte();
        StageSystem.hParallax.scrollSpeed[index] = (int) num5 << 10;
        StageSystem.hParallax.scrollPosition[index] = 0;
        byte num6 = FileIO.ReadByte();
        StageSystem.hParallax.deformationEnabled[index] = num6;
      }
      byte num7 = FileIO.ReadByte();
      StageSystem.vParallax.numEntries = num7;
      for (int index = 0; index < (int) StageSystem.vParallax.numEntries; ++index)
      {
        byte num3 = FileIO.ReadByte();
        StageSystem.vParallax.parallaxFactor[index] = (int) num3 << 8;
        byte num4 = FileIO.ReadByte();
        StageSystem.vParallax.parallaxFactor[index] += (int) num4;
        byte num5 = FileIO.ReadByte();
        StageSystem.vParallax.scrollSpeed[index] = (int) num5 << 10;
        StageSystem.vParallax.scrollPosition[index] = 0;
        byte num6 = FileIO.ReadByte();
        StageSystem.vParallax.deformationEnabled[index] = num6;
      }
      for (int index1 = 1; index1 < (int) num1 + 1; ++index1)
      {
        byte num3 = FileIO.ReadByte();
        StageSystem.stageLayouts[index1].xSize = num3;
        byte num4 = FileIO.ReadByte();
        StageSystem.stageLayouts[index1].ySize = num4;
        byte num5 = FileIO.ReadByte();
        StageSystem.stageLayouts[index1].type = num5;
        byte num6 = FileIO.ReadByte();
        StageSystem.stageLayouts[index1].parallaxFactor = (int) num6 << 8;
        byte num8 = FileIO.ReadByte();
        StageSystem.stageLayouts[index1].parallaxFactor += (int) num8;
        byte num9 = FileIO.ReadByte();
        StageSystem.stageLayouts[index1].scrollSpeed = (int) num9 << 10;
        StageSystem.stageLayouts[index1].scrollPosition = 0;
        for (int index2 = 0; index2 < 65536; ++index2)
          StageSystem.stageLayouts[index1].tileMap[index2] = (ushort) 0;
        for (int index2 = 0; index2 < 32768; ++index2)
          StageSystem.stageLayouts[index1].lineScrollRef[index2] = (byte) 0;
        int index3 = 0;
        int num10 = 0;
        while (num10 < 1)
        {
          numArray1[0] = FileIO.ReadByte();
          if (numArray1[0] == byte.MaxValue)
          {
            numArray1[1] = FileIO.ReadByte();
            if (numArray1[1] == byte.MaxValue)
            {
              num10 = 1;
            }
            else
            {
              numArray1[2] = FileIO.ReadByte();
              numArray2[0] = numArray1[1];
              numArray2[1] = (byte) ((uint) numArray1[2] - 1U);
              for (int index2 = 0; index2 < (int) numArray2[1]; ++index2)
              {
                StageSystem.stageLayouts[index1].lineScrollRef[index3] = numArray2[0];
                ++index3;
              }
            }
          }
          else
          {
            StageSystem.stageLayouts[index1].lineScrollRef[index3] = numArray1[0];
            ++index3;
          }
        }
        for (int index2 = 0; index2 < (int) StageSystem.stageLayouts[index1].ySize; ++index2)
        {
          for (int index4 = 0; index4 < (int) StageSystem.stageLayouts[index1].xSize; ++index4)
          {
            byte num11 = FileIO.ReadByte();
            StageSystem.stageLayouts[index1].tileMap[(index2 << 8) + index4] = (ushort) ((uint) num11 << 8);
            byte num12 = FileIO.ReadByte();
            StageSystem.stageLayouts[index1].tileMap[(index2 << 8) + index4] += (ushort) num12;
          }
        }
      }
      FileIO.CloseFile();
    }

    public static void ResetBackgroundSettings()
    {
      for (int index = 0; index < 9; ++index)
      {
        StageSystem.stageLayouts[index].deformationPos = 0;
        StageSystem.stageLayouts[index].deformationPosW = 0;
        StageSystem.stageLayouts[index].scrollPosition = 0;
      }
      for (int index = 0; index < 256; ++index)
      {
        StageSystem.hParallax.scrollPosition[index] = 0;
        StageSystem.vParallax.scrollPosition[index] = 0;
      }
      for (int index = 0; index < 576; ++index)
      {
        StageSystem.bgDeformationData0[index] = 0;
        StageSystem.bgDeformationData1[index] = 0;
        StageSystem.bgDeformationData2[index] = 0;
        StageSystem.bgDeformationData3[index] = 0;
      }
    }

    public static void SetLayerDeformation(
      int selectedDef,
      int waveLength,
      int waveWidth,
      int wType,
      int yPos,
      int wSize)
    {
      int index1 = 0;
      switch (selectedDef)
      {
        case 0:
          switch (wType)
          {
            case 0:
              for (int index2 = 0; index2 < 131072; index2 += 512)
              {
                StageSystem.bgDeformationData0[index1] = GlobalAppDefinitions.SinValue512[index2 / waveLength & 511] * waveWidth >> 5;
                ++index1;
              }
              break;
            case 1:
              int index3 = index1 + yPos;
              for (int index2 = 0; index2 < wSize; ++index2)
              {
                StageSystem.bgDeformationData0[index3] = GlobalAppDefinitions.SinValue512[(index2 << 9) / waveLength & 511] * waveWidth >> 5;
                ++index3;
              }
              break;
          }
          for (int index2 = 256; index2 < 576; ++index2)
            StageSystem.bgDeformationData0[index2] = StageSystem.bgDeformationData0[index2 - 256];
          break;
        case 1:
          switch (wType)
          {
            case 0:
              for (int index2 = 0; index2 < 131072; index2 += 512)
              {
                StageSystem.bgDeformationData1[index1] = GlobalAppDefinitions.SinValue512[index2 / waveLength & 511] * waveWidth >> 5;
                ++index1;
              }
              break;
            case 1:
              int index4 = index1 + yPos;
              for (int index2 = 0; index2 < wSize; ++index2)
              {
                StageSystem.bgDeformationData1[index4] = GlobalAppDefinitions.SinValue512[(index2 << 9) / waveLength & 511] * waveWidth >> 5;
                ++index4;
              }
              break;
          }
          for (int index2 = 256; index2 < 576; ++index2)
            StageSystem.bgDeformationData1[index2] = StageSystem.bgDeformationData1[index2 - 256];
          break;
        case 2:
          switch (wType)
          {
            case 0:
              for (int index2 = 0; index2 < 131072; index2 += 512)
              {
                StageSystem.bgDeformationData2[index1] = GlobalAppDefinitions.SinValue512[index2 / waveLength & 511] * waveWidth >> 5;
                ++index1;
              }
              break;
            case 1:
              int index5 = index1 + yPos;
              for (int index2 = 0; index2 < wSize; ++index2)
              {
                StageSystem.bgDeformationData2[index5] = GlobalAppDefinitions.SinValue512[(index2 << 9) / waveLength & 511] * waveWidth >> 5;
                ++index5;
              }
              break;
          }
          for (int index2 = 256; index2 < 576; ++index2)
            StageSystem.bgDeformationData2[index2] = StageSystem.bgDeformationData2[index2 - 256];
          break;
        case 3:
          switch (wType)
          {
            case 0:
              for (int index2 = 0; index2 < 131072; index2 += 512)
              {
                StageSystem.bgDeformationData3[index1] = GlobalAppDefinitions.SinValue512[index2 / waveLength & 511] * waveWidth >> 5;
                ++index1;
              }
              break;
            case 1:
              int index6 = index1 + yPos;
              for (int index2 = 0; index2 < wSize; ++index2)
              {
                StageSystem.bgDeformationData3[index6] = GlobalAppDefinitions.SinValue512[(index2 << 9) / waveLength & 511] * waveWidth >> 5;
                ++index6;
              }
              break;
          }
          for (int index2 = 256; index2 < 576; ++index2)
            StageSystem.bgDeformationData3[index2] = StageSystem.bgDeformationData3[index2 - 256];
          break;
      }
    }

    public static void DrawStageGfx()
    {
      GraphicsSystem.gfxVertexSize = (ushort) 0;
      GraphicsSystem.gfxIndexSize = (ushort) 0;
      GraphicsSystem.waterDrawPos = StageSystem.waterLevel - StageSystem.yScrollOffset;
      if (GraphicsSystem.waterDrawPos < -16)
        GraphicsSystem.waterDrawPos = -16;
      if (GraphicsSystem.waterDrawPos >= 240)
        GraphicsSystem.waterDrawPos = 256;
      ObjectSystem.DrawObjectList(0);
      if (StageSystem.activeTileLayers[0] < (byte) 9)
      {
        switch (StageSystem.stageLayouts[(int) StageSystem.activeTileLayers[0]].type)
        {
          case 1:
            StageSystem.DrawHLineScrollLayer8((byte) 0);
            break;
          case 3:
            StageSystem.Draw3DFloorLayer((byte) 0);
            break;
          case 4:
            StageSystem.Draw3DFloorLayer((byte) 0);
            break;
        }
      }
      GraphicsSystem.gfxIndexSizeOpaque = GraphicsSystem.gfxIndexSize;
      GraphicsSystem.gfxVertexSizeOpaque = GraphicsSystem.gfxVertexSize;
      ObjectSystem.DrawObjectList(1);
      if (StageSystem.activeTileLayers[1] < (byte) 9)
      {
        switch (StageSystem.stageLayouts[(int) StageSystem.activeTileLayers[1]].type)
        {
          case 1:
            StageSystem.DrawHLineScrollLayer8((byte) 1);
            break;
          case 3:
            StageSystem.Draw3DFloorLayer((byte) 1);
            break;
          case 4:
            StageSystem.Draw3DFloorLayer((byte) 1);
            break;
        }
      }
      ObjectSystem.DrawObjectList(2);
      if (StageSystem.activeTileLayers[2] < (byte) 9)
      {
        switch (StageSystem.stageLayouts[(int) StageSystem.activeTileLayers[2]].type)
        {
          case 1:
            StageSystem.DrawHLineScrollLayer8((byte) 2);
            break;
          case 3:
            StageSystem.Draw3DFloorLayer((byte) 2);
            break;
          case 4:
            StageSystem.Draw3DFloorLayer((byte) 2);
            break;
        }
      }
      ObjectSystem.DrawObjectList(3);
      ObjectSystem.DrawObjectList(4);
      if (StageSystem.activeTileLayers[3] < (byte) 9)
      {
        switch (StageSystem.stageLayouts[(int) StageSystem.activeTileLayers[3]].type)
        {
          case 1:
            StageSystem.DrawHLineScrollLayer8((byte) 3);
            break;
          case 3:
            StageSystem.Draw3DFloorLayer((byte) 3);
            break;
          case 4:
            StageSystem.Draw3DFloorLayer((byte) 3);
            break;
        }
      }
      ObjectSystem.DrawObjectList(5);
      ObjectSystem.DrawObjectList(6);
    }

    public static void DrawHLineScrollLayer8(byte layerNum)
    {
      int num1 = 0;
      int[] gfxDataPos = StageSystem.tile128x128.gfxDataPos;
      byte[] direction = StageSystem.tile128x128.direction;
      byte[] visualPlane = StageSystem.tile128x128.visualPlane;
      int num2 = (int) StageSystem.stageLayouts[(int) StageSystem.activeTileLayers[(int) layerNum]].xSize;
      int num3 = (int) StageSystem.stageLayouts[(int) StageSystem.activeTileLayers[(int) layerNum]].ySize;
      int num4 = (GlobalAppDefinitions.SCREEN_XSIZE >> 4) + 3;
      byte num5 = (int) layerNum < (int) StageSystem.tLayerMidPoint ? (byte) 0 : (byte) 1;
      ushort[] tileMap;
      byte[] lineScrollRef;
      int num6;
      int num7;
      int[] numArray1;
      int[] numArray2;
      int num8;
      if (StageSystem.activeTileLayers[(int) layerNum] == (byte) 0)
      {
        tileMap = StageSystem.stageLayouts[0].tileMap;
        StageSystem.lastXSize = num2;
        int yScrollOffset = StageSystem.yScrollOffset;
        lineScrollRef = StageSystem.stageLayouts[0].lineScrollRef;
        StageSystem.hParallax.linePos[0] = StageSystem.xScrollOffset;
        num6 = StageSystem.stageLayouts[0].deformationPos + yScrollOffset & (int) byte.MaxValue;
        num7 = StageSystem.stageLayouts[0].deformationPosW + yScrollOffset & (int) byte.MaxValue;
        numArray1 = StageSystem.bgDeformationData0;
        numArray2 = StageSystem.bgDeformationData1;
        num8 = yScrollOffset % (num3 << 7);
      }
      else
      {
        tileMap = StageSystem.stageLayouts[(int) StageSystem.activeTileLayers[(int) layerNum]].tileMap;
        int num9 = StageSystem.stageLayouts[(int) StageSystem.activeTileLayers[(int) layerNum]].parallaxFactor * StageSystem.yScrollOffset >> 8;
        int num10 = num3 << 7;
        StageSystem.stageLayouts[(int) StageSystem.activeTileLayers[(int) layerNum]].scrollPosition += StageSystem.stageLayouts[(int) StageSystem.activeTileLayers[(int) layerNum]].scrollSpeed;
        if (StageSystem.stageLayouts[(int) StageSystem.activeTileLayers[(int) layerNum]].scrollPosition > num10 << 16)
          StageSystem.stageLayouts[(int) StageSystem.activeTileLayers[(int) layerNum]].scrollPosition -= num10 << 16;
        num8 = (num9 + (StageSystem.stageLayouts[(int) StageSystem.activeTileLayers[(int) layerNum]].scrollPosition >> 16)) % num10;
        num3 = num10 >> 7;
        lineScrollRef = StageSystem.stageLayouts[(int) StageSystem.activeTileLayers[(int) layerNum]].lineScrollRef;
        num6 = StageSystem.stageLayouts[(int) StageSystem.activeTileLayers[(int) layerNum]].deformationPos + num8 & (int) byte.MaxValue;
        num7 = StageSystem.stageLayouts[(int) StageSystem.activeTileLayers[(int) layerNum]].deformationPosW + num8 & (int) byte.MaxValue;
        numArray1 = StageSystem.bgDeformationData2;
        numArray2 = StageSystem.bgDeformationData3;
      }
      switch (StageSystem.stageLayouts[(int) StageSystem.activeTileLayers[(int) layerNum]].type)
      {
        case 1:
          if (StageSystem.lastXSize != num2)
          {
            int num9 = num2 << 7;
            for (int index = 0; index < (int) StageSystem.hParallax.numEntries; ++index)
            {
              StageSystem.hParallax.linePos[index] = StageSystem.hParallax.parallaxFactor[index] * StageSystem.xScrollOffset >> 8;
              StageSystem.hParallax.scrollPosition[index] += StageSystem.hParallax.scrollSpeed[index];
              if (StageSystem.hParallax.scrollPosition[index] > num9 << 16)
                StageSystem.hParallax.scrollPosition[index] -= num9 << 16;
              StageSystem.hParallax.linePos[index] += StageSystem.hParallax.scrollPosition[index] >> 16;
              StageSystem.hParallax.linePos[index] %= num9;
            }
            num2 = num9 >> 7;
          }
          StageSystem.lastXSize = num2;
          break;
      }
      if (num8 < 0)
        num8 += num3 << 7;
      int num11 = num8 >> 4 << 4;
      int index1 = num1 + num11;
      int index2 = num6 + (num11 - num8);
      int index3 = num7 + (num11 - num8);
      if (index2 < 0)
        index2 += 256;
      if (index3 < 0)
        index3 += 256;
      int num12 = -(num8 & 15);
      int num13 = num8 >> 7;
      int num14 = (num8 & (int) sbyte.MaxValue) >> 4;
      int num15 = num12 != 0 ? 272 : 256;
      GraphicsSystem.waterDrawPos <<= 4;
      int num16 = num12 << 4;
      for (int index4 = num15; index4 > 0; index4 -= 16)
      {
        int num9 = StageSystem.hParallax.linePos[(int) lineScrollRef[index1]] - 16;
        int index5 = index1 + 8;
        bool flag;
        if (num9 == StageSystem.hParallax.linePos[(int) lineScrollRef[index5]] - 16)
        {
          if (StageSystem.hParallax.deformationEnabled[(int) lineScrollRef[index5]] == (byte) 1)
          {
            int num10 = num16 < GraphicsSystem.waterDrawPos ? numArray1[index2] : numArray2[index3];
            int index6 = index2 + 8;
            int index7 = index3 + 8;
            int num17 = num16 + 64 <= GraphicsSystem.waterDrawPos ? numArray1[index6] : numArray2[index7];
            flag = num10 != num17;
            index2 = index6 - 8;
            index3 = index7 - 8;
          }
          else
            flag = false;
        }
        else
          flag = true;
        int index8 = index5 - 8;
        if (flag)
        {
          int num10 = num2 << 7;
          if (num9 < 0)
            num9 += num10;
          if (num9 >= num10)
            num9 -= num10;
          int num17 = num9 >> 7;
          int num18 = (num9 & (int) sbyte.MaxValue) >> 4;
          int num19 = -((num9 & 15) << 4) - 256;
          int num20 = num19;
          int index6;
          int index7;
          if (StageSystem.hParallax.deformationEnabled[(int) lineScrollRef[index8]] == (byte) 1)
          {
            if (num16 >= GraphicsSystem.waterDrawPos)
              num19 -= numArray2[index3];
            else
              num19 -= numArray1[index2];
            index6 = index2 + 8;
            index7 = index3 + 8;
            if (num16 + 64 > GraphicsSystem.waterDrawPos)
              num20 -= numArray2[index7];
            else
              num20 -= numArray1[index6];
          }
          else
          {
            index6 = index2 + 8;
            index7 = index3 + 8;
          }
          int index9 = index8 + 8;
          int index10 = (num17 <= -1 || num13 <= -1 ? 0 : (int) tileMap[num17 + (num13 << 8)] << 6) + (num18 + (num14 << 3));
          for (int index11 = num4; index11 > 0; --index11)
          {
            if ((int) visualPlane[index10] == (int) num5 && gfxDataPos[index10] > 0)
            {
              int num21 = 0;
              switch (direction[index10])
              {
                case 0:
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.X = (float) num19;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.Y = (float) num16;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.tileUVArray[gfxDataPos[index10] + num21];
                  int num22 = num21 + 1;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.tileUVArray[gfxDataPos[index10] + num22];
                  int num23 = num22 + 1;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
                  ++GraphicsSystem.gfxVertexSize;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.X = (float) (num19 + 256);
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.Y = (float) num16;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.tileUVArray[gfxDataPos[index10] + num23];
                  int num24 = num23 + 1;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].texCoord.Y;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
                  ++GraphicsSystem.gfxVertexSize;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.X = (float) num20;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.Y = (float) (num16 + 128);
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 2].texCoord.X;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.tileUVArray[gfxDataPos[index10] + num24] - 1f / 128f;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
                  ++GraphicsSystem.gfxVertexSize;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.X = (float) (num20 + 256);
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.Y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].position.Y;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 2].texCoord.X;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].texCoord.Y;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
                  ++GraphicsSystem.gfxVertexSize;
                  GraphicsSystem.gfxIndexSize += (ushort) 2;
                  break;
                case 1:
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.X = (float) (num19 + 256);
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.Y = (float) num16;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.tileUVArray[gfxDataPos[index10] + num21];
                  int num25 = num21 + 1;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.tileUVArray[gfxDataPos[index10] + num25];
                  int num26 = num25 + 1;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
                  ++GraphicsSystem.gfxVertexSize;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.X = (float) num19;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.Y = (float) num16;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.tileUVArray[gfxDataPos[index10] + num26];
                  int num27 = num26 + 1;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].texCoord.Y;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
                  ++GraphicsSystem.gfxVertexSize;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.X = (float) (num20 + 256);
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.Y = (float) (num16 + 128);
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 2].texCoord.X;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.tileUVArray[gfxDataPos[index10] + num27] - 1f / 128f;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
                  ++GraphicsSystem.gfxVertexSize;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.X = (float) num20;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.Y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].position.Y;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 2].texCoord.X;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].texCoord.Y;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
                  ++GraphicsSystem.gfxVertexSize;
                  GraphicsSystem.gfxIndexSize += (ushort) 2;
                  break;
                case 2:
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.X = (float) num20;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.Y = (float) (num16 + 128);
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.tileUVArray[gfxDataPos[index10] + num21];
                  int num28 = num21 + 1;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.tileUVArray[gfxDataPos[index10] + num28] + 1f / 128f;
                  int num29 = num28 + 1;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
                  ++GraphicsSystem.gfxVertexSize;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.X = (float) (num20 + 256);
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.Y = (float) (num16 + 128);
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.tileUVArray[gfxDataPos[index10] + num29];
                  int num30 = num29 + 1;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].texCoord.Y;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
                  ++GraphicsSystem.gfxVertexSize;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.X = (float) num19;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.Y = (float) num16;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 2].texCoord.X;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.tileUVArray[gfxDataPos[index10] + num30];
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
                  ++GraphicsSystem.gfxVertexSize;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.X = (float) (num19 + 256);
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.Y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].position.Y;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 2].texCoord.X;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].texCoord.Y;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
                  ++GraphicsSystem.gfxVertexSize;
                  GraphicsSystem.gfxIndexSize += (ushort) 2;
                  break;
                case 3:
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.X = (float) (num20 + 256);
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.Y = (float) (num16 + 128);
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.tileUVArray[gfxDataPos[index10] + num21];
                  int num31 = num21 + 1;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.tileUVArray[gfxDataPos[index10] + num31] + 1f / 128f;
                  int num32 = num31 + 1;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
                  ++GraphicsSystem.gfxVertexSize;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.X = (float) num20;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.Y = (float) (num16 + 128);
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.tileUVArray[gfxDataPos[index10] + num32];
                  int num33 = num32 + 1;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].texCoord.Y;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
                  ++GraphicsSystem.gfxVertexSize;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.X = (float) (num19 + 256);
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.Y = (float) num16;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 2].texCoord.X;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.tileUVArray[gfxDataPos[index10] + num33];
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
                  ++GraphicsSystem.gfxVertexSize;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.X = (float) num19;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.Y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].position.Y;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 2].texCoord.X;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].texCoord.Y;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
                  ++GraphicsSystem.gfxVertexSize;
                  GraphicsSystem.gfxIndexSize += (ushort) 2;
                  break;
              }
            }
            num19 += 256;
            num20 += 256;
            ++num18;
            if (num18 > 7)
            {
              ++num17;
              if (num17 == num2)
                num17 = 0;
              num18 = 0;
              index10 = ((int) tileMap[num17 + (num13 << 8)] << 6) + (num18 + (num14 << 3));
            }
            else
              ++index10;
          }
          int num34 = num16 + 128;
          int num35 = StageSystem.hParallax.linePos[(int) lineScrollRef[index9]] - 16;
          int num36 = num2 << 7;
          if (num35 < 0)
            num35 += num36;
          if (num35 >= num36)
            num35 -= num36;
          int num37 = num35 >> 7;
          int num38 = (num35 & (int) sbyte.MaxValue) >> 4;
          int num39 = -((num35 & 15) << 4) - 256;
          int num40 = num39;
          if (StageSystem.hParallax.deformationEnabled[(int) lineScrollRef[index9]] == (byte) 1)
          {
            if (num34 >= GraphicsSystem.waterDrawPos)
              num39 -= numArray2[index7];
            else
              num39 -= numArray1[index6];
            index2 = index6 + 8;
            index3 = index7 + 8;
            if (num34 + 64 > GraphicsSystem.waterDrawPos)
              num40 -= numArray2[index3];
            else
              num40 -= numArray1[index2];
          }
          else
          {
            index2 = index6 + 8;
            index3 = index7 + 8;
          }
          index1 = index9 + 8;
          int index12 = (num37 <= -1 || num13 <= -1 ? 0 : (int) tileMap[num37 + (num13 << 8)] << 6) + (num38 + (num14 << 3));
          for (int index11 = num4; index11 > 0; --index11)
          {
            if ((int) visualPlane[index12] == (int) num5 && gfxDataPos[index12] > 0)
            {
              int num21 = 0;
              switch (direction[index12])
              {
                case 0:
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.X = (float) num39;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.Y = (float) num34;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.tileUVArray[gfxDataPos[index12] + num21];
                  int num22 = num21 + 1;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.tileUVArray[gfxDataPos[index12] + num22] + 1f / 128f;
                  int num23 = num22 + 1;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
                  ++GraphicsSystem.gfxVertexSize;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.X = (float) (num39 + 256);
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.Y = (float) num34;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.tileUVArray[gfxDataPos[index12] + num23];
                  int num24 = num23 + 1;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].texCoord.Y;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
                  ++GraphicsSystem.gfxVertexSize;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.X = (float) num40;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.Y = (float) (num34 + 128);
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 2].texCoord.X;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.tileUVArray[gfxDataPos[index12] + num24];
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
                  ++GraphicsSystem.gfxVertexSize;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.X = (float) (num40 + 256);
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.Y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].position.Y;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 2].texCoord.X;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].texCoord.Y;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
                  ++GraphicsSystem.gfxVertexSize;
                  GraphicsSystem.gfxIndexSize += (ushort) 2;
                  break;
                case 1:
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.X = (float) (num39 + 256);
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.Y = (float) num34;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.tileUVArray[gfxDataPos[index12] + num21];
                  int num25 = num21 + 1;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.tileUVArray[gfxDataPos[index12] + num25] + 1f / 128f;
                  int num26 = num25 + 1;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
                  ++GraphicsSystem.gfxVertexSize;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.X = (float) num39;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.Y = (float) num34;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.tileUVArray[gfxDataPos[index12] + num26];
                  int num27 = num26 + 1;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].texCoord.Y;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
                  ++GraphicsSystem.gfxVertexSize;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.X = (float) (num40 + 256);
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.Y = (float) (num34 + 128);
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 2].texCoord.X;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.tileUVArray[gfxDataPos[index12] + num27];
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
                  ++GraphicsSystem.gfxVertexSize;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.X = (float) num40;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.Y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].position.Y;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 2].texCoord.X;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].texCoord.Y;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
                  ++GraphicsSystem.gfxVertexSize;
                  GraphicsSystem.gfxIndexSize += (ushort) 2;
                  break;
                case 2:
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.X = (float) num40;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.Y = (float) (num34 + 128);
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.tileUVArray[gfxDataPos[index12] + num21];
                  int num28 = num21 + 1;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.tileUVArray[gfxDataPos[index12] + num28];
                  int num29 = num28 + 1;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
                  ++GraphicsSystem.gfxVertexSize;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.X = (float) (num40 + 256);
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.Y = (float) (num34 + 128);
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.tileUVArray[gfxDataPos[index12] + num29];
                  int num30 = num29 + 1;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].texCoord.Y;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
                  ++GraphicsSystem.gfxVertexSize;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.X = (float) num39;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.Y = (float) num34;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 2].texCoord.X;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.tileUVArray[gfxDataPos[index12] + num30] - 1f / 128f;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
                  ++GraphicsSystem.gfxVertexSize;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.X = (float) (num39 + 256);
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.Y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].position.Y;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 2].texCoord.X;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].texCoord.Y;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
                  ++GraphicsSystem.gfxVertexSize;
                  GraphicsSystem.gfxIndexSize += (ushort) 2;
                  break;
                case 3:
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.X = (float) (num40 + 256);
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.Y = (float) (num34 + 128);
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.tileUVArray[gfxDataPos[index12] + num21];
                  int num31 = num21 + 1;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.tileUVArray[gfxDataPos[index12] + num31];
                  int num32 = num31 + 1;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
                  ++GraphicsSystem.gfxVertexSize;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.X = (float) num40;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.Y = (float) (num34 + 128);
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.tileUVArray[gfxDataPos[index12] + num32];
                  int num33 = num32 + 1;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].texCoord.Y;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
                  ++GraphicsSystem.gfxVertexSize;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.X = (float) (num39 + 256);
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.Y = (float) num34;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 2].texCoord.X;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.tileUVArray[gfxDataPos[index12] + num33] - 1f / 128f;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
                  ++GraphicsSystem.gfxVertexSize;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.X = (float) num39;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.Y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].position.Y;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 2].texCoord.X;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].texCoord.Y;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
                  ++GraphicsSystem.gfxVertexSize;
                  GraphicsSystem.gfxIndexSize += (ushort) 2;
                  break;
              }
            }
            num39 += 256;
            num40 += 256;
            ++num38;
            if (num38 > 7)
            {
              ++num37;
              if (num37 == num2)
                num37 = 0;
              num38 = 0;
              index12 = ((int) tileMap[num37 + (num13 << 8)] << 6) + (num38 + (num14 << 3));
            }
            else
              ++index12;
          }
          num16 = num34 + 128;
        }
        else
        {
          int num10 = num2 << 7;
          if (num9 < 0)
            num9 += num10;
          if (num9 >= num10)
            num9 -= num10;
          int num17 = num9 >> 7;
          int num18 = (num9 & (int) sbyte.MaxValue) >> 4;
          int num19 = -((num9 & 15) << 4) - 256;
          int num20 = num19;
          if (StageSystem.hParallax.deformationEnabled[(int) lineScrollRef[index8]] == (byte) 1)
          {
            if (num16 >= GraphicsSystem.waterDrawPos)
              num19 -= numArray2[index3];
            else
              num19 -= numArray1[index2];
            index2 += 16;
            index3 += 16;
            if (num16 + 128 > GraphicsSystem.waterDrawPos)
              num20 -= numArray2[index3];
            else
              num20 -= numArray1[index2];
          }
          else
          {
            index2 += 16;
            index3 += 16;
          }
          index1 = index8 + 16;
          int index6 = (num17 <= -1 || num13 <= -1 ? 0 : (int) tileMap[num17 + (num13 << 8)] << 6) + (num18 + (num14 << 3));
          for (int index7 = num4; index7 > 0; --index7)
          {
            if ((int) visualPlane[index6] == (int) num5 && gfxDataPos[index6] > 0)
            {
              int num21 = 0;
              switch (direction[index6])
              {
                case 0:
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.X = (float) num19;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.Y = (float) num16;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.tileUVArray[gfxDataPos[index6] + num21];
                  int num22 = num21 + 1;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.tileUVArray[gfxDataPos[index6] + num22];
                  int num23 = num22 + 1;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
                  ++GraphicsSystem.gfxVertexSize;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.X = (float) (num19 + 256);
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.Y = (float) num16;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.tileUVArray[gfxDataPos[index6] + num23];
                  int num24 = num23 + 1;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].texCoord.Y;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
                  ++GraphicsSystem.gfxVertexSize;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.X = (float) num20;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.Y = (float) (num16 + 256);
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 2].texCoord.X;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.tileUVArray[gfxDataPos[index6] + num24];
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
                  ++GraphicsSystem.gfxVertexSize;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.X = (float) (num20 + 256);
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.Y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].position.Y;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 2].texCoord.X;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].texCoord.Y;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
                  ++GraphicsSystem.gfxVertexSize;
                  GraphicsSystem.gfxIndexSize += (ushort) 2;
                  break;
                case 1:
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.X = (float) (num19 + 256);
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.Y = (float) num16;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.tileUVArray[gfxDataPos[index6] + num21];
                  int num25 = num21 + 1;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.tileUVArray[gfxDataPos[index6] + num25];
                  int num26 = num25 + 1;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
                  ++GraphicsSystem.gfxVertexSize;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.X = (float) num19;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.Y = (float) num16;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.tileUVArray[gfxDataPos[index6] + num26];
                  int num27 = num26 + 1;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].texCoord.Y;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
                  ++GraphicsSystem.gfxVertexSize;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.X = (float) (num20 + 256);
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.Y = (float) (num16 + 256);
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 2].texCoord.X;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.tileUVArray[gfxDataPos[index6] + num27];
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
                  ++GraphicsSystem.gfxVertexSize;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.X = (float) num20;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.Y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].position.Y;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 2].texCoord.X;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].texCoord.Y;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
                  ++GraphicsSystem.gfxVertexSize;
                  GraphicsSystem.gfxIndexSize += (ushort) 2;
                  break;
                case 2:
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.X = (float) num20;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.Y = (float) (num16 + 256);
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.tileUVArray[gfxDataPos[index6] + num21];
                  int num28 = num21 + 1;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.tileUVArray[gfxDataPos[index6] + num28];
                  int num29 = num28 + 1;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
                  ++GraphicsSystem.gfxVertexSize;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.X = (float) (num20 + 256);
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.Y = (float) (num16 + 256);
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.tileUVArray[gfxDataPos[index6] + num29];
                  int num30 = num29 + 1;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].texCoord.Y;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
                  ++GraphicsSystem.gfxVertexSize;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.X = (float) num19;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.Y = (float) num16;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 2].texCoord.X;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.tileUVArray[gfxDataPos[index6] + num30];
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
                  ++GraphicsSystem.gfxVertexSize;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.X = (float) (num19 + 256);
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.Y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].position.Y;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 2].texCoord.X;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].texCoord.Y;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
                  ++GraphicsSystem.gfxVertexSize;
                  GraphicsSystem.gfxIndexSize += (ushort) 2;
                  break;
                case 3:
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.X = (float) (num20 + 256);
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.Y = (float) (num16 + 256);
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.tileUVArray[gfxDataPos[index6] + num21];
                  int num31 = num21 + 1;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.tileUVArray[gfxDataPos[index6] + num31];
                  int num32 = num31 + 1;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
                  ++GraphicsSystem.gfxVertexSize;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.X = (float) num20;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.Y = (float) (num16 + 256);
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.tileUVArray[gfxDataPos[index6] + num32];
                  int num33 = num32 + 1;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].texCoord.Y;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
                  ++GraphicsSystem.gfxVertexSize;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.X = (float) (num19 + 256);
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.Y = (float) num16;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 2].texCoord.X;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.tileUVArray[gfxDataPos[index6] + num33];
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
                  ++GraphicsSystem.gfxVertexSize;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.X = (float) num19;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.Y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].position.Y;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 2].texCoord.X;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].texCoord.Y;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
                  GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
                  ++GraphicsSystem.gfxVertexSize;
                  GraphicsSystem.gfxIndexSize += (ushort) 2;
                  break;
              }
            }
            num19 += 256;
            num20 += 256;
            ++num18;
            if (num18 > 7)
            {
              ++num17;
              if (num17 == num2)
                num17 = 0;
              num18 = 0;
              index6 = ((int) tileMap[num17 + (num13 << 8)] << 6) + (num18 + (num14 << 3));
            }
            else
              ++index6;
          }
          num16 += 256;
        }
        ++num14;
        if (num14 > 7)
        {
          ++num13;
          if (num13 == num3)
          {
            num13 = 0;
            index1 -= num3 << 7;
          }
          num14 = 0;
        }
      }
      GraphicsSystem.waterDrawPos >>= 4;
    }

    public static void Draw3DFloorLayer(byte layerNum)
    {
      int[] gfxDataPos = StageSystem.tile128x128.gfxDataPos;
      byte[] direction = StageSystem.tile128x128.direction;
      int num1 = (int) StageSystem.stageLayouts[(int) StageSystem.activeTileLayers[(int) layerNum]].xSize << 7;
      int num2 = (int) StageSystem.stageLayouts[(int) StageSystem.activeTileLayers[(int) layerNum]].ySize << 7;
      ushort[] tileMap = StageSystem.stageLayouts[(int) StageSystem.activeTileLayers[(int) layerNum]].tileMap;
      GraphicsSystem.vertexSize3D = (ushort) 0;
      GraphicsSystem.indexSize3D = (ushort) 0;
      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.X = 0.0f;
      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.Y = 0.0f;
      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.Z = 0.0f;
      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.X = 0.5f;
      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.Y = 0.0f;
      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.R = byte.MaxValue;
      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.G = byte.MaxValue;
      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.B = byte.MaxValue;
      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.A = byte.MaxValue;
      ++GraphicsSystem.vertexSize3D;
      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.X = 4096f;
      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.Y = 0.0f;
      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.Z = 0.0f;
      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.X = 1f;
      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.Y = 0.0f;
      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.R = byte.MaxValue;
      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.G = byte.MaxValue;
      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.B = byte.MaxValue;
      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.A = byte.MaxValue;
      ++GraphicsSystem.vertexSize3D;
      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.X = 0.0f;
      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.Y = 0.0f;
      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.Z = 4096f;
      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.X = 0.5f;
      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.Y = 0.5f;
      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.R = byte.MaxValue;
      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.G = byte.MaxValue;
      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.B = byte.MaxValue;
      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.A = byte.MaxValue;
      ++GraphicsSystem.vertexSize3D;
      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.X = 4096f;
      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.Y = 0.0f;
      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.Z = 4096f;
      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.X = 1f;
      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.Y = 0.5f;
      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.R = byte.MaxValue;
      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.G = byte.MaxValue;
      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.B = byte.MaxValue;
      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.A = byte.MaxValue;
      ++GraphicsSystem.vertexSize3D;
      GraphicsSystem.indexSize3D += (ushort) 2;
      if (!GlobalAppDefinitions.HQ3DFloorEnabled)
      {
        int num3 = (StageSystem.stageLayouts[(int) StageSystem.activeTileLayers[(int) layerNum]].xPos >> 16) - 160 + GlobalAppDefinitions.SinValue512[StageSystem.stageLayouts[(int) StageSystem.activeTileLayers[(int) layerNum]].angle] / 3 >> 4 << 4;
        int num4 = (StageSystem.stageLayouts[(int) StageSystem.activeTileLayers[(int) layerNum]].zPos >> 16) - 160 + GlobalAppDefinitions.CosValue512[StageSystem.stageLayouts[(int) StageSystem.activeTileLayers[(int) layerNum]].angle] / 3 >> 4 << 4;
        for (int index1 = 20; index1 > 0; --index1)
        {
          for (int index2 = 20; index2 > 0; --index2)
          {
            if (num3 > -1 && num3 < num1 && (num4 > -1 && num4 < num2))
            {
              int num5 = num3 >> 7;
              int num6 = num4 >> 7;
              int num7 = (num3 & (int) sbyte.MaxValue) >> 4;
              int num8 = (num4 & (int) sbyte.MaxValue) >> 4;
              int index3 = ((int) tileMap[num5 + (num6 << 8)] << 6) + (num7 + (num8 << 3));
              if (gfxDataPos[index3] > 0)
              {
                int num9 = 0;
                switch (direction[index3])
                {
                  case 0:
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.X = (float) num3;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.Y = 0.0f;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.Z = (float) num4;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.X = GraphicsSystem.tileUVArray[gfxDataPos[index3] + num9];
                    int num10 = num9 + 1;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.Y = GraphicsSystem.tileUVArray[gfxDataPos[index3] + num10];
                    int num11 = num10 + 1;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.R = byte.MaxValue;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.G = byte.MaxValue;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.B = byte.MaxValue;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.A = byte.MaxValue;
                    ++GraphicsSystem.vertexSize3D;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.X = (float) (num3 + 16);
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.Y = 0.0f;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.Z = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 1].position.Z;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.X = GraphicsSystem.tileUVArray[gfxDataPos[index3] + num11];
                    int num12 = num11 + 1;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.Y = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 1].texCoord.Y;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.R = byte.MaxValue;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.G = byte.MaxValue;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.B = byte.MaxValue;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.A = byte.MaxValue;
                    ++GraphicsSystem.vertexSize3D;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.X = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 2].position.X;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.Y = 0.0f;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.Z = (float) (num4 + 16);
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.X = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 2].texCoord.X;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.Y = GraphicsSystem.tileUVArray[gfxDataPos[index3] + num12];
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.R = byte.MaxValue;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.G = byte.MaxValue;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.B = byte.MaxValue;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.A = byte.MaxValue;
                    ++GraphicsSystem.vertexSize3D;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.X = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 2].position.X;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.Y = 0.0f;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.Z = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 1].position.Z;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.X = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 2].texCoord.X;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.Y = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 1].texCoord.Y;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.R = byte.MaxValue;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.G = byte.MaxValue;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.B = byte.MaxValue;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.A = byte.MaxValue;
                    ++GraphicsSystem.vertexSize3D;
                    GraphicsSystem.indexSize3D += (ushort) 2;
                    break;
                  case 1:
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.X = (float) (num3 + 16);
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.Y = 0.0f;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.Z = (float) num4;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.X = GraphicsSystem.tileUVArray[gfxDataPos[index3] + num9];
                    int num13 = num9 + 1;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.Y = GraphicsSystem.tileUVArray[gfxDataPos[index3] + num13];
                    int num14 = num13 + 1;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.R = byte.MaxValue;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.G = byte.MaxValue;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.B = byte.MaxValue;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.A = byte.MaxValue;
                    ++GraphicsSystem.vertexSize3D;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.X = (float) num3;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.Y = 0.0f;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.Z = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 1].position.Z;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.X = GraphicsSystem.tileUVArray[gfxDataPos[index3] + num14];
                    int num15 = num14 + 1;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.Y = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 1].texCoord.Y;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.R = byte.MaxValue;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.G = byte.MaxValue;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.B = byte.MaxValue;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.A = byte.MaxValue;
                    ++GraphicsSystem.vertexSize3D;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.X = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 2].position.X;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.Y = 0.0f;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.Z = (float) (num4 + 16);
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.X = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 2].texCoord.X;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.Y = GraphicsSystem.tileUVArray[gfxDataPos[index3] + num15];
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.R = byte.MaxValue;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.G = byte.MaxValue;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.B = byte.MaxValue;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.A = byte.MaxValue;
                    ++GraphicsSystem.vertexSize3D;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.X = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 2].position.X;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.Y = 0.0f;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.Z = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 1].position.Z;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.X = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 2].texCoord.X;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.Y = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 1].texCoord.Y;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.R = byte.MaxValue;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.G = byte.MaxValue;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.B = byte.MaxValue;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.A = byte.MaxValue;
                    ++GraphicsSystem.vertexSize3D;
                    GraphicsSystem.indexSize3D += (ushort) 2;
                    break;
                  case 2:
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.X = (float) num3;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.Y = 0.0f;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.Z = (float) (num4 + 16);
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.X = GraphicsSystem.tileUVArray[gfxDataPos[index3] + num9];
                    int num16 = num9 + 1;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.Y = GraphicsSystem.tileUVArray[gfxDataPos[index3] + num16];
                    int num17 = num16 + 1;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.R = byte.MaxValue;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.G = byte.MaxValue;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.B = byte.MaxValue;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.A = byte.MaxValue;
                    ++GraphicsSystem.vertexSize3D;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.X = (float) (num3 + 16);
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.Y = 0.0f;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.Z = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 1].position.Z;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.X = GraphicsSystem.tileUVArray[gfxDataPos[index3] + num17];
                    int num18 = num17 + 1;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.Y = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 1].texCoord.Y;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.R = byte.MaxValue;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.G = byte.MaxValue;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.B = byte.MaxValue;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.A = byte.MaxValue;
                    ++GraphicsSystem.vertexSize3D;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.X = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 2].position.X;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.Y = 0.0f;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.Z = (float) num4;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.X = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 2].texCoord.X;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.Y = GraphicsSystem.tileUVArray[gfxDataPos[index3] + num18];
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.R = byte.MaxValue;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.G = byte.MaxValue;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.B = byte.MaxValue;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.A = byte.MaxValue;
                    ++GraphicsSystem.vertexSize3D;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.X = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 2].position.X;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.Y = 0.0f;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.Z = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 1].position.Z;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.X = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 2].texCoord.X;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.Y = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 1].texCoord.Y;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.R = byte.MaxValue;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.G = byte.MaxValue;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.B = byte.MaxValue;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.A = byte.MaxValue;
                    ++GraphicsSystem.vertexSize3D;
                    GraphicsSystem.indexSize3D += (ushort) 2;
                    break;
                  case 3:
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.X = (float) (num3 + 16);
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.Y = 0.0f;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.Z = (float) (num4 + 16);
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.X = GraphicsSystem.tileUVArray[gfxDataPos[index3] + num9];
                    int num19 = num9 + 1;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.Y = GraphicsSystem.tileUVArray[gfxDataPos[index3] + num19];
                    int num20 = num19 + 1;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.R = byte.MaxValue;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.G = byte.MaxValue;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.B = byte.MaxValue;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.A = byte.MaxValue;
                    ++GraphicsSystem.vertexSize3D;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.X = (float) num3;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.Y = 0.0f;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.Z = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 1].position.Z;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.X = GraphicsSystem.tileUVArray[gfxDataPos[index3] + num20];
                    int num21 = num20 + 1;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.Y = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 1].texCoord.Y;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.R = byte.MaxValue;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.G = byte.MaxValue;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.B = byte.MaxValue;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.A = byte.MaxValue;
                    ++GraphicsSystem.vertexSize3D;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.X = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 2].position.X;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.Y = 0.0f;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.Z = (float) num4;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.X = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 2].texCoord.X;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.Y = GraphicsSystem.tileUVArray[gfxDataPos[index3] + num21];
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.R = byte.MaxValue;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.G = byte.MaxValue;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.B = byte.MaxValue;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.A = byte.MaxValue;
                    ++GraphicsSystem.vertexSize3D;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.X = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 2].position.X;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.Y = 0.0f;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.Z = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 1].position.Z;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.X = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 2].texCoord.X;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.Y = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 1].texCoord.Y;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.R = byte.MaxValue;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.G = byte.MaxValue;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.B = byte.MaxValue;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.A = byte.MaxValue;
                    ++GraphicsSystem.vertexSize3D;
                    GraphicsSystem.indexSize3D += (ushort) 2;
                    break;
                }
              }
            }
            num3 += 16;
          }
          num3 -= 320;
          num4 += 16;
        }
      }
      else
      {
        int num3 = (StageSystem.stageLayouts[(int) StageSystem.activeTileLayers[(int) layerNum]].xPos >> 16) - 256 + (GlobalAppDefinitions.SinValue512[StageSystem.stageLayouts[(int) StageSystem.activeTileLayers[(int) layerNum]].angle] >> 1) >> 4 << 4;
        int num4 = (StageSystem.stageLayouts[(int) StageSystem.activeTileLayers[(int) layerNum]].zPos >> 16) - 256 + (GlobalAppDefinitions.CosValue512[StageSystem.stageLayouts[(int) StageSystem.activeTileLayers[(int) layerNum]].angle] >> 1) >> 4 << 4;
        for (int index1 = 32; index1 > 0; --index1)
        {
          for (int index2 = 32; index2 > 0; --index2)
          {
            if (num3 > -1 && num3 < num1 && (num4 > -1 && num4 < num2))
            {
              int num5 = num3 >> 7;
              int num6 = num4 >> 7;
              int num7 = (num3 & (int) sbyte.MaxValue) >> 4;
              int num8 = (num4 & (int) sbyte.MaxValue) >> 4;
              int index3 = ((int) tileMap[num5 + (num6 << 8)] << 6) + (num7 + (num8 << 3));
              if (gfxDataPos[index3] > 0)
              {
                int num9 = 0;
                switch (direction[index3])
                {
                  case 0:
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.X = (float) num3;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.Y = 0.0f;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.Z = (float) num4;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.X = GraphicsSystem.tileUVArray[gfxDataPos[index3] + num9];
                    int num10 = num9 + 1;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.Y = GraphicsSystem.tileUVArray[gfxDataPos[index3] + num10];
                    int num11 = num10 + 1;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.R = byte.MaxValue;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.G = byte.MaxValue;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.B = byte.MaxValue;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.A = byte.MaxValue;
                    ++GraphicsSystem.vertexSize3D;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.X = (float) (num3 + 16);
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.Y = 0.0f;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.Z = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 1].position.Z;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.X = GraphicsSystem.tileUVArray[gfxDataPos[index3] + num11];
                    int num12 = num11 + 1;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.Y = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 1].texCoord.Y;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.R = byte.MaxValue;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.G = byte.MaxValue;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.B = byte.MaxValue;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.A = byte.MaxValue;
                    ++GraphicsSystem.vertexSize3D;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.X = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 2].position.X;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.Y = 0.0f;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.Z = (float) (num4 + 16);
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.X = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 2].texCoord.X;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.Y = GraphicsSystem.tileUVArray[gfxDataPos[index3] + num12];
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.R = byte.MaxValue;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.G = byte.MaxValue;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.B = byte.MaxValue;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.A = byte.MaxValue;
                    ++GraphicsSystem.vertexSize3D;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.X = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 2].position.X;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.Y = 0.0f;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.Z = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 1].position.Z;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.X = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 2].texCoord.X;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.Y = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 1].texCoord.Y;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.R = byte.MaxValue;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.G = byte.MaxValue;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.B = byte.MaxValue;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.A = byte.MaxValue;
                    ++GraphicsSystem.vertexSize3D;
                    GraphicsSystem.indexSize3D += (ushort) 2;
                    break;
                  case 1:
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.X = (float) (num3 + 16);
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.Y = 0.0f;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.Z = (float) num4;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.X = GraphicsSystem.tileUVArray[gfxDataPos[index3] + num9];
                    int num13 = num9 + 1;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.Y = GraphicsSystem.tileUVArray[gfxDataPos[index3] + num13];
                    int num14 = num13 + 1;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.R = byte.MaxValue;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.G = byte.MaxValue;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.B = byte.MaxValue;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.A = byte.MaxValue;
                    ++GraphicsSystem.vertexSize3D;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.X = (float) num3;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.Y = 0.0f;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.Z = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 1].position.Z;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.X = GraphicsSystem.tileUVArray[gfxDataPos[index3] + num14];
                    int num15 = num14 + 1;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.Y = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 1].texCoord.Y;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.R = byte.MaxValue;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.G = byte.MaxValue;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.B = byte.MaxValue;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.A = byte.MaxValue;
                    ++GraphicsSystem.vertexSize3D;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.X = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 2].position.X;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.Y = 0.0f;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.Z = (float) (num4 + 16);
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.X = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 2].texCoord.X;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.Y = GraphicsSystem.tileUVArray[gfxDataPos[index3] + num15];
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.R = byte.MaxValue;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.G = byte.MaxValue;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.B = byte.MaxValue;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.A = byte.MaxValue;
                    ++GraphicsSystem.vertexSize3D;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.X = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 2].position.X;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.Y = 0.0f;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.Z = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 1].position.Z;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.X = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 2].texCoord.X;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.Y = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 1].texCoord.Y;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.R = byte.MaxValue;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.G = byte.MaxValue;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.B = byte.MaxValue;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.A = byte.MaxValue;
                    ++GraphicsSystem.vertexSize3D;
                    GraphicsSystem.indexSize3D += (ushort) 2;
                    break;
                  case 2:
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.X = (float) num3;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.Y = 0.0f;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.Z = (float) (num4 + 16);
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.X = GraphicsSystem.tileUVArray[gfxDataPos[index3] + num9];
                    int num16 = num9 + 1;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.Y = GraphicsSystem.tileUVArray[gfxDataPos[index3] + num16];
                    int num17 = num16 + 1;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.R = byte.MaxValue;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.G = byte.MaxValue;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.B = byte.MaxValue;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.A = byte.MaxValue;
                    ++GraphicsSystem.vertexSize3D;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.X = (float) (num3 + 16);
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.Y = 0.0f;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.Z = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 1].position.Z;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.X = GraphicsSystem.tileUVArray[gfxDataPos[index3] + num17];
                    int num18 = num17 + 1;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.Y = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 1].texCoord.Y;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.R = byte.MaxValue;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.G = byte.MaxValue;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.B = byte.MaxValue;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.A = byte.MaxValue;
                    ++GraphicsSystem.vertexSize3D;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.X = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 2].position.X;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.Y = 0.0f;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.Z = (float) num4;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.X = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 2].texCoord.X;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.Y = GraphicsSystem.tileUVArray[gfxDataPos[index3] + num18];
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.R = byte.MaxValue;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.G = byte.MaxValue;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.B = byte.MaxValue;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.A = byte.MaxValue;
                    ++GraphicsSystem.vertexSize3D;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.X = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 2].position.X;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.Y = 0.0f;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.Z = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 1].position.Z;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.X = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 2].texCoord.X;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.Y = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 1].texCoord.Y;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.R = byte.MaxValue;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.G = byte.MaxValue;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.B = byte.MaxValue;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.A = byte.MaxValue;
                    ++GraphicsSystem.vertexSize3D;
                    GraphicsSystem.indexSize3D += (ushort) 2;
                    break;
                  case 3:
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.X = (float) (num3 + 16);
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.Y = 0.0f;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.Z = (float) (num4 + 16);
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.X = GraphicsSystem.tileUVArray[gfxDataPos[index3] + num9];
                    int num19 = num9 + 1;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.Y = GraphicsSystem.tileUVArray[gfxDataPos[index3] + num19];
                    int num20 = num19 + 1;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.R = byte.MaxValue;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.G = byte.MaxValue;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.B = byte.MaxValue;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.A = byte.MaxValue;
                    ++GraphicsSystem.vertexSize3D;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.X = (float) num3;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.Y = 0.0f;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.Z = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 1].position.Z;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.X = GraphicsSystem.tileUVArray[gfxDataPos[index3] + num20];
                    int num21 = num20 + 1;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.Y = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 1].texCoord.Y;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.R = byte.MaxValue;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.G = byte.MaxValue;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.B = byte.MaxValue;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.A = byte.MaxValue;
                    ++GraphicsSystem.vertexSize3D;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.X = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 2].position.X;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.Y = 0.0f;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.Z = (float) num4;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.X = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 2].texCoord.X;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.Y = GraphicsSystem.tileUVArray[gfxDataPos[index3] + num21];
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.R = byte.MaxValue;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.G = byte.MaxValue;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.B = byte.MaxValue;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.A = byte.MaxValue;
                    ++GraphicsSystem.vertexSize3D;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.X = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 2].position.X;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.Y = 0.0f;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.Z = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 1].position.Z;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.X = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 2].texCoord.X;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.Y = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 1].texCoord.Y;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.R = byte.MaxValue;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.G = byte.MaxValue;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.B = byte.MaxValue;
                    GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.A = byte.MaxValue;
                    ++GraphicsSystem.vertexSize3D;
                    GraphicsSystem.indexSize3D += (ushort) 2;
                    break;
                }
              }
            }
            num3 += 16;
          }
          num3 -= 512;
          num4 += 16;
        }
      }
      GraphicsSystem.floor3DPos.X = (float) (StageSystem.stageLayouts[(int) StageSystem.activeTileLayers[(int) layerNum]].xPos >> 8) * (-1f / 256f);
      GraphicsSystem.floor3DPos.Y = (float) (StageSystem.stageLayouts[(int) StageSystem.activeTileLayers[(int) layerNum]].yPos >> 8) * (1f / 256f);
      GraphicsSystem.floor3DPos.Z = (float) (StageSystem.stageLayouts[(int) StageSystem.activeTileLayers[(int) layerNum]].zPos >> 8) * (-1f / 256f);
      GraphicsSystem.floor3DAngle = (float) ((double) StageSystem.stageLayouts[(int) StageSystem.activeTileLayers[(int) layerNum]].angle / 512.0 * -360.0);
      GraphicsSystem.render3DEnabled = true;
    }

    public static void InitFirstStage()
    {
      StageSystem.xScrollOffset = 0;
      StageSystem.yScrollOffset = 0;
      AudioPlayback.StopMusic();
      AudioPlayback.StopAllSFX();
      AudioPlayback.ReleaseStageSFX();
      GraphicsSystem.fadeMode = (byte) 0;
      PlayerSystem.playerMenuNum = (byte) 0;
      GraphicsSystem.ClearGraphicsData();
      AnimationSystem.ClearAnimationData();
      GraphicsSystem.LoadPalette("MasterPalette.act".ToCharArray(), 0, 0, 0, 256);
      FileIO.activeStageList = (byte) 0;
      StageSystem.stageMode = (byte) 0;
      GlobalAppDefinitions.gameMode = (byte) 1;
      StageSystem.stageListPosition = 0;
    }

    public static void InitStageSelectMenu()
    {
      StageSystem.xScrollOffset = 0;
      StageSystem.yScrollOffset = 0;
      AudioPlayback.StopMusic();
      AudioPlayback.StopAllSFX();
      AudioPlayback.ReleaseStageSFX();
      GraphicsSystem.fadeMode = (byte) 0;
      PlayerSystem.playerMenuNum = (byte) 0;
      GlobalAppDefinitions.gameMode = (byte) 0;
      GraphicsSystem.ClearGraphicsData();
      AnimationSystem.ClearAnimationData();
      GraphicsSystem.LoadPalette("MasterPalette.act".ToCharArray(), 0, 0, 0, 256);
      TextSystem.textMenuSurfaceNo = 0;
      GraphicsSystem.LoadGIFFile("Data/Game/SystemText.gif".ToCharArray(), 0);
      StageSystem.stageMode = (byte) 0;
      TextSystem.SetupTextMenu(StageSystem.gameMenu[0], 0);
      TextSystem.AddTextMenuEntry(StageSystem.gameMenu[0], "RETRO ENGINE DEV MENU".ToCharArray());
      TextSystem.AddTextMenuEntry(StageSystem.gameMenu[0], " ".ToCharArray());
      TextSystem.AddTextMenuEntry(StageSystem.gameMenu[0], "SONIC CD Version".ToCharArray());
      TextSystem.AddTextMenuEntry(StageSystem.gameMenu[0], GlobalAppDefinitions.gameVersion);
      TextSystem.AddTextMenuEntry(StageSystem.gameMenu[0], " ".ToCharArray());
      TextSystem.AddTextMenuEntry(StageSystem.gameMenu[0], " ".ToCharArray());
      TextSystem.AddTextMenuEntry(StageSystem.gameMenu[0], " ".ToCharArray());
      TextSystem.AddTextMenuEntry(StageSystem.gameMenu[0], "PLAY GAME".ToCharArray());
      TextSystem.AddTextMenuEntry(StageSystem.gameMenu[0], " ".ToCharArray());
      TextSystem.AddTextMenuEntry(StageSystem.gameMenu[0], "STAGE SELECT".ToCharArray());
      StageSystem.gameMenu[0].alignment = (byte) 2;
      StageSystem.gameMenu[0].numSelections = (byte) 2;
      StageSystem.gameMenu[0].selection1 = 0;
      StageSystem.gameMenu[0].selection2 = 7;
      StageSystem.gameMenu[1].numVisibleRows = (ushort) 0;
      StageSystem.gameMenu[1].visibleRowOffset = (ushort) 0;
      RenderDevice.UpdateHardwareTextures();
    }

    public static void InitErrorMessage()
    {
      StageSystem.xScrollOffset = 0;
      StageSystem.yScrollOffset = 0;
      AudioPlayback.StopMusic();
      AudioPlayback.StopAllSFX();
      AudioPlayback.ReleaseStageSFX();
      GraphicsSystem.fadeMode = (byte) 0;
      PlayerSystem.playerMenuNum = (byte) 0;
      GlobalAppDefinitions.gameMode = (byte) 0;
      GraphicsSystem.ClearGraphicsData();
      AnimationSystem.ClearAnimationData();
      GraphicsSystem.LoadPalette("MasterPalette.act".ToCharArray(), 0, 0, 0, 256);
      TextSystem.textMenuSurfaceNo = 0;
      GraphicsSystem.LoadGIFFile("Data/Game/SystemText.gif".ToCharArray(), 0);
      StageSystem.gameMenu[0].alignment = (byte) 2;
      StageSystem.gameMenu[0].numSelections = (byte) 1;
      StageSystem.gameMenu[0].selection1 = 0;
      StageSystem.gameMenu[1].numVisibleRows = (ushort) 0;
      StageSystem.gameMenu[1].visibleRowOffset = (ushort) 0;
      RenderDevice.UpdateHardwareTextures();
      StageSystem.stageMode = (byte) 4;
    }

    public static void ProcessStageSelectMenu()
    {
      GraphicsSystem.gfxVertexSize = (ushort) 0;
      GraphicsSystem.gfxIndexSize = (ushort) 0;
      GraphicsSystem.ClearScreen((byte) 240);
      InputSystem.MenuKeyDown(StageSystem.gKeyDown, (byte) 131);
      //GraphicsSystem.DrawSprite(32, 66, 16, 16, 78, 240, 0);
      //GraphicsSystem.DrawSprite(32, 178, 16, 16, 95, 240, 0);
      //GraphicsSystem.DrawSprite(GlobalAppDefinitions.SCREEN_XSIZE - 32, 208, 16, 16, 112, 240, 0);
      StageSystem.gKeyPress.start = (byte) 0;
      StageSystem.gKeyPress.up = (byte) 0;
      StageSystem.gKeyPress.down = (byte) 0;
      if (StageSystem.gKeyDown.touches > 0)
      {
        if (StageSystem.gKeyDown.touchX[0] < 120)
        {
          if (StageSystem.gKeyDown.touchY[0] < 120)
          {
            if (StageSystem.gKeyDown.up == (byte) 0)
              StageSystem.gKeyPress.up = (byte) 1;
            StageSystem.gKeyDown.up = (byte) 1;
          }
          else
          {
            if (StageSystem.gKeyDown.down == (byte) 0)
              StageSystem.gKeyPress.down = (byte) 1;
            StageSystem.gKeyDown.down = (byte) 1;
          }
        }
        if (StageSystem.gKeyDown.touchX[0] > 200)
        {
          if (StageSystem.gKeyDown.start == (byte) 0)
            StageSystem.gKeyPress.start = (byte) 1;
          StageSystem.gKeyDown.start = (byte) 1;
        }
      }
      else
      {
        StageSystem.gKeyDown.start = (byte) 0;
        StageSystem.gKeyDown.up = (byte) 0;
        StageSystem.gKeyDown.down = (byte) 0;
      }
      switch (StageSystem.stageMode)
      {
        case 0:
          if (StageSystem.gKeyPress.down == (byte) 1)
            StageSystem.gameMenu[0].selection2 += 2;
          if (StageSystem.gKeyPress.up == (byte) 1)
            StageSystem.gameMenu[0].selection2 -= 2;
          if (StageSystem.gameMenu[0].selection2 > 9)
            StageSystem.gameMenu[0].selection2 = 7;
          if (StageSystem.gameMenu[0].selection2 < 7)
            StageSystem.gameMenu[0].selection2 = 9;
          TextSystem.DrawTextMenu(StageSystem.gameMenu[0], GlobalAppDefinitions.SCREEN_CENTER, 72);
          if (StageSystem.gKeyPress.start == (byte) 1)
          {
            if (StageSystem.gameMenu[0].selection2 == 7)
            {
              StageSystem.stageMode = (byte) 0;
              GlobalAppDefinitions.gameMode = (byte) 1;
              FileIO.activeStageList = (byte) 0;
              StageSystem.stageListPosition = 0;
              break;
            }
            TextSystem.SetupTextMenu(StageSystem.gameMenu[0], 0);
            TextSystem.AddTextMenuEntry(StageSystem.gameMenu[0], "CHOOSE A PLAYER".ToCharArray());
            TextSystem.SetupTextMenu(StageSystem.gameMenu[1], 0);
            TextSystem.LoadConfigListText(StageSystem.gameMenu[1], 0);
            StageSystem.gameMenu[1].alignment = (byte) 0;
            StageSystem.gameMenu[1].numSelections = (byte) 1;
            StageSystem.gameMenu[1].selection1 = 0;
            StageSystem.stageMode = (byte) 1;
            break;
          }
          break;
        case 1:
          if (StageSystem.gKeyPress.down == (byte) 1)
            ++StageSystem.gameMenu[1].selection1;
          if (StageSystem.gKeyPress.up == (byte) 1)
            --StageSystem.gameMenu[1].selection1;
          if (StageSystem.gameMenu[1].selection1 == (int) StageSystem.gameMenu[1].numRows)
            StageSystem.gameMenu[1].selection1 = 0;
          if (StageSystem.gameMenu[1].selection1 < 0)
            StageSystem.gameMenu[1].selection1 = (int) StageSystem.gameMenu[1].numRows - 1;
          TextSystem.DrawTextMenu(StageSystem.gameMenu[0], GlobalAppDefinitions.SCREEN_CENTER - 4, 72);
          TextSystem.DrawTextMenu(StageSystem.gameMenu[1], GlobalAppDefinitions.SCREEN_CENTER - 40, 96);
          if (StageSystem.gKeyPress.start == (byte) 1)
          {
            PlayerSystem.playerMenuNum = (byte) StageSystem.gameMenu[1].selection1;
            TextSystem.SetupTextMenu(StageSystem.gameMenu[0], 0);
            TextSystem.AddTextMenuEntry(StageSystem.gameMenu[0], "SELECT A STAGE LIST".ToCharArray());
            TextSystem.AddTextMenuEntry(StageSystem.gameMenu[0], " ".ToCharArray());
            TextSystem.AddTextMenuEntry(StageSystem.gameMenu[0], " ".ToCharArray());
            TextSystem.AddTextMenuEntry(StageSystem.gameMenu[0], "   PRESENTATION".ToCharArray());
            TextSystem.AddTextMenuEntry(StageSystem.gameMenu[0], " ".ToCharArray());
            TextSystem.AddTextMenuEntry(StageSystem.gameMenu[0], "   REGULAR".ToCharArray());
            TextSystem.AddTextMenuEntry(StageSystem.gameMenu[0], " ".ToCharArray());
            TextSystem.AddTextMenuEntry(StageSystem.gameMenu[0], "   SPECIAL".ToCharArray());
            TextSystem.AddTextMenuEntry(StageSystem.gameMenu[0], " ".ToCharArray());
            TextSystem.AddTextMenuEntry(StageSystem.gameMenu[0], "   BONUS".ToCharArray());
            StageSystem.gameMenu[0].alignment = (byte) 0;
            StageSystem.gameMenu[0].selection2 = 3;
            StageSystem.stageMode = (byte) 2;
            break;
          }
          break;
        case 2:
          if (StageSystem.gKeyPress.down == (byte) 1)
            StageSystem.gameMenu[0].selection2 += 2;
          if (StageSystem.gKeyPress.up == (byte) 1)
            StageSystem.gameMenu[0].selection2 -= 2;
          if (StageSystem.gameMenu[0].selection2 > 9)
            StageSystem.gameMenu[0].selection2 = 3;
          if (StageSystem.gameMenu[0].selection2 < 3)
            StageSystem.gameMenu[0].selection2 = 9;
          TextSystem.DrawTextMenu(StageSystem.gameMenu[0], GlobalAppDefinitions.SCREEN_CENTER - 80, 72);
          int num = 0;
          switch (StageSystem.gameMenu[0].selection2)
          {
            case 3:
              if (FileIO.noPresentationStages > (byte) 0)
                num = 1;
              FileIO.activeStageList = (byte) 0;
              break;
            case 5:
              if (FileIO.noZoneStages > (byte) 0)
                num = 1;
              FileIO.activeStageList = (byte) 1;
              break;
            case 7:
              if (FileIO.noSpecialStages > (byte) 0)
                num = 1;
              FileIO.activeStageList = (byte) 3;
              break;
            case 9:
              if (FileIO.noBonusStages > (byte) 0)
                num = 1;
              FileIO.activeStageList = (byte) 2;
              break;
          }
          if (StageSystem.gKeyPress.start == (byte) 1 && num == 1)
          {
            TextSystem.SetupTextMenu(StageSystem.gameMenu[0], 0);
            TextSystem.AddTextMenuEntry(StageSystem.gameMenu[0], "SELECT A STAGE".ToCharArray());
            TextSystem.SetupTextMenu(StageSystem.gameMenu[1], 0);
            TextSystem.LoadConfigListText(StageSystem.gameMenu[1], 1 + (StageSystem.gameMenu[0].selection2 - 3 >> 1));
            StageSystem.gameMenu[1].alignment = (byte) 1;
            StageSystem.gameMenu[1].numSelections = (byte) 3;
            StageSystem.gameMenu[1].selection1 = 0;
            if (StageSystem.gameMenu[1].numRows > (ushort) 18)
              StageSystem.gameMenu[1].numVisibleRows = (ushort) 18;
            StageSystem.gameMenu[0].alignment = (byte) 2;
            StageSystem.gameMenu[0].numSelections = (byte) 1;
            StageSystem.gameMenu[1].timer = (sbyte) 0;
            StageSystem.stageMode = (byte) 3;
            break;
          }
          break;
        case 3:
          if (StageSystem.gKeyDown.down == (byte) 1)
          {
            ++StageSystem.gameMenu[1].timer;
            if (StageSystem.gameMenu[1].timer > (sbyte) 4)
            {
              StageSystem.gameMenu[1].timer = (sbyte) 0;
              StageSystem.gKeyPress.down = (byte) 1;
            }
          }
          else if (StageSystem.gKeyDown.up == (byte) 1)
          {
            --StageSystem.gameMenu[1].timer;
            if (StageSystem.gameMenu[1].timer < (sbyte) -4)
            {
              StageSystem.gameMenu[1].timer = (sbyte) 0;
              StageSystem.gKeyPress.up = (byte) 1;
            }
          }
          else
            StageSystem.gameMenu[1].timer = (sbyte) 0;
          if (StageSystem.gKeyPress.down == (byte) 1)
          {
            ++StageSystem.gameMenu[1].selection1;
            if (StageSystem.gameMenu[1].selection1 - (int) StageSystem.gameMenu[1].visibleRowOffset >= (int) StageSystem.gameMenu[1].numVisibleRows)
              ++StageSystem.gameMenu[1].visibleRowOffset;
          }
          if (StageSystem.gKeyPress.up == (byte) 1)
          {
            --StageSystem.gameMenu[1].selection1;
            if (StageSystem.gameMenu[1].selection1 - (int) StageSystem.gameMenu[1].visibleRowOffset < 0)
              --StageSystem.gameMenu[1].visibleRowOffset;
          }
          if (StageSystem.gameMenu[1].selection1 == (int) StageSystem.gameMenu[1].numRows)
          {
            StageSystem.gameMenu[1].selection1 = 0;
            StageSystem.gameMenu[1].visibleRowOffset = (ushort) 0;
          }
          if (StageSystem.gameMenu[1].selection1 < 0)
          {
            StageSystem.gameMenu[1].selection1 = (int) StageSystem.gameMenu[1].numRows - 1;
            StageSystem.gameMenu[1].visibleRowOffset = (ushort) ((uint) StageSystem.gameMenu[1].numRows - (uint) StageSystem.gameMenu[1].numVisibleRows);
          }
          TextSystem.DrawTextMenu(StageSystem.gameMenu[0], GlobalAppDefinitions.SCREEN_CENTER - 4, 40);
          TextSystem.DrawTextMenu(StageSystem.gameMenu[1], GlobalAppDefinitions.SCREEN_CENTER + 100, 64);
          if (StageSystem.gKeyPress.start == (byte) 1)
          {
            StageSystem.debugMode = StageSystem.gKeyDown.touches <= 1 ? (byte) 0 : (byte) 1;
            StageSystem.stageMode = (byte) 0;
            GlobalAppDefinitions.gameMode = (byte) 1;
            StageSystem.stageListPosition = StageSystem.gameMenu[1].selection1;
            break;
          }
          break;
        case 4:
          TextSystem.DrawTextMenu(StageSystem.gameMenu[0], GlobalAppDefinitions.SCREEN_CENTER, 72);
          if (StageSystem.gKeyPress.start == (byte) 1)
          {
            StageSystem.stageMode = (byte) 0;
            TextSystem.SetupTextMenu(StageSystem.gameMenu[0], 0);
            TextSystem.AddTextMenuEntry(StageSystem.gameMenu[0], "RETRO ENGINE DEV MENU".ToCharArray());
            TextSystem.AddTextMenuEntry(StageSystem.gameMenu[0], " ".ToCharArray());
            TextSystem.AddTextMenuEntry(StageSystem.gameMenu[0], " ".ToCharArray());
            TextSystem.AddTextMenuEntry(StageSystem.gameMenu[0], " ".ToCharArray());
            TextSystem.AddTextMenuEntry(StageSystem.gameMenu[0], " ".ToCharArray());
            TextSystem.AddTextMenuEntry(StageSystem.gameMenu[0], " ".ToCharArray());
            TextSystem.AddTextMenuEntry(StageSystem.gameMenu[0], " ".ToCharArray());
            TextSystem.AddTextMenuEntry(StageSystem.gameMenu[0], "PLAY GAME".ToCharArray());
            TextSystem.AddTextMenuEntry(StageSystem.gameMenu[0], " ".ToCharArray());
            TextSystem.AddTextMenuEntry(StageSystem.gameMenu[0], "STAGE SELECT".ToCharArray());
            StageSystem.gameMenu[0].alignment = (byte) 2;
            StageSystem.gameMenu[0].numSelections = (byte) 2;
            StageSystem.gameMenu[0].selection1 = 0;
            StageSystem.gameMenu[0].selection2 = 7;
            StageSystem.gameMenu[1].numVisibleRows = (ushort) 0;
            StageSystem.gameMenu[1].visibleRowOffset = (ushort) 0;
            break;
          }
          break;
      }
      GraphicsSystem.gfxIndexSizeOpaque = GraphicsSystem.gfxIndexSize;
      GraphicsSystem.gfxVertexSizeOpaque = GraphicsSystem.gfxVertexSize;
    }
  }
}
