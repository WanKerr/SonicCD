// Decompiled with JetBrains decompiler
// Type: Retro_Engine.PlayerObject
// Assembly: Sonic CD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D35AF46A-1892-4F52-B201-E664C9200079
// Assembly location: E:\wamwo\Downloads\Sonic CD Mod Via Rebel\Sonic CD Mod Via Rebel\Sonic CD.dll

namespace RetroEngine
{
  public class PlayerObject
  {
    public int[] value = new int[8];
    public PlayerStatistics movementStats = new PlayerStatistics();
    public byte[] flailing = new byte[3];
    public int objectNum;
    public int xPos;
    public int yPos;
    public int xVelocity;
    public int yVelocity;
    public int speed;
    public int screenXPos;
    public int screenYPos;
    public int angle;
    public int timer;
    public int lookPos;
    public byte collisionMode;
    public byte skidding;
    public byte pushing;
    public byte collisionPlane;
    public sbyte controlMode;
    public byte controlLock;
    public byte visible;
    public byte tileCollisions;
    public byte objectInteraction;
    public byte left;
    public byte right;
    public byte up;
    public byte down;
    public byte jumpPress;
    public byte jumpHold;
    public byte followPlayer1;
    public byte trackScroll;
    public byte gravity;
    public byte water;
    public AnimationFileList animationFile;
    public ObjectEntity objectPtr;
  }
}
