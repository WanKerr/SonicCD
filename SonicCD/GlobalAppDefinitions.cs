// Decompiled with JetBrains decompiler
// Type: Retro_Engine.GlobalAppDefinitions
// Assembly: Sonic CD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D35AF46A-1892-4F52-B201-E664C9200079
// Assembly location: E:\wamwo\Downloads\Sonic CD Mod Via Rebel\Sonic CD Mod Via Rebel\Sonic CD.dll

using System;
using System.Reflection;

namespace RetroEngine
{
    public static class GlobalAppDefinitions
    {
        public static char[] gameWindowText = "Retro-Engine".ToCharArray();
        public static char[] gameDescriptionText = new char[256];
        public static char[] gamePlatform = "Mobile".ToCharArray();
        public static char[] gameRenderType = "HW_Rendering".ToCharArray();
        public static char[] gameHapticsSetting = "No_Haptics".ToCharArray();
        public static byte gameMode = 5;
        public static byte gameLanguage = 0;
        public static int gameMessage = 0;
        public static byte gameOnlineActive = 1;
        public static byte gameHapticsEnabled = 0;
        public static byte frameCounter = 0;
        public static int frameSkipTimer = -1;
        public static int frameSkipSetting = 0;
        public static int gameSFXVolume = 100;
        public static int gameBGMVolume = 100;
        public static byte gameTrialMode = 0;
        public static int gamePlatformID = 0;
        public static bool HQ3DFloorEnabled = true;
        public static int SCREEN_XSIZE = 320;
        public static int SCREEN_YSIZE = 240;
        public static int[] SinValue256 = new int[256];
        public static int[] CosValue256 = new int[256];
        public static int[] SinValue512 = new int[512];
        public static int[] CosValue512 = new int[512];
        public static int[] SinValueM7 = new int[512];
        public static int[] CosValueM7 = new int[512];
        public static byte[,] ATanValue256 = new byte[256, 256];
        public const int RETRO_EN = 0;
        public const int RETRO_FR = 1;
        public const int RETRO_IT = 2;
        public const int RETRO_DE = 3;
        public const int RETRO_ES = 4;
        public const int RETRO_JP = 5;
        public const int DEFAULTSCREEN = 0;
        public const int MAINGAME = 1;
        public const int RESETGAME = 2;
        public const int EXITGAME = 3;
        public const int SCRIPTERROR = 4;
        public const int ENTER_HIRESMODE = 5;
        public const int EXIT_HIRESMODE = 6;
        public const int PAUSE_ENGINE = 7;
        public const int PAUSE_WAIT = 8;
        public const int VIDEO_WAIT = 9;

        public const int RETRO_WIN = 0;
        public const int RETRO_OSX = 1;
        public const int RETRO_XBOX_360 = 2;
        public const int RETRO_PS3 = 3;
        public const int RETRO_iOS = 4;
        public const int RETRO_ANDROID = 5;
        public const int RETRO_WP7 = 6;

        public const int MAX_PLAYERS = 2;
        public const int FACING_LEFT = 1;
        public const int FACING_RIGHT = 0;
        public const int GAME_FULL = 0;
        public const int GAME_TRIAL = 1;
        public const int OBJECT_BORDER_Y1 = 256;
        public const int OBJECT_BORDER_Y2 = 496;
        public const double Pi = 3.141592654;
        public static char[] gameVersion;
        public static int SCREEN_CENTER;
        public static int SCREEN_SCROLL_LEFT;
        public static int SCREEN_SCROLL_RIGHT;
        public static int OBJECT_BORDER_X1;
        public static int OBJECT_BORDER_X2;

        static GlobalAppDefinitions()
        {
            //string[] strArray = Assembly.GetExecutingAssembly().FullName.Split(',')[1].Split('=');
            //strArray[1] = strArray[1].Remove(6);
            //gameVersion = strArray[1].ToCharArray();
        }

        public static void CalculateTrigAngles()
        {
            for (int index = 0; index < 512; ++index)
            {
                SinValueM7[index] = (int)(Math.Sin((double)index / 256.0 * 3.141592654) * 4096.0);
                CosValueM7[index] = (int)(Math.Cos((double)index / 256.0 * 3.141592654) * 4096.0);
            }
            SinValueM7[0] = 0;
            CosValueM7[0] = 4096;
            SinValueM7[128] = 4096;
            CosValueM7[128] = 0;
            SinValueM7[256] = 0;
            CosValueM7[256] = -4096;
            SinValueM7[384] = -4096;
            CosValueM7[384] = 0;
            for (int index = 0; index < 512; ++index)
            {
                SinValue512[index] = (int)(Math.Sin((double)index / 256.0 * 3.141592654) * 512.0);
                CosValue512[index] = (int)(Math.Cos((double)index / 256.0 * 3.141592654) * 512.0);
            }
            SinValue512[0] = 0;
            CosValue512[0] = 512;
            SinValue512[128] = 512;
            CosValue512[128] = 0;
            SinValue512[256] = 0;
            CosValue512[256] = -512;
            SinValue512[384] = -512;
            CosValue512[384] = 0;
            for (int index = 0; index < 256; ++index)
            {
                SinValue256[index] = SinValue512[index << 1] >> 1;
                CosValue256[index] = CosValue512[index << 1] >> 1;
            }
            for (int index1 = 0; index1 < 256; ++index1)
            {
                for (int index2 = 0; index2 < 256; ++index2)
                    ATanValue256[index2, index1] = (byte)(Math.Atan2((double)index1, (double)index2) * 40.7436654262052);
            }
        }

        public static byte ArcTanLookup(int x, int y)
        {
            int index1 = x >= 0 ? x : -x;
            int index2 = y >= 0 ? y : -y;
            if (index1 > index2)
            {
                while (index1 > (int)byte.MaxValue)
                {
                    index1 >>= 4;
                    index2 >>= 4;
                }
            }
            else
            {
                for (; index2 > (int)byte.MaxValue; index2 >>= 4)
                    index1 >>= 4;
            }
            return x <= 0 ? (y <= 0 ? (byte)(128U + (uint)ATanValue256[index1, index2]) : (byte)(128U - (uint)ATanValue256[index1, index2])) : (y <= 0 ? (byte)(256U - (uint)ATanValue256[index1, index2]) : ATanValue256[index1, index2]);
        }

        public static void LoadGameConfig(char[] filePath)
        {
            FileData fData = new FileData();
            char[] strB = new char[32];
            if (!FileIO.LoadFile(filePath, fData))
                return;
            byte num1 = FileIO.ReadByte();
            FileIO.ReadCharArray(ref gameWindowText, (int)num1);
            gameWindowText[(int)num1] = char.MinValue;
            byte num2 = FileIO.ReadByte();
            byte num3;
            for (int index = 0; index < (int)num2; ++index)
                num3 = FileIO.ReadByte();
            byte num4 = FileIO.ReadByte();
            FileIO.ReadCharArray(ref gameDescriptionText, (int)num4);
            gameDescriptionText[(int)num4] = char.MinValue;
            byte num5 = FileIO.ReadByte();
            for (int index1 = 0; index1 < (int)num5; ++index1)
            {
                byte num6 = FileIO.ReadByte();
                for (int index2 = 0; index2 < (int)num6; ++index2)
                    num3 = FileIO.ReadByte();
            }
            for (int index1 = 0; index1 < (int)num5; ++index1)
            {
                byte num6 = FileIO.ReadByte();
                for (int index2 = 0; index2 < (int)num6; ++index2)
                    num3 = FileIO.ReadByte();
            }
            byte num7 = FileIO.ReadByte();
            ObjectSystem.NO_GLOBALVARIABLES = (byte)0;
            for (int strPos = 0; strPos < (int)num7; ++strPos)
            {
                ++ObjectSystem.NO_GLOBALVARIABLES;
                byte num6 = FileIO.ReadByte();
                int index;
                for (index = 0; index < (int)num6; ++index)
                {
                    byte num8 = FileIO.ReadByte();
                    strB[index] = (char)num8;
                }
                strB[index] = char.MinValue;
                FileIO.StrCopy2D(ref ObjectSystem.globalVariableNames, ref strB, strPos);
                byte num9 = FileIO.ReadByte();
                ObjectSystem.globalVariables[strPos] = (int)num9 << 24;
                byte num10 = FileIO.ReadByte();
                ObjectSystem.globalVariables[strPos] += (int)num10 << 16;
                byte num11 = FileIO.ReadByte();
                ObjectSystem.globalVariables[strPos] += (int)num11 << 8;
                byte num12 = FileIO.ReadByte();
                ObjectSystem.globalVariables[strPos] += (int)num12;
            }
            byte num13 = FileIO.ReadByte();
            for (int index1 = 0; index1 < (int)num13; ++index1)
            {
                byte num6 = FileIO.ReadByte();
                int index2;
                for (index2 = 0; index2 < (int)num6; ++index2)
                {
                    byte num8 = FileIO.ReadByte();
                    strB[index2] = (char)num8;
                }
                strB[index2] = char.MinValue;
            }
            byte num14 = FileIO.ReadByte();
            for (int index1 = 0; index1 < (int)num14; ++index1)
            {
                byte num6 = FileIO.ReadByte();
                for (int index2 = 0; index2 < (int)num6; ++index2)
                    num3 = FileIO.ReadByte();
            }
            FileIO.noPresentationStages = FileIO.ReadByte();
            byte num15;
            for (int index1 = 0; index1 < (int)FileIO.noPresentationStages; ++index1)
            {
                byte num6 = FileIO.ReadByte();
                int index2;
                for (index2 = 0; index2 < (int)num6; ++index2)
                {
                    byte num8 = FileIO.ReadByte();
                    FileIO.pStageList[index1].stageFolderName[index2] = (char)num8;
                }
                FileIO.pStageList[index1].stageFolderName[index2] = char.MinValue;
                byte num9 = FileIO.ReadByte();
                int index3;
                for (index3 = 0; index3 < (int)num9; ++index3)
                {
                    byte num8 = FileIO.ReadByte();
                    FileIO.pStageList[index1].actNumber[index3] = (char)num8;
                }
                FileIO.pStageList[index1].actNumber[index3] = char.MinValue;
                byte num10 = FileIO.ReadByte();
                for (int index4 = 0; index4 < (int)num10; ++index4)
                    num3 = FileIO.ReadByte();
                num15 = FileIO.ReadByte();
            }
            FileIO.noZoneStages = FileIO.ReadByte();
            for (int index1 = 0; index1 < (int)FileIO.noZoneStages; ++index1)
            {
                byte num6 = FileIO.ReadByte();
                int index2;
                for (index2 = 0; index2 < (int)num6; ++index2)
                {
                    byte num8 = FileIO.ReadByte();
                    FileIO.zStageList[index1].stageFolderName[index2] = (char)num8;
                }
                FileIO.zStageList[index1].stageFolderName[index2] = char.MinValue;
                byte num9 = FileIO.ReadByte();
                int index3;
                for (index3 = 0; index3 < (int)num9; ++index3)
                {
                    byte num8 = FileIO.ReadByte();
                    FileIO.zStageList[index1].actNumber[index3] = (char)num8;
                }
                FileIO.zStageList[index1].actNumber[index3] = char.MinValue;
                byte num10 = FileIO.ReadByte();
                for (int index4 = 0; index4 < (int)num10; ++index4)
                    num3 = FileIO.ReadByte();
                num15 = FileIO.ReadByte();
            }
            FileIO.noSpecialStages = FileIO.ReadByte();
            for (int index1 = 0; index1 < (int)FileIO.noSpecialStages; ++index1)
            {
                byte num6 = FileIO.ReadByte();
                int index2;
                for (index2 = 0; index2 < (int)num6; ++index2)
                {
                    byte num8 = FileIO.ReadByte();
                    FileIO.sStageList[index1].stageFolderName[index2] = (char)num8;
                }
                FileIO.sStageList[index1].stageFolderName[index2] = char.MinValue;
                byte num9 = FileIO.ReadByte();
                int index3;
                for (index3 = 0; index3 < (int)num9; ++index3)
                {
                    byte num8 = FileIO.ReadByte();
                    FileIO.sStageList[index1].actNumber[index3] = (char)num8;
                }
                FileIO.sStageList[index1].actNumber[index3] = char.MinValue;
                byte num10 = FileIO.ReadByte();
                for (int index4 = 0; index4 < (int)num10; ++index4)
                    num3 = FileIO.ReadByte();
                num15 = FileIO.ReadByte();
            }
            FileIO.noBonusStages = FileIO.ReadByte();
            for (int index1 = 0; index1 < (int)FileIO.noBonusStages; ++index1)
            {
                byte num6 = FileIO.ReadByte();
                int index2;
                for (index2 = 0; index2 < (int)num6; ++index2)
                {
                    byte num8 = FileIO.ReadByte();
                    FileIO.bStageList[index1].stageFolderName[index2] = (char)num8;
                }
                FileIO.bStageList[index1].stageFolderName[index2] = char.MinValue;
                byte num9 = FileIO.ReadByte();
                int index3;
                for (index3 = 0; index3 < (int)num9; ++index3)
                {
                    byte num8 = FileIO.ReadByte();
                    FileIO.bStageList[index1].actNumber[index3] = (char)num8;
                }
                FileIO.bStageList[index1].actNumber[index3] = char.MinValue;
                byte num10 = FileIO.ReadByte();
                for (int index4 = 0; index4 < (int)num10; ++index4)
                    num3 = FileIO.ReadByte();
                num15 = FileIO.ReadByte();
            }
            FileIO.CloseFile();
        }
    }
}
