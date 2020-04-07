// Decompiled with JetBrains decompiler
// Type: Retro_Engine.PlayerSystem
// Assembly: Sonic CD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D35AF46A-1892-4F52-B201-E664C9200079
// Assembly location: E:\wamwo\Downloads\Sonic CD Mod Via Rebel\Sonic CD Mod Via Rebel\Sonic CD.dll

using System;

namespace RetroEngine
{
    public static class PlayerSystem
    {
        public static byte numActivePlayers = 1;
        public static byte playerMenuNum = 0;
        public static PlayerObject[] playerList = new PlayerObject[2];
        public static CollisionSensor[] cSensor = new CollisionSensor[6];
        public const int MAX_PLAYERS = 2;
        public static ushort delayLeft;
        public static ushort delayRight;
        public static ushort delayUp;
        public static ushort delayDown;
        public static ushort delayJumpPress;
        public static ushort delayJumpHold;
        public static byte jumpWait;
        public static int collisionLeft;
        public static int collisionTop;
        public static int collisionRight;
        public static int collisionBottom;

        static PlayerSystem()
        {
            for (int index = 0; index < PlayerSystem.playerList.Length; ++index)
                PlayerSystem.playerList[index] = new PlayerObject();
            for (int index = 0; index < PlayerSystem.cSensor.Length; ++index)
                PlayerSystem.cSensor[index] = new CollisionSensor();
        }

        public static void SetPlayerScreenPosition(PlayerObject playerO)
        {
            int num1 = playerO.xPos >> 16;
            int num2 = playerO.yPos >> 16;
            if (StageSystem.newYBoundary1 > StageSystem.yBoundary1)
                StageSystem.yBoundary1 = StageSystem.yScrollOffset <= StageSystem.newYBoundary1 ? StageSystem.yScrollOffset : StageSystem.newYBoundary1;
            if (StageSystem.newYBoundary1 < StageSystem.yBoundary1)
            {
                if (StageSystem.yScrollOffset > StageSystem.yBoundary1)
                    StageSystem.yBoundary1 = StageSystem.newYBoundary1;
                else
                    --StageSystem.yBoundary1;
            }
            if (StageSystem.newYBoundary2 < StageSystem.yBoundary2)
            {
                if (StageSystem.yScrollOffset + 240 < StageSystem.yBoundary2 && StageSystem.yScrollOffset + 240 > StageSystem.newYBoundary2)
                    StageSystem.yBoundary2 = StageSystem.yScrollOffset + 240;
                else
                    --StageSystem.yBoundary2;
            }
            if (StageSystem.newYBoundary2 > StageSystem.yBoundary2)
            {
                if (StageSystem.yScrollOffset + 240 < StageSystem.yBoundary2)
                    StageSystem.yBoundary2 = StageSystem.newYBoundary2;
                else
                    ++StageSystem.yBoundary2;
            }
            if (StageSystem.newXBoundary1 > StageSystem.xBoundary1)
                StageSystem.xBoundary1 = StageSystem.xScrollOffset <= StageSystem.newXBoundary1 ? StageSystem.xScrollOffset : StageSystem.newXBoundary1;
            if (StageSystem.newXBoundary1 < StageSystem.xBoundary1)
            {
                if (StageSystem.xScrollOffset > StageSystem.xBoundary1)
                {
                    StageSystem.xBoundary1 = StageSystem.newXBoundary1;
                }
                else
                {
                    --StageSystem.xBoundary1;
                    if (playerO.xVelocity < 0)
                    {
                        StageSystem.xBoundary1 += playerO.xVelocity >> 16;
                        if (StageSystem.xBoundary1 < StageSystem.newXBoundary1)
                            StageSystem.xBoundary1 = StageSystem.newXBoundary1;
                    }
                }
            }
            if (StageSystem.newXBoundary2 < StageSystem.xBoundary2)
                StageSystem.xBoundary2 = StageSystem.xScrollOffset + GlobalAppDefinitions.SCREEN_XSIZE >= StageSystem.xBoundary2 ? StageSystem.xScrollOffset + GlobalAppDefinitions.SCREEN_XSIZE : StageSystem.newXBoundary2;
            if (StageSystem.newXBoundary2 > StageSystem.xBoundary2)
            {
                if (StageSystem.xScrollOffset + GlobalAppDefinitions.SCREEN_XSIZE < StageSystem.xBoundary2)
                {
                    StageSystem.xBoundary2 = StageSystem.newXBoundary2;
                }
                else
                {
                    ++StageSystem.xBoundary2;
                    if (playerO.xVelocity > 0)
                    {
                        StageSystem.xBoundary2 += playerO.xVelocity >> 16;
                        if (StageSystem.xBoundary2 > StageSystem.newXBoundary2)
                            StageSystem.xBoundary2 = StageSystem.newXBoundary2;
                    }
                }
            }
            int num3 = StageSystem.xScrollA;
            int num4 = StageSystem.xScrollB;
            int num5 = num1 - (num3 + GlobalAppDefinitions.SCREEN_CENTER);
            if (Math.Abs(num5) < 25)
            {
                if (num1 > num3 + GlobalAppDefinitions.SCREEN_SCROLL_RIGHT)
                {
                    num3 += num1 - (num3 + GlobalAppDefinitions.SCREEN_SCROLL_RIGHT);
                    num4 = num3 + GlobalAppDefinitions.SCREEN_XSIZE;
                }
                if (num1 < num3 + GlobalAppDefinitions.SCREEN_SCROLL_LEFT)
                {
                    num3 -= num3 + GlobalAppDefinitions.SCREEN_SCROLL_LEFT - num1;
                    num4 = num3 + GlobalAppDefinitions.SCREEN_XSIZE;
                }
            }
            else
            {
                if (num5 > 0)
                    num3 += 16;
                else
                    num3 -= 16;
                num4 = num3 + GlobalAppDefinitions.SCREEN_XSIZE;
            }
            if (num3 < StageSystem.xBoundary1)
            {
                num3 = StageSystem.xBoundary1;
                num4 = StageSystem.xBoundary1 + GlobalAppDefinitions.SCREEN_XSIZE;
            }
            if (num4 > StageSystem.xBoundary2)
            {
                num4 = StageSystem.xBoundary2;
                num3 = StageSystem.xBoundary2 - GlobalAppDefinitions.SCREEN_XSIZE;
            }
            StageSystem.xScrollA = num3;
            StageSystem.xScrollB = num4;
            if (num1 > num3 + GlobalAppDefinitions.SCREEN_CENTER)
            {
                StageSystem.xScrollOffset = num1 - GlobalAppDefinitions.SCREEN_CENTER + StageSystem.screenShakeX;
                playerO.screenXPos = GlobalAppDefinitions.SCREEN_CENTER - StageSystem.screenShakeX;
                if (num1 > num4 - GlobalAppDefinitions.SCREEN_CENTER)
                {
                    playerO.screenXPos = GlobalAppDefinitions.SCREEN_CENTER + (num1 - (num4 - GlobalAppDefinitions.SCREEN_CENTER)) + StageSystem.screenShakeX;
                    StageSystem.xScrollOffset = num4 - GlobalAppDefinitions.SCREEN_XSIZE - StageSystem.screenShakeX;
                }
            }
            else
            {
                playerO.screenXPos = num1 - num3 + StageSystem.screenShakeX;
                StageSystem.xScrollOffset = num3 - StageSystem.screenShakeX;
            }
            int num6 = StageSystem.yScrollA;
            int num7 = StageSystem.yScrollB;
            int num8 = num2 + StageSystem.cameraAdjustY;
            int num9 = num8 + playerO.lookPos - (num6 + 104);
            if (playerO.trackScroll == (byte)1)
            {
                StageSystem.yScrollMove = 32;
            }
            else
            {
                if (StageSystem.yScrollMove == 32)
                {
                    StageSystem.yScrollMove = 104 - playerO.screenYPos - playerO.lookPos >> 1 << 1;
                    if (StageSystem.yScrollMove > 32)
                        StageSystem.yScrollMove = 32;
                    if (StageSystem.yScrollMove < -32)
                        StageSystem.yScrollMove = -32;
                }
                if (StageSystem.yScrollMove > 0)
                {
                    StageSystem.yScrollMove -= 6;
                    if (StageSystem.yScrollMove < 0)
                        StageSystem.yScrollMove = StageSystem.yScrollMove;
                }
                if (StageSystem.yScrollMove < 0)
                {
                    StageSystem.yScrollMove += 6;
                    if (StageSystem.yScrollMove > 0)
                        StageSystem.yScrollMove = StageSystem.yScrollMove;
                }
            }
            if (Math.Abs(num9) < Math.Abs(StageSystem.yScrollMove) + 17)
            {
                if (StageSystem.yScrollMove == 32)
                {
                    if (num8 + playerO.lookPos > num6 + 104 + StageSystem.yScrollMove)
                    {
                        num6 += num8 + playerO.lookPos - (num6 + 104 + StageSystem.yScrollMove);
                        num7 = num6 + 240;
                    }
                    if (num8 + playerO.lookPos < num6 + 104 - StageSystem.yScrollMove)
                    {
                        num6 -= num6 + 104 - StageSystem.yScrollMove - (num8 + playerO.lookPos);
                        num7 = num6 + 240;
                    }
                }
                else
                {
                    num6 = num8 - 104 + StageSystem.yScrollMove + playerO.lookPos;
                    num7 = num6 + 240;
                }
            }
            else
            {
                if (num9 > 0)
                    num6 += 16;
                else
                    num6 -= 16;
                num7 = num6 + 240;
            }
            if (num6 < StageSystem.yBoundary1)
            {
                num6 = StageSystem.yBoundary1;
                num7 = StageSystem.yBoundary1 + 240;
            }
            if (num7 > StageSystem.yBoundary2)
            {
                num7 = StageSystem.yBoundary2;
                num6 = StageSystem.yBoundary2 - 240;
            }
            StageSystem.yScrollA = num6;
            StageSystem.yScrollB = num7;
            if (num8 + playerO.lookPos > num6 + 104)
            {
                StageSystem.yScrollOffset = num8 - 104 + playerO.lookPos + StageSystem.screenShakeY;
                playerO.screenYPos = 104 - playerO.lookPos - StageSystem.screenShakeY;
                if (num8 + playerO.lookPos > num7 - 136)
                {
                    playerO.screenYPos = 104 + (num8 - (num7 - 136)) + StageSystem.screenShakeY;
                    StageSystem.yScrollOffset = num7 - 240 - StageSystem.screenShakeY;
                }
            }
            else
            {
                playerO.screenYPos = num8 - num6 - StageSystem.screenShakeY;
                StageSystem.yScrollOffset = num6 + StageSystem.screenShakeY;
            }
            playerO.screenYPos -= StageSystem.cameraAdjustY;
            if (StageSystem.screenShakeX != 0)
            {
                if (StageSystem.screenShakeX > 0)
                {
                    StageSystem.screenShakeX = -StageSystem.screenShakeX;
                }
                else
                {
                    StageSystem.screenShakeX = -StageSystem.screenShakeX;
                    --StageSystem.screenShakeX;
                }
            }
            if (StageSystem.screenShakeY == 0)
                return;
            if (StageSystem.screenShakeY > 0)
            {
                StageSystem.screenShakeY = -StageSystem.screenShakeY;
            }
            else
            {
                StageSystem.screenShakeY = -StageSystem.screenShakeY;
                --StageSystem.screenShakeY;
            }
        }

        public static void SetPlayerScreenPositionCDStyle(PlayerObject playerO)
        {
            int num1 = playerO.xPos >> 16;
            int num2 = playerO.yPos >> 16;
            if (StageSystem.newYBoundary1 > StageSystem.yBoundary1)
                StageSystem.yBoundary1 = StageSystem.yScrollOffset <= StageSystem.newYBoundary1 ? StageSystem.yScrollOffset : StageSystem.newYBoundary1;
            if (StageSystem.newYBoundary1 < StageSystem.yBoundary1)
            {
                if (StageSystem.yScrollOffset > StageSystem.yBoundary1)
                    StageSystem.yBoundary1 = StageSystem.newYBoundary1;
                else
                    --StageSystem.yBoundary1;
            }
            if (StageSystem.newYBoundary2 < StageSystem.yBoundary2)
            {
                if (StageSystem.yScrollOffset + 240 < StageSystem.yBoundary2 && StageSystem.yScrollOffset + 240 > StageSystem.newYBoundary2)
                    StageSystem.yBoundary2 = StageSystem.yScrollOffset + 240;
                else
                    --StageSystem.yBoundary2;
            }
            if (StageSystem.newYBoundary2 > StageSystem.yBoundary2)
            {
                if (StageSystem.yScrollOffset + 240 < StageSystem.yBoundary2)
                    StageSystem.yBoundary2 = StageSystem.newYBoundary2;
                else
                    ++StageSystem.yBoundary2;
            }
            if (StageSystem.newXBoundary1 > StageSystem.xBoundary1)
                StageSystem.xBoundary1 = StageSystem.xScrollOffset <= StageSystem.newXBoundary1 ? StageSystem.xScrollOffset : StageSystem.newXBoundary1;
            if (StageSystem.newXBoundary1 < StageSystem.xBoundary1)
            {
                if (StageSystem.xScrollOffset > StageSystem.xBoundary1)
                {
                    StageSystem.xBoundary1 = StageSystem.newXBoundary1;
                }
                else
                {
                    --StageSystem.xBoundary1;
                    if (playerO.xVelocity < 0)
                    {
                        StageSystem.xBoundary1 += playerO.xVelocity >> 16;
                        if (StageSystem.xBoundary1 < StageSystem.newXBoundary1)
                            StageSystem.xBoundary1 = StageSystem.newXBoundary1;
                    }
                }
            }
            if (StageSystem.newXBoundary2 < StageSystem.xBoundary2)
                StageSystem.xBoundary2 = StageSystem.xScrollOffset + GlobalAppDefinitions.SCREEN_XSIZE >= StageSystem.xBoundary2 ? StageSystem.xScrollOffset + GlobalAppDefinitions.SCREEN_XSIZE : StageSystem.newXBoundary2;
            if (StageSystem.newXBoundary2 > StageSystem.xBoundary2)
            {
                if (StageSystem.xScrollOffset + GlobalAppDefinitions.SCREEN_XSIZE < StageSystem.xBoundary2)
                {
                    StageSystem.xBoundary2 = StageSystem.newXBoundary2;
                }
                else
                {
                    ++StageSystem.xBoundary2;
                    if (playerO.xVelocity > 0)
                    {
                        StageSystem.xBoundary2 += playerO.xVelocity >> 16;
                        if (StageSystem.xBoundary2 > StageSystem.newXBoundary2)
                            StageSystem.xBoundary2 = StageSystem.newXBoundary2;
                    }
                }
            }
            if (playerO.gravity == (byte)0)
                StageSystem.cameraShift = playerO.objectPtr.direction != (byte)0 ? (!(playerO.speed < -390594 | StageSystem.cameraStyle == (byte)3) ? (byte)0 : (byte)2) : (!(playerO.speed > 390594 | StageSystem.cameraStyle == (byte)2) ? (byte)0 : (byte)1);
            switch (StageSystem.cameraShift)
            {
                case 0:
                    if (StageSystem.xScrollMove < 0)
                        StageSystem.xScrollMove += 2;
                    if (StageSystem.xScrollMove > 0)
                    {
                        StageSystem.xScrollMove -= 2;
                        break;
                    }
                    break;
                case 1:
                    if (StageSystem.xScrollMove > -64)
                    {
                        StageSystem.xScrollMove -= 2;
                        break;
                    }
                    break;
                case 2:
                    if (StageSystem.xScrollMove < 64)
                    {
                        StageSystem.xScrollMove += 2;
                        break;
                    }
                    break;
            }
            if (num1 > StageSystem.xBoundary1 + GlobalAppDefinitions.SCREEN_CENTER + StageSystem.xScrollMove)
            {
                StageSystem.xScrollOffset = num1 - GlobalAppDefinitions.SCREEN_CENTER + StageSystem.screenShakeX - StageSystem.xScrollMove;
                playerO.screenXPos = GlobalAppDefinitions.SCREEN_CENTER - StageSystem.screenShakeX + StageSystem.xScrollMove;
                if (num1 - StageSystem.xScrollMove > StageSystem.xBoundary2 - GlobalAppDefinitions.SCREEN_CENTER)
                {
                    playerO.screenXPos = GlobalAppDefinitions.SCREEN_CENTER + (num1 - (StageSystem.xBoundary2 - GlobalAppDefinitions.SCREEN_CENTER)) + StageSystem.screenShakeX;
                    StageSystem.xScrollOffset = StageSystem.xBoundary2 - GlobalAppDefinitions.SCREEN_XSIZE - StageSystem.screenShakeX;
                }
            }
            else
            {
                playerO.screenXPos = num1 - StageSystem.xBoundary1 + StageSystem.screenShakeX;
                StageSystem.xScrollOffset = StageSystem.xBoundary1 - StageSystem.screenShakeX;
            }
            StageSystem.xScrollA = StageSystem.xScrollOffset;
            StageSystem.xScrollB = StageSystem.xScrollA + GlobalAppDefinitions.SCREEN_XSIZE;
            int num3 = StageSystem.yScrollA;
            int num4 = StageSystem.yScrollB;
            int num5 = num2 + StageSystem.cameraAdjustY;
            int num6 = num5 + playerO.lookPos - (num3 + 104);
            if (playerO.trackScroll == (byte)1)
            {
                StageSystem.yScrollMove = 32;
            }
            else
            {
                if (StageSystem.yScrollMove == 32)
                {
                    StageSystem.yScrollMove = 104 - playerO.screenYPos - playerO.lookPos >> 1 << 1;
                    if (StageSystem.yScrollMove > 32)
                        StageSystem.yScrollMove = 32;
                    if (StageSystem.yScrollMove < -32)
                        StageSystem.yScrollMove = -32;
                }
                if (StageSystem.yScrollMove > 0)
                {
                    StageSystem.yScrollMove -= 6;
                    if (StageSystem.yScrollMove < 0)
                        StageSystem.yScrollMove = StageSystem.yScrollMove;
                }
                if (StageSystem.yScrollMove < 0)
                {
                    StageSystem.yScrollMove += 6;
                    if (StageSystem.yScrollMove > 0)
                        StageSystem.yScrollMove = StageSystem.yScrollMove;
                }
            }
            if (Math.Abs(num6) < Math.Abs(StageSystem.yScrollMove) + 17)
            {
                if (StageSystem.yScrollMove == 32)
                {
                    if (num5 + playerO.lookPos > num3 + 104 + StageSystem.yScrollMove)
                    {
                        num3 += num5 + playerO.lookPos - (num3 + 104 + StageSystem.yScrollMove);
                        num4 = num3 + 240;
                    }
                    if (num5 + playerO.lookPos < num3 + 104 - StageSystem.yScrollMove)
                    {
                        num3 -= num3 + 104 - StageSystem.yScrollMove - (num5 + playerO.lookPos);
                        num4 = num3 + 240;
                    }
                }
                else
                {
                    num3 = num5 - 104 + StageSystem.yScrollMove + playerO.lookPos;
                    num4 = num3 + 240;
                }
            }
            else
            {
                if (num6 > 0)
                    num3 += 16;
                else
                    num3 -= 16;
                num4 = num3 + 240;
            }
            if (num3 < StageSystem.yBoundary1)
            {
                num3 = StageSystem.yBoundary1;
                num4 = StageSystem.yBoundary1 + 240;
            }
            if (num4 > StageSystem.yBoundary2)
            {
                num4 = StageSystem.yBoundary2;
                num3 = StageSystem.yBoundary2 - 240;
            }
            StageSystem.yScrollA = num3;
            StageSystem.yScrollB = num4;
            if (num5 + playerO.lookPos > num3 + 104)
            {
                StageSystem.yScrollOffset = num5 - 104 + playerO.lookPos + StageSystem.screenShakeY;
                playerO.screenYPos = 104 - playerO.lookPos - StageSystem.screenShakeY;
                if (num5 + playerO.lookPos > num4 - 136)
                {
                    playerO.screenYPos = 104 + (num5 - (num4 - 136)) + StageSystem.screenShakeY;
                    StageSystem.yScrollOffset = num4 - 240 - StageSystem.screenShakeY;
                }
            }
            else
            {
                playerO.screenYPos = num5 - num3 - StageSystem.screenShakeY;
                StageSystem.yScrollOffset = num3 + StageSystem.screenShakeY;
            }
            playerO.screenYPos -= StageSystem.cameraAdjustY;
            if (StageSystem.screenShakeX != 0)
            {
                if (StageSystem.screenShakeX > 0)
                {
                    StageSystem.screenShakeX = -StageSystem.screenShakeX;
                }
                else
                {
                    StageSystem.screenShakeX = -StageSystem.screenShakeX;
                    --StageSystem.screenShakeX;
                }
            }
            if (StageSystem.screenShakeY == 0)
                return;
            if (StageSystem.screenShakeY > 0)
            {
                StageSystem.screenShakeY = -StageSystem.screenShakeY;
            }
            else
            {
                StageSystem.screenShakeY = -StageSystem.screenShakeY;
                --StageSystem.screenShakeY;
            }
        }

        public static void SetPlayerLockedScreenPosition(PlayerObject playerO)
        {
            int num1 = playerO.xPos >> 16;
            int num2 = playerO.yPos >> 16;
            int xScrollA = StageSystem.xScrollA;
            int xScrollB = StageSystem.xScrollB;
            if (num1 > xScrollA + GlobalAppDefinitions.SCREEN_CENTER)
            {
                StageSystem.xScrollOffset = num1 - GlobalAppDefinitions.SCREEN_CENTER + StageSystem.screenShakeX;
                playerO.screenXPos = GlobalAppDefinitions.SCREEN_CENTER - StageSystem.screenShakeX;
                if (num1 > xScrollB - GlobalAppDefinitions.SCREEN_CENTER)
                {
                    playerO.screenXPos = GlobalAppDefinitions.SCREEN_CENTER + (num1 - (xScrollB - GlobalAppDefinitions.SCREEN_CENTER)) + StageSystem.screenShakeX;
                    StageSystem.xScrollOffset = xScrollB - GlobalAppDefinitions.SCREEN_XSIZE - StageSystem.screenShakeX;
                }
            }
            else
            {
                playerO.screenXPos = num1 - xScrollA + StageSystem.screenShakeX;
                StageSystem.xScrollOffset = xScrollA - StageSystem.screenShakeX;
            }
            int yScrollA = StageSystem.yScrollA;
            int yScrollB = StageSystem.yScrollB;
            int num3 = num2 + StageSystem.cameraAdjustY;
            int lookPos = playerO.lookPos;
            StageSystem.yScrollA = yScrollA;
            StageSystem.yScrollB = yScrollB;
            if (num3 + playerO.lookPos > yScrollA + 104)
            {
                StageSystem.yScrollOffset = num3 - 104 + playerO.lookPos + StageSystem.screenShakeY;
                playerO.screenYPos = 104 - playerO.lookPos - StageSystem.screenShakeY;
                if (num3 + playerO.lookPos > yScrollB - 136)
                {
                    playerO.screenYPos = 104 + (num3 - (yScrollB - 136)) + StageSystem.screenShakeY;
                    StageSystem.yScrollOffset = yScrollB - 240 - StageSystem.screenShakeY;
                }
            }
            else
            {
                playerO.screenYPos = num3 - yScrollA - StageSystem.screenShakeY;
                StageSystem.yScrollOffset = yScrollA + StageSystem.screenShakeY;
            }
            playerO.screenYPos -= StageSystem.cameraAdjustY;
            if (StageSystem.screenShakeX != 0)
            {
                if (StageSystem.screenShakeX > 0)
                {
                    StageSystem.screenShakeX = -StageSystem.screenShakeX;
                }
                else
                {
                    StageSystem.screenShakeX = -StageSystem.screenShakeX;
                    --StageSystem.screenShakeX;
                }
            }
            if (StageSystem.screenShakeY == 0)
                return;
            if (StageSystem.screenShakeY > 0)
            {
                StageSystem.screenShakeY = -StageSystem.screenShakeY;
            }
            else
            {
                StageSystem.screenShakeY = -StageSystem.screenShakeY;
                --StageSystem.screenShakeY;
            }
        }

        public static void SetPlayerHLockedScreenPosition(PlayerObject playerO)
        {
            int num1 = playerO.xPos >> 16;
            int num2 = playerO.yPos >> 16;
            if (StageSystem.newYBoundary1 > StageSystem.yBoundary1)
                StageSystem.yBoundary1 = StageSystem.yScrollOffset <= StageSystem.newYBoundary1 ? StageSystem.yScrollOffset : StageSystem.newYBoundary1;
            if (StageSystem.newYBoundary1 < StageSystem.yBoundary1)
            {
                if (StageSystem.yScrollOffset > StageSystem.yBoundary1)
                    StageSystem.yBoundary1 = StageSystem.newYBoundary1;
                else
                    --StageSystem.yBoundary1;
            }
            if (StageSystem.newYBoundary2 < StageSystem.yBoundary2)
            {
                if (StageSystem.yScrollOffset + 240 < StageSystem.yBoundary2 && StageSystem.yScrollOffset + 240 > StageSystem.newYBoundary2)
                    StageSystem.yBoundary2 = StageSystem.yScrollOffset + 240;
                else
                    --StageSystem.yBoundary2;
            }
            if (StageSystem.newYBoundary2 > StageSystem.yBoundary2)
            {
                if (StageSystem.yScrollOffset + 240 < StageSystem.yBoundary2)
                    StageSystem.yBoundary2 = StageSystem.newYBoundary2;
                else
                    ++StageSystem.yBoundary2;
            }
            int xScrollA = StageSystem.xScrollA;
            int xScrollB = StageSystem.xScrollB;
            if (num1 > xScrollA + GlobalAppDefinitions.SCREEN_CENTER)
            {
                StageSystem.xScrollOffset = num1 - GlobalAppDefinitions.SCREEN_CENTER + StageSystem.screenShakeX;
                playerO.screenXPos = GlobalAppDefinitions.SCREEN_CENTER - StageSystem.screenShakeX;
                if (num1 > xScrollB - GlobalAppDefinitions.SCREEN_CENTER)
                {
                    playerO.screenXPos = GlobalAppDefinitions.SCREEN_CENTER + (num1 - (xScrollB - GlobalAppDefinitions.SCREEN_CENTER)) + StageSystem.screenShakeX;
                    StageSystem.xScrollOffset = xScrollB - GlobalAppDefinitions.SCREEN_XSIZE - StageSystem.screenShakeX;
                }
            }
            else
            {
                playerO.screenXPos = num1 - xScrollA + StageSystem.screenShakeX;
                StageSystem.xScrollOffset = xScrollA - StageSystem.screenShakeX;
            }
            int num3 = StageSystem.yScrollA;
            int num4 = StageSystem.yScrollB;
            int num5 = num2 + StageSystem.cameraAdjustY;
            int num6 = num5 + playerO.lookPos - (num3 + 104);
            if (playerO.trackScroll == (byte)1)
            {
                StageSystem.yScrollMove = 32;
            }
            else
            {
                if (StageSystem.yScrollMove == 32)
                {
                    StageSystem.yScrollMove = 104 - playerO.screenYPos - playerO.lookPos >> 1 << 1;
                    if (StageSystem.yScrollMove > 32)
                        StageSystem.yScrollMove = 32;
                    if (StageSystem.yScrollMove < -32)
                        StageSystem.yScrollMove = -32;
                }
                if (StageSystem.yScrollMove > 0)
                {
                    StageSystem.yScrollMove -= 6;
                    if (StageSystem.yScrollMove < 0)
                        StageSystem.yScrollMove = StageSystem.yScrollMove;
                }
                if (StageSystem.yScrollMove < 0)
                {
                    StageSystem.yScrollMove += 6;
                    if (StageSystem.yScrollMove > 0)
                        StageSystem.yScrollMove = StageSystem.yScrollMove;
                }
            }
            if (Math.Abs(num6) < Math.Abs(StageSystem.yScrollMove) + 17)
            {
                if (StageSystem.yScrollMove == 32)
                {
                    if (num5 + playerO.lookPos > num3 + 104 + StageSystem.yScrollMove)
                    {
                        num3 += num5 + playerO.lookPos - (num3 + 104 + StageSystem.yScrollMove);
                        num4 = num3 + 240;
                    }
                    if (num5 + playerO.lookPos < num3 + 104 - StageSystem.yScrollMove)
                    {
                        num3 -= num3 + 104 - StageSystem.yScrollMove - (num5 + playerO.lookPos);
                        num4 = num3 + 240;
                    }
                }
                else
                {
                    num3 = num5 - 104 + StageSystem.yScrollMove + playerO.lookPos;
                    num4 = num3 + 240;
                }
            }
            else
            {
                if (num6 > 0)
                    num3 += 16;
                else
                    num3 -= 16;
                num4 = num3 + 240;
            }
            if (num3 < StageSystem.yBoundary1)
            {
                num3 = StageSystem.yBoundary1;
                num4 = StageSystem.yBoundary1 + 240;
            }
            if (num4 > StageSystem.yBoundary2)
            {
                num4 = StageSystem.yBoundary2;
                num3 = StageSystem.yBoundary2 - 240;
            }
            StageSystem.yScrollA = num3;
            StageSystem.yScrollB = num4;
            if (num5 + playerO.lookPos > num3 + 104)
            {
                StageSystem.yScrollOffset = num5 - 104 + playerO.lookPos + StageSystem.screenShakeY;
                playerO.screenYPos = 104 - playerO.lookPos - StageSystem.screenShakeY;
                if (num5 + playerO.lookPos > num4 - 136)
                {
                    playerO.screenYPos = 104 + (num5 - (num4 - 136)) + StageSystem.screenShakeY;
                    StageSystem.yScrollOffset = num4 - 240 - StageSystem.screenShakeY;
                }
            }
            else
            {
                playerO.screenYPos = num5 - num3 - StageSystem.screenShakeY;
                StageSystem.yScrollOffset = num3 + StageSystem.screenShakeY;
            }
            playerO.screenYPos -= StageSystem.cameraAdjustY;
            if (StageSystem.screenShakeX != 0)
            {
                if (StageSystem.screenShakeX > 0)
                {
                    StageSystem.screenShakeX = -StageSystem.screenShakeX;
                }
                else
                {
                    StageSystem.screenShakeX = -StageSystem.screenShakeX;
                    --StageSystem.screenShakeX;
                }
            }
            if (StageSystem.screenShakeY == 0)
                return;
            if (StageSystem.screenShakeY > 0)
            {
                StageSystem.screenShakeY = -StageSystem.screenShakeY;
            }
            else
            {
                StageSystem.screenShakeY = -StageSystem.screenShakeY;
                --StageSystem.screenShakeY;
            }
        }

        public static void ProcessPlayerControl(PlayerObject playerO)
        {
            switch (playerO.controlMode)
            {
                case -1:
                    PlayerSystem.delayUp <<= 1;
                    PlayerSystem.delayUp |= (ushort)playerO.up;
                    PlayerSystem.delayDown <<= 1;
                    PlayerSystem.delayDown |= (ushort)playerO.down;
                    PlayerSystem.delayLeft <<= 1;
                    PlayerSystem.delayLeft |= (ushort)playerO.left;
                    PlayerSystem.delayRight <<= 1;
                    PlayerSystem.delayRight |= (ushort)playerO.right;
                    PlayerSystem.delayJumpPress <<= 1;
                    PlayerSystem.delayJumpPress |= (ushort)playerO.jumpPress;
                    PlayerSystem.delayJumpHold <<= 1;
                    PlayerSystem.delayJumpHold |= (ushort)playerO.jumpHold;
                    break;
                case 0:
                    playerO.up = StageSystem.gKeyDown.up;
                    playerO.down = StageSystem.gKeyDown.down;
                    if (StageSystem.gKeyDown.left == (byte)1 && StageSystem.gKeyDown.right == (byte)1)
                    {
                        playerO.left = (byte)0;
                        playerO.right = (byte)0;
                    }
                    else
                    {
                        playerO.left = StageSystem.gKeyDown.left;
                        playerO.right = StageSystem.gKeyDown.right;
                    }
                    playerO.jumpHold = (byte)((uint)StageSystem.gKeyDown.buttonA | (uint)StageSystem.gKeyDown.buttonB | (uint)StageSystem.gKeyDown.buttonC);
                    playerO.jumpPress = (byte)((uint)StageSystem.gKeyPress.buttonA | (uint)StageSystem.gKeyPress.buttonB | (uint)StageSystem.gKeyPress.buttonC);
                    PlayerSystem.delayUp <<= 1;
                    PlayerSystem.delayUp |= (ushort)playerO.up;
                    PlayerSystem.delayDown <<= 1;
                    PlayerSystem.delayDown |= (ushort)playerO.down;
                    PlayerSystem.delayLeft <<= 1;
                    PlayerSystem.delayLeft |= (ushort)playerO.left;
                    PlayerSystem.delayRight <<= 1;
                    PlayerSystem.delayRight |= (ushort)playerO.right;
                    PlayerSystem.delayJumpPress <<= 1;
                    PlayerSystem.delayJumpPress |= (ushort)playerO.jumpPress;
                    PlayerSystem.delayJumpHold <<= 1;
                    PlayerSystem.delayJumpHold |= (ushort)playerO.jumpHold;
                    break;
                case 1:
                    playerO.up = (byte)((uint)PlayerSystem.delayUp >> 15);
                    playerO.down = (byte)((uint)PlayerSystem.delayDown >> 15);
                    playerO.left = (byte)((uint)PlayerSystem.delayLeft >> 15);
                    playerO.right = (byte)((uint)PlayerSystem.delayRight >> 15);
                    playerO.jumpPress = (byte)((uint)PlayerSystem.delayJumpPress >> 15);
                    playerO.jumpHold = (byte)((uint)PlayerSystem.delayJumpHold >> 15);
                    break;
            }
        }

        public static void ProcessPlayerTileCollisions(PlayerObject playerO)
        {
            playerO.flailing[0] = (byte)0;
            playerO.flailing[1] = (byte)0;
            playerO.flailing[2] = (byte)0;
            ObjectSystem.scriptEng.checkResult =0;
            if (playerO.gravity == (byte)1)
                PlayerSystem.ProcessAirCollision(playerO);
            else
                PlayerSystem.ProcessPathGrip(playerO);
        }

        public static void ProcessAirCollision(PlayerObject playerO)
        {
            CollisionBox collisionBox = AnimationSystem.collisionBoxList[playerO.animationFile.cbListOffset + (int)AnimationSystem.animationFrames[AnimationSystem.animationList[playerO.animationFile.aniListOffset + (int)playerO.objectPtr.animation].frameListOffset + (int)playerO.objectPtr.frame].collisionBox];
            PlayerSystem.collisionLeft = (int)collisionBox.left[0];
            PlayerSystem.collisionTop = (int)collisionBox.top[0];
            PlayerSystem.collisionRight = (int)collisionBox.right[0];
            PlayerSystem.collisionBottom = (int)collisionBox.bottom[0];
            byte num1;
            if (playerO.xVelocity >= 0)
            {
                num1 = (byte)1;
                PlayerSystem.cSensor[0].yPos = playerO.yPos + 131072;
                PlayerSystem.cSensor[0].collided = (byte)0;
                PlayerSystem.cSensor[0].xPos = playerO.xPos + (PlayerSystem.collisionRight << 16);
            }
            else
                num1 = (byte)0;
            byte num2;
            if (playerO.xVelocity <= 0)
            {
                num2 = (byte)1;
                PlayerSystem.cSensor[1].yPos = playerO.yPos + 131072;
                PlayerSystem.cSensor[1].collided = (byte)0;
                PlayerSystem.cSensor[1].xPos = playerO.xPos + (PlayerSystem.collisionLeft - 1 << 16);
            }
            else
                num2 = (byte)0;
            PlayerSystem.cSensor[2].xPos = playerO.xPos + ((int)collisionBox.left[1] << 16);
            PlayerSystem.cSensor[3].xPos = playerO.xPos + ((int)collisionBox.right[1] << 16);
            PlayerSystem.cSensor[2].collided = (byte)0;
            PlayerSystem.cSensor[3].collided = (byte)0;
            PlayerSystem.cSensor[4].xPos = PlayerSystem.cSensor[2].xPos;
            PlayerSystem.cSensor[5].xPos = PlayerSystem.cSensor[3].xPos;
            PlayerSystem.cSensor[4].collided = (byte)0;
            PlayerSystem.cSensor[5].collided = (byte)0;
            byte num3;
            if (playerO.yVelocity >= 0)
            {
                num3 = (byte)1;
                PlayerSystem.cSensor[2].yPos = playerO.yPos + (PlayerSystem.collisionBottom << 16);
                PlayerSystem.cSensor[3].yPos = playerO.yPos + (PlayerSystem.collisionBottom << 16);
            }
            else
                num3 = (byte)0;
            byte num4 = 1;
            PlayerSystem.cSensor[4].yPos = playerO.yPos + (PlayerSystem.collisionTop - 1 << 16);
            PlayerSystem.cSensor[5].yPos = playerO.yPos + (PlayerSystem.collisionTop - 1 << 16);
            int num5 = Math.Abs(playerO.xVelocity) <= Math.Abs(playerO.yVelocity) ? (Math.Abs(playerO.yVelocity) >> 19) + 1 : (Math.Abs(playerO.xVelocity) >> 19) + 1;
            int num6 = playerO.xVelocity / num5;
            int num7 = playerO.yVelocity / num5;
            int num8 = playerO.xVelocity - num6 * (num5 - 1);
            int num9 = playerO.yVelocity - num7 * (num5 - 1);
            while (num5 > 0)
            {
                if (num5 < 2)
                {
                    num6 = num8;
                    num7 = num9;
                }
                --num5;
                if (num1 == (byte)1)
                {
                    PlayerSystem.cSensor[0].xPos += num6 + 65536;
                    PlayerSystem.cSensor[0].yPos += num7;
                    PlayerSystem.LWallCollision(playerO, PlayerSystem.cSensor[0]);
                    if (PlayerSystem.cSensor[0].collided == (byte)1)
                        num1 = (byte)2;
                }
                if (num2 == (byte)1)
                {
                    PlayerSystem.cSensor[1].xPos += num6 - 65536;
                    PlayerSystem.cSensor[1].yPos += num7;
                    PlayerSystem.RWallCollision(playerO, PlayerSystem.cSensor[1]);
                    if (PlayerSystem.cSensor[1].collided == (byte)1)
                        num2 = (byte)2;
                }
                if (num1 == (byte)2)
                {
                    playerO.xVelocity = 0;
                    playerO.speed = 0;
                    playerO.xPos = PlayerSystem.cSensor[0].xPos - PlayerSystem.collisionRight << 16;
                    PlayerSystem.cSensor[2].xPos = playerO.xPos + (PlayerSystem.collisionLeft + 1 << 16);
                    PlayerSystem.cSensor[3].xPos = playerO.xPos + (PlayerSystem.collisionRight - 2 << 16);
                    PlayerSystem.cSensor[4].xPos = PlayerSystem.cSensor[2].xPos;
                    PlayerSystem.cSensor[5].xPos = PlayerSystem.cSensor[3].xPos;
                    num6 = 0;
                    num8 = 0;
                    num1 = (byte)3;
                }
                if (num2 == (byte)2)
                {
                    playerO.xVelocity = 0;
                    playerO.speed = 0;
                    playerO.xPos = PlayerSystem.cSensor[1].xPos - PlayerSystem.collisionLeft + 1 << 16;
                    PlayerSystem.cSensor[2].xPos = playerO.xPos + (PlayerSystem.collisionLeft + 1 << 16);
                    PlayerSystem.cSensor[3].xPos = playerO.xPos + (PlayerSystem.collisionRight - 2 << 16);
                    PlayerSystem.cSensor[4].xPos = PlayerSystem.cSensor[2].xPos;
                    PlayerSystem.cSensor[5].xPos = PlayerSystem.cSensor[3].xPos;
                    num6 = 0;
                    num8 = 0;
                    num2 = (byte)3;
                }
                if (num3 == (byte)1)
                {
                    for (int index = 2; index < 4; ++index)
                    {
                        if (PlayerSystem.cSensor[index].collided == (byte)0)
                        {
                            PlayerSystem.cSensor[index].xPos += num6;
                            PlayerSystem.cSensor[index].yPos += num7;
                            PlayerSystem.FloorCollision(playerO, PlayerSystem.cSensor[index]);
                        }
                    }
                    if (PlayerSystem.cSensor[2].collided == (byte)1 | PlayerSystem.cSensor[3].collided == (byte)1)
                    {
                        num3 = (byte)2;
                        num5 = 0;
                    }
                }
                if (num4 == (byte)1)
                {
                    for (int index = 4; index < 6; ++index)
                    {
                        if (PlayerSystem.cSensor[index].collided == (byte)0)
                        {
                            PlayerSystem.cSensor[index].xPos += num6;
                            PlayerSystem.cSensor[index].yPos += num7;
                            PlayerSystem.RoofCollision(playerO, PlayerSystem.cSensor[index]);
                        }
                    }
                    if (PlayerSystem.cSensor[4].collided == (byte)1 | PlayerSystem.cSensor[5].collided == (byte)1)
                    {
                        num4 = (byte)2;
                        num5 = 0;
                    }
                }
            }
            if (num1 < (byte)2 && num2 < (byte)2)
                playerO.xPos += playerO.xVelocity;
            if (num4 < (byte)2 && num3 < (byte)2)
            {
                playerO.yPos += playerO.yVelocity;
            }
            else
            {
                if (num3 == (byte)2)
                {
                    playerO.gravity = (byte)0;
                    if (PlayerSystem.cSensor[2].collided == (byte)1 && PlayerSystem.cSensor[3].collided == (byte)1)
                    {
                        if (PlayerSystem.cSensor[2].yPos < PlayerSystem.cSensor[3].yPos)
                        {
                            playerO.yPos = PlayerSystem.cSensor[2].yPos - PlayerSystem.collisionBottom << 16;
                            playerO.angle = PlayerSystem.cSensor[2].angle;
                        }
                        else
                        {
                            playerO.yPos = PlayerSystem.cSensor[3].yPos - PlayerSystem.collisionBottom << 16;
                            playerO.angle = PlayerSystem.cSensor[3].angle;
                        }
                    }
                    else if (PlayerSystem.cSensor[2].collided == (byte)1)
                    {
                        playerO.yPos = PlayerSystem.cSensor[2].yPos - PlayerSystem.collisionBottom << 16;
                        playerO.angle = PlayerSystem.cSensor[2].angle;
                    }
                    else if (PlayerSystem.cSensor[3].collided == (byte)1)
                    {
                        playerO.yPos = PlayerSystem.cSensor[3].yPos - PlayerSystem.collisionBottom << 16;
                        playerO.angle = PlayerSystem.cSensor[3].angle;
                    }
                    if (playerO.angle > 160 && playerO.angle < 224 && playerO.collisionMode != (byte)1)
                    {
                        playerO.collisionMode = (byte)1;
                        playerO.xPos -= 262144;
                    }
                    if (playerO.angle > 32 && playerO.angle < 96 && playerO.collisionMode != (byte)3)
                    {
                        playerO.collisionMode = (byte)3;
                        playerO.xPos += 262144;
                    }
                    if (playerO.angle < 32 | playerO.angle > 224)
                        playerO.controlLock = (byte)0;
                    playerO.objectPtr.rotation = playerO.angle << 1;
                    int num10 = playerO.down != (byte)1 ? (playerO.angle >= 128 ? (playerO.angle <= 240 ? (playerO.angle <= 224 ? (Math.Abs(playerO.xVelocity) <= Math.Abs(playerO.yVelocity) ? -playerO.yVelocity : playerO.xVelocity) : (Math.Abs(playerO.xVelocity) <= Math.Abs(playerO.yVelocity >> 1) ? -(playerO.yVelocity >> 1) : playerO.xVelocity)) : playerO.xVelocity) : (playerO.angle >= 16 ? (playerO.angle >= 32 ? (Math.Abs(playerO.xVelocity) <= Math.Abs(playerO.yVelocity) ? playerO.yVelocity : playerO.xVelocity) : (Math.Abs(playerO.xVelocity) <= Math.Abs(playerO.yVelocity >> 1) ? playerO.yVelocity >> 1 : playerO.xVelocity)) : playerO.xVelocity)) : (playerO.angle >= 128 ? (playerO.angle <= 240 ? (playerO.angle <= 224 ? (Math.Abs(playerO.xVelocity) <= Math.Abs(playerO.yVelocity) ? -(playerO.yVelocity + playerO.yVelocity / 12) : playerO.xVelocity) : (Math.Abs(playerO.xVelocity) <= Math.Abs(playerO.yVelocity >> 1) ? -(playerO.yVelocity + playerO.yVelocity / 12 >> 1) : playerO.xVelocity)) : playerO.xVelocity) : (playerO.angle >= 16 ? (playerO.angle >= 32 ? (Math.Abs(playerO.xVelocity) <= Math.Abs(playerO.yVelocity) ? playerO.yVelocity + playerO.yVelocity / 12 : playerO.xVelocity) : (Math.Abs(playerO.xVelocity) <= Math.Abs(playerO.yVelocity >> 1) ? playerO.yVelocity + playerO.yVelocity / 12 >> 1 : playerO.xVelocity)) : playerO.xVelocity));
                    if (num10 < -1572864)
                        num10 = -1572864;
                    if (num10 > 1572864)
                        num10 = 1572864;
                    playerO.speed = num10;
                    playerO.yVelocity = 0;
                    ObjectSystem.scriptEng.checkResult = 1;
                }
                if (num4 != (byte)2)
                    return;
                if (PlayerSystem.cSensor[4].collided == (byte)1 && PlayerSystem.cSensor[5].collided == (byte)1)
                {
                    if (PlayerSystem.cSensor[4].yPos > PlayerSystem.cSensor[5].yPos)
                    {
                        playerO.yPos = PlayerSystem.cSensor[4].yPos - PlayerSystem.collisionTop + 1 << 16;
                        num5 = PlayerSystem.cSensor[4].angle;
                    }
                    else
                    {
                        playerO.yPos = PlayerSystem.cSensor[5].yPos - PlayerSystem.collisionTop + 1 << 16;
                        num5 = PlayerSystem.cSensor[5].angle;
                    }
                }
                else if (PlayerSystem.cSensor[4].collided == (byte)1)
                {
                    playerO.yPos = PlayerSystem.cSensor[4].yPos - PlayerSystem.collisionTop + 1 << 16;
                    num5 = PlayerSystem.cSensor[4].angle;
                }
                else if (PlayerSystem.cSensor[5].collided == (byte)1)
                {
                    playerO.yPos = PlayerSystem.cSensor[5].yPos - PlayerSystem.collisionTop + 1 << 16;
                    num5 = PlayerSystem.cSensor[5].angle;
                }
                int num11 = num5 & (int)byte.MaxValue;
                int num12 = (int)GlobalAppDefinitions.ArcTanLookup(playerO.xVelocity, playerO.yVelocity);
                if (num11 > 64 && num11 < 98 && (num12 > 160 && num12 < 194))
                {
                    playerO.gravity = (byte)0;
                    playerO.angle = num11;
                    playerO.objectPtr.rotation = playerO.angle << 1;
                    playerO.collisionMode = (byte)3;
                    playerO.xPos += 262144;
                    playerO.yPos -= 131072;
                    playerO.speed = playerO.angle <= 96 ? playerO.yVelocity : playerO.yVelocity >> 1;
                }
                if (num11 > 158 && num11 < 192 && (num12 > 190 && num12 < 224))
                {
                    playerO.gravity = (byte)0;
                    playerO.angle = num11;
                    playerO.objectPtr.rotation = playerO.angle << 1;
                    playerO.collisionMode = (byte)1;
                    playerO.xPos -= 262144;
                    playerO.yPos -= 131072;
                    playerO.speed = playerO.angle >= 160 ? -playerO.yVelocity : -playerO.yVelocity >> 1;
                }
                if (playerO.yVelocity < 0)
                    playerO.yVelocity = 0;
                ObjectSystem.scriptEng.checkResult = 2;
            }
        }

        public static void SetPathGripSensors(PlayerObject playerO)
        {
            CollisionBox collisionBox = AnimationSystem.collisionBoxList[playerO.animationFile.cbListOffset + (int)AnimationSystem.animationFrames[AnimationSystem.animationList[playerO.animationFile.aniListOffset + (int)playerO.objectPtr.animation].frameListOffset + (int)playerO.objectPtr.frame].collisionBox];
            switch (playerO.collisionMode)
            {
                case 0:
                    PlayerSystem.collisionLeft = (int)collisionBox.left[0];
                    PlayerSystem.collisionTop = (int)collisionBox.top[0];
                    PlayerSystem.collisionRight = (int)collisionBox.right[0];
                    PlayerSystem.collisionBottom = (int)collisionBox.bottom[0];
                    PlayerSystem.cSensor[0].yPos = PlayerSystem.cSensor[4].yPos + (PlayerSystem.collisionBottom << 16);
                    PlayerSystem.cSensor[1].yPos = PlayerSystem.cSensor[0].yPos;
                    PlayerSystem.cSensor[2].yPos = PlayerSystem.cSensor[0].yPos;
                    PlayerSystem.cSensor[3].yPos = PlayerSystem.cSensor[4].yPos + 262144;
                    PlayerSystem.cSensor[0].xPos = PlayerSystem.cSensor[4].xPos + ((int)collisionBox.left[1] - 1 << 16);
                    PlayerSystem.cSensor[1].xPos = PlayerSystem.cSensor[4].xPos;
                    PlayerSystem.cSensor[2].xPos = PlayerSystem.cSensor[4].xPos + ((int)collisionBox.right[1] << 16);
                    if (playerO.speed > 0)
                    {
                        PlayerSystem.cSensor[3].xPos = PlayerSystem.cSensor[4].xPos + (PlayerSystem.collisionRight + 1 << 16);
                        break;
                    }
                    PlayerSystem.cSensor[3].xPos = PlayerSystem.cSensor[4].xPos + (PlayerSystem.collisionLeft - 1 << 16);
                    break;
                case 1:
                    PlayerSystem.collisionLeft = (int)collisionBox.left[2];
                    PlayerSystem.collisionTop = (int)collisionBox.top[2];
                    PlayerSystem.collisionRight = (int)collisionBox.right[2];
                    PlayerSystem.collisionBottom = (int)collisionBox.bottom[2];
                    PlayerSystem.cSensor[0].xPos = PlayerSystem.cSensor[4].xPos + (PlayerSystem.collisionRight << 16);
                    PlayerSystem.cSensor[1].xPos = PlayerSystem.cSensor[0].xPos;
                    PlayerSystem.cSensor[2].xPos = PlayerSystem.cSensor[0].xPos;
                    PlayerSystem.cSensor[3].xPos = PlayerSystem.cSensor[4].xPos + 262144;
                    PlayerSystem.cSensor[0].yPos = PlayerSystem.cSensor[4].yPos + ((int)collisionBox.top[3] - 1 << 16);
                    PlayerSystem.cSensor[1].yPos = PlayerSystem.cSensor[4].yPos;
                    PlayerSystem.cSensor[2].yPos = PlayerSystem.cSensor[4].yPos + ((int)collisionBox.bottom[3] << 16);
                    if (playerO.speed > 0)
                    {
                        PlayerSystem.cSensor[3].yPos = PlayerSystem.cSensor[4].yPos + (PlayerSystem.collisionTop << 16);
                        break;
                    }
                    PlayerSystem.cSensor[3].yPos = PlayerSystem.cSensor[4].yPos + (PlayerSystem.collisionBottom - 1 << 16);
                    break;
                case 2:
                    PlayerSystem.collisionLeft = (int)collisionBox.left[4];
                    PlayerSystem.collisionTop = (int)collisionBox.top[4];
                    PlayerSystem.collisionRight = (int)collisionBox.right[4];
                    PlayerSystem.collisionBottom = (int)collisionBox.bottom[4];
                    PlayerSystem.cSensor[0].yPos = PlayerSystem.cSensor[4].yPos + (PlayerSystem.collisionTop - 1 << 16);
                    PlayerSystem.cSensor[1].yPos = PlayerSystem.cSensor[0].yPos;
                    PlayerSystem.cSensor[2].yPos = PlayerSystem.cSensor[0].yPos;
                    PlayerSystem.cSensor[3].yPos = PlayerSystem.cSensor[4].yPos - 262144;
                    PlayerSystem.cSensor[0].xPos = PlayerSystem.cSensor[4].xPos + ((int)collisionBox.left[5] - 1 << 16);
                    PlayerSystem.cSensor[1].xPos = PlayerSystem.cSensor[4].xPos;
                    PlayerSystem.cSensor[2].xPos = PlayerSystem.cSensor[4].xPos + ((int)collisionBox.right[5] << 16);
                    if (playerO.speed < 0)
                    {
                        PlayerSystem.cSensor[3].xPos = PlayerSystem.cSensor[4].xPos + (PlayerSystem.collisionRight + 1 << 16);
                        break;
                    }
                    PlayerSystem.cSensor[3].xPos = PlayerSystem.cSensor[4].xPos + (PlayerSystem.collisionLeft - 1 << 16);
                    break;
                case 3:
                    PlayerSystem.collisionLeft = (int)collisionBox.left[6];
                    PlayerSystem.collisionTop = (int)collisionBox.top[6];
                    PlayerSystem.collisionRight = (int)collisionBox.right[6];
                    PlayerSystem.collisionBottom = (int)collisionBox.bottom[6];
                    PlayerSystem.cSensor[0].xPos = PlayerSystem.cSensor[4].xPos + (PlayerSystem.collisionLeft - 1 << 16);
                    PlayerSystem.cSensor[1].xPos = PlayerSystem.cSensor[0].xPos;
                    PlayerSystem.cSensor[2].xPos = PlayerSystem.cSensor[0].xPos;
                    PlayerSystem.cSensor[3].xPos = PlayerSystem.cSensor[4].xPos - 262144;
                    PlayerSystem.cSensor[0].yPos = PlayerSystem.cSensor[4].yPos + ((int)collisionBox.top[7] - 1 << 16);
                    PlayerSystem.cSensor[1].yPos = PlayerSystem.cSensor[4].yPos;
                    PlayerSystem.cSensor[2].yPos = PlayerSystem.cSensor[4].yPos + ((int)collisionBox.bottom[7] << 16);
                    if (playerO.speed > 0)
                    {
                        PlayerSystem.cSensor[3].yPos = PlayerSystem.cSensor[4].yPos + (PlayerSystem.collisionBottom << 16);
                        break;
                    }
                    PlayerSystem.cSensor[3].yPos = PlayerSystem.cSensor[4].yPos + (PlayerSystem.collisionTop - 1 << 16);
                    break;
            }
        }

        public static void ProcessPathGrip(PlayerObject playerO)
        {
            int index1 = -1;
            PlayerSystem.cSensor[4].xPos = playerO.xPos;
            PlayerSystem.cSensor[4].yPos = playerO.yPos;
            PlayerSystem.cSensor[3].collided = (byte)0;
            PlayerSystem.cSensor[0].angle = playerO.angle;
            PlayerSystem.cSensor[1].angle = playerO.angle;
            PlayerSystem.cSensor[2].angle = playerO.angle;
            PlayerSystem.SetPathGripSensors(playerO);
            int num1 = Math.Abs(playerO.speed);
            int num2 = num1 >> 18;
            int num3 = num1 & 262143;
            while (num2 > -1)
            {
                int num4;
                int num5;
                if (num2 < 1)
                {
                    num4 = GlobalAppDefinitions.CosValue256[playerO.angle] * num3 >> 8;
                    num5 = GlobalAppDefinitions.SinValue256[playerO.angle] * num3 >> 8;
                    num2 = -1;
                }
                else
                {
                    num4 = GlobalAppDefinitions.CosValue256[playerO.angle] << 10;
                    num5 = GlobalAppDefinitions.SinValue256[playerO.angle] << 10;
                    --num2;
                }
                if (playerO.speed < 0)
                {
                    num4 = -num4;
                    num5 = -num5;
                }
                PlayerSystem.cSensor[0].collided = (byte)0;
                PlayerSystem.cSensor[1].collided = (byte)0;
                PlayerSystem.cSensor[2].collided = (byte)0;
                PlayerSystem.cSensor[4].xPos += num4;
                PlayerSystem.cSensor[4].yPos += num5;
                switch (playerO.collisionMode)
                {
                    case 0:
                        PlayerSystem.cSensor[3].xPos += num4;
                        PlayerSystem.cSensor[3].yPos += num5;
                        if (playerO.speed > 0)
                            PlayerSystem.LWallCollision(playerO, PlayerSystem.cSensor[3]);
                        if (playerO.speed < 0)
                            PlayerSystem.RWallCollision(playerO, PlayerSystem.cSensor[3]);
                        if (PlayerSystem.cSensor[3].collided == (byte)1)
                        {
                            num4 = 0;
                            num2 = -1;
                        }
                        for (int index2 = 0; index2 < 3; ++index2)
                        {
                            PlayerSystem.cSensor[index2].xPos += num4;
                            PlayerSystem.cSensor[index2].yPos += num5;
                            PlayerSystem.FindFloorPosition(playerO, PlayerSystem.cSensor[index2], PlayerSystem.cSensor[index2].yPos >> 16);
                        }
                        index1 = -1;
                        for (int index2 = 0; index2 < 3; ++index2)
                        {
                            if (index1 > -1)
                            {
                                if (PlayerSystem.cSensor[index2].collided == (byte)1)
                                {
                                    if (PlayerSystem.cSensor[index2].yPos < PlayerSystem.cSensor[index1].yPos)
                                        index1 = index2;
                                    if (PlayerSystem.cSensor[index2].yPos == PlayerSystem.cSensor[index1].yPos && PlayerSystem.cSensor[index2].angle < 8 | PlayerSystem.cSensor[index2].angle > 248)
                                        index1 = index2;
                                }
                            }
                            else if (PlayerSystem.cSensor[index2].collided == (byte)1)
                                index1 = index2;
                        }
                        if (index1 > -1)
                        {
                            PlayerSystem.cSensor[0].yPos = PlayerSystem.cSensor[index1].yPos << 16;
                            PlayerSystem.cSensor[0].angle = PlayerSystem.cSensor[index1].angle;
                            PlayerSystem.cSensor[1].yPos = PlayerSystem.cSensor[0].yPos;
                            PlayerSystem.cSensor[1].angle = PlayerSystem.cSensor[0].angle;
                            PlayerSystem.cSensor[2].yPos = PlayerSystem.cSensor[0].yPos;
                            PlayerSystem.cSensor[2].angle = PlayerSystem.cSensor[0].angle;
                            PlayerSystem.cSensor[3].yPos = PlayerSystem.cSensor[0].yPos - 262144;
                            PlayerSystem.cSensor[3].angle = PlayerSystem.cSensor[0].angle;
                            PlayerSystem.cSensor[4].xPos = PlayerSystem.cSensor[1].xPos;
                            PlayerSystem.cSensor[4].yPos = PlayerSystem.cSensor[1].yPos - (PlayerSystem.collisionBottom << 16);
                        }
                        else
                            num2 = -1;
                        if (PlayerSystem.cSensor[0].angle < 222 && PlayerSystem.cSensor[0].angle > 128)
                            playerO.collisionMode = (byte)1;
                        if (PlayerSystem.cSensor[0].angle > 34 && PlayerSystem.cSensor[0].angle < 128)
                        {
                            playerO.collisionMode = (byte)3;
                            break;
                        }
                        break;
                    case 1:
                        PlayerSystem.cSensor[3].xPos += num4;
                        PlayerSystem.cSensor[3].yPos += num5;
                        if (playerO.speed > 0)
                            PlayerSystem.RoofCollision(playerO, PlayerSystem.cSensor[3]);
                        if (playerO.speed < 0)
                            PlayerSystem.FloorCollision(playerO, PlayerSystem.cSensor[3]);
                        if (PlayerSystem.cSensor[3].collided == (byte)1)
                        {
                            num5 = 0;
                            num2 = -1;
                        }
                        for (int index2 = 0; index2 < 3; ++index2)
                        {
                            PlayerSystem.cSensor[index2].xPos += num4;
                            PlayerSystem.cSensor[index2].yPos += num5;
                            PlayerSystem.FindLWallPosition(playerO, PlayerSystem.cSensor[index2], PlayerSystem.cSensor[index2].xPos >> 16);
                        }
                        index1 = -1;
                        for (int index2 = 0; index2 < 3; ++index2)
                        {
                            if (index1 > -1)
                            {
                                if (PlayerSystem.cSensor[index2].xPos < PlayerSystem.cSensor[index1].xPos && PlayerSystem.cSensor[index2].collided == (byte)1)
                                    index1 = index2;
                            }
                            else if (PlayerSystem.cSensor[index2].collided == (byte)1)
                                index1 = index2;
                        }
                        if (index1 > -1)
                        {
                            PlayerSystem.cSensor[0].xPos = PlayerSystem.cSensor[index1].xPos << 16;
                            PlayerSystem.cSensor[0].angle = PlayerSystem.cSensor[index1].angle;
                            PlayerSystem.cSensor[1].xPos = PlayerSystem.cSensor[0].xPos;
                            PlayerSystem.cSensor[1].angle = PlayerSystem.cSensor[0].angle;
                            PlayerSystem.cSensor[2].xPos = PlayerSystem.cSensor[0].xPos;
                            PlayerSystem.cSensor[2].angle = PlayerSystem.cSensor[0].angle;
                            PlayerSystem.cSensor[4].yPos = PlayerSystem.cSensor[1].yPos;
                            PlayerSystem.cSensor[4].xPos = PlayerSystem.cSensor[1].xPos - (PlayerSystem.collisionRight << 16);
                        }
                        else
                            num2 = -1;
                        if (PlayerSystem.cSensor[0].angle > 226)
                            playerO.collisionMode = (byte)0;
                        if (PlayerSystem.cSensor[0].angle < 158)
                        {
                            playerO.collisionMode = (byte)2;
                            break;
                        }
                        break;
                    case 2:
                        PlayerSystem.cSensor[3].xPos += num4;
                        PlayerSystem.cSensor[3].yPos += num5;
                        if (playerO.speed > 0)
                            PlayerSystem.RWallCollision(playerO, PlayerSystem.cSensor[3]);
                        if (playerO.speed < 0)
                            PlayerSystem.LWallCollision(playerO, PlayerSystem.cSensor[3]);
                        if (PlayerSystem.cSensor[3].collided == (byte)1)
                        {
                            num4 = 0;
                            num2 = -1;
                        }
                        for (int index2 = 0; index2 < 3; ++index2)
                        {
                            PlayerSystem.cSensor[index2].xPos += num4;
                            PlayerSystem.cSensor[index2].yPos += num5;
                            PlayerSystem.FindRoofPosition(playerO, PlayerSystem.cSensor[index2], PlayerSystem.cSensor[index2].yPos >> 16);
                        }
                        index1 = -1;
                        for (int index2 = 0; index2 < 3; ++index2)
                        {
                            if (index1 > -1)
                            {
                                if (PlayerSystem.cSensor[index2].yPos > PlayerSystem.cSensor[index1].yPos && PlayerSystem.cSensor[index2].collided == (byte)1)
                                    index1 = index2;
                            }
                            else if (PlayerSystem.cSensor[index2].collided == (byte)1)
                                index1 = index2;
                        }
                        if (index1 > -1)
                        {
                            PlayerSystem.cSensor[0].yPos = PlayerSystem.cSensor[index1].yPos << 16;
                            PlayerSystem.cSensor[0].angle = PlayerSystem.cSensor[index1].angle;
                            PlayerSystem.cSensor[1].yPos = PlayerSystem.cSensor[0].yPos;
                            PlayerSystem.cSensor[1].angle = PlayerSystem.cSensor[0].angle;
                            PlayerSystem.cSensor[2].yPos = PlayerSystem.cSensor[0].yPos;
                            PlayerSystem.cSensor[2].angle = PlayerSystem.cSensor[0].angle;
                            PlayerSystem.cSensor[3].yPos = PlayerSystem.cSensor[0].yPos + 262144;
                            PlayerSystem.cSensor[3].angle = PlayerSystem.cSensor[0].angle;
                            PlayerSystem.cSensor[4].xPos = PlayerSystem.cSensor[1].xPos;
                            PlayerSystem.cSensor[4].yPos = PlayerSystem.cSensor[1].yPos - (PlayerSystem.collisionTop - 1 << 16);
                        }
                        else
                            num2 = -1;
                        if (PlayerSystem.cSensor[0].angle > 162)
                            playerO.collisionMode = (byte)1;
                        if (PlayerSystem.cSensor[0].angle < 94)
                        {
                            playerO.collisionMode = (byte)3;
                            break;
                        }
                        break;
                    case 3:
                        PlayerSystem.cSensor[3].xPos += num4;
                        PlayerSystem.cSensor[3].yPos += num5;
                        if (playerO.speed > 0)
                            PlayerSystem.FloorCollision(playerO, PlayerSystem.cSensor[3]);
                        if (playerO.speed < 0)
                            PlayerSystem.RoofCollision(playerO, PlayerSystem.cSensor[3]);
                        if (PlayerSystem.cSensor[3].collided == (byte)1)
                        {
                            num5 = 0;
                            num2 = -1;
                        }
                        for (int index2 = 0; index2 < 3; ++index2)
                        {
                            PlayerSystem.cSensor[index2].xPos += num4;
                            PlayerSystem.cSensor[index2].yPos += num5;
                            PlayerSystem.FindRWallPosition(playerO, PlayerSystem.cSensor[index2], PlayerSystem.cSensor[index2].xPos >> 16);
                        }
                        index1 = -1;
                        for (int index2 = 0; index2 < 3; ++index2)
                        {
                            if (index1 > -1)
                            {
                                if (PlayerSystem.cSensor[index2].xPos > PlayerSystem.cSensor[index1].xPos && PlayerSystem.cSensor[index2].collided == (byte)1)
                                    index1 = index2;
                            }
                            else if (PlayerSystem.cSensor[index2].collided == (byte)1)
                                index1 = index2;
                        }
                        if (index1 > -1)
                        {
                            PlayerSystem.cSensor[0].xPos = PlayerSystem.cSensor[index1].xPos << 16;
                            PlayerSystem.cSensor[0].angle = PlayerSystem.cSensor[index1].angle;
                            PlayerSystem.cSensor[1].xPos = PlayerSystem.cSensor[0].xPos;
                            PlayerSystem.cSensor[1].angle = PlayerSystem.cSensor[0].angle;
                            PlayerSystem.cSensor[2].xPos = PlayerSystem.cSensor[0].xPos;
                            PlayerSystem.cSensor[2].angle = PlayerSystem.cSensor[0].angle;
                            PlayerSystem.cSensor[4].yPos = PlayerSystem.cSensor[1].yPos;
                            PlayerSystem.cSensor[4].xPos = PlayerSystem.cSensor[1].xPos - (PlayerSystem.collisionLeft - 1 << 16);
                        }
                        else
                            num2 = -1;
                        if (PlayerSystem.cSensor[0].angle < 30)
                            playerO.collisionMode = (byte)0;
                        if (PlayerSystem.cSensor[0].angle > 98)
                        {
                            playerO.collisionMode = (byte)2;
                            break;
                        }
                        break;
                }
                if (index1 > -1)
                    playerO.angle = PlayerSystem.cSensor[0].angle;
                if (PlayerSystem.cSensor[3].collided == (byte)1)
                    num2 = -2;
                else
                    PlayerSystem.SetPathGripSensors(playerO);
            }
            switch (playerO.collisionMode)
            {
                case 0:
                    if (PlayerSystem.cSensor[0].collided == (byte)0 && PlayerSystem.cSensor[1].collided == (byte)0 && PlayerSystem.cSensor[2].collided == (byte)0)
                    {
                        playerO.gravity = (byte)1;
                        playerO.collisionMode = (byte)0;
                        playerO.xVelocity = GlobalAppDefinitions.CosValue256[playerO.angle] * playerO.speed >> 8;
                        playerO.yVelocity = GlobalAppDefinitions.SinValue256[playerO.angle] * playerO.speed >> 8;
                        if (playerO.yVelocity < -1048576)
                            playerO.yVelocity = -1048576;
                        if (playerO.yVelocity > 1048576)
                            playerO.yVelocity = 1048576;
                        playerO.speed = playerO.xVelocity;
                        playerO.angle = 0;
                        if (PlayerSystem.cSensor[3].collided == (byte)1)
                        {
                            if (playerO.speed > 0)
                                playerO.xPos = PlayerSystem.cSensor[3].xPos - PlayerSystem.collisionRight << 16;
                            if (playerO.speed < 0)
                                playerO.xPos = PlayerSystem.cSensor[3].xPos - PlayerSystem.collisionLeft + 1 << 16;
                            playerO.speed = 0;
                            if (playerO.left == (byte)1 | playerO.right == (byte)1 && playerO.pushing < (byte)2)
                                ++playerO.pushing;
                        }
                        else
                        {
                            playerO.pushing = (byte)0;
                            playerO.xPos += playerO.xVelocity;
                        }
                        playerO.yPos += playerO.yVelocity;
                        break;
                    }
                    playerO.angle = PlayerSystem.cSensor[0].angle;
                    playerO.objectPtr.rotation = playerO.angle << 1;
                    playerO.flailing[0] = PlayerSystem.cSensor[0].collided;
                    playerO.flailing[1] = PlayerSystem.cSensor[1].collided;
                    playerO.flailing[2] = PlayerSystem.cSensor[2].collided;
                    if (PlayerSystem.cSensor[3].collided == (byte)1)
                    {
                        if (playerO.speed > 0)
                            playerO.xPos = PlayerSystem.cSensor[3].xPos - PlayerSystem.collisionRight << 16;
                        if (playerO.speed < 0)
                            playerO.xPos = PlayerSystem.cSensor[3].xPos - PlayerSystem.collisionLeft + 1 << 16;
                        playerO.speed = 0;
                        if (playerO.left == (byte)1 | playerO.right == (byte)1 && playerO.pushing < (byte)2)
                            ++playerO.pushing;
                    }
                    else
                    {
                        playerO.pushing = (byte)0;
                        playerO.xPos = PlayerSystem.cSensor[4].xPos;
                    }
                    playerO.yPos = PlayerSystem.cSensor[4].yPos;
                    break;
                case 1:
                    if (PlayerSystem.cSensor[0].collided == (byte)0 && PlayerSystem.cSensor[1].collided == (byte)0 && PlayerSystem.cSensor[2].collided == (byte)0)
                    {
                        playerO.gravity = (byte)1;
                        playerO.collisionMode = (byte)0;
                        playerO.xVelocity = GlobalAppDefinitions.CosValue256[playerO.angle] * playerO.speed >> 8;
                        playerO.yVelocity = GlobalAppDefinitions.SinValue256[playerO.angle] * playerO.speed >> 8;
                        if (playerO.yVelocity < -1048576)
                            playerO.yVelocity = -1048576;
                        if (playerO.yVelocity > 1048576)
                            playerO.yVelocity = 1048576;
                        playerO.speed = playerO.xVelocity;
                        playerO.angle = 0;
                    }
                    else if (playerO.speed < 163840 && playerO.speed > -163840 && playerO.controlLock == (byte)0)
                    {
                        playerO.gravity = (byte)1;
                        playerO.angle = 0;
                        playerO.collisionMode = (byte)0;
                        playerO.speed = playerO.xVelocity;
                        playerO.controlLock = (byte)30;
                    }
                    else
                    {
                        playerO.angle = PlayerSystem.cSensor[0].angle;
                        playerO.objectPtr.rotation = playerO.angle << 1;
                    }
                    if (PlayerSystem.cSensor[3].collided == (byte)1)
                    {
                        if (playerO.speed > 0)
                            playerO.yPos = PlayerSystem.cSensor[3].yPos - PlayerSystem.collisionTop << 16;
                        if (playerO.speed < 0)
                            playerO.yPos = PlayerSystem.cSensor[3].yPos - PlayerSystem.collisionBottom << 16;
                        playerO.speed = 0;
                    }
                    else
                        playerO.yPos = PlayerSystem.cSensor[4].yPos;
                    playerO.xPos = PlayerSystem.cSensor[4].xPos;
                    break;
                case 2:
                    if (PlayerSystem.cSensor[0].collided == (byte)0 && PlayerSystem.cSensor[1].collided == (byte)0 && PlayerSystem.cSensor[2].collided == (byte)0)
                    {
                        playerO.gravity = (byte)1;
                        playerO.collisionMode = (byte)0;
                        playerO.xVelocity = GlobalAppDefinitions.CosValue256[playerO.angle] * playerO.speed >> 8;
                        playerO.yVelocity = GlobalAppDefinitions.SinValue256[playerO.angle] * playerO.speed >> 8;
                        playerO.flailing[0] = (byte)0;
                        playerO.flailing[1] = (byte)0;
                        playerO.flailing[2] = (byte)0;
                        if (playerO.yVelocity < -1048576)
                            playerO.yVelocity = -1048576;
                        if (playerO.yVelocity > 1048576)
                            playerO.yVelocity = 1048576;
                        playerO.angle = 0;
                        playerO.speed = playerO.xVelocity;
                        if (PlayerSystem.cSensor[3].collided == (byte)1)
                        {
                            if (playerO.speed > 0)
                                playerO.xPos = PlayerSystem.cSensor[3].xPos - PlayerSystem.collisionRight << 16;
                            if (playerO.speed < 0)
                                playerO.xPos = PlayerSystem.cSensor[3].xPos - PlayerSystem.collisionLeft + 1 << 16;
                            playerO.speed = 0;
                        }
                        else
                            playerO.xPos += playerO.xVelocity;
                    }
                    else if (playerO.speed > -163840 && playerO.speed < 163840)
                    {
                        playerO.gravity = (byte)1;
                        playerO.angle = 0;
                        playerO.collisionMode = (byte)0;
                        playerO.speed = playerO.xVelocity;
                        playerO.flailing[0] = (byte)0;
                        playerO.flailing[1] = (byte)0;
                        playerO.flailing[2] = (byte)0;
                        if (PlayerSystem.cSensor[3].collided == (byte)1)
                        {
                            if (playerO.speed > 0)
                                playerO.xPos = PlayerSystem.cSensor[3].xPos - PlayerSystem.collisionRight << 16;
                            if (playerO.speed < 0)
                                playerO.xPos = PlayerSystem.cSensor[3].xPos - PlayerSystem.collisionLeft + 1 << 16;
                            playerO.speed = 0;
                        }
                        else
                            playerO.xPos += playerO.xVelocity;
                    }
                    else
                    {
                        playerO.angle = PlayerSystem.cSensor[0].angle;
                        playerO.objectPtr.rotation = playerO.angle << 1;
                        if (PlayerSystem.cSensor[3].collided == (byte)1)
                        {
                            if (playerO.speed < 0)
                                playerO.xPos = PlayerSystem.cSensor[3].xPos - PlayerSystem.collisionRight << 16;
                            if (playerO.speed > 0)
                                playerO.xPos = PlayerSystem.cSensor[3].xPos - PlayerSystem.collisionLeft + 1 << 16;
                            playerO.speed = 0;
                        }
                        else
                            playerO.xPos = PlayerSystem.cSensor[4].xPos;
                    }
                    playerO.yPos = PlayerSystem.cSensor[4].yPos;
                    break;
                case 3:
                    if (PlayerSystem.cSensor[0].collided == (byte)0 && PlayerSystem.cSensor[1].collided == (byte)0 && PlayerSystem.cSensor[2].collided == (byte)0)
                    {
                        playerO.gravity = (byte)1;
                        playerO.collisionMode = (byte)0;
                        playerO.xVelocity = GlobalAppDefinitions.CosValue256[playerO.angle] * playerO.speed >> 8;
                        playerO.yVelocity = GlobalAppDefinitions.SinValue256[playerO.angle] * playerO.speed >> 8;
                        if (playerO.yVelocity < -1048576)
                            playerO.yVelocity = -1048576;
                        if (playerO.yVelocity > 1048576)
                            playerO.yVelocity = 1048576;
                        playerO.speed = playerO.xVelocity;
                        playerO.angle = 0;
                    }
                    else if (playerO.speed > -163840 && playerO.speed < 163840 && playerO.controlLock == (byte)0)
                    {
                        playerO.gravity = (byte)1;
                        playerO.angle = 0;
                        playerO.collisionMode = (byte)0;
                        playerO.speed = playerO.xVelocity;
                        playerO.controlLock = (byte)30;
                    }
                    else
                    {
                        playerO.angle = PlayerSystem.cSensor[0].angle;
                        playerO.objectPtr.rotation = playerO.angle << 1;
                    }
                    if (PlayerSystem.cSensor[3].collided == (byte)1)
                    {
                        if (playerO.speed > 0)
                            playerO.yPos = PlayerSystem.cSensor[3].yPos - PlayerSystem.collisionBottom << 16;
                        if (playerO.speed < 0)
                            playerO.yPos = PlayerSystem.cSensor[3].yPos - PlayerSystem.collisionTop + 1 << 16;
                        playerO.speed = 0;
                    }
                    else
                        playerO.yPos = PlayerSystem.cSensor[4].yPos;
                    playerO.xPos = PlayerSystem.cSensor[4].xPos;
                    break;
            }
        }

        public static void FloorCollision(PlayerObject playerO, CollisionSensor cSensorRef)
        {
            int num1 = cSensorRef.yPos >> 16;
            for (int index1 = 0; index1 < 48; index1 += 16)
            {
                if (cSensorRef.collided == (byte)0)
                {
                    int num2 = cSensorRef.xPos >> 16;
                    int num3 = num2 >> 7;
                    int num4 = (num2 & (int)sbyte.MaxValue) >> 4;
                    int num5 = (cSensorRef.yPos >> 16) - 16 + index1;
                    int num6 = num5 >> 7;
                    int num7 = (num5 & (int)sbyte.MaxValue) >> 4;
                    if (num2 > -1 && num5 > -1)
                    {
                        int index2 = ((int)StageSystem.stageLayouts[0].tileMap[num3 + (num6 << 8)] << 6) + (num4 + (num7 << 3));
                        int index3 = (int)StageSystem.tile128x128.tile16x16[index2];
                        if (StageSystem.tile128x128.collisionFlag[(int)playerO.collisionPlane, index2] != (byte)2 && StageSystem.tile128x128.collisionFlag[(int)playerO.collisionPlane, index2] != (byte)3)
                        {
                            switch (StageSystem.tile128x128.direction[index2])
                            {
                                case 0:
                                    int index4 = (num2 & 15) + (index3 << 4);
                                    if ((num5 & 15) > (int)StageSystem.tileCollisions[(int)playerO.collisionPlane].floorMask[index4] - 16 + index1 && StageSystem.tileCollisions[(int)playerO.collisionPlane].floorMask[index4] < (sbyte)15)
                                    {
                                        cSensorRef.yPos = (int)StageSystem.tileCollisions[(int)playerO.collisionPlane].floorMask[index4] + (num6 << 7) + (num7 << 4);
                                        cSensorRef.collided = (byte)1;
                                        cSensorRef.angle = (int)StageSystem.tileCollisions[(int)playerO.collisionPlane].angle[index3] & (int)byte.MaxValue;
                                        break;
                                    }
                                    break;
                                case 1:
                                    int index5 = 15 - (num2 & 15) + (index3 << 4);
                                    if ((num5 & 15) > (int)StageSystem.tileCollisions[(int)playerO.collisionPlane].floorMask[index5] - 16 + index1 && StageSystem.tileCollisions[(int)playerO.collisionPlane].floorMask[index5] < (sbyte)15)
                                    {
                                        cSensorRef.yPos = (int)StageSystem.tileCollisions[(int)playerO.collisionPlane].floorMask[index5] + (num6 << 7) + (num7 << 4);
                                        cSensorRef.collided = (byte)1;
                                        cSensorRef.angle = 256 - ((int)StageSystem.tileCollisions[(int)playerO.collisionPlane].angle[index3] & (int)byte.MaxValue);
                                        break;
                                    }
                                    break;
                                case 2:
                                    int index6 = (num2 & 15) + (index3 << 4);
                                    if ((num5 & 15) > 15 - (int)StageSystem.tileCollisions[(int)playerO.collisionPlane].roofMask[index6] - 16 + index1)
                                    {
                                        cSensorRef.yPos = 15 - (int)StageSystem.tileCollisions[(int)playerO.collisionPlane].roofMask[index6] + (num6 << 7) + (num7 << 4);
                                        cSensorRef.collided = (byte)1;
                                        cSensorRef.angle = 384 - (int)((StageSystem.tileCollisions[(int)playerO.collisionPlane].angle[index3] & 4278190080U) >> 24) & (int)byte.MaxValue;
                                        break;
                                    }
                                    break;
                                case 3:
                                    int index7 = 15 - (num2 & 15) + (index3 << 4);
                                    if ((num5 & 15) > 15 - (int)StageSystem.tileCollisions[(int)playerO.collisionPlane].roofMask[index7] - 16 + index1)
                                    {
                                        cSensorRef.yPos = 15 - (int)StageSystem.tileCollisions[(int)playerO.collisionPlane].roofMask[index7] + (num6 << 7) + (num7 << 4);
                                        cSensorRef.collided = (byte)1;
                                        cSensorRef.angle = 256 - (384 - (int)((StageSystem.tileCollisions[(int)playerO.collisionPlane].angle[index3] & 4278190080U) >> 24) & (int)byte.MaxValue);
                                        break;
                                    }
                                    break;
                            }
                        }
                        if (cSensorRef.collided == (byte)1)
                        {
                            if (cSensorRef.angle < 0)
                                cSensorRef.angle += 256;
                            if (cSensorRef.angle > (int)byte.MaxValue)
                                cSensorRef.angle -= 256;
                            if (cSensorRef.yPos - num1 > 14)
                            {
                                cSensorRef.yPos = num1 << 16;
                                cSensorRef.collided = (byte)0;
                            }
                            else if (cSensorRef.yPos - num1 < -17)
                            {
                                cSensorRef.yPos = num1 << 16;
                                cSensorRef.collided = (byte)0;
                            }
                        }
                    }
                }
            }
        }

        public static void FindFloorPosition(
          PlayerObject playerO,
          CollisionSensor cSensorRef,
          int prevCollisionPos)
        {
            int angle = cSensorRef.angle;
            for (int index1 = 0; index1 < 48; index1 += 16)
            {
                if (cSensorRef.collided == (byte)0)
                {
                    int num1 = cSensorRef.xPos >> 16;
                    int num2 = num1 >> 7;
                    int num3 = (num1 & (int)sbyte.MaxValue) >> 4;
                    int num4 = (cSensorRef.yPos >> 16) - 16 + index1;
                    int num5 = num4 >> 7;
                    int num6 = (num4 & (int)sbyte.MaxValue) >> 4;
                    if (num1 > -1 && num4 > -1)
                    {
                        int index2 = ((int)StageSystem.stageLayouts[0].tileMap[num2 + (num5 << 8)] << 6) + (num3 + (num6 << 3));
                        int index3 = (int)StageSystem.tile128x128.tile16x16[index2];
                        if (StageSystem.tile128x128.collisionFlag[(int)playerO.collisionPlane, index2] != (byte)2 && StageSystem.tile128x128.collisionFlag[(int)playerO.collisionPlane, index2] != (byte)3)
                        {
                            switch (StageSystem.tile128x128.direction[index2])
                            {
                                case 0:
                                    int index4 = (num1 & 15) + (index3 << 4);
                                    if (StageSystem.tileCollisions[(int)playerO.collisionPlane].floorMask[index4] < (sbyte)64)
                                    {
                                        cSensorRef.yPos = (int)StageSystem.tileCollisions[(int)playerO.collisionPlane].floorMask[index4] + (num5 << 7) + (num6 << 4);
                                        cSensorRef.collided = (byte)1;
                                        cSensorRef.angle = (int)StageSystem.tileCollisions[(int)playerO.collisionPlane].angle[index3] & (int)byte.MaxValue;
                                        break;
                                    }
                                    break;
                                case 1:
                                    int index5 = 15 - (num1 & 15) + (index3 << 4);
                                    if (StageSystem.tileCollisions[(int)playerO.collisionPlane].floorMask[index5] < (sbyte)64)
                                    {
                                        cSensorRef.yPos = (int)StageSystem.tileCollisions[(int)playerO.collisionPlane].floorMask[index5] + (num5 << 7) + (num6 << 4);
                                        cSensorRef.collided = (byte)1;
                                        cSensorRef.angle = 256 - ((int)StageSystem.tileCollisions[(int)playerO.collisionPlane].angle[index3] & (int)byte.MaxValue);
                                        break;
                                    }
                                    break;
                                case 2:
                                    int index6 = (num1 & 15) + (index3 << 4);
                                    if (StageSystem.tileCollisions[(int)playerO.collisionPlane].roofMask[index6] > (sbyte)-64)
                                    {
                                        cSensorRef.yPos = 15 - (int)StageSystem.tileCollisions[(int)playerO.collisionPlane].roofMask[index6] + (num5 << 7) + (num6 << 4);
                                        cSensorRef.collided = (byte)1;
                                        cSensorRef.angle = 384 - (int)((StageSystem.tileCollisions[(int)playerO.collisionPlane].angle[index3] & 4278190080U) >> 24) & (int)byte.MaxValue;
                                        break;
                                    }
                                    break;
                                case 3:
                                    int index7 = 15 - (num1 & 15) + (index3 << 4);
                                    if (StageSystem.tileCollisions[(int)playerO.collisionPlane].roofMask[index7] > (sbyte)-64)
                                    {
                                        cSensorRef.yPos = 15 - (int)StageSystem.tileCollisions[(int)playerO.collisionPlane].roofMask[index7] + (num5 << 7) + (num6 << 4);
                                        cSensorRef.collided = (byte)1;
                                        cSensorRef.angle = 256 - (384 - (int)((StageSystem.tileCollisions[(int)playerO.collisionPlane].angle[index3] & 4278190080U) >> 24) & (int)byte.MaxValue);
                                        break;
                                    }
                                    break;
                            }
                        }
                        if (cSensorRef.collided == (byte)1)
                        {
                            if (cSensorRef.angle < 0)
                                cSensorRef.angle += 256;
                            if (cSensorRef.angle > (int)byte.MaxValue)
                                cSensorRef.angle -= 256;
                            if (Math.Abs(cSensorRef.angle - angle) > 32 && Math.Abs(cSensorRef.angle - 256 - angle) > 32 && Math.Abs(cSensorRef.angle + 256 - angle) > 32)
                            {
                                cSensorRef.yPos = prevCollisionPos << 16;
                                cSensorRef.collided = (byte)0;
                                cSensorRef.angle = angle;
                                index1 = 48;
                            }
                            else if (cSensorRef.yPos - prevCollisionPos > 14)
                            {
                                cSensorRef.yPos = prevCollisionPos << 16;
                                cSensorRef.collided = (byte)0;
                            }
                            else if (cSensorRef.yPos - prevCollisionPos < -14)
                            {
                                cSensorRef.yPos = prevCollisionPos << 16;
                                cSensorRef.collided = (byte)0;
                            }
                        }
                    }
                }
            }
        }

        public static void LWallCollision(PlayerObject playerO, CollisionSensor cSensorRef)
        {
            int num1 = cSensorRef.xPos >> 16;
            for (int index1 = 0; index1 < 48; index1 += 16)
            {
                if (cSensorRef.collided == (byte)0)
                {
                    int num2 = (cSensorRef.xPos >> 16) - 16 + index1;
                    int num3 = num2 >> 7;
                    int num4 = (num2 & (int)sbyte.MaxValue) >> 4;
                    int num5 = cSensorRef.yPos >> 16;
                    int num6 = num5 >> 7;
                    int num7 = (num5 & (int)sbyte.MaxValue) >> 4;
                    if (num2 > -1 && num5 > -1)
                    {
                        int index2 = ((int)StageSystem.stageLayouts[0].tileMap[num3 + (num6 << 8)] << 6) + (num4 + (num7 << 3));
                        int num8 = (int)StageSystem.tile128x128.tile16x16[index2];
                        if (StageSystem.tile128x128.collisionFlag[(int)playerO.collisionPlane, index2] != (byte)1 && StageSystem.tile128x128.collisionFlag[(int)playerO.collisionPlane, index2] < (byte)3)
                        {
                            switch (StageSystem.tile128x128.direction[index2])
                            {
                                case 0:
                                    int index3 = (num5 & 15) + (num8 << 4);
                                    if ((num2 & 15) > (int)StageSystem.tileCollisions[(int)playerO.collisionPlane].leftWallMask[index3] - 16 + index1)
                                    {
                                        cSensorRef.xPos = (int)StageSystem.tileCollisions[(int)playerO.collisionPlane].leftWallMask[index3] + (num3 << 7) + (num4 << 4);
                                        cSensorRef.collided = (byte)1;
                                        break;
                                    }
                                    break;
                                case 1:
                                    int index4 = (num5 & 15) + (num8 << 4);
                                    if ((num2 & 15) > 15 - (int)StageSystem.tileCollisions[(int)playerO.collisionPlane].rightWallMask[index4] - 16 + index1)
                                    {
                                        cSensorRef.xPos = 15 - (int)StageSystem.tileCollisions[(int)playerO.collisionPlane].rightWallMask[index4] + (num3 << 7) + (num4 << 4);
                                        cSensorRef.collided = (byte)1;
                                        break;
                                    }
                                    break;
                                case 2:
                                    int index5 = 15 - (num5 & 15) + (num8 << 4);
                                    if ((num2 & 15) > (int)StageSystem.tileCollisions[(int)playerO.collisionPlane].leftWallMask[index5] - 16 + index1)
                                    {
                                        cSensorRef.xPos = (int)StageSystem.tileCollisions[(int)playerO.collisionPlane].leftWallMask[index5] + (num3 << 7) + (num4 << 4);
                                        cSensorRef.collided = (byte)1;
                                        break;
                                    }
                                    break;
                                case 3:
                                    int index6 = 15 - (num5 & 15) + (num8 << 4);
                                    if ((num2 & 15) > 15 - (int)StageSystem.tileCollisions[(int)playerO.collisionPlane].rightWallMask[index6] - 16 + index1)
                                    {
                                        cSensorRef.xPos = 15 - (int)StageSystem.tileCollisions[(int)playerO.collisionPlane].rightWallMask[index6] + (num3 << 7) + (num4 << 4);
                                        cSensorRef.collided = (byte)1;
                                        break;
                                    }
                                    break;
                            }
                        }
                        if (cSensorRef.collided == (byte)1)
                        {
                            if (cSensorRef.xPos - num1 > 15)
                            {
                                cSensorRef.xPos = num1 << 16;
                                cSensorRef.collided = (byte)0;
                            }
                            else if (cSensorRef.xPos - num1 < -15)
                            {
                                cSensorRef.xPos = num1 << 16;
                                cSensorRef.collided = (byte)0;
                            }
                        }
                    }
                }
            }
        }

        public static void FindLWallPosition(
          PlayerObject playerO,
          CollisionSensor cSensorRef,
          int prevCollisionPos)
        {
            int angle = cSensorRef.angle;
            for (int index1 = 0; index1 < 48; index1 += 16)
            {
                if (cSensorRef.collided == (byte)0)
                {
                    int num1 = (cSensorRef.xPos >> 16) - 16 + index1;
                    int num2 = num1 >> 7;
                    int num3 = (num1 & (int)sbyte.MaxValue) >> 4;
                    int num4 = cSensorRef.yPos >> 16;
                    int num5 = num4 >> 7;
                    int num6 = (num4 & (int)sbyte.MaxValue) >> 4;
                    if (num1 > -1 && num4 > -1)
                    {
                        int index2 = ((int)StageSystem.stageLayouts[0].tileMap[num2 + (num5 << 8)] << 6) + (num3 + (num6 << 3));
                        int index3 = (int)StageSystem.tile128x128.tile16x16[index2];
                        if (StageSystem.tile128x128.collisionFlag[(int)playerO.collisionPlane, index2] < (byte)3)
                        {
                            switch (StageSystem.tile128x128.direction[index2])
                            {
                                case 0:
                                    int index4 = (num4 & 15) + (index3 << 4);
                                    if (StageSystem.tileCollisions[(int)playerO.collisionPlane].leftWallMask[index4] < (sbyte)64)
                                    {
                                        cSensorRef.xPos = (int)StageSystem.tileCollisions[(int)playerO.collisionPlane].leftWallMask[index4] + (num2 << 7) + (num3 << 4);
                                        cSensorRef.collided = (byte)1;
                                        cSensorRef.angle = (int)((StageSystem.tileCollisions[(int)playerO.collisionPlane].angle[index3] & 65280U) >> 8);
                                        break;
                                    }
                                    break;
                                case 1:
                                    int index5 = (num4 & 15) + (index3 << 4);
                                    if (StageSystem.tileCollisions[(int)playerO.collisionPlane].rightWallMask[index5] > (sbyte)-64)
                                    {
                                        cSensorRef.xPos = 15 - (int)StageSystem.tileCollisions[(int)playerO.collisionPlane].rightWallMask[index5] + (num2 << 7) + (num3 << 4);
                                        cSensorRef.collided = (byte)1;
                                        cSensorRef.angle = 256 - (int)((StageSystem.tileCollisions[(int)playerO.collisionPlane].angle[index3] & 16711680U) >> 16);
                                        break;
                                    }
                                    break;
                                case 2:
                                    int index6 = 15 - (num4 & 15) + (index3 << 4);
                                    if (StageSystem.tileCollisions[(int)playerO.collisionPlane].leftWallMask[index6] < (sbyte)64)
                                    {
                                        cSensorRef.xPos = (int)StageSystem.tileCollisions[(int)playerO.collisionPlane].leftWallMask[index6] + (num2 << 7) + (num3 << 4);
                                        cSensorRef.collided = (byte)1;
                                        cSensorRef.angle = 384 - (int)((StageSystem.tileCollisions[(int)playerO.collisionPlane].angle[index3] & 65280U) >> 8) & (int)byte.MaxValue;
                                        break;
                                    }
                                    break;
                                case 3:
                                    int index7 = 15 - (num4 & 15) + (index3 << 4);
                                    if (StageSystem.tileCollisions[(int)playerO.collisionPlane].rightWallMask[index7] > (sbyte)-64)
                                    {
                                        cSensorRef.xPos = 15 - (int)StageSystem.tileCollisions[(int)playerO.collisionPlane].rightWallMask[index7] + (num2 << 7) + (num3 << 4);
                                        cSensorRef.collided = (byte)1;
                                        cSensorRef.angle = 256 - (384 - (int)((StageSystem.tileCollisions[(int)playerO.collisionPlane].angle[index3] & 16711680U) >> 16) & (int)byte.MaxValue);
                                        break;
                                    }
                                    break;
                            }
                        }
                        if (cSensorRef.collided == (byte)1)
                        {
                            if (cSensorRef.angle < 0)
                                cSensorRef.angle += 256;
                            if (cSensorRef.angle > (int)byte.MaxValue)
                                cSensorRef.angle -= 256;
                            if (Math.Abs(angle - cSensorRef.angle) > 32)
                            {
                                cSensorRef.xPos = prevCollisionPos << 16;
                                cSensorRef.collided = (byte)0;
                                cSensorRef.angle = angle;
                                index1 = 48;
                            }
                            else if (cSensorRef.xPos - prevCollisionPos > 14)
                            {
                                cSensorRef.xPos = prevCollisionPos << 16;
                                cSensorRef.collided = (byte)0;
                            }
                            else if (cSensorRef.xPos - prevCollisionPos < -14)
                            {
                                cSensorRef.xPos = prevCollisionPos << 16;
                                cSensorRef.collided = (byte)0;
                            }
                        }
                    }
                }
            }
        }

        public static void RWallCollision(PlayerObject playerO, CollisionSensor cSensorRef)
        {
            int num1 = cSensorRef.xPos >> 16;
            for (int index1 = 0; index1 < 48; index1 += 16)
            {
                if (cSensorRef.collided == (byte)0)
                {
                    int num2 = (cSensorRef.xPos >> 16) + 16 - index1;
                    int num3 = num2 >> 7;
                    int num4 = (num2 & (int)sbyte.MaxValue) >> 4;
                    int num5 = cSensorRef.yPos >> 16;
                    int num6 = num5 >> 7;
                    int num7 = (num5 & (int)sbyte.MaxValue) >> 4;
                    if (num2 > -1 && num5 > -1)
                    {
                        int index2 = ((int)StageSystem.stageLayouts[0].tileMap[num3 + (num6 << 8)] << 6) + (num4 + (num7 << 3));
                        int num8 = (int)StageSystem.tile128x128.tile16x16[index2];
                        if (StageSystem.tile128x128.collisionFlag[(int)playerO.collisionPlane, index2] != (byte)1 && StageSystem.tile128x128.collisionFlag[(int)playerO.collisionPlane, index2] < (byte)3)
                        {
                            switch (StageSystem.tile128x128.direction[index2])
                            {
                                case 0:
                                    int index3 = (num5 & 15) + (num8 << 4);
                                    if ((num2 & 15) < (int)StageSystem.tileCollisions[(int)playerO.collisionPlane].rightWallMask[index3] + 16 - index1)
                                    {
                                        cSensorRef.xPos = (int)StageSystem.tileCollisions[(int)playerO.collisionPlane].rightWallMask[index3] + (num3 << 7) + (num4 << 4);
                                        cSensorRef.collided = (byte)1;
                                        break;
                                    }
                                    break;
                                case 1:
                                    int index4 = (num5 & 15) + (num8 << 4);
                                    if ((num2 & 15) < 15 - (int)StageSystem.tileCollisions[(int)playerO.collisionPlane].leftWallMask[index4] + 16 - index1)
                                    {
                                        cSensorRef.xPos = 15 - (int)StageSystem.tileCollisions[(int)playerO.collisionPlane].leftWallMask[index4] + (num3 << 7) + (num4 << 4);
                                        cSensorRef.collided = (byte)1;
                                        break;
                                    }
                                    break;
                                case 2:
                                    int index5 = 15 - (num5 & 15) + (num8 << 4);
                                    if ((num2 & 15) < (int)StageSystem.tileCollisions[(int)playerO.collisionPlane].rightWallMask[index5] + 16 - index1)
                                    {
                                        cSensorRef.xPos = (int)StageSystem.tileCollisions[(int)playerO.collisionPlane].rightWallMask[index5] + (num3 << 7) + (num4 << 4);
                                        cSensorRef.collided = (byte)1;
                                        break;
                                    }
                                    break;
                                case 3:
                                    int index6 = 15 - (num5 & 15) + (num8 << 4);
                                    if ((num2 & 15) < 15 - (int)StageSystem.tileCollisions[(int)playerO.collisionPlane].leftWallMask[index6] + 16 - index1)
                                    {
                                        cSensorRef.xPos = 15 - (int)StageSystem.tileCollisions[(int)playerO.collisionPlane].leftWallMask[index6] + (num3 << 7) + (num4 << 4);
                                        cSensorRef.collided = (byte)1;
                                        break;
                                    }
                                    break;
                            }
                        }
                        if (cSensorRef.collided == (byte)1)
                        {
                            if (cSensorRef.xPos - num1 > 15)
                            {
                                cSensorRef.xPos = num1 << 16;
                                cSensorRef.collided = (byte)0;
                            }
                            else if (cSensorRef.xPos - num1 < -15)
                            {
                                cSensorRef.xPos = num1 << 16;
                                cSensorRef.collided = (byte)0;
                            }
                        }
                    }
                }
            }
        }

        public static void FindRWallPosition(
          PlayerObject playerO,
          CollisionSensor cSensorRef,
          int prevCollisionPos)
        {
            int angle = cSensorRef.angle;
            for (int index1 = 0; index1 < 48; index1 += 16)
            {
                if (cSensorRef.collided == (byte)0)
                {
                    int num1 = (cSensorRef.xPos >> 16) + 16 - index1;
                    int num2 = num1 >> 7;
                    int num3 = (num1 & (int)sbyte.MaxValue) >> 4;
                    int num4 = cSensorRef.yPos >> 16;
                    int num5 = num4 >> 7;
                    int num6 = (num4 & (int)sbyte.MaxValue) >> 4;
                    if (num1 > -1 && num4 > -1)
                    {
                        int index2 = ((int)StageSystem.stageLayouts[0].tileMap[num2 + (num5 << 8)] << 6) + (num3 + (num6 << 3));
                        int index3 = (int)StageSystem.tile128x128.tile16x16[index2];
                        if (StageSystem.tile128x128.collisionFlag[(int)playerO.collisionPlane, index2] < (byte)3)
                        {
                            switch (StageSystem.tile128x128.direction[index2])
                            {
                                case 0:
                                    int index4 = (num4 & 15) + (index3 << 4);
                                    if (StageSystem.tileCollisions[(int)playerO.collisionPlane].rightWallMask[index4] > (sbyte)-64)
                                    {
                                        cSensorRef.xPos = (int)StageSystem.tileCollisions[(int)playerO.collisionPlane].rightWallMask[index4] + (num2 << 7) + (num3 << 4);
                                        cSensorRef.collided = (byte)1;
                                        cSensorRef.angle = (int)((StageSystem.tileCollisions[(int)playerO.collisionPlane].angle[index3] & 16711680U) >> 16);
                                        break;
                                    }
                                    break;
                                case 1:
                                    int index5 = (num4 & 15) + (index3 << 4);
                                    if (StageSystem.tileCollisions[(int)playerO.collisionPlane].leftWallMask[index5] < (sbyte)64)
                                    {
                                        cSensorRef.xPos = 15 - (int)StageSystem.tileCollisions[(int)playerO.collisionPlane].leftWallMask[index5] + (num2 << 7) + (num3 << 4);
                                        cSensorRef.collided = (byte)1;
                                        cSensorRef.angle = 256 - (int)((StageSystem.tileCollisions[(int)playerO.collisionPlane].angle[index3] & 65280U) >> 8);
                                        break;
                                    }
                                    break;
                                case 2:
                                    int index6 = 15 - (num4 & 15) + (index3 << 4);
                                    if (StageSystem.tileCollisions[(int)playerO.collisionPlane].rightWallMask[index6] > (sbyte)-64)
                                    {
                                        cSensorRef.xPos = (int)StageSystem.tileCollisions[(int)playerO.collisionPlane].rightWallMask[index6] + (num2 << 7) + (num3 << 4);
                                        cSensorRef.collided = (byte)1;
                                        cSensorRef.angle = 384 - (int)((StageSystem.tileCollisions[(int)playerO.collisionPlane].angle[index3] & 16711680U) >> 16) & (int)byte.MaxValue;
                                        break;
                                    }
                                    break;
                                case 3:
                                    int index7 = 15 - (num4 & 15) + (index3 << 4);
                                    if (StageSystem.tileCollisions[(int)playerO.collisionPlane].leftWallMask[index7] < (sbyte)64)
                                    {
                                        cSensorRef.xPos = 15 - (int)StageSystem.tileCollisions[(int)playerO.collisionPlane].leftWallMask[index7] + (num2 << 7) + (num3 << 4);
                                        cSensorRef.collided = (byte)1;
                                        cSensorRef.angle = 256 - (384 - (int)((StageSystem.tileCollisions[(int)playerO.collisionPlane].angle[index3] & 65280U) >> 8) & (int)byte.MaxValue);
                                        break;
                                    }
                                    break;
                            }
                        }
                        if (cSensorRef.collided == (byte)1)
                        {
                            if (cSensorRef.angle < 0)
                                cSensorRef.angle += 256;
                            if (cSensorRef.angle > (int)byte.MaxValue)
                                cSensorRef.angle -= 256;
                            if (Math.Abs(cSensorRef.angle - angle) > 32)
                            {
                                cSensorRef.xPos = prevCollisionPos << 16;
                                cSensorRef.collided = (byte)0;
                                cSensorRef.angle = angle;
                                index1 = 48;
                            }
                            else if (cSensorRef.xPos - prevCollisionPos > 14)
                            {
                                cSensorRef.xPos = prevCollisionPos >> 16;
                                cSensorRef.collided = (byte)0;
                            }
                            else if (cSensorRef.xPos - prevCollisionPos < -14)
                            {
                                cSensorRef.xPos = prevCollisionPos << 16;
                                cSensorRef.collided = (byte)0;
                            }
                        }
                    }
                }
            }
        }

        public static void RoofCollision(PlayerObject playerO, CollisionSensor cSensorRef)
        {
            int num1 = cSensorRef.yPos >> 16;
            for (int index1 = 0; index1 < 48; index1 += 16)
            {
                if (cSensorRef.collided == (byte)0)
                {
                    int num2 = cSensorRef.xPos >> 16;
                    int num3 = num2 >> 7;
                    int num4 = (num2 & (int)sbyte.MaxValue) >> 4;
                    int num5 = (cSensorRef.yPos >> 16) + 16 - index1;
                    int num6 = num5 >> 7;
                    int num7 = (num5 & (int)sbyte.MaxValue) >> 4;
                    if (num2 > -1 && num5 > -1)
                    {
                        int index2 = ((int)StageSystem.stageLayouts[0].tileMap[num3 + (num6 << 8)] << 6) + (num4 + (num7 << 3));
                        int index3 = (int)StageSystem.tile128x128.tile16x16[index2];
                        if (StageSystem.tile128x128.collisionFlag[(int)playerO.collisionPlane, index2] != (byte)1 && StageSystem.tile128x128.collisionFlag[(int)playerO.collisionPlane, index2] < (byte)3)
                        {
                            switch (StageSystem.tile128x128.direction[index2])
                            {
                                case 0:
                                    int index4 = (num2 & 15) + (index3 << 4);
                                    if ((num5 & 15) < (int)StageSystem.tileCollisions[(int)playerO.collisionPlane].roofMask[index4] + 16 - index1)
                                    {
                                        cSensorRef.yPos = (int)StageSystem.tileCollisions[(int)playerO.collisionPlane].roofMask[index4] + (num6 << 7) + (num7 << 4);
                                        cSensorRef.collided = (byte)1;
                                        cSensorRef.angle = (int)((StageSystem.tileCollisions[(int)playerO.collisionPlane].angle[index3] & 4278190080U) >> 24);
                                        break;
                                    }
                                    break;
                                case 1:
                                    int index5 = 15 - (num2 & 15) + (index3 << 4);
                                    if ((num5 & 15) < (int)StageSystem.tileCollisions[(int)playerO.collisionPlane].roofMask[index5] + 16 - index1)
                                    {
                                        cSensorRef.yPos = (int)StageSystem.tileCollisions[(int)playerO.collisionPlane].roofMask[index5] + (num6 << 7) + (num7 << 4);
                                        cSensorRef.collided = (byte)1;
                                        cSensorRef.angle = 256 - (int)((StageSystem.tileCollisions[(int)playerO.collisionPlane].angle[index3] & 4278190080U) >> 24);
                                        break;
                                    }
                                    break;
                                case 2:
                                    int index6 = (num2 & 15) + (index3 << 4);
                                    if ((num5 & 15) < 15 - (int)StageSystem.tileCollisions[(int)playerO.collisionPlane].floorMask[index6] + 16 - index1)
                                    {
                                        cSensorRef.yPos = 15 - (int)StageSystem.tileCollisions[(int)playerO.collisionPlane].floorMask[index6] + (num6 << 7) + (num7 << 4);
                                        cSensorRef.collided = (byte)1;
                                        cSensorRef.angle = 384 - ((int)StageSystem.tileCollisions[(int)playerO.collisionPlane].angle[index3] & (int)byte.MaxValue) & (int)byte.MaxValue;
                                        break;
                                    }
                                    break;
                                case 3:
                                    int index7 = 15 - (num2 & 15) + (index3 << 4);
                                    if ((num5 & 15) < 15 - (int)StageSystem.tileCollisions[(int)playerO.collisionPlane].floorMask[index7] + 16 - index1)
                                    {
                                        cSensorRef.yPos = 15 - (int)StageSystem.tileCollisions[(int)playerO.collisionPlane].floorMask[index7] + (num6 << 7) + (num7 << 4);
                                        cSensorRef.collided = (byte)1;
                                        cSensorRef.angle = 256 - (384 - ((int)StageSystem.tileCollisions[(int)playerO.collisionPlane].angle[index3] & (int)byte.MaxValue) & (int)byte.MaxValue);
                                        break;
                                    }
                                    break;
                            }
                        }
                        if (cSensorRef.collided == (byte)1)
                        {
                            if (cSensorRef.angle < 0)
                                cSensorRef.angle += 256;
                            if (cSensorRef.angle > (int)byte.MaxValue)
                                cSensorRef.angle -= 256;
                            if (cSensorRef.yPos - num1 > 14)
                            {
                                cSensorRef.yPos = num1 << 16;
                                cSensorRef.collided = (byte)0;
                            }
                            else if (cSensorRef.yPos - num1 < -14)
                            {
                                cSensorRef.yPos = num1 << 16;
                                cSensorRef.collided = (byte)0;
                            }
                        }
                    }
                }
            }
        }

        public static void FindRoofPosition(
          PlayerObject playerO,
          CollisionSensor cSensorRef,
          int prevCollisionPos)
        {
            int angle = cSensorRef.angle;
            for (int index1 = 0; index1 < 48; index1 += 16)
            {
                if (cSensorRef.collided == (byte)0)
                {
                    int num1 = cSensorRef.xPos >> 16;
                    int num2 = num1 >> 7;
                    int num3 = (num1 & (int)sbyte.MaxValue) >> 4;
                    int num4 = (cSensorRef.yPos >> 16) + 16 - index1;
                    int num5 = num4 >> 7;
                    int num6 = (num4 & (int)sbyte.MaxValue) >> 4;
                    if (num1 > -1 && num4 > -1)
                    {
                        int index2 = ((int)StageSystem.stageLayouts[0].tileMap[num2 + (num5 << 8)] << 6) + (num3 + (num6 << 3));
                        int index3 = (int)StageSystem.tile128x128.tile16x16[index2];
                        if (StageSystem.tile128x128.collisionFlag[(int)playerO.collisionPlane, index2] < (byte)3)
                        {
                            switch (StageSystem.tile128x128.direction[index2])
                            {
                                case 0:
                                    int index4 = (num1 & 15) + (index3 << 4);
                                    if (StageSystem.tileCollisions[(int)playerO.collisionPlane].roofMask[index4] > (sbyte)-64)
                                    {
                                        cSensorRef.yPos = (int)StageSystem.tileCollisions[(int)playerO.collisionPlane].roofMask[index4] + (num5 << 7) + (num6 << 4);
                                        cSensorRef.collided = (byte)1;
                                        cSensorRef.angle = (int)((StageSystem.tileCollisions[(int)playerO.collisionPlane].angle[index3] & 4278190080U) >> 24);
                                        break;
                                    }
                                    break;
                                case 1:
                                    int index5 = 15 - (num1 & 15) + (index3 << 4);
                                    if (StageSystem.tileCollisions[(int)playerO.collisionPlane].roofMask[index5] > (sbyte)-64)
                                    {
                                        cSensorRef.yPos = (int)StageSystem.tileCollisions[(int)playerO.collisionPlane].roofMask[index5] + (num5 << 7) + (num6 << 4);
                                        cSensorRef.collided = (byte)1;
                                        cSensorRef.angle = 256 - (int)((StageSystem.tileCollisions[(int)playerO.collisionPlane].angle[index3] & 4278190080U) >> 24);
                                        break;
                                    }
                                    break;
                                case 2:
                                    int index6 = (num1 & 15) + (index3 << 4);
                                    if (StageSystem.tileCollisions[(int)playerO.collisionPlane].floorMask[index6] < (sbyte)64)
                                    {
                                        cSensorRef.yPos = 15 - (int)StageSystem.tileCollisions[(int)playerO.collisionPlane].floorMask[index6] + (num5 << 7) + (num6 << 4);
                                        cSensorRef.collided = (byte)1;
                                        cSensorRef.angle = 384 - ((int)StageSystem.tileCollisions[(int)playerO.collisionPlane].angle[index3] & (int)byte.MaxValue) & (int)byte.MaxValue;
                                        break;
                                    }
                                    break;
                                case 3:
                                    int index7 = 15 - (num1 & 15) + (index3 << 4);
                                    if (StageSystem.tileCollisions[(int)playerO.collisionPlane].floorMask[index7] < (sbyte)64)
                                    {
                                        cSensorRef.yPos = 15 - (int)StageSystem.tileCollisions[(int)playerO.collisionPlane].floorMask[index7] + (num5 << 7) + (num6 << 4);
                                        cSensorRef.collided = (byte)1;
                                        cSensorRef.angle = 256 - (384 - ((int)StageSystem.tileCollisions[(int)playerO.collisionPlane].angle[index3] & (int)byte.MaxValue) & (int)byte.MaxValue);
                                        break;
                                    }
                                    break;
                            }
                        }
                        if (cSensorRef.collided == (byte)1)
                        {
                            if (cSensorRef.angle < 0)
                                cSensorRef.angle += 256;
                            if (cSensorRef.angle > (int)byte.MaxValue)
                                cSensorRef.angle -= 256;
                            if (Math.Abs(cSensorRef.angle - angle) > 32)
                            {
                                cSensorRef.yPos = prevCollisionPos << 16;
                                cSensorRef.collided = (byte)0;
                                cSensorRef.angle = angle;
                                index1 = 48;
                            }
                            else
                            {
                                if (cSensorRef.yPos - prevCollisionPos > 15)
                                {
                                    cSensorRef.yPos = prevCollisionPos << 16;
                                    cSensorRef.collided = (byte)0;
                                }
                                if (cSensorRef.yPos - prevCollisionPos < -15)
                                {
                                    cSensorRef.yPos = prevCollisionPos << 16;
                                    cSensorRef.collided = (byte)0;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
