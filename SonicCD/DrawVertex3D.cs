// Decompiled with JetBrains decompiler
// Type: Retro_Engine.DrawVertex3D
// Assembly: Sonic CD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D35AF46A-1892-4F52-B201-E664C9200079
// Assembly location: E:\wamwo\Downloads\Sonic CD Mod Via Rebel\Sonic CD Mod Via Rebel\Sonic CD.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RetroEngine
{
  public struct DrawVertex3D : IVertexType
  {
    public static readonly VertexDeclaration VertexDeclaration = new VertexDeclaration(new VertexElement[3]
    {
      new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
      new VertexElement(12, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0),
      new VertexElement(20, VertexElementFormat.Color, VertexElementUsage.Color, 0)
    });
    public Vector3 position;
    public Vector2 texCoord;
    public Color color;

    public DrawVertex3D(Vector3 position, Vector2 texCoord, Color color)
    {
      this.position = position;
      this.texCoord = texCoord;
      this.color = color;
    }

    VertexDeclaration IVertexType.VertexDeclaration
    {
      get
      {
        return DrawVertex3D.VertexDeclaration;
      }
    }
  }
}
