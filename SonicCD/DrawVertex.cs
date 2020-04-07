// Decompiled with JetBrains decompiler
// Type: Retro_Engine.DrawVertex
// Assembly: Sonic CD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D35AF46A-1892-4F52-B201-E664C9200079
// Assembly location: E:\wamwo\Downloads\Sonic CD Mod Via Rebel\Sonic CD Mod Via Rebel\Sonic CD.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RetroEngine
{
  public struct DrawVertex : IVertexType
  {
    public static readonly VertexDeclaration VertexDeclaration = new VertexDeclaration(new VertexElement[3]
    {
      new VertexElement(0, VertexElementFormat.Vector2, VertexElementUsage.Position, 0),
      new VertexElement(8, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0),
      new VertexElement(16, VertexElementFormat.Color, VertexElementUsage.Color, 0)
    });
    public Vector2 position;
    public Vector2 texCoord;
    public Color color;

    public DrawVertex(Vector2 position, Vector2 texCoord, Color color)
    {
      this.position = position;
      this.texCoord = texCoord;
      this.color = color;
    }

    VertexDeclaration IVertexType.VertexDeclaration
    {
      get
      {
        return DrawVertex.VertexDeclaration;
      }
    }
  }
}
