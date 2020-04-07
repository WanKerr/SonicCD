// Decompiled with JetBrains decompiler
// Type: Retro_Engine.ObjectSystem
// Assembly: Sonic CD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D35AF46A-1892-4F52-B201-E664C9200079
// Assembly location: E:\wamwo\Downloads\Sonic CD Mod Via Rebel\Sonic CD Mod Via Rebel\Sonic CD.dll

using System;

namespace RetroEngine
{
    public static class ObjectSystem
    {
        private static char[,] functionNames = new char[512, 32];
        private static char[,] typeNames = new char[256, 32];
        public static int[] scriptData = new int[262144];
        public static int scriptDataPos = 0;
        public static int[] jumpTableData = new int[16384];
        public static int jumpTableDataPos = 0;
        public static int[] jumpTableStack = new int[1024];
        public static int jumpTableStackPos = 0;
        public static int[] functionStack = new int[1024];
        public static int functionStackPos = 0;
        public static SpriteFrame[] scriptFrames = new SpriteFrame[4096];
        public static int scriptFramesNo = 0;
        public static char[,] globalVariableNames = new char[256, 32];
        public static int[] globalVariables = new int[256];
        public static ScriptEngine scriptEng = new ScriptEngine();
        public static char[] scriptText = new char[256];
        public static ObjectScript[] objectScriptList = new ObjectScript[256];
        public static FunctionScript[] functionScriptList = new FunctionScript[512];
        public static ObjectEntity[] objectEntityList = new ObjectEntity[1184];
        public static ObjectDrawList[] objectDrawOrderList = new ObjectDrawList[7];
        public static CollisionSensor[] cSensor = new CollisionSensor[6];
        private static Random rand = new Random();
        private static sbyte[] scriptOpcodeSizes = new sbyte[135]
        {
      (sbyte) 0,
      (sbyte) 2,
      (sbyte) 2,
      (sbyte) 2,
      (sbyte) 1,
      (sbyte) 1,
      (sbyte) 2,
      (sbyte) 2,
      (sbyte) 2,
      (sbyte) 2,
      (sbyte) 2,
      (sbyte) 2,
      (sbyte) 2,
      (sbyte) 2,
      (sbyte) 1,
      (sbyte) 2,
      (sbyte) 2,
      (sbyte) 2,
      (sbyte) 2,
      (sbyte) 3,
      (sbyte) 3,
      (sbyte) 3,
      (sbyte) 3,
      (sbyte) 3,
      (sbyte) 3,
      (sbyte) 0,
      (sbyte) 0,
      (sbyte) 3,
      (sbyte) 3,
      (sbyte) 3,
      (sbyte) 3,
      (sbyte) 3,
      (sbyte) 3,
      (sbyte) 0,
      (sbyte) 2,
      (sbyte) 0,
      (sbyte) 0,
      (sbyte) 2,
      (sbyte) 2,
      (sbyte) 2,
      (sbyte) 2,
      (sbyte) 2,
      (sbyte) 5,
      (sbyte) 5,
      (sbyte) 3,
      (sbyte) 4,
      (sbyte) 7,
      (sbyte) 1,
      (sbyte) 1,
      (sbyte) 1,
      (sbyte) 3,
      (sbyte) 3,
      (sbyte) 4,
      (sbyte) 7,
      (sbyte) 7,
      (sbyte) 3,
      (sbyte) 6,
      (sbyte) 6,
      (sbyte) 5,
      (sbyte) 3,
      (sbyte) 4,
      (sbyte) 3,
      (sbyte) 7,
      (sbyte) 2,
      (sbyte) 1,
      (sbyte) 4,
      (sbyte) 4,
      (sbyte) 1,
      (sbyte) 4,
      (sbyte) 3,
      (sbyte) 4,
      (sbyte) 0,
      (sbyte) 8,
      (sbyte) 5,
      (sbyte) 5,
      (sbyte) 4,
      (sbyte) 2,
      (sbyte) 0,
      (sbyte) 0,
      (sbyte) 0,
      (sbyte) 0,
      (sbyte) 0,
      (sbyte) 3,
      (sbyte) 1,
      (sbyte) 0,
      (sbyte) 2,
      (sbyte) 1,
      (sbyte) 3,
      (sbyte) 4,
      (sbyte) 4,
      (sbyte) 1,
      (sbyte) 0,
      (sbyte) 2,
      (sbyte) 1,
      (sbyte) 1,
      (sbyte) 0,
      (sbyte) 1,
      (sbyte) 2,
      (sbyte) 4,
      (sbyte) 4,
      (sbyte) 2,
      (sbyte) 2,
      (sbyte) 2,
      (sbyte) 4,
      (sbyte) 3,
      (sbyte) 1,
      (sbyte) 0,
      (sbyte) 6,
      (sbyte) 4,
      (sbyte) 4,
      (sbyte) 4,
      (sbyte) 3,
      (sbyte) 3,
      (sbyte) 0,
      (sbyte) 0,
      (sbyte) 1,
      (sbyte) 2,
      (sbyte) 3,
      (sbyte) 3,
      (sbyte) 4,
      (sbyte) 2,
      (sbyte) 4,
      (sbyte) 2,
      (sbyte) 0,
      (sbyte) 0,
      (sbyte) 1,
      (sbyte) 3,
      (sbyte) 7,
      (sbyte) 5,
      (sbyte) 2,
      (sbyte) 2,
      (sbyte) 2,
      (sbyte) 1,
      (sbyte) 1,
      (sbyte) 4
        };
        public const int SUB_MAIN = 0;
        public const int SUB_PLAYER = 1;
        public const int SUB_DRAW = 2;
        public const int SUB_STARTUP = 3;
        public const int NUM_ARITHMETIC_TOKENS = 13;
        public const int NUM_EVALUATION_TOKENS = 6;
        public const int NUM_OPCODES = 135;
        public const int NUM_VARIABLE_NAMES = 229;
        public const int NUM_CONSTANTS = 31;
        public const int SCRIPT_DATA_SIZE = 262144;
        public const int JUMP_TABLE_SIZE = 16384;
        public static int scriptDataOffset;
        public static int scriptLineNumber;
        public static int jumpTableOffset;
        public static int NUM_FUNCTIONS;
        public static byte NO_GLOBALVARIABLES;
        public static int objectLoop;
        public static int playerNum;

        static ObjectSystem()
        {
            for (int index = 0; index < scriptFrames.Length; ++index)
                scriptFrames[index] = new SpriteFrame();
            for (int index = 0; index < objectScriptList.Length; ++index)
                objectScriptList[index] = new ObjectScript();
            for (int index = 0; index < functionScriptList.Length; ++index)
                functionScriptList[index] = new FunctionScript();
            for (int index = 0; index < objectEntityList.Length; ++index)
                objectEntityList[index] = new ObjectEntity();
            for (int index = 0; index < objectDrawOrderList.Length; ++index)
                objectDrawOrderList[index] = new ObjectDrawList();
            for (int index = 0; index < cSensor.Length; ++index)
                cSensor[index] = new CollisionSensor();
        }

        public static void ClearScriptData()
        {
            char[] charArray = "BlankObject".ToCharArray();
            for (int index = 0; index < 262144; ++index)
                scriptData[index] = 0;
            for (int index = 0; index < 16384; ++index)
                jumpTableData[index] = 0;
            scriptDataPos = 0;
            jumpTableDataPos = 0;
            scriptFramesNo = 0;
            NUM_FUNCTIONS = 0;
            AnimationSystem.ClearAnimationData();
            for (int index = 0; index < 2; ++index)
            {
                PlayerSystem.playerList[index].animationFile = AnimationSystem.GetDefaultAnimationRef();
                PlayerSystem.playerList[index].objectPtr = objectEntityList[0];
            }
            for (int index = 0; index < 256; ++index)
            {
                objectScriptList[index].mainScript = 262143;
                objectScriptList[index].mainJumpTable = 16383;
                objectScriptList[index].playerScript = 262143;
                objectScriptList[index].playerJumpTable = 16383;
                objectScriptList[index].drawScript = 262143;
                objectScriptList[index].drawJumpTable = 16383;
                objectScriptList[index].startupScript = 262143;
                objectScriptList[index].startupJumpTable = 16383;
                objectScriptList[index].frameListOffset = 0;
                objectScriptList[index].numFrames = 0;
                objectScriptList[index].surfaceNum = (byte)0;
                objectScriptList[index].animationFile = AnimationSystem.GetDefaultAnimationRef();
                functionScriptList[index].mainScript = 262143;
                functionScriptList[index].mainJumpTable = 16383;
                typeNames[index, 0] = char.MinValue;
            }
            SetObjectTypeName(charArray, 0);
        }

        public static void SetObjectTypeName(char[] typeName, int scriptNum)
        {
            int index1 = 0;
            int index2 = 0;
            while (index1 < typeName.Length)
            {
                if (typeName[index1] != char.MinValue)
                {
                    if (typeName[index1] != ' ')
                    {
                        typeNames[scriptNum, index2] = typeName[index1];
                        ++index2;
                    }
                    ++index1;
                }
                else
                    index1 = typeName.Length;
            }
            if (index2 >= typeNames.GetLength(1))
                return;
            typeNames[scriptNum, index2] = char.MinValue;
        }

        public static void LoadByteCodeFile(int fileType, int scriptNum)
        {
            FileData fData = new FileData();
            char[] charArray1 = "Data/Scripts/ByteCode/".ToCharArray();
            char[] charArray2 = ".bin".ToCharArray();
            char[] charArray3 = "GlobalCode.bin".ToCharArray();
            FileIO.StrCopy(ref scriptText, ref charArray1);
            switch (fileType)
            {
                case 0:
                    FileIO.StrAdd(ref scriptText, ref FileIO.pStageList[StageSystem.stageListPosition].stageFolderName);
                    FileIO.StrAdd(ref scriptText, ref charArray2);
                    break;
                case 1:
                    FileIO.StrAdd(ref scriptText, ref FileIO.zStageList[StageSystem.stageListPosition].stageFolderName);
                    FileIO.StrAdd(ref scriptText, ref charArray2);
                    break;
                case 2:
                    FileIO.StrAdd(ref scriptText, ref FileIO.bStageList[StageSystem.stageListPosition].stageFolderName);
                    FileIO.StrAdd(ref scriptText, ref charArray2);
                    break;
                case 3:
                    FileIO.StrAdd(ref scriptText, ref FileIO.sStageList[StageSystem.stageListPosition].stageFolderName);
                    FileIO.StrAdd(ref scriptText, ref charArray2);
                    break;
                case 4:
                    FileIO.StrAdd(ref scriptText, ref charArray3);
                    break;
            }
            if (!FileIO.LoadFile(scriptText, fData))
                return;
            int scriptDataPos = ObjectSystem.scriptDataPos;
            int num1 = (int)FileIO.ReadByte() + ((int)FileIO.ReadByte() << 8) + ((int)FileIO.ReadByte() << 16) + ((int)FileIO.ReadByte() << 24);
            while (num1 > 0)
            {
                byte num2 = FileIO.ReadByte();
                byte num3 = (byte)((uint)num2 & (uint)sbyte.MaxValue);
                if (num2 < (byte)128)
                {
                    for (; num3 > (byte)0; --num3)
                    {
                        byte num4 = FileIO.ReadByte();
                        scriptData[scriptDataPos] = (int)num4;
                        ++scriptDataPos;
                        ++ObjectSystem.scriptDataPos;
                        --num1;
                    }
                }
                else
                {
                    for (; num3 > (byte)0; --num3)
                    {
                        int num4 = (int)FileIO.ReadByte() + ((int)FileIO.ReadByte() << 8) + ((int)FileIO.ReadByte() << 16) + ((int)FileIO.ReadByte() << 24);
                        scriptData[scriptDataPos] = num4;
                        ++scriptDataPos;
                        ++ObjectSystem.scriptDataPos;
                        --num1;
                    }
                }
            }
            int jumpTableDataPos = ObjectSystem.jumpTableDataPos;
            int num5 = (int)FileIO.ReadByte() + ((int)FileIO.ReadByte() << 8) + ((int)FileIO.ReadByte() << 16) + ((int)FileIO.ReadByte() << 24);
            while (num5 > 0)
            {
                byte num2 = FileIO.ReadByte();
                byte num3 = (byte)((uint)num2 & (uint)sbyte.MaxValue);
                if (num2 < (byte)128)
                {
                    for (; num3 > (byte)0; --num3)
                    {
                        byte num4 = FileIO.ReadByte();
                        jumpTableData[jumpTableDataPos] = (int)num4;
                        ++jumpTableDataPos;
                        ++ObjectSystem.jumpTableDataPos;
                        --num5;
                    }
                }
                else
                {
                    for (; num3 > (byte)0; --num3)
                    {
                        int num4 = (int)FileIO.ReadByte() + ((int)FileIO.ReadByte() << 8) + ((int)FileIO.ReadByte() << 16) + ((int)FileIO.ReadByte() << 24);
                        jumpTableData[jumpTableDataPos] = num4;
                        ++jumpTableDataPos;
                        ++ObjectSystem.jumpTableDataPos;
                        --num5;
                    }
                }
            }
            int num6 = (int)FileIO.ReadByte() + ((int)FileIO.ReadByte() << 8);
            int index1 = scriptNum;
            for (int index2 = num6; index2 > 0; --index2)
            {
                int num2 = (int)FileIO.ReadByte() + ((int)FileIO.ReadByte() << 8) + ((int)FileIO.ReadByte() << 16) + ((int)FileIO.ReadByte() << 24);
                objectScriptList[index1].mainScript = num2;
                int num3 = (int)FileIO.ReadByte() + ((int)FileIO.ReadByte() << 8) + ((int)FileIO.ReadByte() << 16) + ((int)FileIO.ReadByte() << 24);
                objectScriptList[index1].playerScript = num3;
                int num4 = (int)FileIO.ReadByte() + ((int)FileIO.ReadByte() << 8) + ((int)FileIO.ReadByte() << 16) + ((int)FileIO.ReadByte() << 24);
                objectScriptList[index1].drawScript = num4;
                int num7 = (int)FileIO.ReadByte() + ((int)FileIO.ReadByte() << 8) + ((int)FileIO.ReadByte() << 16) + ((int)FileIO.ReadByte() << 24);
                objectScriptList[index1].startupScript = num7;
                ++index1;
            }
            int index3 = scriptNum;
            for (int index2 = num6; index2 > 0; --index2)
            {
                int num2 = (int)FileIO.ReadByte() + ((int)FileIO.ReadByte() << 8) + ((int)FileIO.ReadByte() << 16) + ((int)FileIO.ReadByte() << 24);
                objectScriptList[index3].mainJumpTable = num2;
                int num3 = (int)FileIO.ReadByte() + ((int)FileIO.ReadByte() << 8) + ((int)FileIO.ReadByte() << 16) + ((int)FileIO.ReadByte() << 24);
                objectScriptList[index3].playerJumpTable = num3;
                int num4 = (int)FileIO.ReadByte() + ((int)FileIO.ReadByte() << 8) + ((int)FileIO.ReadByte() << 16) + ((int)FileIO.ReadByte() << 24);
                objectScriptList[index3].drawJumpTable = num4;
                int num7 = (int)FileIO.ReadByte() + ((int)FileIO.ReadByte() << 8) + ((int)FileIO.ReadByte() << 16) + ((int)FileIO.ReadByte() << 24);
                objectScriptList[index3].startupJumpTable = num7;
                ++index3;
            }
            int num8 = (int)FileIO.ReadByte() + ((int)FileIO.ReadByte() << 8);
            int index4 = 0;
            for (int index2 = num8; index2 > 0; --index2)
            {
                int num2 = (int)FileIO.ReadByte() + ((int)FileIO.ReadByte() << 8) + ((int)FileIO.ReadByte() << 16) + ((int)FileIO.ReadByte() << 24);
                functionScriptList[index4].mainScript = num2;
                ++index4;
            }
            int index5 = 0;
            for (int index2 = num8; index2 > 0; --index2)
            {
                int num2 = (int)FileIO.ReadByte() + ((int)FileIO.ReadByte() << 8) + ((int)FileIO.ReadByte() << 16) + ((int)FileIO.ReadByte() << 24);
                functionScriptList[index5].mainJumpTable = num2;
                ++index5;
            }
            FileIO.CloseFile();
        }

        public static void ProcessScript(int scriptCodePtr, int jumpTablePtr, int scriptSub)
        {
            bool flag = false;
            int index1 = 0;
            int num1 = scriptCodePtr;
            jumpTableStackPos = 0;
            functionStackPos = 0;
            while (!flag)
            {
                int index2 = scriptData[scriptCodePtr];
                ++scriptCodePtr;
                int num2 = 0;
                sbyte num3 = scriptOpcodeSizes[index2];
                for (int index3 = 0; index3 < (int)num3; ++index3)
                {
                    switch (scriptData[scriptCodePtr])
                    {
                        case 1:
                            ++scriptCodePtr;
                            int num4 = num2 + 1;
                            switch (scriptData[scriptCodePtr])
                            {
                                case 0:
                                    index1 = objectLoop;
                                    break;
                                case 1:
                                    ++scriptCodePtr;
                                    if (scriptData[scriptCodePtr] == 1)
                                    {
                                        ++scriptCodePtr;
                                        index1 = scriptEng.arrayPosition[scriptData[scriptCodePtr]];
                                    }
                                    else
                                    {
                                        ++scriptCodePtr;
                                        index1 = scriptData[scriptCodePtr];
                                    }
                                    num4 += 2;
                                    break;
                                case 2:
                                    ++scriptCodePtr;
                                    if (scriptData[scriptCodePtr] == 1)
                                    {
                                        ++scriptCodePtr;
                                        index1 = objectLoop + scriptEng.arrayPosition[scriptData[scriptCodePtr]];
                                    }
                                    else
                                    {
                                        ++scriptCodePtr;
                                        index1 = objectLoop + scriptData[scriptCodePtr];
                                    }
                                    num4 += 2;
                                    break;
                                case 3:
                                    ++scriptCodePtr;
                                    if (scriptData[scriptCodePtr] == 1)
                                    {
                                        ++scriptCodePtr;
                                        index1 = objectLoop - scriptEng.arrayPosition[scriptData[scriptCodePtr]];
                                    }
                                    else
                                    {
                                        ++scriptCodePtr;
                                        index1 = objectLoop - scriptData[scriptCodePtr];
                                    }
                                    num4 += 2;
                                    break;
                            }
                            ++scriptCodePtr;
                            int num5 = num4 + 1;
                            switch (scriptData[scriptCodePtr])
                            {
                                case 0:
                                    scriptEng.operands[index3] = scriptEng.tempValue[0];
                                    break;
                                case 1:
                                    scriptEng.operands[index3] = scriptEng.tempValue[1];
                                    break;
                                case 2:
                                    scriptEng.operands[index3] = scriptEng.tempValue[2];
                                    break;
                                case 3:
                                    scriptEng.operands[index3] = scriptEng.tempValue[3];
                                    break;
                                case 4:
                                    scriptEng.operands[index3] = scriptEng.tempValue[4];
                                    break;
                                case 5:
                                    scriptEng.operands[index3] = scriptEng.tempValue[5];
                                    break;
                                case 6:
                                    scriptEng.operands[index3] = scriptEng.tempValue[6];
                                    break;
                                case 7:
                                    scriptEng.operands[index3] = scriptEng.tempValue[7];
                                    break;
                                case 8:
                                    scriptEng.operands[index3] = scriptEng.checkResult;
                                    break;
                                case 9:
                                    scriptEng.operands[index3] = scriptEng.arrayPosition[0];
                                    break;
                                case 10:
                                    scriptEng.operands[index3] = scriptEng.arrayPosition[1];
                                    break;
                                case 11:
                                    scriptEng.operands[index3] = globalVariables[index1];
                                    break;
                                case 12:
                                    scriptEng.operands[index3] = index1;
                                    break;
                                case 13:
                                    scriptEng.operands[index3] = (int)objectEntityList[index1].type;
                                    break;
                                case 14:
                                    scriptEng.operands[index3] = (int)objectEntityList[index1].propertyValue;
                                    break;
                                case 15:
                                    scriptEng.operands[index3] = objectEntityList[index1].xPos;
                                    break;
                                case 16:
                                    scriptEng.operands[index3] = objectEntityList[index1].yPos;
                                    break;
                                case 17:
                                    scriptEng.operands[index3] = objectEntityList[index1].xPos >> 16;
                                    break;
                                case 18:
                                    scriptEng.operands[index3] = objectEntityList[index1].yPos >> 16;
                                    break;
                                case 19:
                                    scriptEng.operands[index3] = (int)objectEntityList[index1].state;
                                    break;
                                case 20:
                                    scriptEng.operands[index3] = objectEntityList[index1].rotation;
                                    break;
                                case 21:
                                    scriptEng.operands[index3] = objectEntityList[index1].scale;
                                    break;
                                case 22:
                                    scriptEng.operands[index3] = (int)objectEntityList[index1].priority;
                                    break;
                                case 23:
                                    scriptEng.operands[index3] = (int)objectEntityList[index1].drawOrder;
                                    break;
                                case 24:
                                    scriptEng.operands[index3] = (int)objectEntityList[index1].direction;
                                    break;
                                case 25:
                                    scriptEng.operands[index3] = (int)objectEntityList[index1].inkEffect;
                                    break;
                                case 26:
                                    scriptEng.operands[index3] = (int)objectEntityList[index1].alpha;
                                    break;
                                case 27:
                                    scriptEng.operands[index3] = (int)objectEntityList[index1].frame;
                                    break;
                                case 28:
                                    scriptEng.operands[index3] = (int)objectEntityList[index1].animation;
                                    break;
                                case 29:
                                    scriptEng.operands[index3] = (int)objectEntityList[index1].prevAnimation;
                                    break;
                                case 30:
                                    scriptEng.operands[index3] = objectEntityList[index1].animationSpeed;
                                    break;
                                case 31:
                                    scriptEng.operands[index3] = objectEntityList[index1].animationTimer;
                                    break;
                                case 32:
                                    scriptEng.operands[index3] = objectEntityList[index1].value[0];
                                    break;
                                case 33:
                                    scriptEng.operands[index3] = objectEntityList[index1].value[1];
                                    break;
                                case 34:
                                    scriptEng.operands[index3] = objectEntityList[index1].value[2];
                                    break;
                                case 35:
                                    scriptEng.operands[index3] = objectEntityList[index1].value[3];
                                    break;
                                case 36:
                                    scriptEng.operands[index3] = objectEntityList[index1].value[4];
                                    break;
                                case 37:
                                    scriptEng.operands[index3] = objectEntityList[index1].value[5];
                                    break;
                                case 38:
                                    scriptEng.operands[index3] = objectEntityList[index1].value[6];
                                    break;
                                case 39:
                                    scriptEng.operands[index3] = objectEntityList[index1].value[7];
                                    break;
                                case 40:
                                    scriptEng.sRegister = objectEntityList[objectLoop].xPos >> 16;
                                    if (scriptEng.sRegister > StageSystem.xScrollOffset - GlobalAppDefinitions.OBJECT_BORDER_X1 && scriptEng.sRegister < StageSystem.xScrollOffset + GlobalAppDefinitions.OBJECT_BORDER_X2)
                                    {
                                        scriptEng.sRegister = objectEntityList[objectLoop].yPos >> 16;
                                        scriptEng.operands[index3] = scriptEng.sRegister <= StageSystem.yScrollOffset - 256 || scriptEng.sRegister >= StageSystem.yScrollOffset + 496 ? 1 : 0;
                                        break;
                                    }
                                    scriptEng.operands[index3] = 1;
                                    break;
                                case 41:
                                    scriptEng.operands[index3] = (int)PlayerSystem.playerList[playerNum].objectPtr.state;
                                    break;
                                case 42:
                                    scriptEng.operands[index3] = (int)PlayerSystem.playerList[playerNum].controlMode;
                                    break;
                                case 43:
                                    scriptEng.operands[index3] = (int)PlayerSystem.playerList[playerNum].controlLock;
                                    break;
                                case 44:
                                    scriptEng.operands[index3] = (int)PlayerSystem.playerList[playerNum].collisionMode;
                                    break;
                                case 45:
                                    scriptEng.operands[index3] = (int)PlayerSystem.playerList[playerNum].collisionPlane;
                                    break;
                                case 46:
                                    scriptEng.operands[index3] = PlayerSystem.playerList[playerNum].xPos;
                                    break;
                                case 47:
                                    scriptEng.operands[index3] = PlayerSystem.playerList[playerNum].yPos;
                                    break;
                                case 48:
                                    scriptEng.operands[index3] = PlayerSystem.playerList[playerNum].xPos >> 16;
                                    break;
                                case 49:
                                    scriptEng.operands[index3] = PlayerSystem.playerList[playerNum].yPos >> 16;
                                    break;
                                case 50:
                                    scriptEng.operands[index3] = PlayerSystem.playerList[playerNum].screenXPos;
                                    break;
                                case 51:
                                    scriptEng.operands[index3] = PlayerSystem.playerList[playerNum].screenYPos;
                                    break;
                                case 52:
                                    scriptEng.operands[index3] = PlayerSystem.playerList[playerNum].speed;
                                    break;
                                case 53:
                                    scriptEng.operands[index3] = PlayerSystem.playerList[playerNum].xVelocity;
                                    break;
                                case 54:
                                    scriptEng.operands[index3] = PlayerSystem.playerList[playerNum].yVelocity;
                                    break;
                                case 55:
                                    scriptEng.operands[index3] = (int)PlayerSystem.playerList[playerNum].gravity;
                                    break;
                                case 56:
                                    scriptEng.operands[index3] = PlayerSystem.playerList[playerNum].angle;
                                    break;
                                case 57:
                                    scriptEng.operands[index3] = (int)PlayerSystem.playerList[playerNum].skidding;
                                    break;
                                case 58:
                                    scriptEng.operands[index3] = (int)PlayerSystem.playerList[playerNum].pushing;
                                    break;
                                case 59:
                                    scriptEng.operands[index3] = (int)PlayerSystem.playerList[playerNum].trackScroll;
                                    break;
                                case 60:
                                    scriptEng.operands[index3] = (int)PlayerSystem.playerList[playerNum].up;
                                    break;
                                case 61:
                                    scriptEng.operands[index3] = (int)PlayerSystem.playerList[playerNum].down;
                                    break;
                                case 62:
                                    scriptEng.operands[index3] = (int)PlayerSystem.playerList[playerNum].left;
                                    break;
                                case 63:
                                    scriptEng.operands[index3] = (int)PlayerSystem.playerList[playerNum].right;
                                    break;
                                case 64:
                                    scriptEng.operands[index3] = (int)PlayerSystem.playerList[playerNum].jumpPress;
                                    break;
                                case 65:
                                    scriptEng.operands[index3] = (int)PlayerSystem.playerList[playerNum].jumpHold;
                                    break;
                                case 66:
                                    scriptEng.operands[index3] = (int)PlayerSystem.playerList[playerNum].followPlayer1;
                                    break;
                                case 67:
                                    scriptEng.operands[index3] = PlayerSystem.playerList[playerNum].lookPos;
                                    break;
                                case 68:
                                    scriptEng.operands[index3] = (int)PlayerSystem.playerList[playerNum].water;
                                    break;
                                case 69:
                                    scriptEng.operands[index3] = PlayerSystem.playerList[playerNum].movementStats.topSpeed;
                                    break;
                                case 70:
                                    scriptEng.operands[index3] = PlayerSystem.playerList[playerNum].movementStats.acceleration;
                                    break;
                                case 71:
                                    scriptEng.operands[index3] = PlayerSystem.playerList[playerNum].movementStats.deceleration;
                                    break;
                                case 72:
                                    scriptEng.operands[index3] = PlayerSystem.playerList[playerNum].movementStats.airAcceleration;
                                    break;
                                case 73:
                                    scriptEng.operands[index3] = PlayerSystem.playerList[playerNum].movementStats.airDeceleration;
                                    break;
                                case 74:
                                    scriptEng.operands[index3] = PlayerSystem.playerList[playerNum].movementStats.gravity;
                                    break;
                                case 75:
                                    scriptEng.operands[index3] = PlayerSystem.playerList[playerNum].movementStats.jumpStrength;
                                    break;
                                case 76:
                                    scriptEng.operands[index3] = PlayerSystem.playerList[playerNum].movementStats.jumpCap;
                                    break;
                                case 77:
                                    scriptEng.operands[index3] = PlayerSystem.playerList[playerNum].movementStats.rollingAcceleration;
                                    break;
                                case 78:
                                    scriptEng.operands[index3] = PlayerSystem.playerList[playerNum].movementStats.rollingDeceleration;
                                    break;
                                case 79:
                                    scriptEng.operands[index3] = PlayerSystem.playerList[playerNum].objectNum;
                                    break;
                                case 80:
                                    scriptEng.operands[index3] = (int)AnimationSystem.collisionBoxList[PlayerSystem.playerList[playerNum].animationFile.cbListOffset + (int)AnimationSystem.animationFrames[AnimationSystem.animationList[PlayerSystem.playerList[playerNum].animationFile.aniListOffset + (int)PlayerSystem.playerList[playerNum].objectPtr.animation].frameListOffset + (int)PlayerSystem.playerList[playerNum].objectPtr.frame].collisionBox].left[0];
                                    break;
                                case 81:
                                    scriptEng.operands[index3] = (int)AnimationSystem.collisionBoxList[PlayerSystem.playerList[playerNum].animationFile.cbListOffset + (int)AnimationSystem.animationFrames[AnimationSystem.animationList[PlayerSystem.playerList[playerNum].animationFile.aniListOffset + (int)PlayerSystem.playerList[playerNum].objectPtr.animation].frameListOffset + (int)PlayerSystem.playerList[playerNum].objectPtr.frame].collisionBox].top[0];
                                    break;
                                case 82:
                                    scriptEng.operands[index3] = (int)AnimationSystem.collisionBoxList[PlayerSystem.playerList[playerNum].animationFile.cbListOffset + (int)AnimationSystem.animationFrames[AnimationSystem.animationList[PlayerSystem.playerList[playerNum].animationFile.aniListOffset + (int)PlayerSystem.playerList[playerNum].objectPtr.animation].frameListOffset + (int)PlayerSystem.playerList[playerNum].objectPtr.frame].collisionBox].right[0];
                                    break;
                                case 83:
                                    scriptEng.operands[index3] = (int)AnimationSystem.collisionBoxList[PlayerSystem.playerList[playerNum].animationFile.cbListOffset + (int)AnimationSystem.animationFrames[AnimationSystem.animationList[PlayerSystem.playerList[playerNum].animationFile.aniListOffset + (int)PlayerSystem.playerList[playerNum].objectPtr.animation].frameListOffset + (int)PlayerSystem.playerList[playerNum].objectPtr.frame].collisionBox].bottom[0];
                                    break;
                                case 84:
                                    scriptEng.operands[index3] = (int)PlayerSystem.playerList[playerNum].flailing[index1];
                                    break;
                                case 85:
                                    scriptEng.operands[index3] = PlayerSystem.playerList[playerNum].timer;
                                    break;
                                case 86:
                                    scriptEng.operands[index3] = (int)PlayerSystem.playerList[playerNum].tileCollisions;
                                    break;
                                case 87:
                                    scriptEng.operands[index3] = (int)PlayerSystem.playerList[playerNum].objectInteraction;
                                    break;
                                case 88:
                                    scriptEng.operands[index3] = (int)PlayerSystem.playerList[playerNum].visible;
                                    break;
                                case 89:
                                    scriptEng.operands[index3] = PlayerSystem.playerList[playerNum].objectPtr.rotation;
                                    break;
                                case 90:
                                    scriptEng.operands[index3] = PlayerSystem.playerList[playerNum].objectPtr.scale;
                                    break;
                                case 91:
                                    scriptEng.operands[index3] = (int)PlayerSystem.playerList[playerNum].objectPtr.priority;
                                    break;
                                case 92:
                                    scriptEng.operands[index3] = (int)PlayerSystem.playerList[playerNum].objectPtr.drawOrder;
                                    break;
                                case 93:
                                    scriptEng.operands[index3] = (int)PlayerSystem.playerList[playerNum].objectPtr.direction;
                                    break;
                                case 94:
                                    scriptEng.operands[index3] = (int)PlayerSystem.playerList[playerNum].objectPtr.inkEffect;
                                    break;
                                case 95:
                                    scriptEng.operands[index3] = (int)PlayerSystem.playerList[playerNum].objectPtr.alpha;
                                    break;
                                case 96:
                                    scriptEng.operands[index3] = (int)PlayerSystem.playerList[playerNum].objectPtr.frame;
                                    break;
                                case 97:
                                    scriptEng.operands[index3] = (int)PlayerSystem.playerList[playerNum].objectPtr.animation;
                                    break;
                                case 98:
                                    scriptEng.operands[index3] = (int)PlayerSystem.playerList[playerNum].objectPtr.prevAnimation;
                                    break;
                                case 99:
                                    scriptEng.operands[index3] = PlayerSystem.playerList[playerNum].objectPtr.animationSpeed;
                                    break;
                                case 100:
                                    scriptEng.operands[index3] = PlayerSystem.playerList[playerNum].objectPtr.animationTimer;
                                    break;
                                case 101:
                                    scriptEng.operands[index3] = PlayerSystem.playerList[playerNum].objectPtr.value[0];
                                    break;
                                case 102:
                                    scriptEng.operands[index3] = PlayerSystem.playerList[playerNum].objectPtr.value[1];
                                    break;
                                case 103:
                                    scriptEng.operands[index3] = PlayerSystem.playerList[playerNum].objectPtr.value[2];
                                    break;
                                case 104:
                                    scriptEng.operands[index3] = PlayerSystem.playerList[playerNum].objectPtr.value[3];
                                    break;
                                case 105:
                                    scriptEng.operands[index3] = PlayerSystem.playerList[playerNum].objectPtr.value[4];
                                    break;
                                case 106:
                                    scriptEng.operands[index3] = PlayerSystem.playerList[playerNum].objectPtr.value[5];
                                    break;
                                case 107:
                                    scriptEng.operands[index3] = PlayerSystem.playerList[playerNum].objectPtr.value[6];
                                    break;
                                case 108:
                                    scriptEng.operands[index3] = PlayerSystem.playerList[playerNum].objectPtr.value[7];
                                    break;
                                case 109:
                                    scriptEng.operands[index3] = PlayerSystem.playerList[playerNum].value[0];
                                    break;
                                case 110:
                                    scriptEng.operands[index3] = PlayerSystem.playerList[playerNum].value[1];
                                    break;
                                case 111:
                                    scriptEng.operands[index3] = PlayerSystem.playerList[playerNum].value[2];
                                    break;
                                case 112:
                                    scriptEng.operands[index3] = PlayerSystem.playerList[playerNum].value[3];
                                    break;
                                case 113:
                                    scriptEng.operands[index3] = PlayerSystem.playerList[playerNum].value[4];
                                    break;
                                case 114:
                                    scriptEng.operands[index3] = PlayerSystem.playerList[playerNum].value[5];
                                    break;
                                case 115:
                                    scriptEng.operands[index3] = PlayerSystem.playerList[playerNum].value[6];
                                    break;
                                case 116:
                                    scriptEng.operands[index3] = PlayerSystem.playerList[playerNum].value[7];
                                    break;
                                case 117:
                                    scriptEng.sRegister = PlayerSystem.playerList[playerNum].objectPtr.xPos >> 16;
                                    if (scriptEng.sRegister > StageSystem.xScrollOffset - GlobalAppDefinitions.OBJECT_BORDER_X1 && scriptEng.sRegister < StageSystem.xScrollOffset + GlobalAppDefinitions.OBJECT_BORDER_X2)
                                    {
                                        scriptEng.sRegister = PlayerSystem.playerList[playerNum].objectPtr.yPos >> 16;
                                        scriptEng.operands[index3] = scriptEng.sRegister <= StageSystem.yScrollOffset - 256 || scriptEng.sRegister >= StageSystem.yScrollOffset + 496 ? 1 : 0;
                                        break;
                                    }
                                    scriptEng.operands[index3] = 1;
                                    break;
                                case 118:
                                    scriptEng.operands[index3] = (int)StageSystem.stageMode;
                                    break;
                                case 119:
                                    scriptEng.operands[index3] = (int)FileIO.activeStageList;
                                    break;
                                case 120:
                                    scriptEng.operands[index3] = StageSystem.stageListPosition;
                                    break;
                                case 121:
                                    scriptEng.operands[index3] = (int)StageSystem.timeEnabled;
                                    break;
                                case 122:
                                    scriptEng.operands[index3] = (int)StageSystem.milliSeconds;
                                    break;
                                case 123:
                                    scriptEng.operands[index3] = (int)StageSystem.seconds;
                                    break;
                                case 124:
                                    scriptEng.operands[index3] = (int)StageSystem.minutes;
                                    break;
                                case 125:
                                    scriptEng.operands[index3] = FileIO.actNumber;
                                    break;
                                case 126:
                                    scriptEng.operands[index3] = (int)StageSystem.pauseEnabled;
                                    break;
                                case (int)sbyte.MaxValue:
                                    switch (FileIO.activeStageList)
                                    {
                                        case 0:
                                            scriptEng.operands[index3] = (int)FileIO.noPresentationStages;
                                            break;
                                        case 1:
                                            scriptEng.operands[index3] = (int)FileIO.noZoneStages;
                                            break;
                                        case 2:
                                            scriptEng.operands[index3] = (int)FileIO.noBonusStages;
                                            break;
                                        case 3:
                                            scriptEng.operands[index3] = (int)FileIO.noSpecialStages;
                                            break;
                                    }
                                    break;
                                case 128:
                                    scriptEng.operands[index3] = StageSystem.newXBoundary1;
                                    break;
                                case 129:
                                    scriptEng.operands[index3] = StageSystem.newXBoundary2;
                                    break;
                                case 130:
                                    scriptEng.operands[index3] = StageSystem.newYBoundary1;
                                    break;
                                case 131:
                                    scriptEng.operands[index3] = StageSystem.newYBoundary2;
                                    break;
                                case 132:
                                    scriptEng.operands[index3] = StageSystem.xBoundary1;
                                    break;
                                case 133:
                                    scriptEng.operands[index3] = StageSystem.xBoundary2;
                                    break;
                                case 134:
                                    scriptEng.operands[index3] = StageSystem.yBoundary1;
                                    break;
                                case 135:
                                    scriptEng.operands[index3] = StageSystem.yBoundary2;
                                    break;
                                case 136:
                                    scriptEng.operands[index3] = StageSystem.bgDeformationData0[index1];
                                    break;
                                case 137:
                                    scriptEng.operands[index3] = StageSystem.bgDeformationData1[index1];
                                    break;
                                case 138:
                                    scriptEng.operands[index3] = StageSystem.bgDeformationData2[index1];
                                    break;
                                case 139:
                                    scriptEng.operands[index3] = StageSystem.bgDeformationData3[index1];
                                    break;
                                case 140:
                                    scriptEng.operands[index3] = StageSystem.waterLevel;
                                    break;
                                case 141:
                                    scriptEng.operands[index3] = (int)StageSystem.activeTileLayers[index1];
                                    break;
                                case 142:
                                    scriptEng.operands[index3] = (int)StageSystem.tLayerMidPoint;
                                    break;
                                case 143:
                                    scriptEng.operands[index3] = (int)PlayerSystem.playerMenuNum;
                                    break;
                                case 144:
                                    scriptEng.operands[index3] = playerNum;
                                    break;
                                case 145:
                                    scriptEng.operands[index3] = (int)StageSystem.cameraEnabled;
                                    break;
                                case 146:
                                    scriptEng.operands[index3] = (int)StageSystem.cameraTarget;
                                    break;
                                case 147:
                                    scriptEng.operands[index3] = (int)StageSystem.cameraStyle;
                                    break;
                                case 148:
                                    scriptEng.operands[index3] = objectDrawOrderList[index1].listSize;
                                    break;
                                case 149:
                                    scriptEng.operands[index3] = GlobalAppDefinitions.SCREEN_CENTER;
                                    break;
                                case 150:
                                    scriptEng.operands[index3] = 120;
                                    break;
                                case 151:
                                    scriptEng.operands[index3] = GlobalAppDefinitions.SCREEN_XSIZE;
                                    break;
                                case 152:
                                    scriptEng.operands[index3] = 240;
                                    break;
                                case 153:
                                    scriptEng.operands[index3] = StageSystem.xScrollOffset;
                                    break;
                                case 154:
                                    scriptEng.operands[index3] = StageSystem.yScrollOffset;
                                    break;
                                case 155:
                                    scriptEng.operands[index3] = StageSystem.screenShakeX;
                                    break;
                                case 156:
                                    scriptEng.operands[index3] = StageSystem.screenShakeY;
                                    break;
                                case 157:
                                    scriptEng.operands[index3] = StageSystem.cameraAdjustY;
                                    break;
                                case 158:
                                    scriptEng.operands[index3] = (int)StageSystem.gKeyDown.touchDown[index1];
                                    break;
                                case 159:
                                    scriptEng.operands[index3] = StageSystem.gKeyDown.touchX[index1];
                                    break;
                                case 160:
                                    scriptEng.operands[index3] = StageSystem.gKeyDown.touchY[index1];
                                    break;
                                case 161:
                                    scriptEng.operands[index3] = AudioPlayback.musicVolume;
                                    break;
                                case 162:
                                    scriptEng.operands[index3] = AudioPlayback.currentMusicTrack;
                                    break;
                                case 163:
                                    scriptEng.operands[index3] = (int)StageSystem.gKeyDown.up;
                                    break;
                                case 164:
                                    scriptEng.operands[index3] = (int)StageSystem.gKeyDown.down;
                                    break;
                                case 165:
                                    scriptEng.operands[index3] = (int)StageSystem.gKeyDown.left;
                                    break;
                                case 166:
                                    scriptEng.operands[index3] = (int)StageSystem.gKeyDown.right;
                                    break;
                                case 167:
                                    scriptEng.operands[index3] = (int)StageSystem.gKeyDown.buttonA;
                                    break;
                                case 168:
                                    scriptEng.operands[index3] = (int)StageSystem.gKeyDown.buttonB;
                                    break;
                                case 169:
                                    scriptEng.operands[index3] = (int)StageSystem.gKeyDown.buttonC;
                                    break;
                                case 170:
                                    scriptEng.operands[index3] = (int)StageSystem.gKeyDown.start;
                                    break;
                                case 171:
                                    scriptEng.operands[index3] = (int)StageSystem.gKeyPress.up;
                                    break;
                                case 172:
                                    scriptEng.operands[index3] = (int)StageSystem.gKeyPress.down;
                                    break;
                                case 173:
                                    scriptEng.operands[index3] = (int)StageSystem.gKeyPress.left;
                                    break;
                                case 174:
                                    scriptEng.operands[index3] = (int)StageSystem.gKeyPress.right;
                                    break;
                                case 175:
                                    scriptEng.operands[index3] = (int)StageSystem.gKeyPress.buttonA;
                                    break;
                                case 176:
                                    scriptEng.operands[index3] = (int)StageSystem.gKeyPress.buttonB;
                                    break;
                                case 177:
                                    scriptEng.operands[index3] = (int)StageSystem.gKeyPress.buttonC;
                                    break;
                                case 178:
                                    scriptEng.operands[index3] = (int)StageSystem.gKeyPress.start;
                                    break;
                                case 179:
                                    scriptEng.operands[index3] = StageSystem.gameMenu[0].selection1;
                                    break;
                                case 180:
                                    scriptEng.operands[index3] = StageSystem.gameMenu[1].selection1;
                                    break;
                                case 181:
                                    scriptEng.operands[index3] = (int)StageSystem.stageLayouts[index1].xSize;
                                    break;
                                case 182:
                                    scriptEng.operands[index3] = (int)StageSystem.stageLayouts[index1].ySize;
                                    break;
                                case 183:
                                    scriptEng.operands[index3] = (int)StageSystem.stageLayouts[index1].type;
                                    break;
                                case 184:
                                    scriptEng.operands[index3] = StageSystem.stageLayouts[index1].angle;
                                    break;
                                case 185:
                                    scriptEng.operands[index3] = StageSystem.stageLayouts[index1].xPos;
                                    break;
                                case 186:
                                    scriptEng.operands[index3] = StageSystem.stageLayouts[index1].yPos;
                                    break;
                                case 187:
                                    scriptEng.operands[index3] = StageSystem.stageLayouts[index1].zPos;
                                    break;
                                case 188:
                                    scriptEng.operands[index3] = StageSystem.stageLayouts[index1].parallaxFactor;
                                    break;
                                case 189:
                                    scriptEng.operands[index3] = StageSystem.stageLayouts[index1].scrollSpeed;
                                    break;
                                case 190:
                                    scriptEng.operands[index3] = StageSystem.stageLayouts[index1].scrollPosition;
                                    break;
                                case 191:
                                    scriptEng.operands[index3] = StageSystem.stageLayouts[index1].deformationPos;
                                    break;
                                case 192:
                                    scriptEng.operands[index3] = StageSystem.stageLayouts[index1].deformationPosW;
                                    break;
                                case 193:
                                    scriptEng.operands[index3] = StageSystem.hParallax.parallaxFactor[index1];
                                    break;
                                case 194:
                                    scriptEng.operands[index3] = StageSystem.hParallax.scrollSpeed[index1];
                                    break;
                                case 195:
                                    scriptEng.operands[index3] = StageSystem.hParallax.scrollPosition[index1];
                                    break;
                                case 196:
                                    scriptEng.operands[index3] = StageSystem.vParallax.parallaxFactor[index1];
                                    break;
                                case 197:
                                    scriptEng.operands[index3] = StageSystem.vParallax.scrollSpeed[index1];
                                    break;
                                case 198:
                                    scriptEng.operands[index3] = StageSystem.vParallax.scrollPosition[index1];
                                    break;
                                case 199:
                                    scriptEng.operands[index3] = Scene3D.numVertices;
                                    break;
                                case 200:
                                    scriptEng.operands[index3] = Scene3D.numFaces;
                                    break;
                                case 201:
                                    scriptEng.operands[index3] = Scene3D.vertexBuffer[index1].x;
                                    break;
                                case 202:
                                    scriptEng.operands[index3] = Scene3D.vertexBuffer[index1].y;
                                    break;
                                case 203:
                                    scriptEng.operands[index3] = Scene3D.vertexBuffer[index1].z;
                                    break;
                                case 204:
                                    scriptEng.operands[index3] = Scene3D.vertexBuffer[index1].u;
                                    break;
                                case 205:
                                    scriptEng.operands[index3] = Scene3D.vertexBuffer[index1].v;
                                    break;
                                case 206:
                                    scriptEng.operands[index3] = Scene3D.indexBuffer[index1].a;
                                    break;
                                case 207:
                                    scriptEng.operands[index3] = Scene3D.indexBuffer[index1].b;
                                    break;
                                case 208:
                                    scriptEng.operands[index3] = Scene3D.indexBuffer[index1].c;
                                    break;
                                case 209:
                                    scriptEng.operands[index3] = Scene3D.indexBuffer[index1].d;
                                    break;
                                case 210:
                                    scriptEng.operands[index3] = (int)Scene3D.indexBuffer[index1].flag;
                                    break;
                                case 211:
                                    scriptEng.operands[index3] = Scene3D.indexBuffer[index1].color;
                                    break;
                                case 212:
                                    scriptEng.operands[index3] = Scene3D.projectionX;
                                    break;
                                case 213:
                                    scriptEng.operands[index3] = Scene3D.projectionY;
                                    break;
                                case 214:
                                    scriptEng.operands[index3] = (int)GlobalAppDefinitions.gameMode;
                                    break;
                                case 215:
                                    scriptEng.operands[index3] = (int)StageSystem.debugMode;
                                    break;
                                case 216:
                                    scriptEng.operands[index3] = GlobalAppDefinitions.gameMessage;
                                    break;
                                case 217:
                                    scriptEng.operands[index3] = FileIO.saveRAM[index1];
                                    break;
                                case 218:
                                    scriptEng.operands[index3] = (int)GlobalAppDefinitions.gameLanguage;
                                    break;
                                case 219:
                                    scriptEng.operands[index3] = (int)objectScriptList[(int)objectEntityList[objectLoop].type].surfaceNum;
                                    break;
                                case 220:
                                    scriptEng.operands[index3] = (int)GlobalAppDefinitions.gameOnlineActive;
                                    break;
                                case 221:
                                    scriptEng.operands[index3] = GlobalAppDefinitions.frameSkipTimer;
                                    break;
                                case 222:
                                    scriptEng.operands[index3] = GlobalAppDefinitions.frameSkipSetting;
                                    break;
                                case 223:
                                    scriptEng.operands[index3] = GlobalAppDefinitions.gameSFXVolume;
                                    break;
                                case 224:
                                    scriptEng.operands[index3] = GlobalAppDefinitions.gameBGMVolume;
                                    break;
                                case 225:
                                    scriptEng.operands[index3] = GlobalAppDefinitions.gamePlatformID;
                                    break;
                                case 226:
                                    scriptEng.operands[index3] = (int)GlobalAppDefinitions.gameTrialMode;
                                    break;
                                case 227:
                                    scriptEng.operands[index3] = (int)StageSystem.gKeyPress.start;
                                    break;
                                case 228:
                                    scriptEng.operands[index3] = (int)GlobalAppDefinitions.gameHapticsEnabled;
                                    break;
                            }
                            ++scriptCodePtr;
                            num2 = num5 + 1;
                            break;
                        case 2:
                            ++scriptCodePtr;
                            scriptEng.operands[index3] = scriptData[scriptCodePtr];
                            ++scriptCodePtr;
                            num2 += 2;
                            break;
                        case 3:
                            ++scriptCodePtr;
                            int num6 = num2 + 1;
                            int index4 = 0;
                            index1 = 0;
                            scriptEng.sRegister = scriptData[scriptCodePtr];
                            scriptText[scriptEng.sRegister] = char.MinValue;
                            for (; index4 < scriptEng.sRegister; ++index4)
                            {
                                switch (index1)
                                {
                                    case 0:
                                        ++scriptCodePtr;
                                        ++num6;
                                        scriptText[index4] = (char)(scriptData[scriptCodePtr] >> 24);
                                        ++index1;
                                        break;
                                    case 1:
                                        scriptText[index4] = (char)((scriptData[scriptCodePtr] & 16777215) >> 16);
                                        ++index1;
                                        break;
                                    case 2:
                                        scriptText[index4] = (char)((scriptData[scriptCodePtr] & (int)ushort.MaxValue) >> 8);
                                        ++index1;
                                        break;
                                    case 3:
                                        scriptText[index4] = (char)(scriptData[scriptCodePtr] & (int)byte.MaxValue);
                                        index1 = 0;
                                        break;
                                }
                            }
                            if (index1 == 0)
                            {
                                scriptCodePtr += 2;
                                num2 = num6 + 2;
                                break;
                            }
                            ++scriptCodePtr;
                            num2 = num6 + 1;
                            break;
                    }
                }
                switch (index2)
                {
                    case 0:
                        flag = true;
                        break;
                    case 1:
                        scriptEng.operands[0] = scriptEng.operands[1];
                        break;
                    case 2:
                        scriptEng.operands[0] += scriptEng.operands[1];
                        break;
                    case 3:
                        scriptEng.operands[0] -= scriptEng.operands[1];
                        break;
                    case 4:
                        ++scriptEng.operands[0];
                        break;
                    case 5:
                        --scriptEng.operands[0];
                        break;
                    case 6:
                        scriptEng.operands[0] *= scriptEng.operands[1];
                        break;
                    case 7:
                        scriptEng.operands[0] /= scriptEng.operands[1];
                        break;
                    case 8:
                        scriptEng.operands[0] >>= scriptEng.operands[1];
                        break;
                    case 9:
                        scriptEng.operands[0] <<= scriptEng.operands[1];
                        break;
                    case 10:
                        scriptEng.operands[0] &= scriptEng.operands[1];
                        break;
                    case 11:
                        scriptEng.operands[0] |= scriptEng.operands[1];
                        break;
                    case 12:
                        scriptEng.operands[0] ^= scriptEng.operands[1];
                        break;
                    case 13:
                        scriptEng.operands[0] %= scriptEng.operands[1];
                        break;
                    case 14:
                        scriptEng.operands[0] = -scriptEng.operands[0];
                        break;
                    case 15:
                        scriptEng.checkResult = scriptEng.operands[0] != scriptEng.operands[1] ? 0 : 1;
                        num3 = (sbyte)0;
                        break;
                    case 16:
                        scriptEng.checkResult = scriptEng.operands[0] <= scriptEng.operands[1] ? 0 : 1;
                        num3 = (sbyte)0;
                        break;
                    case 17:
                        scriptEng.checkResult = scriptEng.operands[0] >= scriptEng.operands[1] ? 0 : 1;
                        num3 = (sbyte)0;
                        break;
                    case 18:
                        scriptEng.checkResult = scriptEng.operands[0] == scriptEng.operands[1] ? 0 : 1;
                        num3 = (sbyte)0;
                        break;
                    case 19:
                        if (scriptEng.operands[1] == scriptEng.operands[2])
                        {
                            ++jumpTableStackPos;
                            jumpTableStack[jumpTableStackPos] = scriptEng.operands[0];
                        }
                        else
                        {
                            scriptCodePtr = num1;
                            scriptCodePtr += jumpTableData[jumpTablePtr + scriptEng.operands[0]];
                            ++jumpTableStackPos;
                            jumpTableStack[jumpTableStackPos] = scriptEng.operands[0];
                        }
                        num3 = (sbyte)0;
                        break;
                    case 20:
                        if (scriptEng.operands[1] > scriptEng.operands[2])
                        {
                            ++jumpTableStackPos;
                            jumpTableStack[jumpTableStackPos] = scriptEng.operands[0];
                        }
                        else
                        {
                            scriptCodePtr = num1;
                            scriptCodePtr += jumpTableData[jumpTablePtr + scriptEng.operands[0]];
                            ++jumpTableStackPos;
                            jumpTableStack[jumpTableStackPos] = scriptEng.operands[0];
                        }
                        num3 = (sbyte)0;
                        break;
                    case 21:
                        if (scriptEng.operands[1] >= scriptEng.operands[2])
                        {
                            ++jumpTableStackPos;
                            jumpTableStack[jumpTableStackPos] = scriptEng.operands[0];
                        }
                        else
                        {
                            scriptCodePtr = num1;
                            scriptCodePtr += jumpTableData[jumpTablePtr + scriptEng.operands[0]];
                            ++jumpTableStackPos;
                            jumpTableStack[jumpTableStackPos] = scriptEng.operands[0];
                        }
                        num3 = (sbyte)0;
                        break;
                    case 22:
                        if (scriptEng.operands[1] < scriptEng.operands[2])
                        {
                            ++jumpTableStackPos;
                            jumpTableStack[jumpTableStackPos] = scriptEng.operands[0];
                        }
                        else
                        {
                            scriptCodePtr = num1;
                            scriptCodePtr += jumpTableData[jumpTablePtr + scriptEng.operands[0]];
                            ++jumpTableStackPos;
                            jumpTableStack[jumpTableStackPos] = scriptEng.operands[0];
                        }
                        num3 = (sbyte)0;
                        break;
                    case 23:
                        if (scriptEng.operands[1] <= scriptEng.operands[2])
                        {
                            ++jumpTableStackPos;
                            jumpTableStack[jumpTableStackPos] = scriptEng.operands[0];
                        }
                        else
                        {
                            scriptCodePtr = num1;
                            scriptCodePtr += jumpTableData[jumpTablePtr + scriptEng.operands[0]];
                            ++jumpTableStackPos;
                            jumpTableStack[jumpTableStackPos] = scriptEng.operands[0];
                        }
                        num3 = (sbyte)0;
                        break;
                    case 24:
                        if (scriptEng.operands[1] != scriptEng.operands[2])
                        {
                            ++jumpTableStackPos;
                            jumpTableStack[jumpTableStackPos] = scriptEng.operands[0];
                        }
                        else
                        {
                            scriptCodePtr = num1;
                            scriptCodePtr += jumpTableData[jumpTablePtr + scriptEng.operands[0]];
                            ++jumpTableStackPos;
                            jumpTableStack[jumpTableStackPos] = scriptEng.operands[0];
                        }
                        num3 = (sbyte)0;
                        break;
                    case 25:
                        num3 = (sbyte)0;
                        scriptCodePtr = num1;
                        scriptCodePtr += jumpTableData[jumpTablePtr + jumpTableStack[jumpTableStackPos] + 1];
                        --jumpTableStackPos;
                        break;
                    case 26:
                        num3 = (sbyte)0;
                        --jumpTableStackPos;
                        break;
                    case 27:
                        if (scriptEng.operands[1] == scriptEng.operands[2])
                        {
                            ++jumpTableStackPos;
                            jumpTableStack[jumpTableStackPos] = scriptEng.operands[0];
                        }
                        else
                        {
                            scriptCodePtr = num1;
                            scriptCodePtr += jumpTableData[jumpTablePtr + scriptEng.operands[0] + 1];
                        }
                        num3 = (sbyte)0;
                        break;
                    case 28:
                        if (scriptEng.operands[1] > scriptEng.operands[2])
                        {
                            ++jumpTableStackPos;
                            jumpTableStack[jumpTableStackPos] = scriptEng.operands[0];
                        }
                        else
                        {
                            scriptCodePtr = num1;
                            scriptCodePtr += jumpTableData[jumpTablePtr + scriptEng.operands[0] + 1];
                        }
                        num3 = (sbyte)0;
                        break;
                    case 29:
                        if (scriptEng.operands[1] >= scriptEng.operands[2])
                        {
                            ++jumpTableStackPos;
                            jumpTableStack[jumpTableStackPos] = scriptEng.operands[0];
                        }
                        else
                        {
                            scriptCodePtr = num1;
                            scriptCodePtr += jumpTableData[jumpTablePtr + scriptEng.operands[0] + 1];
                        }
                        num3 = (sbyte)0;
                        break;
                    case 30:
                        if (scriptEng.operands[1] < scriptEng.operands[2])
                        {
                            ++jumpTableStackPos;
                            jumpTableStack[jumpTableStackPos] = scriptEng.operands[0];
                        }
                        else
                        {
                            scriptCodePtr = num1;
                            scriptCodePtr += jumpTableData[jumpTablePtr + scriptEng.operands[0] + 1];
                        }
                        num3 = (sbyte)0;
                        break;
                    case 31:
                        if (scriptEng.operands[1] <= scriptEng.operands[2])
                        {
                            ++jumpTableStackPos;
                            jumpTableStack[jumpTableStackPos] = scriptEng.operands[0];
                        }
                        else
                        {
                            scriptCodePtr = num1;
                            scriptCodePtr += jumpTableData[jumpTablePtr + scriptEng.operands[0] + 1];
                        }
                        num3 = (sbyte)0;
                        break;
                    case 32:
                        if (scriptEng.operands[1] != scriptEng.operands[2])
                        {
                            ++jumpTableStackPos;
                            jumpTableStack[jumpTableStackPos] = scriptEng.operands[0];
                        }
                        else
                        {
                            scriptCodePtr = num1;
                            scriptCodePtr += jumpTableData[jumpTablePtr + scriptEng.operands[0] + 1];
                        }
                        num3 = (sbyte)0;
                        break;
                    case 33:
                        num3 = (sbyte)0;
                        scriptCodePtr = num1;
                        scriptCodePtr += jumpTableData[jumpTablePtr + jumpTableStack[jumpTableStackPos]];
                        --jumpTableStackPos;
                        break;
                    case 34:
                        ++jumpTableStackPos;
                        jumpTableStack[jumpTableStackPos] = scriptEng.operands[0];
                        if (scriptEng.operands[1] >= jumpTableData[jumpTablePtr + scriptEng.operands[0]] && scriptEng.operands[1] <= jumpTableData[jumpTablePtr + scriptEng.operands[0] + 1])
                        {
                            scriptCodePtr = num1;
                            scriptCodePtr += jumpTableData[jumpTablePtr + scriptEng.operands[0] + 4 + (scriptEng.operands[1] - jumpTableData[jumpTablePtr + scriptEng.operands[0]])];
                        }
                        else
                        {
                            scriptCodePtr = num1;
                            scriptCodePtr += jumpTableData[jumpTablePtr + scriptEng.operands[0] + 2];
                        }
                        num3 = (sbyte)0;
                        break;
                    case 35:
                        num3 = (sbyte)0;
                        scriptCodePtr = num1;
                        scriptCodePtr += jumpTableData[jumpTablePtr + jumpTableStack[jumpTableStackPos] + 3];
                        --jumpTableStackPos;
                        break;
                    case 36:
                        num3 = (sbyte)0;
                        --jumpTableStackPos;
                        break;
                    case 37:
                        scriptEng.operands[0] = rand.Next(0, scriptEng.operands[1]);
                        break;
                    case 38:
                        scriptEng.sRegister = scriptEng.operands[1];
                        if (scriptEng.sRegister < 0)
                            scriptEng.sRegister = 512 - scriptEng.sRegister;
                        scriptEng.sRegister &= 511;
                        scriptEng.operands[0] = GlobalAppDefinitions.SinValue512[scriptEng.sRegister];
                        break;
                    case 39:
                        scriptEng.sRegister = scriptEng.operands[1];
                        if (scriptEng.sRegister < 0)
                            scriptEng.sRegister = 512 - scriptEng.sRegister;
                        scriptEng.sRegister &= 511;
                        scriptEng.operands[0] = GlobalAppDefinitions.CosValue512[scriptEng.sRegister];
                        break;
                    case 40:
                        scriptEng.sRegister = scriptEng.operands[1];
                        if (scriptEng.sRegister < 0)
                            scriptEng.sRegister = 256 - scriptEng.sRegister;
                        scriptEng.sRegister &= (int)byte.MaxValue;
                        scriptEng.operands[0] = GlobalAppDefinitions.SinValue256[scriptEng.sRegister];
                        break;
                    case 41:
                        scriptEng.sRegister = scriptEng.operands[1];
                        if (scriptEng.sRegister < 0)
                            scriptEng.sRegister = 256 - scriptEng.sRegister;
                        scriptEng.sRegister &= (int)byte.MaxValue;
                        scriptEng.operands[0] = GlobalAppDefinitions.CosValue256[scriptEng.sRegister];
                        break;
                    case 42:
                        scriptEng.sRegister = scriptEng.operands[1];
                        if (scriptEng.sRegister < 0)
                            scriptEng.sRegister = 512 - scriptEng.sRegister;
                        scriptEng.sRegister &= 511;
                        scriptEng.operands[0] = (GlobalAppDefinitions.SinValue512[scriptEng.sRegister] >> scriptEng.operands[2]) + scriptEng.operands[3] - scriptEng.operands[4];
                        break;
                    case 43:
                        scriptEng.sRegister = scriptEng.operands[1];
                        if (scriptEng.sRegister < 0)
                            scriptEng.sRegister = 512 - scriptEng.sRegister;
                        scriptEng.sRegister &= 511;
                        scriptEng.operands[0] = (GlobalAppDefinitions.CosValue512[scriptEng.sRegister] >> scriptEng.operands[2]) + scriptEng.operands[3] - scriptEng.operands[4];
                        break;
                    case 44:
                        scriptEng.operands[0] = (int)GlobalAppDefinitions.ArcTanLookup(scriptEng.operands[1], scriptEng.operands[2]);
                        break;
                    case 45:
                        scriptEng.operands[0] = scriptEng.operands[1] * scriptEng.operands[3] + scriptEng.operands[2] * (256 - scriptEng.operands[3]) >> 8;
                        break;
                    case 46:
                        scriptEng.operands[0] = (scriptEng.operands[2] * scriptEng.operands[6] >> 8) + (scriptEng.operands[3] * (256 - scriptEng.operands[6]) >> 8);
                        scriptEng.operands[1] = (scriptEng.operands[4] * scriptEng.operands[6] >> 8) + (scriptEng.operands[5] * (256 - scriptEng.operands[6]) >> 8);
                        break;
                    case 47:
                        num3 = (sbyte)0;
                        objectScriptList[(int)objectEntityList[objectLoop].type].surfaceNum = GraphicsSystem.AddGraphicsFile(scriptText);
                        break;
                    case 48:
                        num3 = (sbyte)0;
                        GraphicsSystem.RemoveGraphicsFile(scriptText, -1);
                        break;
                    case 49:
                        num3 = (sbyte)0;
                        GraphicsSystem.DrawSprite((objectEntityList[objectLoop].xPos >> 16) - StageSystem.xScrollOffset + scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].xPivot, (objectEntityList[objectLoop].yPos >> 16) - StageSystem.yScrollOffset + scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].yPivot, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].xSize, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].ySize, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].left, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].top, (int)objectScriptList[(int)objectEntityList[objectLoop].type].surfaceNum);
                        break;
                    case 50:
                        num3 = (sbyte)0;
                        GraphicsSystem.DrawSprite((scriptEng.operands[1] >> 16) - StageSystem.xScrollOffset + scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].xPivot, (scriptEng.operands[2] >> 16) - StageSystem.yScrollOffset + scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].yPivot, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].xSize, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].ySize, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].left, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].top, (int)objectScriptList[(int)objectEntityList[objectLoop].type].surfaceNum);
                        break;
                    case 51:
                        num3 = (sbyte)0;
                        GraphicsSystem.DrawSprite(scriptEng.operands[1] + scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].xPivot, scriptEng.operands[2] + scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].yPivot, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].xSize, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].ySize, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].left, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].top, (int)objectScriptList[(int)objectEntityList[objectLoop].type].surfaceNum);
                        break;
                    case 52:
                        num3 = (sbyte)0;
                        GraphicsSystem.DrawTintRectangle(scriptEng.operands[0], scriptEng.operands[1], scriptEng.operands[2], scriptEng.operands[3]);
                        break;
                    case 53:
                        num3 = (sbyte)0;
                        scriptEng.operands[7] = 10;
                        if (scriptEng.operands[6] == 0)
                        {
                            scriptEng.operands[8] = scriptEng.operands[3] != 0 ? scriptEng.operands[3] * 10 : 10;
                            while (scriptEng.operands[4] > 0)
                            {
                                if (scriptEng.operands[8] >= scriptEng.operands[7])
                                {
                                    scriptEng.sRegister = (scriptEng.operands[3] - scriptEng.operands[3] / scriptEng.operands[7] * scriptEng.operands[7]) / (scriptEng.operands[7] / 10);
                                    scriptEng.sRegister += scriptEng.operands[0];
                                    GraphicsSystem.DrawSprite(scriptEng.operands[1] + scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.sRegister].xPivot, scriptEng.operands[2] + scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.sRegister].yPivot, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.sRegister].xSize, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.sRegister].ySize, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.sRegister].left, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.sRegister].top, (int)objectScriptList[(int)objectEntityList[objectLoop].type].surfaceNum);
                                }
                                scriptEng.operands[1] -= scriptEng.operands[5];
                                scriptEng.operands[7] *= 10;
                                --scriptEng.operands[4];
                            }
                            break;
                        }
                        while (scriptEng.operands[4] > 0)
                        {
                            scriptEng.sRegister = (scriptEng.operands[3] - scriptEng.operands[3] / scriptEng.operands[7] * scriptEng.operands[7]) / (scriptEng.operands[7] / 10);
                            scriptEng.sRegister += scriptEng.operands[0];
                            GraphicsSystem.DrawSprite(scriptEng.operands[1] + scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.sRegister].xPivot, scriptEng.operands[2] + scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.sRegister].yPivot, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.sRegister].xSize, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.sRegister].ySize, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.sRegister].left, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.sRegister].top, (int)objectScriptList[(int)objectEntityList[objectLoop].type].surfaceNum);
                            scriptEng.operands[1] -= scriptEng.operands[5];
                            scriptEng.operands[7] *= 10;
                            --scriptEng.operands[4];
                        }
                        break;
                    case 54:
                        num3 = (sbyte)0;
                        switch (scriptEng.operands[3])
                        {
                            case 1:
                                scriptEng.sRegister = 0;
                                if (scriptEng.operands[4] == 1 && StageSystem.titleCardText[scriptEng.sRegister] != char.MinValue)
                                {
                                    scriptEng.operands[7] = (int)StageSystem.titleCardText[scriptEng.sRegister];
                                    if (scriptEng.operands[7] == 32)
                                        scriptEng.operands[7] = 0;
                                    if (scriptEng.operands[7] == 45)
                                        scriptEng.operands[7] = 0;
                                    if (scriptEng.operands[7] > 47 && scriptEng.operands[7] < 58)
                                        scriptEng.operands[7] -= 22;
                                    if (scriptEng.operands[7] > 57 && scriptEng.operands[7] < 102)
                                        scriptEng.operands[7] -= 65;
                                    if (scriptEng.operands[7] > -1)
                                    {
                                        scriptEng.operands[7] += scriptEng.operands[0];
                                        GraphicsSystem.DrawSprite(scriptEng.operands[1] + scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[7]].xPivot, scriptEng.operands[2] + scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[7]].yPivot, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[7]].xSize, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[7]].ySize, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[7]].left, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[7]].top, (int)objectScriptList[(int)objectEntityList[objectLoop].type].surfaceNum);
                                        scriptEng.operands[1] += scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[7]].xSize + scriptEng.operands[6];
                                    }
                                    else
                                        scriptEng.operands[1] += scriptEng.operands[5] + scriptEng.operands[6];
                                    scriptEng.operands[0] += 26;
                                    ++scriptEng.sRegister;
                                }
                                for (; StageSystem.titleCardText[scriptEng.sRegister] != char.MinValue && StageSystem.titleCardText[scriptEng.sRegister] != '-'; ++scriptEng.sRegister)
                                {
                                    scriptEng.operands[7] = (int)StageSystem.titleCardText[scriptEng.sRegister];
                                    if (scriptEng.operands[7] == 32)
                                        scriptEng.operands[7] = 0;
                                    if (scriptEng.operands[7] == 45)
                                        scriptEng.operands[7] = 0;
                                    if (scriptEng.operands[7] > 47 && scriptEng.operands[7] < 58)
                                        scriptEng.operands[7] -= 22;
                                    if (scriptEng.operands[7] > 57 && scriptEng.operands[7] < 102)
                                        scriptEng.operands[7] -= 65;
                                    if (scriptEng.operands[7] > -1)
                                    {
                                        scriptEng.operands[7] += scriptEng.operands[0];
                                        GraphicsSystem.DrawSprite(scriptEng.operands[1] + scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[7]].xPivot, scriptEng.operands[2] + scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[7]].yPivot, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[7]].xSize, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[7]].ySize, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[7]].left, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[7]].top, (int)objectScriptList[(int)objectEntityList[objectLoop].type].surfaceNum);
                                        scriptEng.operands[1] += scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[7]].xSize + scriptEng.operands[6];
                                    }
                                    else
                                        scriptEng.operands[1] += scriptEng.operands[5] + scriptEng.operands[6];
                                }
                                break;
                            case 2:
                                scriptEng.sRegister = (int)StageSystem.titleCardWord2;
                                if (scriptEng.operands[4] == 1 && StageSystem.titleCardText[scriptEng.sRegister] != char.MinValue)
                                {
                                    scriptEng.operands[7] = (int)StageSystem.titleCardText[scriptEng.sRegister];
                                    if (scriptEng.operands[7] == 32)
                                        scriptEng.operands[7] = 0;
                                    if (scriptEng.operands[7] == 45)
                                        scriptEng.operands[7] = 0;
                                    if (scriptEng.operands[7] > 47 && scriptEng.operands[7] < 58)
                                        scriptEng.operands[7] -= 22;
                                    if (scriptEng.operands[7] > 57 && scriptEng.operands[7] < 102)
                                        scriptEng.operands[7] -= 65;
                                    if (scriptEng.operands[7] > -1)
                                    {
                                        scriptEng.operands[7] += scriptEng.operands[0];
                                        GraphicsSystem.DrawSprite(scriptEng.operands[1] + scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[7]].xPivot, scriptEng.operands[2] + scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[7]].yPivot, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[7]].xSize, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[7]].ySize, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[7]].left, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[7]].top, (int)objectScriptList[(int)objectEntityList[objectLoop].type].surfaceNum);
                                        scriptEng.operands[1] += scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[7]].xSize + scriptEng.operands[6];
                                    }
                                    else
                                        scriptEng.operands[1] += scriptEng.operands[5] + scriptEng.operands[6];
                                    scriptEng.operands[0] += 26;
                                    ++scriptEng.sRegister;
                                }
                                for (; StageSystem.titleCardText[scriptEng.sRegister] != char.MinValue; ++scriptEng.sRegister)
                                {
                                    scriptEng.operands[7] = (int)StageSystem.titleCardText[scriptEng.sRegister];
                                    if (scriptEng.operands[7] == 32)
                                        scriptEng.operands[7] = 0;
                                    if (scriptEng.operands[7] == 45)
                                        scriptEng.operands[7] = 0;
                                    if (scriptEng.operands[7] > 47 && scriptEng.operands[7] < 58)
                                        scriptEng.operands[7] -= 22;
                                    if (scriptEng.operands[7] > 57 && scriptEng.operands[7] < 102)
                                        scriptEng.operands[7] -= 65;
                                    if (scriptEng.operands[7] > -1)
                                    {
                                        scriptEng.operands[7] += scriptEng.operands[0];
                                        GraphicsSystem.DrawSprite(scriptEng.operands[1] + scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[7]].xPivot, scriptEng.operands[2] + scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[7]].yPivot, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[7]].xSize, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[7]].ySize, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[7]].left, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[7]].top, (int)objectScriptList[(int)objectEntityList[objectLoop].type].surfaceNum);
                                        scriptEng.operands[1] += scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[7]].xSize + scriptEng.operands[6];
                                    }
                                    else
                                        scriptEng.operands[1] += scriptEng.operands[5] + scriptEng.operands[6];
                                }
                                break;
                        }
                        break;
                    case 55:
                        num3 = (sbyte)0;
                        TextSystem.textMenuSurfaceNo = (int)objectScriptList[(int)objectEntityList[objectLoop].type].surfaceNum;
                        TextSystem.DrawTextMenu(StageSystem.gameMenu[scriptEng.operands[0]], scriptEng.operands[1], scriptEng.operands[2]);
                        break;
                    case 56:
                        num3 = (sbyte)0;
                        if (scriptSub == 3 && scriptFramesNo < 4096)
                        {
                            scriptFrames[scriptFramesNo].xPivot = scriptEng.operands[0];
                            scriptFrames[scriptFramesNo].yPivot = scriptEng.operands[1];
                            scriptFrames[scriptFramesNo].xSize = scriptEng.operands[2];
                            scriptFrames[scriptFramesNo].ySize = scriptEng.operands[3];
                            scriptFrames[scriptFramesNo].left = scriptEng.operands[4];
                            scriptFrames[scriptFramesNo].top = scriptEng.operands[5];
                            ++scriptFramesNo;
                            break;
                        }
                        break;
                    case 57:
                        num3 = (sbyte)0;
                        break;
                    case 58:
                        num3 = (sbyte)0;
                        GraphicsSystem.LoadPalette(scriptText, scriptEng.operands[1], scriptEng.operands[2], scriptEng.operands[3], scriptEng.operands[4]);
                        break;
                    case 59:
                        num3 = (sbyte)0;
                        GraphicsSystem.RotatePalette((byte)scriptEng.operands[0], (byte)scriptEng.operands[1], (byte)scriptEng.operands[2]);
                        break;
                    case 60:
                        num3 = (sbyte)0;
                        GraphicsSystem.SetFade((byte)scriptEng.operands[0], (byte)scriptEng.operands[1], (byte)scriptEng.operands[2], (ushort)scriptEng.operands[3]);
                        break;
                    case 61:
                        num3 = (sbyte)0;
                        GraphicsSystem.SetActivePalette((byte)scriptEng.operands[0], scriptEng.operands[1], scriptEng.operands[2]);
                        break;
                    case 62:
                        GraphicsSystem.SetLimitedFade((byte)scriptEng.operands[0], (byte)scriptEng.operands[1], (byte)scriptEng.operands[2], (byte)scriptEng.operands[3], (ushort)scriptEng.operands[4], scriptEng.operands[5], scriptEng.operands[6]);
                        break;
                    case 63:
                        num3 = (sbyte)0;
                        GraphicsSystem.CopyPalette((byte)scriptEng.operands[0], (byte)scriptEng.operands[1]);
                        break;
                    case 64:
                        num3 = (sbyte)0;
                        GraphicsSystem.ClearScreen((byte)scriptEng.operands[0]);
                        break;
                    case 65:
                        num3 = (sbyte)0;
                        switch (scriptEng.operands[1])
                        {
                            case 0:
                                GraphicsSystem.DrawScaledSprite(objectEntityList[objectLoop].direction, (scriptEng.operands[2] >> 16) - StageSystem.xScrollOffset, (scriptEng.operands[3] >> 16) - StageSystem.yScrollOffset, -scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].xPivot, -scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].yPivot, objectEntityList[objectLoop].scale, objectEntityList[objectLoop].scale, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].xSize, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].ySize, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].left, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].top, (int)objectScriptList[(int)objectEntityList[objectLoop].type].surfaceNum);
                                break;
                            case 1:
                                GraphicsSystem.DrawRotatedSprite(objectEntityList[objectLoop].direction, (scriptEng.operands[2] >> 16) - StageSystem.xScrollOffset, (scriptEng.operands[3] >> 16) - StageSystem.yScrollOffset, -scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].xPivot, -scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].yPivot, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].left, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].top, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].xSize, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].ySize, objectEntityList[objectLoop].rotation, (int)objectScriptList[(int)objectEntityList[objectLoop].type].surfaceNum);
                                break;
                            case 2:
                                GraphicsSystem.DrawRotoZoomSprite(objectEntityList[objectLoop].direction, (scriptEng.operands[2] >> 16) - StageSystem.xScrollOffset, (scriptEng.operands[3] >> 16) - StageSystem.yScrollOffset, -scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].xPivot, -scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].yPivot, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].left, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].top, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].xSize, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].ySize, objectEntityList[objectLoop].rotation, objectEntityList[objectLoop].scale, (int)objectScriptList[(int)objectEntityList[objectLoop].type].surfaceNum);
                                break;
                            case 3:
                                switch (objectEntityList[objectLoop].inkEffect)
                                {
                                    case 0:
                                        GraphicsSystem.DrawSprite((scriptEng.operands[2] >> 16) - StageSystem.xScrollOffset + scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].xPivot, (scriptEng.operands[3] >> 16) - StageSystem.yScrollOffset + scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].yPivot, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].xSize, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].ySize, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].left, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].top, (int)objectScriptList[(int)objectEntityList[objectLoop].type].surfaceNum);
                                        break;
                                    case 1:
                                        GraphicsSystem.DrawBlendedSprite((scriptEng.operands[2] >> 16) - StageSystem.xScrollOffset + scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].xPivot, (scriptEng.operands[3] >> 16) - StageSystem.yScrollOffset + scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].yPivot, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].xSize, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].ySize, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].left, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].top, (int)objectScriptList[(int)objectEntityList[objectLoop].type].surfaceNum);
                                        break;
                                    case 2:
                                        GraphicsSystem.DrawAlphaBlendedSprite((scriptEng.operands[2] >> 16) - StageSystem.xScrollOffset + scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].xPivot, (scriptEng.operands[3] >> 16) - StageSystem.yScrollOffset + scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].yPivot, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].xSize, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].ySize, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].left, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].top, (int)objectEntityList[objectLoop].alpha, (int)objectScriptList[(int)objectEntityList[objectLoop].type].surfaceNum);
                                        break;
                                    case 3:
                                        GraphicsSystem.DrawAdditiveBlendedSprite((scriptEng.operands[2] >> 16) - StageSystem.xScrollOffset + scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].xPivot, (scriptEng.operands[3] >> 16) - StageSystem.yScrollOffset + scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].yPivot, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].xSize, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].ySize, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].left, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].top, (int)objectEntityList[objectLoop].alpha, (int)objectScriptList[(int)objectEntityList[objectLoop].type].surfaceNum);
                                        break;
                                    case 4:
                                        GraphicsSystem.DrawSubtractiveBlendedSprite((scriptEng.operands[2] >> 16) - StageSystem.xScrollOffset + scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].xPivot, (scriptEng.operands[3] >> 16) - StageSystem.yScrollOffset + scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].yPivot, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].xSize, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].ySize, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].left, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].top, (int)objectEntityList[objectLoop].alpha, (int)objectScriptList[(int)objectEntityList[objectLoop].type].surfaceNum);
                                        break;
                                }
                                break;
                            case 4:
                                if (objectEntityList[objectLoop].inkEffect != (byte)2)
                                {
                                    GraphicsSystem.DrawScaledSprite(objectEntityList[objectLoop].direction, (scriptEng.operands[2] >> 16) - StageSystem.xScrollOffset, (scriptEng.operands[3] >> 16) - StageSystem.yScrollOffset, -scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].xPivot, -scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].yPivot, objectEntityList[objectLoop].scale, objectEntityList[objectLoop].scale, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].xSize, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].ySize, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].left, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].top, (int)objectScriptList[(int)objectEntityList[objectLoop].type].surfaceNum);
                                    break;
                                }
                                GraphicsSystem.DrawScaledTintMask(objectEntityList[objectLoop].direction, (scriptEng.operands[2] >> 16) - StageSystem.xScrollOffset, (scriptEng.operands[3] >> 16) - StageSystem.yScrollOffset, -scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].xPivot, -scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].yPivot, objectEntityList[objectLoop].scale, objectEntityList[objectLoop].scale, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].xSize, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].ySize, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].left, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].top, (int)objectScriptList[(int)objectEntityList[objectLoop].type].surfaceNum);
                                break;
                            case 5:
                                switch (objectEntityList[objectLoop].direction)
                                {
                                    case 0:
                                        GraphicsSystem.DrawSpriteFlipped((scriptEng.operands[2] >> 16) - StageSystem.xScrollOffset + scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].xPivot, (scriptEng.operands[3] >> 16) - StageSystem.yScrollOffset + scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].yPivot, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].xSize, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].ySize, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].left, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].top, (int)objectEntityList[objectLoop].direction, (int)objectScriptList[(int)objectEntityList[objectLoop].type].surfaceNum);
                                        break;
                                    case 1:
                                        GraphicsSystem.DrawSpriteFlipped((scriptEng.operands[2] >> 16) - StageSystem.xScrollOffset - scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].xSize - scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].xPivot, (scriptEng.operands[3] >> 16) - StageSystem.yScrollOffset + scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].yPivot, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].xSize, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].ySize, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].left, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].top, (int)objectEntityList[objectLoop].direction, (int)objectScriptList[(int)objectEntityList[objectLoop].type].surfaceNum);
                                        break;
                                    case 2:
                                        GraphicsSystem.DrawSpriteFlipped((scriptEng.operands[2] >> 16) - StageSystem.xScrollOffset + scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].xPivot, (scriptEng.operands[3] >> 16) - StageSystem.yScrollOffset - scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].ySize - scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].yPivot, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].xSize, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].ySize, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].left, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].top, (int)objectEntityList[objectLoop].direction, (int)objectScriptList[(int)objectEntityList[objectLoop].type].surfaceNum);
                                        break;
                                    case 3:
                                        GraphicsSystem.DrawSpriteFlipped((scriptEng.operands[2] >> 16) - StageSystem.xScrollOffset - scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].xSize - scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].xPivot, (scriptEng.operands[3] >> 16) - StageSystem.yScrollOffset - scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].ySize - scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].yPivot, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].xSize, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].ySize, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].left, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].top, (int)objectEntityList[objectLoop].direction, (int)objectScriptList[(int)objectEntityList[objectLoop].type].surfaceNum);
                                        break;
                                }
                                break;
                        }
                        break;
                    case 66:
                        num3 = (sbyte)0;
                        switch (scriptEng.operands[1])
                        {
                            case 0:
                                GraphicsSystem.DrawScaledSprite(objectEntityList[objectLoop].direction, scriptEng.operands[2], scriptEng.operands[3], -scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].xPivot, -scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].yPivot, objectEntityList[objectLoop].scale, objectEntityList[objectLoop].scale, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].xSize, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].ySize, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].left, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].top, (int)objectScriptList[(int)objectEntityList[objectLoop].type].surfaceNum);
                                break;
                            case 1:
                                GraphicsSystem.DrawRotatedSprite(objectEntityList[objectLoop].direction, scriptEng.operands[2], scriptEng.operands[3], -scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].xPivot, -scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].yPivot, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].left, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].top, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].xSize, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].ySize, objectEntityList[objectLoop].rotation, (int)objectScriptList[(int)objectEntityList[objectLoop].type].surfaceNum);
                                break;
                            case 2:
                                GraphicsSystem.DrawRotoZoomSprite(objectEntityList[objectLoop].direction, scriptEng.operands[2], scriptEng.operands[3], -scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].xPivot, -scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].yPivot, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].left, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].top, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].xSize, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].ySize, objectEntityList[objectLoop].rotation, objectEntityList[objectLoop].scale, (int)objectScriptList[(int)objectEntityList[objectLoop].type].surfaceNum);
                                break;
                            case 3:
                                switch (objectEntityList[objectLoop].inkEffect)
                                {
                                    case 0:
                                        GraphicsSystem.DrawSprite(scriptEng.operands[2] + scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].xPivot, scriptEng.operands[3] + scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].yPivot, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].xSize, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].ySize, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].left, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].top, (int)objectScriptList[(int)objectEntityList[objectLoop].type].surfaceNum);
                                        break;
                                    case 1:
                                        GraphicsSystem.DrawBlendedSprite(scriptEng.operands[2] + scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].xPivot, scriptEng.operands[3] + scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].yPivot, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].xSize, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].ySize, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].left, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].top, (int)objectScriptList[(int)objectEntityList[objectLoop].type].surfaceNum);
                                        break;
                                    case 2:
                                        GraphicsSystem.DrawAlphaBlendedSprite(scriptEng.operands[2] + scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].xPivot, scriptEng.operands[3] + scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].yPivot, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].xSize, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].ySize, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].left, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].top, (int)objectEntityList[objectLoop].alpha, (int)objectScriptList[(int)objectEntityList[objectLoop].type].surfaceNum);
                                        break;
                                    case 3:
                                        GraphicsSystem.DrawAdditiveBlendedSprite(scriptEng.operands[2] + scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].xPivot, scriptEng.operands[3] + scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].yPivot, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].xSize, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].ySize, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].left, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].top, (int)objectEntityList[objectLoop].alpha, (int)objectScriptList[(int)objectEntityList[objectLoop].type].surfaceNum);
                                        break;
                                    case 4:
                                        GraphicsSystem.DrawSubtractiveBlendedSprite(scriptEng.operands[2] + scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].xPivot, scriptEng.operands[3] + scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].yPivot, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].xSize, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].ySize, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].left, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].top, (int)objectEntityList[objectLoop].alpha, (int)objectScriptList[(int)objectEntityList[objectLoop].type].surfaceNum);
                                        break;
                                }
                                break;
                            case 4:
                                if (objectEntityList[objectLoop].inkEffect != (byte)2)
                                {
                                    GraphicsSystem.DrawScaledSprite(objectEntityList[objectLoop].direction, scriptEng.operands[2], scriptEng.operands[3], -scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].xPivot, -scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].yPivot, objectEntityList[objectLoop].scale, objectEntityList[objectLoop].scale, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].xSize, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].ySize, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].left, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].top, (int)objectScriptList[(int)objectEntityList[objectLoop].type].surfaceNum);
                                    break;
                                }
                                GraphicsSystem.DrawScaledTintMask(objectEntityList[objectLoop].direction, scriptEng.operands[2], scriptEng.operands[3], -scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].xPivot, -scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].yPivot, objectEntityList[objectLoop].scale, objectEntityList[objectLoop].scale, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].xSize, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].ySize, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].left, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].top, (int)objectScriptList[(int)objectEntityList[objectLoop].type].surfaceNum);
                                break;
                            case 5:
                                switch (objectEntityList[objectLoop].direction)
                                {
                                    case 0:
                                        GraphicsSystem.DrawSpriteFlipped(scriptEng.operands[2] + scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].xPivot, scriptEng.operands[3] + scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].yPivot, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].xSize, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].ySize, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].left, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].top, (int)objectEntityList[objectLoop].direction, (int)objectScriptList[(int)objectEntityList[objectLoop].type].surfaceNum);
                                        break;
                                    case 1:
                                        GraphicsSystem.DrawSpriteFlipped(scriptEng.operands[2] - scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].xSize - scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].xPivot, scriptEng.operands[3] + scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].yPivot, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].xSize, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].ySize, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].left, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].top, (int)objectEntityList[objectLoop].direction, (int)objectScriptList[(int)objectEntityList[objectLoop].type].surfaceNum);
                                        break;
                                    case 2:
                                        GraphicsSystem.DrawSpriteFlipped(scriptEng.operands[2] + scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].xPivot, scriptEng.operands[3] - scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].ySize - scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].yPivot, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].xSize, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].ySize, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].left, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].top, (int)objectEntityList[objectLoop].direction, (int)objectScriptList[(int)objectEntityList[objectLoop].type].surfaceNum);
                                        break;
                                    case 3:
                                        GraphicsSystem.DrawSpriteFlipped(scriptEng.operands[2] - scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].xSize - scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].xPivot, scriptEng.operands[3] - scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].ySize - scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].yPivot, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].xSize, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].ySize, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].left, scriptFrames[objectScriptList[(int)objectEntityList[objectLoop].type].frameListOffset + scriptEng.operands[0]].top, (int)objectEntityList[objectLoop].direction, (int)objectScriptList[(int)objectEntityList[objectLoop].type].surfaceNum);
                                        break;
                                }
                                break;
                        }
                        break;
                    case 67:
                        num3 = (sbyte)0;
                        objectScriptList[(int)objectEntityList[objectLoop].type].animationFile = AnimationSystem.AddAnimationFile(scriptText);
                        break;
                    case 68:
                        num3 = (sbyte)0;
                        TextSystem.SetupTextMenu(StageSystem.gameMenu[scriptEng.operands[0]], scriptEng.operands[1]);
                        StageSystem.gameMenu[scriptEng.operands[0]].numSelections = (byte)scriptEng.operands[2];
                        StageSystem.gameMenu[scriptEng.operands[0]].alignment = (byte)scriptEng.operands[3];
                        break;
                    case 69:
                        num3 = (sbyte)0;
                        StageSystem.gameMenu[scriptEng.operands[0]].entryHighlight[(int)StageSystem.gameMenu[scriptEng.operands[0]].numRows] = (byte)scriptEng.operands[2];
                        TextSystem.AddTextMenuEntry(StageSystem.gameMenu[scriptEng.operands[0]], scriptText);
                        break;
                    case 70:
                        num3 = (sbyte)0;
                        TextSystem.EditTextMenuEntry(StageSystem.gameMenu[scriptEng.operands[0]], scriptText, scriptEng.operands[2]);
                        StageSystem.gameMenu[scriptEng.operands[0]].entryHighlight[scriptEng.operands[2]] = (byte)scriptEng.operands[3];
                        break;
                    case 71:
                        num3 = (sbyte)0;
                        StageSystem.stageMode = (byte)0;
                        break;
                    case 72:
                        num3 = (sbyte)0;
                        GraphicsSystem.DrawRectangle(scriptEng.operands[0], scriptEng.operands[1], scriptEng.operands[2], scriptEng.operands[3], scriptEng.operands[4], scriptEng.operands[5], scriptEng.operands[6], scriptEng.operands[7]);
                        break;
                    case 73:
                        num3 = (sbyte)0;
                        objectEntityList[scriptEng.operands[0]].type = (byte)scriptEng.operands[1];
                        objectEntityList[scriptEng.operands[0]].propertyValue = (byte)scriptEng.operands[2];
                        objectEntityList[scriptEng.operands[0]].xPos = scriptEng.operands[3];
                        objectEntityList[scriptEng.operands[0]].yPos = scriptEng.operands[4];
                        objectEntityList[scriptEng.operands[0]].direction = (byte)0;
                        objectEntityList[scriptEng.operands[0]].frame = (byte)0;
                        objectEntityList[scriptEng.operands[0]].priority = (byte)0;
                        objectEntityList[scriptEng.operands[0]].rotation = 0;
                        objectEntityList[scriptEng.operands[0]].state = (byte)0;
                        objectEntityList[scriptEng.operands[0]].drawOrder = (byte)3;
                        objectEntityList[scriptEng.operands[0]].scale = 512;
                        objectEntityList[scriptEng.operands[0]].inkEffect = (byte)0;
                        objectEntityList[scriptEng.operands[0]].value[0] = 0;
                        objectEntityList[scriptEng.operands[0]].value[1] = 0;
                        objectEntityList[scriptEng.operands[0]].value[2] = 0;
                        objectEntityList[scriptEng.operands[0]].value[3] = 0;
                        objectEntityList[scriptEng.operands[0]].value[4] = 0;
                        objectEntityList[scriptEng.operands[0]].value[5] = 0;
                        objectEntityList[scriptEng.operands[0]].value[6] = 0;
                        objectEntityList[scriptEng.operands[0]].value[7] = 0;
                        break;
                    case 74:
                        num3 = (sbyte)0;
                        switch (scriptEng.operands[0])
                        {
                            case 0:
                                scriptEng.operands[5] = objectEntityList[objectLoop].xPos >> 16;
                                scriptEng.operands[6] = objectEntityList[objectLoop].yPos >> 16;
                                BasicCollision(scriptEng.operands[1] + scriptEng.operands[5], scriptEng.operands[2] + scriptEng.operands[6], scriptEng.operands[3] + scriptEng.operands[5], scriptEng.operands[4] + scriptEng.operands[6]);
                                break;
                            case 1:
                            case 2:
                                BoxCollision((scriptEng.operands[1] << 16) + objectEntityList[objectLoop].xPos, (scriptEng.operands[2] << 16) + objectEntityList[objectLoop].yPos, (scriptEng.operands[3] << 16) + objectEntityList[objectLoop].xPos, (scriptEng.operands[4] << 16) + objectEntityList[objectLoop].yPos);
                                break;
                            case 3:
                                PlatformCollision((scriptEng.operands[1] << 16) + objectEntityList[objectLoop].xPos, (scriptEng.operands[2] << 16) + objectEntityList[objectLoop].yPos, (scriptEng.operands[3] << 16) + objectEntityList[objectLoop].xPos, (scriptEng.operands[4] << 16) + objectEntityList[objectLoop].yPos);
                                break;
                        }
                        break;
                    case 75:
                        num3 = (sbyte)0;
                        if (objectEntityList[scriptEng.arrayPosition[2]].type > (byte)0)
                        {
                            ++scriptEng.arrayPosition[2];
                            if (scriptEng.arrayPosition[2] == 1184)
                                scriptEng.arrayPosition[2] = 1056;
                        }
                        objectEntityList[scriptEng.arrayPosition[2]].type = (byte)scriptEng.operands[0];
                        objectEntityList[scriptEng.arrayPosition[2]].propertyValue = (byte)scriptEng.operands[1];
                        objectEntityList[scriptEng.arrayPosition[2]].xPos = scriptEng.operands[2];
                        objectEntityList[scriptEng.arrayPosition[2]].yPos = scriptEng.operands[3];
                        objectEntityList[scriptEng.arrayPosition[2]].direction = (byte)0;
                        objectEntityList[scriptEng.arrayPosition[2]].frame = (byte)0;
                        objectEntityList[scriptEng.arrayPosition[2]].priority = (byte)1;
                        objectEntityList[scriptEng.arrayPosition[2]].rotation = 0;
                        objectEntityList[scriptEng.arrayPosition[2]].state = (byte)0;
                        objectEntityList[scriptEng.arrayPosition[2]].drawOrder = (byte)3;
                        objectEntityList[scriptEng.arrayPosition[2]].scale = 512;
                        objectEntityList[scriptEng.arrayPosition[2]].inkEffect = (byte)0;
                        objectEntityList[scriptEng.arrayPosition[2]].alpha = (byte)0;
                        objectEntityList[scriptEng.arrayPosition[2]].animation = (byte)0;
                        objectEntityList[scriptEng.arrayPosition[2]].prevAnimation = (byte)0;
                        objectEntityList[scriptEng.arrayPosition[2]].animationSpeed = 0;
                        objectEntityList[scriptEng.arrayPosition[2]].animationTimer = 0;
                        objectEntityList[scriptEng.arrayPosition[2]].value[0] = 0;
                        objectEntityList[scriptEng.arrayPosition[2]].value[1] = 0;
                        objectEntityList[scriptEng.arrayPosition[2]].value[2] = 0;
                        objectEntityList[scriptEng.arrayPosition[2]].value[3] = 0;
                        objectEntityList[scriptEng.arrayPosition[2]].value[4] = 0;
                        objectEntityList[scriptEng.arrayPosition[2]].value[5] = 0;
                        objectEntityList[scriptEng.arrayPosition[2]].value[6] = 0;
                        objectEntityList[scriptEng.arrayPosition[2]].value[7] = 0;
                        break;
                    case 76:
                        num3 = (sbyte)0;
                        PlayerSystem.playerList[scriptEng.operands[0]].animationFile = objectScriptList[(int)objectEntityList[scriptEng.operands[1]].type].animationFile;
                        PlayerSystem.playerList[scriptEng.operands[0]].objectPtr = objectEntityList[scriptEng.operands[1]];
                        PlayerSystem.playerList[scriptEng.operands[0]].objectNum = scriptEng.operands[1];
                        break;
                    case 77:
                        num3 = (sbyte)0;
                        if (PlayerSystem.playerList[playerNum].tileCollisions == (byte)1)
                        {
                            PlayerSystem.ProcessPlayerTileCollisions(PlayerSystem.playerList[playerNum]);
                            break;
                        }
                        PlayerSystem.playerList[playerNum].xPos += PlayerSystem.playerList[playerNum].xVelocity;
                        PlayerSystem.playerList[playerNum].yPos += PlayerSystem.playerList[playerNum].yVelocity;
                        break;
                    case 78:
                        num3 = (sbyte)0;
                        PlayerSystem.ProcessPlayerControl(PlayerSystem.playerList[playerNum]);
                        break;
                    case 79:
                        AnimationSystem.ProcessObjectAnimation(AnimationSystem.animationList[objectScriptList[(int)objectEntityList[objectLoop].type].animationFile.aniListOffset + (int)objectEntityList[objectLoop].animation], objectEntityList[objectLoop]);
                        num3 = (sbyte)0;
                        break;
                    case 80:
                        num3 = (sbyte)0;
                        AnimationSystem.DrawObjectAnimation(AnimationSystem.animationList[objectScriptList[(int)objectEntityList[objectLoop].type].animationFile.aniListOffset + (int)objectEntityList[objectLoop].animation], objectEntityList[objectLoop], (objectEntityList[objectLoop].xPos >> 16) - StageSystem.xScrollOffset, (objectEntityList[objectLoop].yPos >> 16) - StageSystem.yScrollOffset);
                        break;
                    case 81:
                        num3 = (sbyte)0;
                        if (PlayerSystem.playerList[playerNum].visible == (byte)1)
                        {
                            if ((int)StageSystem.cameraEnabled == playerNum)
                            {
                                AnimationSystem.DrawObjectAnimation(AnimationSystem.animationList[objectScriptList[(int)objectEntityList[objectLoop].type].animationFile.aniListOffset + (int)objectEntityList[objectLoop].animation], objectEntityList[objectLoop], PlayerSystem.playerList[playerNum].screenXPos, PlayerSystem.playerList[playerNum].screenYPos);
                                break;
                            }
                            AnimationSystem.DrawObjectAnimation(AnimationSystem.animationList[objectScriptList[(int)objectEntityList[objectLoop].type].animationFile.aniListOffset + (int)objectEntityList[objectLoop].animation], objectEntityList[objectLoop], (PlayerSystem.playerList[playerNum].xPos >> 16) - StageSystem.xScrollOffset, (PlayerSystem.playerList[playerNum].yPos >> 16) - StageSystem.yScrollOffset);
                            break;
                        }
                        break;
                    case 82:
                        num3 = (sbyte)0;
                        if (scriptEng.operands[2] > 1)
                        {
                            AudioPlayback.SetMusicTrack(scriptText, scriptEng.operands[1], (byte)1, (uint)scriptEng.operands[2]);
                            break;
                        }
                        AudioPlayback.SetMusicTrack(scriptText, scriptEng.operands[1], (byte)scriptEng.operands[2], 0U);
                        break;
                    case 83:
                        num3 = (sbyte)0;
                        AudioPlayback.PlayMusic(scriptEng.operands[0]);
                        break;
                    case 84:
                        num3 = (sbyte)0;
                        AudioPlayback.StopMusic();
                        break;
                    case 85:
                        num3 = (sbyte)0;
                        AudioPlayback.PlaySfx(scriptEng.operands[0], (byte)scriptEng.operands[1]);
                        break;
                    case 86:
                        num3 = (sbyte)0;
                        AudioPlayback.StopSfx(scriptEng.operands[0]);
                        break;
                    case 87:
                        num3 = (sbyte)0;
                        AudioPlayback.SetSfxAttributes(scriptEng.operands[0], scriptEng.operands[1], scriptEng.operands[2]);
                        break;
                    case 88:
                        num3 = (sbyte)0;
                        switch (scriptEng.operands[0])
                        {
                            case 0:
                                ObjectFloorCollision(scriptEng.operands[1], scriptEng.operands[2], scriptEng.operands[3]);
                                break;
                            case 1:
                                ObjectLWallCollision(scriptEng.operands[1], scriptEng.operands[2], scriptEng.operands[3]);
                                break;
                            case 2:
                                ObjectRWallCollision(scriptEng.operands[1], scriptEng.operands[2], scriptEng.operands[3]);
                                break;
                            case 3:
                                ObjectRoofCollision(scriptEng.operands[1], scriptEng.operands[2], scriptEng.operands[3]);
                                break;
                        }
                        break;
                    case 89:
                        num3 = (sbyte)0;
                        switch (scriptEng.operands[0])
                        {
                            case 0:
                                ObjectFloorGrip(scriptEng.operands[1], scriptEng.operands[2], scriptEng.operands[3]);
                                break;
                            case 1:
                                ObjectLWallGrip(scriptEng.operands[1], scriptEng.operands[2], scriptEng.operands[3]);
                                break;
                            case 2:
                                ObjectRWallGrip(scriptEng.operands[1], scriptEng.operands[2], scriptEng.operands[3]);
                                break;
                            case 3:
                                ObjectRoofGrip(scriptEng.operands[1], scriptEng.operands[2], scriptEng.operands[3]);
                                break;
                        }
                        break;
                    case 90:
                        num3 = (sbyte)0;
                        AudioPlayback.PauseSound();
                        EngineCallbacks.PlayVideoFile(scriptText);
                        AudioPlayback.ResumeSound();
                        break;
                    case 91:
                        num3 = (sbyte)0;
                        break;
                    case 92:
                        num3 = (sbyte)0;
                        AudioPlayback.PlaySfx(scriptEng.operands[0] + AudioPlayback.numGlobalSFX, (byte)scriptEng.operands[1]);
                        break;
                    case 93:
                        num3 = (sbyte)0;
                        AudioPlayback.StopSfx(scriptEng.operands[0] + AudioPlayback.numGlobalSFX);
                        break;
                    case 94:
                        scriptEng.operands[0] = ~scriptEng.operands[0];
                        break;
                    case 95:
                        num3 = (sbyte)0;
                        Scene3D.TransformVertexBuffer();
                        Scene3D.Sort3DDrawList();
                        Scene3D.Draw3DScene((int)objectScriptList[(int)objectEntityList[objectLoop].type].surfaceNum);
                        break;
                    case 96:
                        num3 = (sbyte)0;
                        switch (scriptEng.operands[0])
                        {
                            case 0:
                                Scene3D.SetIdentityMatrix(ref Scene3D.matWorld);
                                break;
                            case 1:
                                Scene3D.SetIdentityMatrix(ref Scene3D.matView);
                                break;
                            case 2:
                                Scene3D.SetIdentityMatrix(ref Scene3D.matTemp);
                                break;
                        }
                        break;
                    case 97:
                        num3 = (sbyte)0;
                        switch (scriptEng.operands[0])
                        {
                            case 0:
                                switch (scriptEng.operands[1])
                                {
                                    case 0:
                                        Scene3D.MatrixMultiply(ref Scene3D.matWorld, ref Scene3D.matWorld);
                                        break;
                                    case 1:
                                        Scene3D.MatrixMultiply(ref Scene3D.matWorld, ref Scene3D.matView);
                                        break;
                                    case 2:
                                        Scene3D.MatrixMultiply(ref Scene3D.matWorld, ref Scene3D.matTemp);
                                        break;
                                }
                                break;
                            case 1:
                                switch (scriptEng.operands[1])
                                {
                                    case 0:
                                        Scene3D.MatrixMultiply(ref Scene3D.matView, ref Scene3D.matWorld);
                                        break;
                                    case 1:
                                        Scene3D.MatrixMultiply(ref Scene3D.matView, ref Scene3D.matView);
                                        break;
                                    case 2:
                                        Scene3D.MatrixMultiply(ref Scene3D.matView, ref Scene3D.matTemp);
                                        break;
                                }
                                break;
                            case 2:
                                switch (scriptEng.operands[1])
                                {
                                    case 0:
                                        Scene3D.MatrixMultiply(ref Scene3D.matTemp, ref Scene3D.matWorld);
                                        break;
                                    case 1:
                                        Scene3D.MatrixMultiply(ref Scene3D.matTemp, ref Scene3D.matView);
                                        break;
                                    case 2:
                                        Scene3D.MatrixMultiply(ref Scene3D.matTemp, ref Scene3D.matTemp);
                                        break;
                                }
                                break;
                        }
                        break;
                    case 98:
                        num3 = (sbyte)0;
                        switch (scriptEng.operands[0])
                        {
                            case 0:
                                Scene3D.MatrixTranslateXYZ(ref Scene3D.matWorld, scriptEng.operands[1], scriptEng.operands[2], scriptEng.operands[3]);
                                break;
                            case 1:
                                Scene3D.MatrixTranslateXYZ(ref Scene3D.matView, scriptEng.operands[1], scriptEng.operands[2], scriptEng.operands[3]);
                                break;
                            case 2:
                                Scene3D.MatrixTranslateXYZ(ref Scene3D.matTemp, scriptEng.operands[1], scriptEng.operands[2], scriptEng.operands[3]);
                                break;
                        }
                        break;
                    case 99:
                        num3 = (sbyte)0;
                        switch (scriptEng.operands[0])
                        {
                            case 0:
                                Scene3D.MatrixScaleXYZ(ref Scene3D.matWorld, scriptEng.operands[1], scriptEng.operands[2], scriptEng.operands[3]);
                                break;
                            case 1:
                                Scene3D.MatrixScaleXYZ(ref Scene3D.matView, scriptEng.operands[1], scriptEng.operands[2], scriptEng.operands[3]);
                                break;
                            case 2:
                                Scene3D.MatrixScaleXYZ(ref Scene3D.matTemp, scriptEng.operands[1], scriptEng.operands[2], scriptEng.operands[3]);
                                break;
                        }
                        break;
                    case 100:
                        num3 = (sbyte)0;
                        switch (scriptEng.operands[0])
                        {
                            case 0:
                                Scene3D.MatrixRotateX(ref Scene3D.matWorld, scriptEng.operands[1]);
                                break;
                            case 1:
                                Scene3D.MatrixRotateX(ref Scene3D.matView, scriptEng.operands[1]);
                                break;
                            case 2:
                                Scene3D.MatrixRotateX(ref Scene3D.matTemp, scriptEng.operands[1]);
                                break;
                        }
                        break;
                    case 101:
                        num3 = (sbyte)0;
                        switch (scriptEng.operands[0])
                        {
                            case 0:
                                Scene3D.MatrixRotateY(ref Scene3D.matWorld, scriptEng.operands[1]);
                                break;
                            case 1:
                                Scene3D.MatrixRotateY(ref Scene3D.matView, scriptEng.operands[1]);
                                break;
                            case 2:
                                Scene3D.MatrixRotateY(ref Scene3D.matTemp, scriptEng.operands[1]);
                                break;
                        }
                        break;
                    case 102:
                        num3 = (sbyte)0;
                        switch (scriptEng.operands[0])
                        {
                            case 0:
                                Scene3D.MatrixRotateZ(ref Scene3D.matWorld, scriptEng.operands[1]);
                                break;
                            case 1:
                                Scene3D.MatrixRotateZ(ref Scene3D.matView, scriptEng.operands[1]);
                                break;
                            case 2:
                                Scene3D.MatrixRotateZ(ref Scene3D.matTemp, scriptEng.operands[1]);
                                break;
                        }
                        break;
                    case 103:
                        num3 = (sbyte)0;
                        switch (scriptEng.operands[0])
                        {
                            case 0:
                                Scene3D.MatrixRotateXYZ(ref Scene3D.matWorld, scriptEng.operands[1], scriptEng.operands[2], scriptEng.operands[3]);
                                break;
                            case 1:
                                Scene3D.MatrixRotateXYZ(ref Scene3D.matView, scriptEng.operands[1], scriptEng.operands[2], scriptEng.operands[3]);
                                break;
                            case 2:
                                Scene3D.MatrixRotateXYZ(ref Scene3D.matTemp, scriptEng.operands[1], scriptEng.operands[2], scriptEng.operands[3]);
                                break;
                        }
                        break;
                    case 104:
                        num3 = (sbyte)0;
                        switch (scriptEng.operands[0])
                        {
                            case 0:
                                Scene3D.TransformVertices(ref Scene3D.matWorld, scriptEng.operands[1], scriptEng.operands[2]);
                                break;
                            case 1:
                                Scene3D.TransformVertices(ref Scene3D.matView, scriptEng.operands[1], scriptEng.operands[2]);
                                break;
                            case 2:
                                Scene3D.TransformVertices(ref Scene3D.matTemp, scriptEng.operands[1], scriptEng.operands[2]);
                                break;
                        }
                        break;
                    case 105:
                        num3 = (sbyte)0;
                        functionStack[functionStackPos] = scriptCodePtr;
                        ++functionStackPos;
                        functionStack[functionStackPos] = jumpTablePtr;
                        ++functionStackPos;
                        functionStack[functionStackPos] = num1;
                        ++functionStackPos;
                        scriptCodePtr = functionScriptList[scriptEng.operands[0]].mainScript;
                        num1 = scriptCodePtr;
                        jumpTablePtr = functionScriptList[scriptEng.operands[0]].mainJumpTable;
                        break;
                    case 106:
                        num3 = (sbyte)0;
                        --functionStackPos;
                        num1 = functionStack[functionStackPos];
                        --functionStackPos;
                        jumpTablePtr = functionStack[functionStackPos];
                        --functionStackPos;
                        scriptCodePtr = functionStack[functionStackPos];
                        break;
                    case 107:
                        num3 = (sbyte)0;
                        StageSystem.SetLayerDeformation(scriptEng.operands[0], scriptEng.operands[1], scriptEng.operands[2], scriptEng.operands[3], scriptEng.operands[4], scriptEng.operands[5]);
                        break;
                    case 108:
                        sbyte num7 = 0;
                        scriptEng.checkResult = -1;
                        for (; (int)num7 < StageSystem.gKeyDown.touches; ++num7)
                        {
                            if (StageSystem.gKeyDown.touchDown[(int)num7] == (byte)1 && StageSystem.gKeyDown.touchX[(int)num7] > scriptEng.operands[0] && (StageSystem.gKeyDown.touchX[(int)num7] < scriptEng.operands[2] && StageSystem.gKeyDown.touchY[(int)num7] > scriptEng.operands[1]) && StageSystem.gKeyDown.touchY[(int)num7] < scriptEng.operands[3])
                                scriptEng.checkResult = (int)num7;
                        }
                        num3 = (sbyte)0;
                        break;
                    case 109:
                        scriptEng.operands[0] = scriptEng.operands[2] <= -1 || scriptEng.operands[3] <= -1 ? 0 : (int)StageSystem.stageLayouts[scriptEng.operands[1]].tileMap[scriptEng.operands[2] + (scriptEng.operands[3] << 8)];
                        break;
                    case 110:
                        if (scriptEng.operands[2] > -1 && scriptEng.operands[3] > -1)
                        {
                            StageSystem.stageLayouts[scriptEng.operands[1]].tileMap[scriptEng.operands[2] + (scriptEng.operands[3] << 8)] = (ushort)scriptEng.operands[0];
                            break;
                        }
                        break;
                    case 111:
                        scriptEng.operands[0] = (scriptEng.operands[1] & 1 << scriptEng.operands[2]) >> scriptEng.operands[2];
                        break;
                    case 112:
                        if (scriptEng.operands[2] > 0)
                        {
                            scriptEng.operands[0] |= 1 << scriptEng.operands[1];
                            break;
                        }
                        scriptEng.operands[0] &= ~(1 << scriptEng.operands[1]);
                        break;
                    case 113:
                        num3 = (sbyte)0;
                        AudioPlayback.PauseSound();
                        break;
                    case 114:
                        num3 = (sbyte)0;
                        AudioPlayback.ResumeSound();
                        break;
                    case 115:
                        num3 = (sbyte)0;
                        objectDrawOrderList[scriptEng.operands[0]].listSize = 0;
                        break;
                    case 116:
                        num3 = (sbyte)0;
                        objectDrawOrderList[scriptEng.operands[0]].entityRef[objectDrawOrderList[scriptEng.operands[0]].listSize] = scriptEng.operands[1];
                        ++objectDrawOrderList[scriptEng.operands[0]].listSize;
                        break;
                    case 117:
                        scriptEng.operands[0] = objectDrawOrderList[scriptEng.operands[1]].entityRef[scriptEng.operands[2]];
                        break;
                    case 118:
                        num3 = (sbyte)0;
                        objectDrawOrderList[scriptEng.operands[1]].entityRef[scriptEng.operands[2]] = scriptEng.operands[0];
                        break;
                    case 119:
                        scriptEng.operands[4] = scriptEng.operands[1] >> 7;
                        scriptEng.operands[5] = scriptEng.operands[2] >> 7;
                        scriptEng.operands[6] = scriptEng.operands[4] <= -1 || scriptEng.operands[5] <= -1 ? 0 : (int)StageSystem.stageLayouts[0].tileMap[scriptEng.operands[4] + (scriptEng.operands[5] << 8)] << 6;
                        scriptEng.operands[6] += ((scriptEng.operands[1] & (int)sbyte.MaxValue) >> 4) + ((scriptEng.operands[2] & (int)sbyte.MaxValue) >> 4 << 3);
                        switch (scriptEng.operands[3])
                        {
                            case 0:
                                scriptEng.operands[0] = (int)StageSystem.tile128x128.tile16x16[scriptEng.operands[6]];
                                break;
                            case 1:
                                scriptEng.operands[0] = (int)StageSystem.tile128x128.direction[scriptEng.operands[6]];
                                break;
                            case 2:
                                scriptEng.operands[0] = (int)StageSystem.tile128x128.visualPlane[scriptEng.operands[6]];
                                break;
                            case 3:
                                scriptEng.operands[0] = (int)StageSystem.tile128x128.collisionFlag[0, scriptEng.operands[6]];
                                break;
                            case 4:
                                scriptEng.operands[0] = (int)StageSystem.tile128x128.collisionFlag[1, scriptEng.operands[6]];
                                break;
                            case 5:
                                scriptEng.operands[0] = (int)StageSystem.tileCollisions[0].flags[(int)StageSystem.tile128x128.tile16x16[scriptEng.operands[6]]];
                                break;
                            case 6:
                                scriptEng.operands[0] = (int)StageSystem.tileCollisions[0].angle[(int)StageSystem.tile128x128.tile16x16[scriptEng.operands[6]]];
                                break;
                            case 7:
                                scriptEng.operands[0] = (int)StageSystem.tileCollisions[1].flags[(int)StageSystem.tile128x128.tile16x16[scriptEng.operands[6]]];
                                break;
                            case 8:
                                scriptEng.operands[0] = (int)StageSystem.tileCollisions[1].angle[(int)StageSystem.tile128x128.tile16x16[scriptEng.operands[6]]];
                                break;
                        }
                        break;
                    case 120:
                        num3 = (sbyte)0;
                        GraphicsSystem.Copy16x16Tile(scriptEng.operands[0], scriptEng.operands[1]);
                        break;
                    case 121:
                        scriptEng.operands[4] = scriptEng.operands[1] >> 7;
                        scriptEng.operands[5] = scriptEng.operands[2] >> 7;
                        scriptEng.operands[6] = scriptEng.operands[4] <= -1 || scriptEng.operands[5] <= -1 ? 0 : (int)StageSystem.stageLayouts[0].tileMap[scriptEng.operands[4] + (scriptEng.operands[5] << 8)] << 6;
                        scriptEng.operands[6] += ((scriptEng.operands[1] & (int)sbyte.MaxValue) >> 4) + ((scriptEng.operands[2] & (int)sbyte.MaxValue) >> 4 << 3);
                        switch (scriptEng.operands[3])
                        {
                            case 0:
                                StageSystem.tile128x128.tile16x16[scriptEng.operands[6]] = (ushort)scriptEng.operands[0];
                                StageSystem.tile128x128.gfxDataPos[scriptEng.operands[6]] = (int)StageSystem.tile128x128.tile16x16[scriptEng.operands[6]] << 2;
                                break;
                            case 1:
                                StageSystem.tile128x128.direction[scriptEng.operands[6]] = (byte)scriptEng.operands[0];
                                break;
                            case 2:
                                StageSystem.tile128x128.visualPlane[scriptEng.operands[6]] = (byte)scriptEng.operands[0];
                                break;
                            case 3:
                                StageSystem.tile128x128.collisionFlag[0, scriptEng.operands[6]] = (byte)scriptEng.operands[0];
                                break;
                            case 4:
                                StageSystem.tile128x128.collisionFlag[1, scriptEng.operands[6]] = (byte)scriptEng.operands[0];
                                break;
                            case 5:
                                StageSystem.tileCollisions[0].flags[(int)StageSystem.tile128x128.tile16x16[scriptEng.operands[6]]] = (byte)scriptEng.operands[0];
                                break;
                            case 6:
                                StageSystem.tileCollisions[0].angle[(int)StageSystem.tile128x128.tile16x16[scriptEng.operands[6]]] = (uint)(byte)scriptEng.operands[0];
                                break;
                        }
                        break;
                    case 122:
                        scriptEng.operands[0] = -1;
                        scriptEng.sRegister = 0;
                        while (scriptEng.operands[0] == -1)
                        {
                            if (FileIO.StringComp(ref scriptText, ref AnimationSystem.animationList[PlayerSystem.playerList[playerNum].animationFile.aniListOffset + scriptEng.sRegister].name))
                            {
                                scriptEng.operands[0] = scriptEng.sRegister;
                            }
                            else
                            {
                                ++scriptEng.sRegister;
                                if (scriptEng.sRegister == PlayerSystem.playerList[playerNum].animationFile.numAnimations)
                                    scriptEng.operands[0] = 0;
                            }
                        }
                        break;
                    case 123:
                        num3 = (sbyte)0;
                        scriptEng.checkResult = (int)FileIO.ReadSaveRAMData();
                        break;
                    case 124:
                        num3 = (sbyte)0;
                        scriptEng.checkResult = (int)FileIO.WriteSaveRAMData();
                        break;
                    case 125:
                        num3 = (sbyte)0;
                        TextSystem.LoadFontFile(scriptText);
                        break;
                    case 126:
                        num3 = (sbyte)0;
                        if (scriptEng.operands[2] == 0)
                        {
                            TextSystem.LoadTextFile(StageSystem.gameMenu[scriptEng.operands[0]], scriptText, (byte)0);
                            break;
                        }
                        TextSystem.LoadTextFile(StageSystem.gameMenu[scriptEng.operands[0]], scriptText, (byte)1);
                        break;
                    case (int)sbyte.MaxValue:
                        num3 = (sbyte)0;
                        TextSystem.textMenuSurfaceNo = (int)objectScriptList[(int)objectEntityList[objectLoop].type].surfaceNum;
                        TextSystem.DrawBitmapText(StageSystem.gameMenu[scriptEng.operands[0]], scriptEng.operands[1], scriptEng.operands[2], scriptEng.operands[3], scriptEng.operands[4], scriptEng.operands[5], scriptEng.operands[6]);
                        break;
                    case 128:
                        switch (scriptEng.operands[2])
                        {
                            case 0:
                                scriptEng.operands[0] = (int)StageSystem.gameMenu[scriptEng.operands[1]].textData[StageSystem.gameMenu[scriptEng.operands[1]].entryStart[scriptEng.operands[3]] + scriptEng.operands[4]];
                                break;
                            case 1:
                                scriptEng.operands[0] = StageSystem.gameMenu[scriptEng.operands[1]].entrySize[scriptEng.operands[3]];
                                break;
                            case 2:
                                scriptEng.operands[0] = (int)StageSystem.gameMenu[scriptEng.operands[1]].numRows;
                                break;
                        }
                        break;
                    case 129:
                        num3 = (sbyte)0;
                        StageSystem.gameMenu[scriptEng.operands[0]].entryHighlight[(int)StageSystem.gameMenu[scriptEng.operands[0]].numRows] = (byte)scriptEng.operands[1];
                        TextSystem.AddTextMenuEntry(StageSystem.gameMenu[scriptEng.operands[0]], GlobalAppDefinitions.gameVersion);
                        break;
                    case 130:
                        num3 = (sbyte)0;
                        EngineCallbacks.OnlineSetAchievement(scriptEng.operands[0], scriptEng.operands[1]);
                        break;
                    case 131:
                        num3 = (sbyte)0;
                        EngineCallbacks.OnlineSetLeaderboard(scriptEng.operands[0], scriptEng.operands[1]);
                        break;
                    case 132:
                        num3 = (sbyte)0;
                        switch (scriptEng.operands[0])
                        {
                            case 0:
                                EngineCallbacks.OnlineLoadAchievementsMenu();
                                break;
                            case 1:
                                EngineCallbacks.OnlineLoadLeaderboardsMenu();
                                break;
                        }
                        break;
                    case 133:
                        num3 = (sbyte)0;
                        EngineCallbacks.RetroEngineCallback(scriptEng.operands[0]);
                        break;
                    case 134:
                        num3 = (sbyte)0;
                        break;
                }
                if (num3 > (sbyte)0)
                    scriptCodePtr -= num2;
                for (int index3 = 0; index3 < (int)num3; ++index3)
                {
                    switch (scriptData[scriptCodePtr])
                    {
                        case 1:
                            ++scriptCodePtr;
                            switch (scriptData[scriptCodePtr])
                            {
                                case 0:
                                    index1 = objectLoop;
                                    break;
                                case 1:
                                    ++scriptCodePtr;
                                    if (scriptData[scriptCodePtr] == 1)
                                    {
                                        ++scriptCodePtr;
                                        index1 = scriptEng.arrayPosition[scriptData[scriptCodePtr]];
                                        break;
                                    }
                                    ++scriptCodePtr;
                                    index1 = scriptData[scriptCodePtr];
                                    break;
                                case 2:
                                    ++scriptCodePtr;
                                    if (scriptData[scriptCodePtr] == 1)
                                    {
                                        ++scriptCodePtr;
                                        index1 = objectLoop + scriptEng.arrayPosition[scriptData[scriptCodePtr]];
                                        break;
                                    }
                                    ++scriptCodePtr;
                                    index1 = objectLoop + scriptData[scriptCodePtr];
                                    break;
                                case 3:
                                    ++scriptCodePtr;
                                    if (scriptData[scriptCodePtr] == 1)
                                    {
                                        ++scriptCodePtr;
                                        index1 = objectLoop - scriptEng.arrayPosition[scriptData[scriptCodePtr]];
                                        break;
                                    }
                                    ++scriptCodePtr;
                                    index1 = objectLoop - scriptData[scriptCodePtr];
                                    break;
                            }
                            ++scriptCodePtr;
                            switch (scriptData[scriptCodePtr])
                            {
                                case 0:
                                    scriptEng.tempValue[0] = scriptEng.operands[index3];
                                    break;
                                case 1:
                                    scriptEng.tempValue[1] = scriptEng.operands[index3];
                                    break;
                                case 2:
                                    scriptEng.tempValue[2] = scriptEng.operands[index3];
                                    break;
                                case 3:
                                    scriptEng.tempValue[3] = scriptEng.operands[index3];
                                    break;
                                case 4:
                                    scriptEng.tempValue[4] = scriptEng.operands[index3];
                                    break;
                                case 5:
                                    scriptEng.tempValue[5] = scriptEng.operands[index3];
                                    break;
                                case 6:
                                    scriptEng.tempValue[6] = scriptEng.operands[index3];
                                    break;
                                case 7:
                                    scriptEng.tempValue[7] = scriptEng.operands[index3];
                                    break;
                                case 8:
                                    scriptEng.checkResult = scriptEng.operands[index3];
                                    break;
                                case 9:
                                    scriptEng.arrayPosition[0] = scriptEng.operands[index3];
                                    break;
                                case 10:
                                    scriptEng.arrayPosition[1] = scriptEng.operands[index3];
                                    break;
                                case 11:
                                    globalVariables[index1] = scriptEng.operands[index3];
                                    break;
                                case 13:
                                    objectEntityList[index1].type = (byte)scriptEng.operands[index3];
                                    break;
                                case 14:
                                    objectEntityList[index1].propertyValue = (byte)scriptEng.operands[index3];
                                    break;
                                case 15:
                                    objectEntityList[index1].xPos = scriptEng.operands[index3];
                                    break;
                                case 16:
                                    objectEntityList[index1].yPos = scriptEng.operands[index3];
                                    break;
                                case 17:
                                    objectEntityList[index1].xPos = scriptEng.operands[index3] << 16;
                                    break;
                                case 18:
                                    objectEntityList[index1].yPos = scriptEng.operands[index3] << 16;
                                    break;
                                case 19:
                                    objectEntityList[index1].state = (byte)scriptEng.operands[index3];
                                    break;
                                case 20:
                                    objectEntityList[index1].rotation = scriptEng.operands[index3];
                                    break;
                                case 21:
                                    objectEntityList[index1].scale = scriptEng.operands[index3];
                                    break;
                                case 22:
                                    objectEntityList[index1].priority = (byte)scriptEng.operands[index3];
                                    break;
                                case 23:
                                    objectEntityList[index1].drawOrder = (byte)scriptEng.operands[index3];
                                    break;
                                case 24:
                                    objectEntityList[index1].direction = (byte)scriptEng.operands[index3];
                                    break;
                                case 25:
                                    objectEntityList[index1].inkEffect = (byte)scriptEng.operands[index3];
                                    break;
                                case 26:
                                    objectEntityList[index1].alpha = (byte)scriptEng.operands[index3];
                                    break;
                                case 27:
                                    objectEntityList[index1].frame = (byte)scriptEng.operands[index3];
                                    break;
                                case 28:
                                    objectEntityList[index1].animation = (byte)scriptEng.operands[index3];
                                    break;
                                case 29:
                                    objectEntityList[index1].prevAnimation = (byte)scriptEng.operands[index3];
                                    break;
                                case 30:
                                    objectEntityList[index1].animationSpeed = scriptEng.operands[index3];
                                    break;
                                case 31:
                                    objectEntityList[index1].animationTimer = scriptEng.operands[index3];
                                    break;
                                case 32:
                                    objectEntityList[index1].value[0] = scriptEng.operands[index3];
                                    break;
                                case 33:
                                    objectEntityList[index1].value[1] = scriptEng.operands[index3];
                                    break;
                                case 34:
                                    objectEntityList[index1].value[2] = scriptEng.operands[index3];
                                    break;
                                case 35:
                                    objectEntityList[index1].value[3] = scriptEng.operands[index3];
                                    break;
                                case 36:
                                    objectEntityList[index1].value[4] = scriptEng.operands[index3];
                                    break;
                                case 37:
                                    objectEntityList[index1].value[5] = scriptEng.operands[index3];
                                    break;
                                case 38:
                                    objectEntityList[index1].value[6] = scriptEng.operands[index3];
                                    break;
                                case 39:
                                    objectEntityList[index1].value[7] = scriptEng.operands[index3];
                                    break;
                                case 41:
                                    PlayerSystem.playerList[playerNum].objectPtr.state = (byte)scriptEng.operands[index3];
                                    break;
                                case 42:
                                    PlayerSystem.playerList[playerNum].controlMode = (sbyte)scriptEng.operands[index3];
                                    break;
                                case 43:
                                    PlayerSystem.playerList[playerNum].controlLock = (byte)scriptEng.operands[index3];
                                    break;
                                case 44:
                                    PlayerSystem.playerList[playerNum].collisionMode = (byte)scriptEng.operands[index3];
                                    break;
                                case 45:
                                    PlayerSystem.playerList[playerNum].collisionPlane = (byte)scriptEng.operands[index3];
                                    break;
                                case 46:
                                    PlayerSystem.playerList[playerNum].xPos = scriptEng.operands[index3];
                                    break;
                                case 47:
                                    PlayerSystem.playerList[playerNum].yPos = scriptEng.operands[index3];
                                    break;
                                case 48:
                                    PlayerSystem.playerList[playerNum].xPos = scriptEng.operands[index3] << 16;
                                    break;
                                case 49:
                                    PlayerSystem.playerList[playerNum].yPos = scriptEng.operands[index3] << 16;
                                    break;
                                case 50:
                                    PlayerSystem.playerList[playerNum].screenXPos = scriptEng.operands[index3];
                                    break;
                                case 51:
                                    PlayerSystem.playerList[playerNum].screenYPos = scriptEng.operands[index3];
                                    break;
                                case 52:
                                    PlayerSystem.playerList[playerNum].speed = scriptEng.operands[index3];
                                    break;
                                case 53:
                                    PlayerSystem.playerList[playerNum].xVelocity = scriptEng.operands[index3];
                                    break;
                                case 54:
                                    PlayerSystem.playerList[playerNum].yVelocity = scriptEng.operands[index3];
                                    break;
                                case 55:
                                    PlayerSystem.playerList[playerNum].gravity = (byte)scriptEng.operands[index3];
                                    break;
                                case 56:
                                    PlayerSystem.playerList[playerNum].angle = scriptEng.operands[index3];
                                    break;
                                case 57:
                                    PlayerSystem.playerList[playerNum].skidding = (byte)scriptEng.operands[index3];
                                    break;
                                case 58:
                                    PlayerSystem.playerList[playerNum].pushing = (byte)scriptEng.operands[index3];
                                    break;
                                case 59:
                                    PlayerSystem.playerList[playerNum].trackScroll = (byte)scriptEng.operands[index3];
                                    break;
                                case 60:
                                    PlayerSystem.playerList[playerNum].up = (byte)scriptEng.operands[index3];
                                    break;
                                case 61:
                                    PlayerSystem.playerList[playerNum].down = (byte)scriptEng.operands[index3];
                                    break;
                                case 62:
                                    PlayerSystem.playerList[playerNum].left = (byte)scriptEng.operands[index3];
                                    break;
                                case 63:
                                    PlayerSystem.playerList[playerNum].right = (byte)scriptEng.operands[index3];
                                    break;
                                case 64:
                                    PlayerSystem.playerList[playerNum].jumpPress = (byte)scriptEng.operands[index3];
                                    break;
                                case 65:
                                    PlayerSystem.playerList[playerNum].jumpHold = (byte)scriptEng.operands[index3];
                                    break;
                                case 66:
                                    PlayerSystem.playerList[playerNum].followPlayer1 = (byte)scriptEng.operands[index3];
                                    break;
                                case 67:
                                    PlayerSystem.playerList[playerNum].lookPos = scriptEng.operands[index3];
                                    break;
                                case 68:
                                    PlayerSystem.playerList[playerNum].water = (byte)scriptEng.operands[index3];
                                    break;
                                case 69:
                                    PlayerSystem.playerList[playerNum].movementStats.topSpeed = scriptEng.operands[index3];
                                    break;
                                case 70:
                                    PlayerSystem.playerList[playerNum].movementStats.acceleration = scriptEng.operands[index3];
                                    break;
                                case 71:
                                    PlayerSystem.playerList[playerNum].movementStats.deceleration = scriptEng.operands[index3];
                                    break;
                                case 72:
                                    PlayerSystem.playerList[playerNum].movementStats.airAcceleration = scriptEng.operands[index3];
                                    break;
                                case 73:
                                    PlayerSystem.playerList[playerNum].movementStats.airDeceleration = scriptEng.operands[index3];
                                    break;
                                case 74:
                                    PlayerSystem.playerList[playerNum].movementStats.gravity = scriptEng.operands[index3];
                                    break;
                                case 75:
                                    PlayerSystem.playerList[playerNum].movementStats.jumpStrength = scriptEng.operands[index3];
                                    break;
                                case 76:
                                    PlayerSystem.playerList[playerNum].movementStats.jumpCap = scriptEng.operands[index3];
                                    break;
                                case 77:
                                    PlayerSystem.playerList[playerNum].movementStats.rollingAcceleration = scriptEng.operands[index3];
                                    break;
                                case 78:
                                    PlayerSystem.playerList[playerNum].movementStats.rollingDeceleration = scriptEng.operands[index3];
                                    break;
                                case 84:
                                    PlayerSystem.playerList[playerNum].flailing[index1] = (byte)scriptEng.operands[index3];
                                    break;
                                case 85:
                                    PlayerSystem.playerList[playerNum].timer = scriptEng.operands[index3];
                                    break;
                                case 86:
                                    PlayerSystem.playerList[playerNum].tileCollisions = (byte)scriptEng.operands[index3];
                                    break;
                                case 87:
                                    PlayerSystem.playerList[playerNum].objectInteraction = (byte)scriptEng.operands[index3];
                                    break;
                                case 88:
                                    PlayerSystem.playerList[playerNum].visible = (byte)scriptEng.operands[index3];
                                    break;
                                case 89:
                                    PlayerSystem.playerList[playerNum].objectPtr.rotation = scriptEng.operands[index3];
                                    break;
                                case 90:
                                    PlayerSystem.playerList[playerNum].objectPtr.scale = scriptEng.operands[index3];
                                    break;
                                case 91:
                                    PlayerSystem.playerList[playerNum].objectPtr.priority = (byte)scriptEng.operands[index3];
                                    break;
                                case 92:
                                    PlayerSystem.playerList[playerNum].objectPtr.drawOrder = (byte)scriptEng.operands[index3];
                                    break;
                                case 93:
                                    PlayerSystem.playerList[playerNum].objectPtr.direction = (byte)scriptEng.operands[index3];
                                    break;
                                case 94:
                                    PlayerSystem.playerList[playerNum].objectPtr.inkEffect = (byte)scriptEng.operands[index3];
                                    break;
                                case 95:
                                    PlayerSystem.playerList[playerNum].objectPtr.alpha = (byte)scriptEng.operands[index3];
                                    break;
                                case 96:
                                    PlayerSystem.playerList[playerNum].objectPtr.frame = (byte)scriptEng.operands[index3];
                                    break;
                                case 97:
                                    PlayerSystem.playerList[playerNum].objectPtr.animation = (byte)scriptEng.operands[index3];
                                    break;
                                case 98:
                                    PlayerSystem.playerList[playerNum].objectPtr.prevAnimation = (byte)scriptEng.operands[index3];
                                    break;
                                case 99:
                                    PlayerSystem.playerList[playerNum].objectPtr.animationSpeed = scriptEng.operands[index3];
                                    break;
                                case 100:
                                    PlayerSystem.playerList[playerNum].objectPtr.animationTimer = scriptEng.operands[index3];
                                    break;
                                case 101:
                                    PlayerSystem.playerList[playerNum].objectPtr.value[0] = scriptEng.operands[index3];
                                    break;
                                case 102:
                                    PlayerSystem.playerList[playerNum].objectPtr.value[1] = scriptEng.operands[index3];
                                    break;
                                case 103:
                                    PlayerSystem.playerList[playerNum].objectPtr.value[2] = scriptEng.operands[index3];
                                    break;
                                case 104:
                                    PlayerSystem.playerList[playerNum].objectPtr.value[3] = scriptEng.operands[index3];
                                    break;
                                case 105:
                                    PlayerSystem.playerList[playerNum].objectPtr.value[4] = scriptEng.operands[index3];
                                    break;
                                case 106:
                                    PlayerSystem.playerList[playerNum].objectPtr.value[5] = scriptEng.operands[index3];
                                    break;
                                case 107:
                                    PlayerSystem.playerList[playerNum].objectPtr.value[6] = scriptEng.operands[index3];
                                    break;
                                case 108:
                                    PlayerSystem.playerList[playerNum].objectPtr.value[7] = scriptEng.operands[index3];
                                    break;
                                case 109:
                                    PlayerSystem.playerList[playerNum].value[0] = scriptEng.operands[index3];
                                    break;
                                case 110:
                                    PlayerSystem.playerList[playerNum].value[1] = scriptEng.operands[index3];
                                    break;
                                case 111:
                                    PlayerSystem.playerList[playerNum].value[2] = scriptEng.operands[index3];
                                    break;
                                case 112:
                                    PlayerSystem.playerList[playerNum].value[3] = scriptEng.operands[index3];
                                    break;
                                case 113:
                                    PlayerSystem.playerList[playerNum].value[4] = scriptEng.operands[index3];
                                    break;
                                case 114:
                                    PlayerSystem.playerList[playerNum].value[5] = scriptEng.operands[index3];
                                    break;
                                case 115:
                                    PlayerSystem.playerList[playerNum].value[6] = scriptEng.operands[index3];
                                    break;
                                case 116:
                                    PlayerSystem.playerList[playerNum].value[7] = scriptEng.operands[index3];
                                    break;
                                case 118:
                                    StageSystem.stageMode = (byte)scriptEng.operands[index3];
                                    break;
                                case 119:
                                    FileIO.activeStageList = (byte)scriptEng.operands[index3];
                                    break;
                                case 120:
                                    StageSystem.stageListPosition = scriptEng.operands[index3];
                                    break;
                                case 121:
                                    StageSystem.timeEnabled = (byte)scriptEng.operands[index3];
                                    break;
                                case 122:
                                    StageSystem.milliSeconds = (byte)scriptEng.operands[index3];
                                    break;
                                case 123:
                                    StageSystem.seconds = (byte)scriptEng.operands[index3];
                                    break;
                                case 124:
                                    StageSystem.minutes = (byte)scriptEng.operands[index3];
                                    break;
                                case 125:
                                    FileIO.actNumber = scriptEng.operands[index3];
                                    break;
                                case 126:
                                    StageSystem.pauseEnabled = (byte)scriptEng.operands[index3];
                                    break;
                                case 128:
                                    StageSystem.newXBoundary1 = scriptEng.operands[index3];
                                    break;
                                case 129:
                                    StageSystem.newXBoundary2 = scriptEng.operands[index3];
                                    break;
                                case 130:
                                    StageSystem.newYBoundary1 = scriptEng.operands[index3];
                                    break;
                                case 131:
                                    StageSystem.newYBoundary2 = scriptEng.operands[index3];
                                    break;
                                case 132:
                                    if (StageSystem.xBoundary1 != scriptEng.operands[index3])
                                    {
                                        StageSystem.xBoundary1 = scriptEng.operands[index3];
                                        StageSystem.newXBoundary1 = scriptEng.operands[index3];
                                        break;
                                    }
                                    break;
                                case 133:
                                    if (StageSystem.xBoundary2 != scriptEng.operands[index3])
                                    {
                                        StageSystem.xBoundary2 = scriptEng.operands[index3];
                                        StageSystem.newXBoundary2 = scriptEng.operands[index3];
                                        break;
                                    }
                                    break;
                                case 134:
                                    if (StageSystem.yBoundary1 != scriptEng.operands[index3])
                                    {
                                        StageSystem.yBoundary1 = scriptEng.operands[index3];
                                        StageSystem.newYBoundary1 = scriptEng.operands[index3];
                                        break;
                                    }
                                    break;
                                case 135:
                                    if (StageSystem.yBoundary2 != scriptEng.operands[index3])
                                    {
                                        StageSystem.yBoundary2 = scriptEng.operands[index3];
                                        StageSystem.newYBoundary2 = scriptEng.operands[index3];
                                        break;
                                    }
                                    break;
                                case 136:
                                    StageSystem.bgDeformationData0[index1] = scriptEng.operands[index3];
                                    break;
                                case 137:
                                    StageSystem.bgDeformationData1[index1] = scriptEng.operands[index3];
                                    break;
                                case 138:
                                    StageSystem.bgDeformationData2[index1] = scriptEng.operands[index3];
                                    break;
                                case 139:
                                    StageSystem.bgDeformationData3[index1] = scriptEng.operands[index3];
                                    break;
                                case 140:
                                    StageSystem.waterLevel = scriptEng.operands[index3];
                                    break;
                                case 141:
                                    StageSystem.activeTileLayers[index1] = (byte)scriptEng.operands[index3];
                                    break;
                                case 142:
                                    StageSystem.tLayerMidPoint = (byte)scriptEng.operands[index3];
                                    break;
                                case 143:
                                    PlayerSystem.playerMenuNum = (byte)scriptEng.operands[index3];
                                    break;
                                case 144:
                                    playerNum = scriptEng.operands[index3];
                                    break;
                                case 145:
                                    StageSystem.cameraEnabled = (byte)scriptEng.operands[index3];
                                    break;
                                case 146:
                                    StageSystem.cameraTarget = (sbyte)scriptEng.operands[index3];
                                    break;
                                case 147:
                                    StageSystem.cameraStyle = (byte)scriptEng.operands[index3];
                                    break;
                                case 148:
                                    objectDrawOrderList[index1].listSize = scriptEng.operands[index3];
                                    break;
                                case 153:
                                    StageSystem.xScrollOffset = scriptEng.operands[index3];
                                    StageSystem.xScrollA = StageSystem.xScrollOffset;
                                    StageSystem.xScrollB = StageSystem.xScrollOffset + GlobalAppDefinitions.SCREEN_XSIZE;
                                    break;
                                case 154:
                                    StageSystem.yScrollOffset = scriptEng.operands[index3];
                                    StageSystem.yScrollA = StageSystem.yScrollOffset;
                                    StageSystem.yScrollB = StageSystem.yScrollOffset + 240;
                                    break;
                                case 155:
                                    StageSystem.screenShakeX = scriptEng.operands[index3];
                                    break;
                                case 156:
                                    StageSystem.screenShakeY = scriptEng.operands[index3];
                                    break;
                                case 157:
                                    StageSystem.cameraAdjustY = scriptEng.operands[index3];
                                    break;
                                case 161:
                                    AudioPlayback.SetMusicVolume(scriptEng.operands[index3]);
                                    break;
                                case 163:
                                    StageSystem.gKeyDown.up = (byte)scriptEng.operands[index3];
                                    break;
                                case 164:
                                    StageSystem.gKeyDown.down = (byte)scriptEng.operands[index3];
                                    break;
                                case 165:
                                    StageSystem.gKeyDown.left = (byte)scriptEng.operands[index3];
                                    break;
                                case 166:
                                    StageSystem.gKeyDown.right = (byte)scriptEng.operands[index3];
                                    break;
                                case 167:
                                    StageSystem.gKeyDown.buttonA = (byte)scriptEng.operands[index3];
                                    break;
                                case 168:
                                    StageSystem.gKeyDown.buttonB = (byte)scriptEng.operands[index3];
                                    break;
                                case 169:
                                    StageSystem.gKeyDown.buttonC = (byte)scriptEng.operands[index3];
                                    break;
                                case 170:
                                    StageSystem.gKeyDown.start = (byte)scriptEng.operands[index3];
                                    break;
                                case 171:
                                    StageSystem.gKeyPress.up = (byte)scriptEng.operands[index3];
                                    break;
                                case 172:
                                    StageSystem.gKeyPress.down = (byte)scriptEng.operands[index3];
                                    break;
                                case 173:
                                    StageSystem.gKeyPress.left = (byte)scriptEng.operands[index3];
                                    break;
                                case 174:
                                    StageSystem.gKeyPress.right = (byte)scriptEng.operands[index3];
                                    break;
                                case 175:
                                    StageSystem.gKeyPress.buttonA = (byte)scriptEng.operands[index3];
                                    break;
                                case 176:
                                    StageSystem.gKeyPress.buttonB = (byte)scriptEng.operands[index3];
                                    break;
                                case 177:
                                    StageSystem.gKeyPress.buttonC = (byte)scriptEng.operands[index3];
                                    break;
                                case 178:
                                    StageSystem.gKeyPress.start = (byte)scriptEng.operands[index3];
                                    break;
                                case 179:
                                    StageSystem.gameMenu[0].selection1 = scriptEng.operands[index3];
                                    break;
                                case 180:
                                    StageSystem.gameMenu[1].selection1 = scriptEng.operands[index3];
                                    break;
                                case 181:
                                    StageSystem.stageLayouts[index1].xSize = (byte)scriptEng.operands[index3];
                                    break;
                                case 182:
                                    StageSystem.stageLayouts[index1].ySize = (byte)scriptEng.operands[index3];
                                    break;
                                case 183:
                                    StageSystem.stageLayouts[index1].type = (byte)scriptEng.operands[index3];
                                    break;
                                case 184:
                                    StageSystem.stageLayouts[index1].angle = scriptEng.operands[index3];
                                    if (StageSystem.stageLayouts[index1].angle < 0)
                                        StageSystem.stageLayouts[index1].angle += 512;
                                    StageSystem.stageLayouts[index1].angle &= 511;
                                    break;
                                case 185:
                                    StageSystem.stageLayouts[index1].xPos = scriptEng.operands[index3];
                                    break;
                                case 186:
                                    StageSystem.stageLayouts[index1].yPos = scriptEng.operands[index3];
                                    break;
                                case 187:
                                    StageSystem.stageLayouts[index1].zPos = scriptEng.operands[index3];
                                    break;
                                case 188:
                                    StageSystem.stageLayouts[index1].parallaxFactor = scriptEng.operands[index3];
                                    break;
                                case 189:
                                    StageSystem.stageLayouts[index1].scrollSpeed = scriptEng.operands[index3];
                                    break;
                                case 190:
                                    StageSystem.stageLayouts[index1].scrollPosition = scriptEng.operands[index3];
                                    break;
                                case 191:
                                    StageSystem.stageLayouts[index1].deformationPos = scriptEng.operands[index3];
                                    StageSystem.stageLayouts[index1].deformationPos &= (int)byte.MaxValue;
                                    break;
                                case 192:
                                    StageSystem.stageLayouts[index1].deformationPosW = scriptEng.operands[index3];
                                    StageSystem.stageLayouts[index1].deformationPosW &= (int)byte.MaxValue;
                                    break;
                                case 193:
                                    StageSystem.hParallax.parallaxFactor[index1] = scriptEng.operands[index3];
                                    break;
                                case 194:
                                    StageSystem.hParallax.scrollSpeed[index1] = scriptEng.operands[index3];
                                    break;
                                case 195:
                                    StageSystem.hParallax.scrollPosition[index1] = scriptEng.operands[index3];
                                    break;
                                case 196:
                                    StageSystem.vParallax.parallaxFactor[index1] = scriptEng.operands[index3];
                                    break;
                                case 197:
                                    StageSystem.vParallax.scrollSpeed[index1] = scriptEng.operands[index3];
                                    break;
                                case 198:
                                    StageSystem.vParallax.scrollPosition[index1] = scriptEng.operands[index3];
                                    break;
                                case 199:
                                    Scene3D.numVertices = scriptEng.operands[index3];
                                    break;
                                case 200:
                                    Scene3D.numFaces = scriptEng.operands[index3];
                                    break;
                                case 201:
                                    Scene3D.vertexBuffer[index1].x = scriptEng.operands[index3];
                                    break;
                                case 202:
                                    Scene3D.vertexBuffer[index1].y = scriptEng.operands[index3];
                                    break;
                                case 203:
                                    Scene3D.vertexBuffer[index1].z = scriptEng.operands[index3];
                                    break;
                                case 204:
                                    Scene3D.vertexBuffer[index1].u = scriptEng.operands[index3];
                                    break;
                                case 205:
                                    Scene3D.vertexBuffer[index1].v = scriptEng.operands[index3];
                                    break;
                                case 206:
                                    Scene3D.indexBuffer[index1].a = scriptEng.operands[index3];
                                    break;
                                case 207:
                                    Scene3D.indexBuffer[index1].b = scriptEng.operands[index3];
                                    break;
                                case 208:
                                    Scene3D.indexBuffer[index1].c = scriptEng.operands[index3];
                                    break;
                                case 209:
                                    Scene3D.indexBuffer[index1].d = scriptEng.operands[index3];
                                    break;
                                case 210:
                                    Scene3D.indexBuffer[index1].flag = (byte)scriptEng.operands[index3];
                                    break;
                                case 211:
                                    Scene3D.indexBuffer[index1].color = scriptEng.operands[index3];
                                    break;
                                case 212:
                                    Scene3D.projectionX = scriptEng.operands[index3];
                                    break;
                                case 213:
                                    Scene3D.projectionY = scriptEng.operands[index3];
                                    break;
                                case 214:
                                    GlobalAppDefinitions.gameMode = (byte)scriptEng.operands[index3];
                                    break;
                                case 215:
                                    StageSystem.debugMode = (byte)scriptEng.operands[index3];
                                    break;
                                case 217:
                                    FileIO.saveRAM[index1] = scriptEng.operands[index3];
                                    break;
                                case 218:
                                    GlobalAppDefinitions.gameLanguage = (byte)scriptEng.operands[index3];
                                    break;
                                case 219:
                                    objectScriptList[(int)objectEntityList[objectLoop].type].surfaceNum = (byte)scriptEng.operands[index3];
                                    break;
                                case 221:
                                    GlobalAppDefinitions.frameSkipTimer = scriptEng.operands[index3];
                                    break;
                                case 222:
                                    GlobalAppDefinitions.frameSkipSetting = scriptEng.operands[index3];
                                    break;
                                case 223:
                                    GlobalAppDefinitions.gameSFXVolume = scriptEng.operands[index3];
                                    AudioPlayback.SetGameVolumes(GlobalAppDefinitions.gameBGMVolume, GlobalAppDefinitions.gameSFXVolume);
                                    break;
                                case 224:
                                    GlobalAppDefinitions.gameBGMVolume = scriptEng.operands[index3];
                                    AudioPlayback.SetGameVolumes(GlobalAppDefinitions.gameBGMVolume, GlobalAppDefinitions.gameSFXVolume);
                                    break;
                                case 227:
                                    StageSystem.gKeyPress.start = (byte)scriptEng.operands[index3];
                                    break;
                                case 228:
                                    GlobalAppDefinitions.gameHapticsEnabled = (byte)scriptEng.operands[index3];
                                    break;
                            }
                            ++scriptCodePtr;
                            break;
                        case 2:
                            scriptCodePtr += 2;
                            break;
                        case 3:
                            ++scriptCodePtr;
                            int num4 = 0;
                            index1 = 0;
                            for (scriptEng.sRegister = scriptData[scriptCodePtr]; num4 < scriptEng.sRegister; ++num4)
                            {
                                switch (index1)
                                {
                                    case 0:
                                        ++scriptCodePtr;
                                        ++index1;
                                        break;
                                    case 1:
                                        ++index1;
                                        break;
                                    case 2:
                                        ++index1;
                                        break;
                                    case 3:
                                        index1 = 0;
                                        break;
                                }
                            }
                            if (index1 == 0)
                            {
                                scriptCodePtr += 2;
                                break;
                            }
                            ++scriptCodePtr;
                            break;
                    }
                }
            }
        }

        public static void ProcessStartupScripts()
        {
            objectEntityList[1057].type = objectEntityList[0].type;
            scriptFramesNo = 0;
            playerNum = 0;
            scriptEng.arrayPosition[2] = 1056;
            for (int index = 0; index < 256; ++index)
            {
                objectLoop = 1056;
                objectEntityList[1056].type = (byte)index;
                objectScriptList[index].numFrames = 0;
                objectScriptList[index].surfaceNum = (byte)0;
                objectScriptList[index].frameListOffset = scriptFramesNo;
                objectScriptList[index].numFrames = scriptFramesNo;
                if (scriptData[objectScriptList[index].startupScript] > 0)
                    ProcessScript(objectScriptList[index].startupScript, objectScriptList[index].startupJumpTable, 3);
                objectScriptList[index].numFrames = scriptFramesNo - objectScriptList[index].numFrames;
            }
            objectEntityList[1056].type = objectEntityList[1057].type;
            objectEntityList[1056].type = (byte)0;
        }

        public static void ProcessObjects()
        {
            bool flag = false;
            objectDrawOrderList[0].listSize = 0;
            objectDrawOrderList[1].listSize = 0;
            objectDrawOrderList[2].listSize = 0;
            objectDrawOrderList[3].listSize = 0;
            objectDrawOrderList[4].listSize = 0;
            objectDrawOrderList[5].listSize = 0;
            objectDrawOrderList[6].listSize = 0;
            for (objectLoop = 0; objectLoop < 1184; ++objectLoop)
            {
                switch (objectEntityList[objectLoop].priority)
                {
                    case 0:
                        int num1 = objectEntityList[objectLoop].xPos >> 16;
                        int num2 = objectEntityList[objectLoop].yPos >> 16;
                        flag = num1 > StageSystem.xScrollOffset - GlobalAppDefinitions.OBJECT_BORDER_X1 && num1 < StageSystem.xScrollOffset + GlobalAppDefinitions.OBJECT_BORDER_X2 && (num2 > StageSystem.yScrollOffset - 256 && num2 < StageSystem.yScrollOffset + 496);
                        break;
                    case 1:
                        flag = true;
                        break;
                    case 2:
                        flag = true;
                        break;
                    case 3:
                        int num3 = objectEntityList[objectLoop].xPos >> 16;
                        flag = num3 > StageSystem.xScrollOffset - GlobalAppDefinitions.OBJECT_BORDER_X1 && num3 < StageSystem.xScrollOffset + GlobalAppDefinitions.OBJECT_BORDER_X2;
                        break;
                    case 4:
                        int num4 = objectEntityList[objectLoop].xPos >> 16;
                        int num5 = objectEntityList[objectLoop].yPos >> 16;
                        if (num4 > StageSystem.xScrollOffset - GlobalAppDefinitions.OBJECT_BORDER_X1 && num4 < StageSystem.xScrollOffset + GlobalAppDefinitions.OBJECT_BORDER_X2 && (num5 > StageSystem.yScrollOffset - 256 && num5 < StageSystem.yScrollOffset + 496))
                        {
                            flag = true;
                            break;
                        }
                        flag = false;
                        objectEntityList[objectLoop].type = (byte)0;
                        break;
                    case 5:
                        flag = false;
                        break;
                }
                if (flag && objectEntityList[objectLoop].type > (byte)0)
                {
                    int type = (int)objectEntityList[objectLoop].type;
                    playerNum = 0;
                    if (scriptData[objectScriptList[type].mainScript] > 0)
                        ProcessScript(objectScriptList[type].mainScript, objectScriptList[type].mainJumpTable, 0);
                    if (scriptData[objectScriptList[type].playerScript] > 0)
                    {
                        for (; playerNum < (int)PlayerSystem.numActivePlayers; ++playerNum)
                        {
                            if (PlayerSystem.playerList[playerNum].objectInteraction == (byte)1)
                                ProcessScript(objectScriptList[type].playerScript, objectScriptList[type].playerJumpTable, 1);
                        }
                    }
                    int drawOrder = (int)objectEntityList[objectLoop].drawOrder;
                    if (drawOrder < 7)
                    {
                        objectDrawOrderList[drawOrder].entityRef[objectDrawOrderList[drawOrder].listSize] = objectLoop;
                        ++objectDrawOrderList[drawOrder].listSize;
                    }
                }
            }
        }

        public static void ProcessPausedObjects()
        {
            objectDrawOrderList[0].listSize = 0;
            objectDrawOrderList[1].listSize = 0;
            objectDrawOrderList[2].listSize = 0;
            objectDrawOrderList[3].listSize = 0;
            objectDrawOrderList[4].listSize = 0;
            objectDrawOrderList[5].listSize = 0;
            objectDrawOrderList[6].listSize = 0;
            for (objectLoop = 0; objectLoop < 1184; ++objectLoop)
            {
                if (objectEntityList[objectLoop].priority == (byte)2 && objectEntityList[objectLoop].type > (byte)0)
                {
                    int type = (int)objectEntityList[objectLoop].type;
                    playerNum = 0;
                    if (scriptData[objectScriptList[type].mainScript] > 0)
                        ProcessScript(objectScriptList[type].mainScript, objectScriptList[type].mainJumpTable, 0);
                    if (scriptData[objectScriptList[type].playerScript] > 0)
                    {
                        for (; playerNum < (int)PlayerSystem.numActivePlayers; ++playerNum)
                        {
                            if (PlayerSystem.playerList[playerNum].objectInteraction == (byte)1)
                                ProcessScript(objectScriptList[type].playerScript, objectScriptList[type].playerJumpTable, 1);
                        }
                    }
                    int drawOrder = (int)objectEntityList[objectLoop].drawOrder;
                    if (drawOrder < 7)
                    {
                        objectDrawOrderList[drawOrder].entityRef[objectDrawOrderList[drawOrder].listSize] = objectLoop;
                        ++objectDrawOrderList[drawOrder].listSize;
                    }
                }
            }
        }

        public static void DrawObjectList(int DrawListNo)
        {
            int listSize = objectDrawOrderList[DrawListNo].listSize;
            for (int index = 0; index < listSize; ++index)
            {
                objectLoop = objectDrawOrderList[DrawListNo].entityRef[index];
                if (objectEntityList[objectLoop].type > (byte)0)
                {
                    playerNum = 0;
                    if (scriptData[objectScriptList[(int)objectEntityList[objectLoop].type].drawScript] > 0)
                        ProcessScript(objectScriptList[(int)objectEntityList[objectLoop].type].drawScript, objectScriptList[(int)objectEntityList[objectLoop].type].drawJumpTable, 2);
                }
            }
        }

        public static void BasicCollision(int cLeft, int cTop, int cRight, int cBottom)
        {
            PlayerObject player = PlayerSystem.playerList[playerNum];
            CollisionBox collisionBox = AnimationSystem.collisionBoxList[player.animationFile.cbListOffset + (int)AnimationSystem.animationFrames[AnimationSystem.animationList[player.animationFile.aniListOffset + (int)player.objectPtr.animation].frameListOffset + (int)player.objectPtr.frame].collisionBox];
            PlayerSystem.collisionLeft = player.xPos >> 16;
            PlayerSystem.collisionTop = player.yPos >> 16;
            PlayerSystem.collisionRight = PlayerSystem.collisionLeft;
            PlayerSystem.collisionBottom = PlayerSystem.collisionTop;
            PlayerSystem.collisionLeft += (int)collisionBox.left[0];
            PlayerSystem.collisionTop += (int)collisionBox.top[0];
            PlayerSystem.collisionRight += (int)collisionBox.right[0];
            PlayerSystem.collisionBottom += (int)collisionBox.bottom[0];
            if (PlayerSystem.collisionRight > cLeft && PlayerSystem.collisionLeft < cRight && (PlayerSystem.collisionBottom > cTop && PlayerSystem.collisionTop < cBottom))
                scriptEng.checkResult = 1;
            else
                scriptEng.checkResult = 0;
        }

        public static void BoxCollision(int cLeft, int cTop, int cRight, int cBottom)
        {
            int num = 0;
            PlayerObject player = PlayerSystem.playerList[playerNum];
            CollisionBox collisionBox = AnimationSystem.collisionBoxList[player.animationFile.cbListOffset + (int)AnimationSystem.animationFrames[AnimationSystem.animationList[player.animationFile.aniListOffset + (int)player.objectPtr.animation].frameListOffset + (int)player.objectPtr.frame].collisionBox];
            PlayerSystem.collisionLeft = (int)collisionBox.left[0];
            PlayerSystem.collisionTop = (int)collisionBox.top[0];
            PlayerSystem.collisionRight = (int)collisionBox.right[0];
            PlayerSystem.collisionBottom = (int)collisionBox.bottom[0];
            scriptEng.checkResult = 0;
            switch (player.collisionMode)
            {
                case 0:
                case 2:
                    num = player.xVelocity == 0 ? Math.Abs(player.speed) : Math.Abs(player.xVelocity);
                    break;
                case 1:
                case 3:
                    num = Math.Abs(player.xVelocity);
                    break;
            }
            if (num > Math.Abs(player.yVelocity))
            {
                cSensor[0].collided = (byte)0;
                cSensor[1].collided = (byte)0;
                cSensor[0].xPos = player.xPos + (PlayerSystem.collisionRight << 16);
                cSensor[1].xPos = cSensor[0].xPos;
                cSensor[0].yPos = player.yPos - 131072;
                cSensor[1].yPos = player.yPos + 524288;
                for (int index = 0; index < 2; ++index)
                {
                    if (cSensor[index].xPos >= cLeft && player.xPos - player.xVelocity < cLeft && (cSensor[1].yPos > cTop && cSensor[0].yPos < cBottom))
                        cSensor[index].collided = (byte)1;
                }
                if (cSensor[0].collided == (byte)1 || cSensor[1].collided == (byte)1)
                {
                    player.xPos = cLeft - (PlayerSystem.collisionRight << 16);
                    if (player.xVelocity > 0)
                    {
                        if (player.objectPtr.direction == (byte)0)
                            player.pushing = (byte)2;
                        player.xVelocity = 0;
                        player.speed = 0;
                    }
                    scriptEng.checkResult = 2;
                }
                else
                {
                    cSensor[0].collided = (byte)0;
                    cSensor[1].collided = (byte)0;
                    cSensor[0].xPos = player.xPos + (PlayerSystem.collisionLeft << 16);
                    cSensor[1].xPos = cSensor[0].xPos;
                    cSensor[0].yPos = player.yPos - 131072;
                    cSensor[1].yPos = player.yPos + 524288;
                    for (int index = 0; index < 2; ++index)
                    {
                        if (cSensor[index].xPos <= cRight && player.xPos - player.xVelocity > cRight && (cSensor[1].yPos > cTop && cSensor[0].yPos < cBottom))
                            cSensor[index].collided = (byte)1;
                    }
                    if (cSensor[0].collided == (byte)1 || cSensor[1].collided == (byte)1)
                    {
                        player.xPos = cRight - (PlayerSystem.collisionLeft << 16);
                        if (player.xVelocity < 0)
                        {
                            if (player.objectPtr.direction == (byte)1)
                                player.pushing = (byte)2;
                            player.xVelocity = 0;
                            player.speed = 0;
                        }
                        scriptEng.checkResult = 3;
                    }
                    else
                    {
                        cSensor[0].collided = (byte)0;
                        cSensor[1].collided = (byte)0;
                        cSensor[2].collided = (byte)0;
                        cSensor[0].xPos = player.xPos + (PlayerSystem.collisionLeft + 2 << 16);
                        cSensor[1].xPos = player.xPos;
                        cSensor[2].xPos = player.xPos + (PlayerSystem.collisionRight - 2 << 16);
                        cSensor[0].yPos = player.yPos + (PlayerSystem.collisionBottom << 16);
                        cSensor[1].yPos = cSensor[0].yPos;
                        cSensor[2].yPos = cSensor[0].yPos;
                        if (player.yVelocity > -1)
                        {
                            for (int index = 0; index < 3; ++index)
                            {
                                if (cSensor[index].xPos > cLeft && cSensor[index].xPos < cRight && (cSensor[index].yPos >= cTop && player.yPos - player.yVelocity < cTop))
                                {
                                    cSensor[index].collided = (byte)1;
                                    player.flailing[index] = (byte)1;
                                }
                            }
                        }
                        if (cSensor[0].collided == (byte)1 || cSensor[1].collided == (byte)1 || cSensor[2].collided == (byte)1)
                        {
                            if (player.gravity == (byte)0 && (player.collisionMode == (byte)1 || player.collisionMode == (byte)3))
                            {
                                player.xVelocity = 0;
                                player.speed = 0;
                            }
                            player.yPos = cTop - (PlayerSystem.collisionBottom << 16);
                            player.gravity = (byte)0;
                            player.yVelocity = 0;
                            player.angle = 0;
                            player.objectPtr.rotation = 0;
                            player.controlLock = (byte)0;
                            scriptEng.checkResult = 1;
                        }
                        else
                        {
                            cSensor[0].collided = (byte)0;
                            cSensor[1].collided = (byte)0;
                            cSensor[0].xPos = player.xPos + (PlayerSystem.collisionLeft + 2 << 16);
                            cSensor[1].xPos = player.xPos + (PlayerSystem.collisionRight - 2 << 16);
                            cSensor[0].yPos = player.yPos + (PlayerSystem.collisionTop << 16);
                            cSensor[1].yPos = cSensor[0].yPos;
                            for (int index = 0; index < 2; ++index)
                            {
                                if (cSensor[index].xPos > cLeft && cSensor[index].xPos < cRight && (cSensor[index].yPos <= cBottom && player.yPos - player.yVelocity > cBottom))
                                    cSensor[index].collided = (byte)1;
                            }
                            if (cSensor[0].collided != (byte)1 && cSensor[1].collided != (byte)1)
                                return;
                            if (player.gravity == (byte)1)
                                player.yPos = cBottom - (PlayerSystem.collisionTop << 16);
                            if (player.yVelocity < 1)
                                player.yVelocity = 0;
                            scriptEng.checkResult = 4;
                        }
                    }
                }
            }
            else
            {
                cSensor[0].collided = (byte)0;
                cSensor[1].collided = (byte)0;
                cSensor[2].collided = (byte)0;
                cSensor[0].xPos = player.xPos + (PlayerSystem.collisionLeft + 2 << 16);
                cSensor[1].xPos = player.xPos;
                cSensor[2].xPos = player.xPos + (PlayerSystem.collisionRight - 2 << 16);
                cSensor[0].yPos = player.yPos + (PlayerSystem.collisionBottom << 16);
                cSensor[1].yPos = cSensor[0].yPos;
                cSensor[2].yPos = cSensor[0].yPos;
                if (player.yVelocity > -1)
                {
                    for (int index = 0; index < 3; ++index)
                    {
                        if (cSensor[index].xPos > cLeft && cSensor[index].xPos < cRight && (cSensor[index].yPos >= cTop && player.yPos - player.yVelocity < cTop))
                        {
                            cSensor[index].collided = (byte)1;
                            player.flailing[index] = (byte)1;
                        }
                    }
                }
                if (cSensor[0].collided == (byte)1 || cSensor[1].collided == (byte)1 || cSensor[2].collided == (byte)1)
                {
                    if (player.gravity == (byte)0 && (player.collisionMode == (byte)1 || player.collisionMode == (byte)3))
                    {
                        player.xVelocity = 0;
                        player.speed = 0;
                    }
                    player.yPos = cTop - (PlayerSystem.collisionBottom << 16);
                    player.gravity = (byte)0;
                    player.yVelocity = 0;
                    player.angle = 0;
                    player.objectPtr.rotation = 0;
                    player.controlLock = (byte)0;
                    scriptEng.checkResult = 1;
                }
                else
                {
                    cSensor[0].collided = (byte)0;
                    cSensor[1].collided = (byte)0;
                    cSensor[0].xPos = player.xPos + (PlayerSystem.collisionLeft + 2 << 16);
                    cSensor[1].xPos = player.xPos + (PlayerSystem.collisionRight - 2 << 16);
                    cSensor[0].yPos = player.yPos + (PlayerSystem.collisionTop << 16);
                    cSensor[1].yPos = cSensor[0].yPos;
                    for (int index = 0; index < 2; ++index)
                    {
                        if (cSensor[index].xPos > cLeft && cSensor[index].xPos < cRight && (cSensor[index].yPos <= cBottom && player.yPos - player.yVelocity > cBottom))
                            cSensor[index].collided = (byte)1;
                    }
                    if (cSensor[0].collided == (byte)1 || cSensor[1].collided == (byte)1)
                    {
                        if (player.gravity == (byte)1)
                            player.yPos = cBottom - (PlayerSystem.collisionTop << 16);
                        if (player.yVelocity < 1)
                            player.yVelocity = 0;
                        scriptEng.checkResult = 4;
                    }
                    else
                    {
                        cSensor[0].collided = (byte)0;
                        cSensor[1].collided = (byte)0;
                        cSensor[0].xPos = player.xPos + (PlayerSystem.collisionRight << 16);
                        cSensor[1].xPos = cSensor[0].xPos;
                        cSensor[0].yPos = player.yPos - 131072;
                        cSensor[1].yPos = player.yPos + 524288;
                        for (int index = 0; index < 2; ++index)
                        {
                            if (cSensor[index].xPos >= cLeft && player.xPos - player.xVelocity < cLeft && (cSensor[1].yPos > cTop && cSensor[0].yPos < cBottom))
                                cSensor[index].collided = (byte)1;
                        }
                        if (cSensor[0].collided == (byte)1 || cSensor[1].collided == (byte)1)
                        {
                            player.xPos = cLeft - (PlayerSystem.collisionRight << 16);
                            if (player.xVelocity > 0)
                            {
                                if (player.objectPtr.direction == (byte)0)
                                    player.pushing = (byte)2;
                                player.xVelocity = 0;
                                player.speed = 0;
                            }
                            scriptEng.checkResult = 2;
                        }
                        else
                        {
                            cSensor[0].collided = (byte)0;
                            cSensor[1].collided = (byte)0;
                            cSensor[0].xPos = player.xPos + (PlayerSystem.collisionLeft << 16);
                            cSensor[1].xPos = cSensor[0].xPos;
                            cSensor[0].yPos = player.yPos - 131072;
                            cSensor[1].yPos = player.yPos + 524288;
                            for (int index = 0; index < 2; ++index)
                            {
                                if (cSensor[index].xPos <= cRight && player.xPos - player.xVelocity > cRight && (cSensor[1].yPos > cTop && cSensor[0].yPos < cBottom))
                                    cSensor[index].collided = (byte)1;
                            }
                            if (cSensor[0].collided != (byte)1 && cSensor[1].collided != (byte)1)
                                return;
                            player.xPos = cRight - (PlayerSystem.collisionLeft << 16);
                            if (player.xVelocity < 0)
                            {
                                if (player.objectPtr.direction == (byte)1)
                                    player.pushing = (byte)2;
                                player.xVelocity = 0;
                                player.speed = 0;
                            }
                            scriptEng.checkResult = 3;
                        }
                    }
                }
            }
        }

        public static void PlatformCollision(int cLeft, int cTop, int cRight, int cBottom)
        {
            PlayerObject player = PlayerSystem.playerList[playerNum];
            CollisionBox collisionBox = AnimationSystem.collisionBoxList[player.animationFile.cbListOffset + (int)AnimationSystem.animationFrames[AnimationSystem.animationList[player.animationFile.aniListOffset + (int)player.objectPtr.animation].frameListOffset + (int)player.objectPtr.frame].collisionBox];
            PlayerSystem.collisionLeft = (int)collisionBox.left[0];
            PlayerSystem.collisionTop = (int)collisionBox.top[0];
            PlayerSystem.collisionRight = (int)collisionBox.right[0];
            PlayerSystem.collisionBottom = (int)collisionBox.bottom[0];
            cSensor[0].collided = (byte)0;
            cSensor[1].collided = (byte)0;
            cSensor[2].collided = (byte)0;
            cSensor[0].xPos = player.xPos + (PlayerSystem.collisionLeft + 1 << 16);
            cSensor[1].xPos = player.xPos;
            cSensor[2].xPos = player.xPos + (PlayerSystem.collisionRight << 16);
            cSensor[0].yPos = player.yPos + (PlayerSystem.collisionBottom << 16);
            cSensor[1].yPos = cSensor[0].yPos;
            cSensor[2].yPos = cSensor[0].yPos;
            scriptEng.checkResult = 0;
            for (int index = 0; index < 3; ++index)
            {
                if (cSensor[index].xPos > cLeft && cSensor[index].xPos < cRight && (cSensor[index].yPos > cTop - 2 && cSensor[index].yPos < cBottom) && player.yVelocity >= 0)
                {
                    cSensor[index].collided = (byte)1;
                    player.flailing[index] = (byte)1;
                }
            }
            if (cSensor[0].collided != (byte)1 && cSensor[1].collided != (byte)1 && cSensor[2].collided != (byte)1)
                return;
            if (player.gravity == (byte)0 && (player.collisionMode == (byte)1 || player.collisionMode == (byte)3))
            {
                player.xVelocity = 0;
                player.speed = 0;
            }
            player.yPos = cTop - (PlayerSystem.collisionBottom << 16);
            player.gravity = (byte)0;
            player.yVelocity = 0;
            player.angle = 0;
            player.objectPtr.rotation = 0;
            player.controlLock = (byte)0;
            scriptEng.checkResult = 1;
        }

        public static void ObjectFloorCollision(int xOffset, int yOffset, int cPlane)
        {
            scriptEng.checkResult = 0;
            int num1 = (objectEntityList[objectLoop].xPos >> 16) + xOffset;
            int num2 = (objectEntityList[objectLoop].yPos >> 16) + yOffset;
            if (num1 <= 0 || num1 >= (int)StageSystem.stageLayouts[0].xSize << 7 || (num2 <= 0 || num2 >= (int)StageSystem.stageLayouts[0].ySize << 7))
                return;
            int num3 = num1 >> 7;
            int num4 = (num1 & (int)sbyte.MaxValue) >> 4;
            int num5 = num2 >> 7;
            int num6 = (num2 & (int)sbyte.MaxValue) >> 4;
            int index1 = ((int)StageSystem.stageLayouts[0].tileMap[num3 + (num5 << 8)] << 6) + (num4 + (num6 << 3));
            int num7 = (int)StageSystem.tile128x128.tile16x16[index1];
            if (StageSystem.tile128x128.collisionFlag[cPlane, index1] != (byte)2 && StageSystem.tile128x128.collisionFlag[cPlane, index1] != (byte)3)
            {
                switch (StageSystem.tile128x128.direction[index1])
                {
                    case 0:
                        int index2 = (num1 & 15) + (num7 << 4);
                        if ((num2 & 15) > (int)StageSystem.tileCollisions[cPlane].floorMask[index2])
                        {
                            num2 = (int)StageSystem.tileCollisions[cPlane].floorMask[index2] + (num5 << 7) + (num6 << 4);
                            scriptEng.checkResult = 1;
                            break;
                        }
                        break;
                    case 1:
                        int index3 = 15 - (num1 & 15) + (num7 << 4);
                        if ((num2 & 15) > (int)StageSystem.tileCollisions[cPlane].floorMask[index3])
                        {
                            num2 = (int)StageSystem.tileCollisions[cPlane].floorMask[index3] + (num5 << 7) + (num6 << 4);
                            scriptEng.checkResult = 1;
                            break;
                        }
                        break;
                    case 2:
                        int index4 = (num1 & 15) + (num7 << 4);
                        if ((num2 & 15) > 15 - (int)StageSystem.tileCollisions[cPlane].roofMask[index4])
                        {
                            num2 = 15 - (int)StageSystem.tileCollisions[cPlane].roofMask[index4] + (num5 << 7) + (num6 << 4);
                            scriptEng.checkResult = 1;
                            break;
                        }
                        break;
                    case 3:
                        int index5 = 15 - (num1 & 15) + (num7 << 4);
                        if ((num2 & 15) > 15 - (int)StageSystem.tileCollisions[cPlane].roofMask[index5])
                        {
                            num2 = 15 - (int)StageSystem.tileCollisions[cPlane].roofMask[index5] + (num5 << 7) + (num6 << 4);
                            scriptEng.checkResult = 1;
                            break;
                        }
                        break;
                }
            }
            if (scriptEng.checkResult != 1)
                return;
            objectEntityList[objectLoop].yPos = num2 - yOffset << 16;
        }

        public static void ObjectLWallCollision(int xOffset, int yOffset, int cPlane)
        {
            scriptEng.checkResult = 0;
            int num1 = (objectEntityList[objectLoop].xPos >> 16) + xOffset;
            int num2 = (objectEntityList[objectLoop].yPos >> 16) + yOffset;
            if (num1 <= 0 || num1 >= (int)StageSystem.stageLayouts[0].xSize << 7 || (num2 <= 0 || num2 >= (int)StageSystem.stageLayouts[0].ySize << 7))
                return;
            int num3 = num1 >> 7;
            int num4 = (num1 & (int)sbyte.MaxValue) >> 4;
            int num5 = num2 >> 7;
            int num6 = (num2 & (int)sbyte.MaxValue) >> 4;
            int index1 = ((int)StageSystem.stageLayouts[0].tileMap[num3 + (num5 << 8)] << 6) + (num4 + (num6 << 3));
            int num7 = (int)StageSystem.tile128x128.tile16x16[index1];
            if (StageSystem.tile128x128.collisionFlag[cPlane, index1] != (byte)1 && StageSystem.tile128x128.collisionFlag[cPlane, index1] < (byte)3)
            {
                switch (StageSystem.tile128x128.direction[index1])
                {
                    case 0:
                        int index2 = (num2 & 15) + (num7 << 4);
                        if ((num1 & 15) > (int)StageSystem.tileCollisions[cPlane].leftWallMask[index2])
                        {
                            num1 = (int)StageSystem.tileCollisions[cPlane].leftWallMask[index2] + (num3 << 7) + (num4 << 4);
                            scriptEng.checkResult = 1;
                            break;
                        }
                        break;
                    case 1:
                        int index3 = (num2 & 15) + (num7 << 4);
                        if ((num1 & 15) > 15 - (int)StageSystem.tileCollisions[cPlane].rightWallMask[index3])
                        {
                            num1 = 15 - (int)StageSystem.tileCollisions[cPlane].rightWallMask[index3] + (num3 << 7) + (num4 << 4);
                            scriptEng.checkResult = 1;
                            break;
                        }
                        break;
                    case 2:
                        int index4 = 15 - (num2 & 15) + (num7 << 4);
                        if ((num1 & 15) > (int)StageSystem.tileCollisions[cPlane].leftWallMask[index4])
                        {
                            num1 = (int)StageSystem.tileCollisions[cPlane].leftWallMask[index4] + (num3 << 7) + (num4 << 4);
                            scriptEng.checkResult = 1;
                            break;
                        }
                        break;
                    case 3:
                        int index5 = 15 - (num2 & 15) + (num7 << 4);
                        if ((num1 & 15) > 15 - (int)StageSystem.tileCollisions[cPlane].rightWallMask[index5])
                        {
                            num1 = 15 - (int)StageSystem.tileCollisions[cPlane].rightWallMask[index5] + (num3 << 7) + (num4 << 4);
                            scriptEng.checkResult = 1;
                            break;
                        }
                        break;
                }
            }
            if (scriptEng.checkResult != 1)
                return;
            objectEntityList[objectLoop].xPos = num1 - xOffset << 16;
        }

        public static void ObjectRWallCollision(int xOffset, int yOffset, int cPlane)
        {
            scriptEng.checkResult = 0;
            int num1 = (objectEntityList[objectLoop].xPos >> 16) + xOffset;
            int num2 = (objectEntityList[objectLoop].yPos >> 16) + yOffset;
            if (num1 <= 0 || num1 >= (int)StageSystem.stageLayouts[0].xSize << 7 || (num2 <= 0 || num2 >= (int)StageSystem.stageLayouts[0].ySize << 7))
                return;
            int num3 = num1 >> 7;
            int num4 = (num1 & (int)sbyte.MaxValue) >> 4;
            int num5 = num2 >> 7;
            int num6 = (num2 & (int)sbyte.MaxValue) >> 4;
            int index1 = ((int)StageSystem.stageLayouts[0].tileMap[num3 + (num5 << 8)] << 6) + (num4 + (num6 << 3));
            int num7 = (int)StageSystem.tile128x128.tile16x16[index1];
            if (StageSystem.tile128x128.collisionFlag[cPlane, index1] != (byte)1 && StageSystem.tile128x128.collisionFlag[cPlane, index1] < (byte)3)
            {
                switch (StageSystem.tile128x128.direction[index1])
                {
                    case 0:
                        int index2 = (num2 & 15) + (num7 << 4);
                        if ((num1 & 15) < (int)StageSystem.tileCollisions[cPlane].rightWallMask[index2])
                        {
                            num1 = (int)StageSystem.tileCollisions[cPlane].rightWallMask[index2] + (num3 << 7) + (num4 << 4);
                            scriptEng.checkResult = 1;
                            break;
                        }
                        break;
                    case 1:
                        int index3 = (num2 & 15) + (num7 << 4);
                        if ((num1 & 15) < 15 - (int)StageSystem.tileCollisions[cPlane].leftWallMask[index3])
                        {
                            num1 = 15 - (int)StageSystem.tileCollisions[cPlane].leftWallMask[index3] + (num3 << 7) + (num4 << 4);
                            scriptEng.checkResult = 1;
                            break;
                        }
                        break;
                    case 2:
                        int index4 = 15 - (num2 & 15) + (num7 << 4);
                        if ((num1 & 15) < (int)StageSystem.tileCollisions[cPlane].rightWallMask[index4])
                        {
                            num1 = (int)StageSystem.tileCollisions[cPlane].rightWallMask[index4] + (num3 << 7) + (num4 << 4);
                            scriptEng.checkResult = 1;
                            break;
                        }
                        break;
                    case 3:
                        int index5 = 15 - (num2 & 15) + (num7 << 4);
                        if ((num1 & 15) < 15 - (int)StageSystem.tileCollisions[cPlane].leftWallMask[index5])
                        {
                            num1 = 15 - (int)StageSystem.tileCollisions[cPlane].leftWallMask[index5] + (num3 << 7) + (num4 << 4);
                            scriptEng.checkResult = 1;
                            break;
                        }
                        break;
                }
            }
            if (scriptEng.checkResult != 1)
                return;
            objectEntityList[objectLoop].xPos = num1 - xOffset << 16;
        }

        public static void ObjectRoofCollision(int xOffset, int yOffset, int cPlane)
        {
            scriptEng.checkResult = 0;
            int num1 = (objectEntityList[objectLoop].xPos >> 16) + xOffset;
            int num2 = (objectEntityList[objectLoop].yPos >> 16) + yOffset;
            if (num1 <= 0 || num1 >= (int)StageSystem.stageLayouts[0].xSize << 7 || (num2 <= 0 || num2 >= (int)StageSystem.stageLayouts[0].ySize << 7))
                return;
            int num3 = num1 >> 7;
            int num4 = (num1 & (int)sbyte.MaxValue) >> 4;
            int num5 = num2 >> 7;
            int num6 = (num2 & (int)sbyte.MaxValue) >> 4;
            int index1 = ((int)StageSystem.stageLayouts[0].tileMap[num3 + (num5 << 8)] << 6) + (num4 + (num6 << 3));
            int num7 = (int)StageSystem.tile128x128.tile16x16[index1];
            if (StageSystem.tile128x128.collisionFlag[cPlane, index1] != (byte)1 && StageSystem.tile128x128.collisionFlag[cPlane, index1] < (byte)3)
            {
                switch (StageSystem.tile128x128.direction[index1])
                {
                    case 0:
                        int index2 = (num1 & 15) + (num7 << 4);
                        if ((num2 & 15) < (int)StageSystem.tileCollisions[cPlane].roofMask[index2])
                        {
                            num2 = (int)StageSystem.tileCollisions[cPlane].roofMask[index2] + (num5 << 7) + (num6 << 4);
                            scriptEng.checkResult = 1;
                            break;
                        }
                        break;
                    case 1:
                        int index3 = 15 - (num1 & 15) + (num7 << 4);
                        if ((num2 & 15) < (int)StageSystem.tileCollisions[cPlane].roofMask[index3])
                        {
                            num2 = (int)StageSystem.tileCollisions[cPlane].roofMask[index3] + (num5 << 7) + (num6 << 4);
                            scriptEng.checkResult = 1;
                            break;
                        }
                        break;
                    case 2:
                        int index4 = (num1 & 15) + (num7 << 4);
                        if ((num2 & 15) < 15 - (int)StageSystem.tileCollisions[cPlane].floorMask[index4])
                        {
                            num2 = 15 - (int)StageSystem.tileCollisions[cPlane].floorMask[index4] + (num5 << 7) + (num6 << 4);
                            scriptEng.checkResult = 1;
                            break;
                        }
                        break;
                    case 3:
                        int index5 = 15 - (num1 & 15) + (num7 << 4);
                        if ((num2 & 15) < 15 - (int)StageSystem.tileCollisions[cPlane].floorMask[index5])
                        {
                            num2 = 15 - (int)StageSystem.tileCollisions[cPlane].floorMask[index5] + (num5 << 7) + (num6 << 4);
                            scriptEng.checkResult = 1;
                            break;
                        }
                        break;
                }
            }
            if (scriptEng.checkResult != 1)
                return;
            objectEntityList[objectLoop].yPos = num2 - yOffset << 16;
        }

        public static void ObjectFloorGrip(int xOffset, int yOffset, int cPlane)
        {
            scriptEng.checkResult = 0;
            int num1 = (objectEntityList[objectLoop].xPos >> 16) + xOffset;
            int num2 = (objectEntityList[objectLoop].yPos >> 16) + yOffset;
            int num3 = num2;
            int num4 = num2 - 16;
            for (int index1 = 3; index1 > 0; --index1)
            {
                if (num1 > 0 && num1 < (int)StageSystem.stageLayouts[0].xSize << 7 && (num4 > 0 && num4 < (int)StageSystem.stageLayouts[0].ySize << 7) && scriptEng.checkResult == 0)
                {
                    int num5 = num1 >> 7;
                    int num6 = (num1 & (int)sbyte.MaxValue) >> 4;
                    int num7 = num4 >> 7;
                    int num8 = (num4 & (int)sbyte.MaxValue) >> 4;
                    int index2 = ((int)StageSystem.stageLayouts[0].tileMap[num5 + (num7 << 8)] << 6) + (num6 + (num8 << 3));
                    int num9 = (int)StageSystem.tile128x128.tile16x16[index2];
                    if (StageSystem.tile128x128.collisionFlag[cPlane, index2] != (byte)2 && StageSystem.tile128x128.collisionFlag[cPlane, index2] != (byte)3)
                    {
                        switch (StageSystem.tile128x128.direction[index2])
                        {
                            case 0:
                                int index3 = (num1 & 15) + (num9 << 4);
                                if (StageSystem.tileCollisions[cPlane].floorMask[index3] < (sbyte)64)
                                {
                                    objectEntityList[objectLoop].yPos = (int)StageSystem.tileCollisions[cPlane].floorMask[index3] + (num7 << 7) + (num8 << 4);
                                    scriptEng.checkResult = 1;
                                    break;
                                }
                                break;
                            case 1:
                                int index4 = 15 - (num1 & 15) + (num9 << 4);
                                if (StageSystem.tileCollisions[cPlane].floorMask[index4] < (sbyte)64)
                                {
                                    objectEntityList[objectLoop].yPos = (int)StageSystem.tileCollisions[cPlane].floorMask[index4] + (num7 << 7) + (num8 << 4);
                                    scriptEng.checkResult = 1;
                                    break;
                                }
                                break;
                            case 2:
                                int index5 = (num1 & 15) + (num9 << 4);
                                if (StageSystem.tileCollisions[cPlane].roofMask[index5] > (sbyte)-64)
                                {
                                    objectEntityList[objectLoop].yPos = 15 - (int)StageSystem.tileCollisions[cPlane].roofMask[index5] + (num7 << 7) + (num8 << 4);
                                    scriptEng.checkResult = 1;
                                    break;
                                }
                                break;
                            case 3:
                                int index6 = 15 - (num1 & 15) + (num9 << 4);
                                if (StageSystem.tileCollisions[cPlane].roofMask[index6] > (sbyte)-64)
                                {
                                    objectEntityList[objectLoop].yPos = 15 - (int)StageSystem.tileCollisions[cPlane].roofMask[index6] + (num7 << 7) + (num8 << 4);
                                    scriptEng.checkResult = 1;
                                    break;
                                }
                                break;
                        }
                    }
                }
                num4 += 16;
            }
            if (scriptEng.checkResult != 1)
                return;
            if (Math.Abs(objectEntityList[objectLoop].yPos - num3) < 16)
            {
                objectEntityList[objectLoop].yPos = objectEntityList[objectLoop].yPos - yOffset << 16;
            }
            else
            {
                objectEntityList[objectLoop].yPos = num3 - yOffset << 16;
                scriptEng.checkResult = 0;
            }
        }

        public static void ObjectLWallGrip(int xOffset, int yOffset, int cPlane)
        {
            scriptEng.checkResult = 0;
            int num1 = (objectEntityList[objectLoop].xPos >> 16) + xOffset;
            int num2 = (objectEntityList[objectLoop].yPos >> 16) + yOffset;
            int num3 = num1;
            int num4 = num1 - 16;
            for (int index1 = 3; index1 > 0; --index1)
            {
                if (num4 > 0 && num4 < (int)StageSystem.stageLayouts[0].xSize << 7 && (num2 > 0 && num2 < (int)StageSystem.stageLayouts[0].ySize << 7) && scriptEng.checkResult == 0)
                {
                    int num5 = num4 >> 7;
                    int num6 = (num4 & (int)sbyte.MaxValue) >> 4;
                    int num7 = num2 >> 7;
                    int num8 = (num2 & (int)sbyte.MaxValue) >> 4;
                    int index2 = ((int)StageSystem.stageLayouts[0].tileMap[num5 + (num7 << 8)] << 6) + (num6 + (num8 << 3));
                    int num9 = (int)StageSystem.tile128x128.tile16x16[index2];
                    if (StageSystem.tile128x128.collisionFlag[cPlane, index2] < (byte)3)
                    {
                        switch (StageSystem.tile128x128.direction[index2])
                        {
                            case 0:
                                int index3 = (num2 & 15) + (num9 << 4);
                                if (StageSystem.tileCollisions[cPlane].leftWallMask[index3] < (sbyte)64)
                                {
                                    objectEntityList[objectLoop].xPos = (int)StageSystem.tileCollisions[cPlane].leftWallMask[index3] + (num5 << 7) + (num6 << 4);
                                    scriptEng.checkResult = 1;
                                    break;
                                }
                                break;
                            case 1:
                                int index4 = (num2 & 15) + (num9 << 4);
                                if (StageSystem.tileCollisions[cPlane].rightWallMask[index4] > (sbyte)-64)
                                {
                                    objectEntityList[objectLoop].xPos = 15 - (int)StageSystem.tileCollisions[cPlane].rightWallMask[index4] + (num5 << 7) + (num6 << 4);
                                    scriptEng.checkResult = 1;
                                    break;
                                }
                                break;
                            case 2:
                                int index5 = 15 - (num2 & 15) + (num9 << 4);
                                if (StageSystem.tileCollisions[cPlane].leftWallMask[index5] < (sbyte)64)
                                {
                                    objectEntityList[objectLoop].xPos = (int)StageSystem.tileCollisions[cPlane].leftWallMask[index5] + (num5 << 7) + (num6 << 4);
                                    scriptEng.checkResult = 1;
                                    break;
                                }
                                break;
                            case 3:
                                int index6 = 15 - (num2 & 15) + (num9 << 4);
                                if (StageSystem.tileCollisions[cPlane].rightWallMask[index6] > (sbyte)-64)
                                {
                                    objectEntityList[objectLoop].xPos = 15 - (int)StageSystem.tileCollisions[cPlane].rightWallMask[index6] + (num5 << 7) + (num6 << 4);
                                    scriptEng.checkResult = 1;
                                    break;
                                }
                                break;
                        }
                    }
                }
                num4 += 16;
            }
            if (scriptEng.checkResult != 1)
                return;
            if (Math.Abs(objectEntityList[objectLoop].xPos - num3) < 16)
            {
                objectEntityList[objectLoop].xPos = objectEntityList[objectLoop].xPos - xOffset << 16;
            }
            else
            {
                objectEntityList[objectLoop].xPos = num3 - xOffset << 16;
                scriptEng.checkResult = 0;
            }
        }

        public static void ObjectRWallGrip(int xOffset, int yOffset, int cPlane)
        {
            scriptEng.checkResult = 0;
            int num1 = (objectEntityList[objectLoop].xPos >> 16) + xOffset;
            int num2 = (objectEntityList[objectLoop].yPos >> 16) + yOffset;
            int num3 = num1;
            int num4 = num1 + 16;
            for (int index1 = 3; index1 > 0; --index1)
            {
                if (num4 > 0 && num4 < (int)StageSystem.stageLayouts[0].xSize << 7 && (num2 > 0 && num2 < (int)StageSystem.stageLayouts[0].ySize << 7) && scriptEng.checkResult == 0)
                {
                    int num5 = num4 >> 7;
                    int num6 = (num4 & (int)sbyte.MaxValue) >> 4;
                    int num7 = num2 >> 7;
                    int num8 = (num2 & (int)sbyte.MaxValue) >> 4;
                    int index2 = ((int)StageSystem.stageLayouts[0].tileMap[num5 + (num7 << 8)] << 6) + (num6 + (num8 << 3));
                    int num9 = (int)StageSystem.tile128x128.tile16x16[index2];
                    if (StageSystem.tile128x128.collisionFlag[cPlane, index2] < (byte)3)
                    {
                        switch (StageSystem.tile128x128.direction[index2])
                        {
                            case 0:
                                int index3 = (num2 & 15) + (num9 << 4);
                                if (StageSystem.tileCollisions[cPlane].rightWallMask[index3] > (sbyte)-64)
                                {
                                    objectEntityList[objectLoop].xPos = (int)StageSystem.tileCollisions[cPlane].rightWallMask[index3] + (num5 << 7) + (num6 << 4);
                                    scriptEng.checkResult = 1;
                                    break;
                                }
                                break;
                            case 1:
                                int index4 = (num2 & 15) + (num9 << 4);
                                if (StageSystem.tileCollisions[cPlane].leftWallMask[index4] < (sbyte)64)
                                {
                                    objectEntityList[objectLoop].xPos = 15 - (int)StageSystem.tileCollisions[cPlane].leftWallMask[index4] + (num5 << 7) + (num6 << 4);
                                    scriptEng.checkResult = 1;
                                    break;
                                }
                                break;
                            case 2:
                                int index5 = 15 - (num2 & 15) + (num9 << 4);
                                if (StageSystem.tileCollisions[cPlane].rightWallMask[index5] > (sbyte)-64)
                                {
                                    objectEntityList[objectLoop].xPos = (int)StageSystem.tileCollisions[cPlane].rightWallMask[index5] + (num5 << 7) + (num6 << 4);
                                    scriptEng.checkResult = 1;
                                    break;
                                }
                                break;
                            case 3:
                                int index6 = 15 - (num2 & 15) + (num9 << 4);
                                if (StageSystem.tileCollisions[cPlane].leftWallMask[index6] < (sbyte)64)
                                {
                                    objectEntityList[objectLoop].xPos = 15 - (int)StageSystem.tileCollisions[cPlane].leftWallMask[index6] + (num5 << 7) + (num6 << 4);
                                    scriptEng.checkResult = 1;
                                    break;
                                }
                                break;
                        }
                    }
                }
                num4 -= 16;
            }
            if (scriptEng.checkResult != 1)
                return;
            if (Math.Abs(objectEntityList[objectLoop].xPos - num3) < 16)
            {
                objectEntityList[objectLoop].xPos = objectEntityList[objectLoop].xPos - xOffset << 16;
            }
            else
            {
                objectEntityList[objectLoop].xPos = num3 - xOffset << 16;
                scriptEng.checkResult = 0;
            }
        }

        public static void ObjectRoofGrip(int xOffset, int yOffset, int cPlane)
        {
            scriptEng.checkResult = 0;
            int num1 = (objectEntityList[objectLoop].xPos >> 16) + xOffset;
            int num2 = (objectEntityList[objectLoop].yPos >> 16) + yOffset;
            int num3 = num2;
            int num4 = num2 + 16;
            for (int index1 = 3; index1 > 0; --index1)
            {
                if (num1 > 0 && num1 < (int)StageSystem.stageLayouts[0].xSize << 7 && (num4 > 0 && num4 < (int)StageSystem.stageLayouts[0].ySize << 7) && scriptEng.checkResult == 0)
                {
                    int num5 = num1 >> 7;
                    int num6 = (num1 & (int)sbyte.MaxValue) >> 4;
                    int num7 = num4 >> 7;
                    int num8 = (num4 & (int)sbyte.MaxValue) >> 4;
                    int index2 = ((int)StageSystem.stageLayouts[0].tileMap[num5 + (num7 << 8)] << 6) + (num6 + (num8 << 3));
                    int num9 = (int)StageSystem.tile128x128.tile16x16[index2];
                    if (StageSystem.tile128x128.collisionFlag[cPlane, index2] < (byte)3)
                    {
                        switch (StageSystem.tile128x128.direction[index2])
                        {
                            case 0:
                                int index3 = (num1 & 15) + (num9 << 4);
                                if (StageSystem.tileCollisions[cPlane].roofMask[index3] > (sbyte)-64)
                                {
                                    objectEntityList[objectLoop].yPos = (int)StageSystem.tileCollisions[cPlane].roofMask[index3] + (num7 << 7) + (num8 << 4);
                                    scriptEng.checkResult = 1;
                                    break;
                                }
                                break;
                            case 1:
                                int index4 = 15 - (num1 & 15) + (num9 << 4);
                                if (StageSystem.tileCollisions[cPlane].roofMask[index4] > (sbyte)-64)
                                {
                                    objectEntityList[objectLoop].yPos = (int)StageSystem.tileCollisions[cPlane].roofMask[index4] + (num7 << 7) + (num8 << 4);
                                    scriptEng.checkResult = 1;
                                    break;
                                }
                                break;
                            case 2:
                                int index5 = (num1 & 15) + (num9 << 4);
                                if (StageSystem.tileCollisions[cPlane].floorMask[index5] < (sbyte)64)
                                {
                                    objectEntityList[objectLoop].yPos = 15 - (int)StageSystem.tileCollisions[cPlane].floorMask[index5] + (num7 << 7) + (num8 << 4);
                                    scriptEng.checkResult = 1;
                                    break;
                                }
                                break;
                            case 3:
                                int index6 = 15 - (num1 & 15) + (num9 << 4);
                                if (StageSystem.tileCollisions[cPlane].floorMask[index6] < (sbyte)64)
                                {
                                    objectEntityList[objectLoop].yPos = 15 - (int)StageSystem.tileCollisions[cPlane].floorMask[index6] + (num7 << 7) + (num8 << 4);
                                    scriptEng.checkResult = 1;
                                    break;
                                }
                                break;
                        }
                    }
                }
                num4 -= 16;
            }
            if (scriptEng.checkResult != 1)
                return;
            if (Math.Abs(objectEntityList[objectLoop].yPos - num3) < 16)
            {
                objectEntityList[objectLoop].yPos = objectEntityList[objectLoop].yPos - yOffset << 16;
            }
            else
            {
                objectEntityList[objectLoop].yPos = num3 - yOffset << 16;
                scriptEng.checkResult = 0;
            }
        }
    }
}
