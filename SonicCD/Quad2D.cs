// Decompiled with JetBrains decompiler
// Type: Retro_Engine.Quad2D
// Assembly: Sonic CD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D35AF46A-1892-4F52-B201-E664C9200079
// Assembly location: E:\wamwo\Downloads\Sonic CD Mod Via Rebel\Sonic CD Mod Via Rebel\Sonic CD.dll

namespace RetroEngine
{
  public class Quad2D
  {
    public Vertex2D[] vertex = new Vertex2D[4];

    public Quad2D()
    {
      for (int index = 0; index < 4; ++index)
        this.vertex[index] = new Vertex2D();
    }
  }
}
