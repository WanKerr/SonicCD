// Decompiled with JetBrains decompiler
// Type: Retro_Engine.Scene3D
// Assembly: Sonic CD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D35AF46A-1892-4F52-B201-E664C9200079
// Assembly location: E:\wamwo\Downloads\Sonic CD Mod Via Rebel\Sonic CD Mod Via Rebel\Sonic CD.dll

using System;

namespace RetroEngine
{
  public static class Scene3D
  {
    public static Vertex3D[] vertexBuffer = new Vertex3D[4096];
    public static Vertex3D[] vertexBufferT = new Vertex3D[4096];
    public static Face3D[] indexBuffer = new Face3D[1024];
    public static SortList[] drawList = new SortList[1024];
    public static int numVertices = 0;
    public static int numFaces = 0;
    public static int projectionX = 136;
    public static int projectionY = 160;
    public static int[] matWorld = new int[16];
    public static int[] matView = new int[16];
    public static int[] matFinal = new int[16];
    public static int[] matTemp = new int[16];

    static Scene3D()
    {
      for (int index = 0; index < Scene3D.vertexBuffer.Length; ++index)
        Scene3D.vertexBuffer[index] = new Vertex3D();
      for (int index = 0; index < Scene3D.vertexBufferT.Length; ++index)
        Scene3D.vertexBufferT[index] = new Vertex3D();
      for (int index = 0; index < Scene3D.indexBuffer.Length; ++index)
        Scene3D.indexBuffer[index] = new Face3D();
      for (int index = 0; index < Scene3D.drawList.Length; ++index)
        Scene3D.drawList[index] = new SortList();
    }

    public static void SetIdentityMatrix(ref int[] m)
    {
      m[0] = 256;
      m[1] = 0;
      m[2] = 0;
      m[3] = 0;
      m[4] = 0;
      m[5] = 256;
      m[6] = 0;
      m[7] = 0;
      m[8] = 0;
      m[9] = 0;
      m[10] = 256;
      m[11] = 0;
      m[12] = 0;
      m[13] = 0;
      m[14] = 0;
      m[15] = 256;
    }

    public static void MatrixMultiply(ref int[] a, ref int[] b)
    {
      int[] numArray = new int[16];
      for (uint index = 0; index < 16U; ++index)
      {
        uint num1 = index & 3U;
        uint num2 = index & 12U;
        numArray[index] = (b[num1] * a[num2] >> 8) + (b[(num1 + 4U)] * a[(num2 + 1U)] >> 8) + (b[(num1 + 8U)] * a[(num2 + 2U)] >> 8) + (b[(num1 + 12U)] * a[(num2 + 3U)] >> 8);
      }
      for (uint index = 0; index < 16U; ++index)
        a[index] = numArray[index];
    }

    public static void MatrixTranslateXYZ(ref int[] m, int xPos, int yPos, int zPos)
    {
      m[0] = 256;
      m[1] = 0;
      m[2] = 0;
      m[3] = 0;
      m[4] = 0;
      m[5] = 256;
      m[6] = 0;
      m[7] = 0;
      m[8] = 0;
      m[9] = 0;
      m[10] = 256;
      m[11] = 0;
      m[12] = xPos;
      m[13] = yPos;
      m[14] = zPos;
      m[15] = 256;
    }

    public static void MatrixScaleXYZ(ref int[] m, int xScale, int yScale, int zScale)
    {
      m[0] = xScale;
      m[1] = 0;
      m[2] = 0;
      m[3] = 0;
      m[4] = 0;
      m[5] = yScale;
      m[6] = 0;
      m[7] = 0;
      m[8] = 0;
      m[9] = 0;
      m[10] = zScale;
      m[11] = 0;
      m[12] = 0;
      m[13] = 0;
      m[14] = 0;
      m[15] = 256;
    }

    public static void MatrixRotateX(ref int[] m, int angle)
    {
      if (angle < 0)
        angle = 512 - angle;
      angle &= 511;
      int num1 = GlobalAppDefinitions.SinValue512[angle] >> 1;
      int num2 = GlobalAppDefinitions.CosValue512[angle] >> 1;
      m[0] = 256;
      m[1] = 0;
      m[2] = 0;
      m[3] = 0;
      m[4] = 0;
      m[5] = num2;
      m[6] = num1;
      m[7] = 0;
      m[8] = 0;
      m[9] = -num1;
      m[10] = num2;
      m[11] = 0;
      m[12] = 0;
      m[13] = 0;
      m[14] = 0;
      m[15] = 256;
    }

    public static void MatrixRotateY(ref int[] m, int angle)
    {
      if (angle < 0)
        angle = 512 - angle;
      angle &= 511;
      int num1 = GlobalAppDefinitions.SinValue512[angle] >> 1;
      int num2 = GlobalAppDefinitions.CosValue512[angle] >> 1;
      m[0] = num2;
      m[1] = 0;
      m[2] = num1;
      m[3] = 0;
      m[4] = 0;
      m[5] = 256;
      m[6] = 0;
      m[7] = 0;
      m[8] = -num1;
      m[9] = 0;
      m[10] = num2;
      m[11] = 0;
      m[12] = 0;
      m[13] = 0;
      m[14] = 0;
      m[15] = 256;
    }

    public static void MatrixRotateZ(ref int[] m, int angle)
    {
      if (angle < 0)
        angle = 512 - angle;
      angle &= 511;
      int num1 = GlobalAppDefinitions.SinValue512[angle] >> 1;
      int num2 = GlobalAppDefinitions.CosValue512[angle] >> 1;
      m[0] = num2;
      m[1] = 0;
      m[2] = num1;
      m[3] = 0;
      m[4] = 0;
      m[5] = 256;
      m[6] = 0;
      m[7] = 0;
      m[8] = -num1;
      m[9] = 0;
      m[10] = num2;
      m[11] = 0;
      m[12] = 0;
      m[13] = 0;
      m[14] = 0;
      m[15] = 256;
    }

    public static void MatrixRotateXYZ(ref int[] m, int angleX, int angleY, int angleZ)
    {
      if (angleX < 0)
        angleX = 512 - angleX;
      angleX &= 511;
      if (angleY < 0)
        angleY = 512 - angleY;
      angleY &= 511;
      if (angleZ < 0)
        angleZ = 512 - angleZ;
      angleZ &= 511;
      int num1 = GlobalAppDefinitions.SinValue512[angleX] >> 1;
      int num2 = GlobalAppDefinitions.CosValue512[angleX] >> 1;
      int num3 = GlobalAppDefinitions.SinValue512[angleY] >> 1;
      int num4 = GlobalAppDefinitions.CosValue512[angleY] >> 1;
      int num5 = GlobalAppDefinitions.SinValue512[angleZ] >> 1;
      int num6 = GlobalAppDefinitions.CosValue512[angleZ] >> 1;
      m[0] = (num4 * num6 >> 8) + ((num1 * num3 >> 8) * num5 >> 8);
      m[1] = (num4 * num5 >> 8) - ((num1 * num3 >> 8) * num6 >> 8);
      m[2] = num2 * num3 >> 8;
      m[3] = 0;
      m[4] = -num2 * num5 >> 8;
      m[5] = num2 * num6 >> 8;
      m[6] = num1;
      m[7] = 0;
      m[8] = ((num1 * num4 >> 8) * num5 >> 8) - (num3 * num6 >> 8);
      m[9] = (-num3 * num5 >> 8) - ((num1 * num4 >> 8) * num6 >> 8);
      m[10] = num2 * num4 >> 8;
      m[11] = 0;
      m[12] = 0;
      m[13] = 0;
      m[14] = 0;
      m[15] = 256;
    }

    public static void TransformVertexBuffer()
    {
      int index1 = 0;
      int index2 = 0;
      for (int index3 = 0; index3 < 16; ++index3)
        Scene3D.matFinal[index3] = Scene3D.matWorld[index3];
      Scene3D.MatrixMultiply(ref Scene3D.matFinal, ref Scene3D.matView);
      for (int index3 = 0; index3 < Scene3D.numVertices; ++index3)
      {
        Vertex3D vertex3D = Scene3D.vertexBuffer[index1];
        Scene3D.vertexBufferT[index2].x = (Scene3D.matFinal[0] * vertex3D.x >> 8) + (Scene3D.matFinal[4] * vertex3D.y >> 8) + (Scene3D.matFinal[8] * vertex3D.z >> 8) + Scene3D.matFinal[12];
        Scene3D.vertexBufferT[index2].y = (Scene3D.matFinal[1] * vertex3D.x >> 8) + (Scene3D.matFinal[5] * vertex3D.y >> 8) + (Scene3D.matFinal[9] * vertex3D.z >> 8) + Scene3D.matFinal[13];
        Scene3D.vertexBufferT[index2].z = (Scene3D.matFinal[2] * vertex3D.x >> 8) + (Scene3D.matFinal[6] * vertex3D.y >> 8) + (Scene3D.matFinal[10] * vertex3D.z >> 8) + Scene3D.matFinal[14];
        if (Scene3D.vertexBufferT[index2].z < 1 && Scene3D.vertexBufferT[index2].z > 0)
          Scene3D.vertexBufferT[index2].z = 1;
        ++index1;
        ++index2;
      }
    }

    public static void TransformVertices(ref int[] m, int vStart, int vEnd)
    {
      int num = 0;
      Vertex3D vertex3D1 = new Vertex3D();
      ++vEnd;
      for (int index = vStart; index < vEnd; ++index)
      {
        Vertex3D vertex3D2 = Scene3D.vertexBuffer[index];
        vertex3D1.x = (m[0] * vertex3D2.x >> 8) + (m[4] * vertex3D2.y >> 8) + (m[8] * vertex3D2.z >> 8) + m[12];
        vertex3D1.y = (m[1] * vertex3D2.x >> 8) + (m[5] * vertex3D2.y >> 8) + (m[9] * vertex3D2.z >> 8) + m[13];
        vertex3D1.z = (m[2] * vertex3D2.x >> 8) + (m[6] * vertex3D2.y >> 8) + (m[10] * vertex3D2.z >> 8) + m[14];
        Scene3D.vertexBuffer[index].x = vertex3D1.x;
        Scene3D.vertexBuffer[index].y = vertex3D1.y;
        Scene3D.vertexBuffer[index].z = vertex3D1.z;
        ++num;
      }
    }

    public static void Sort3DDrawList()
    {
      for (int index = 0; index < Scene3D.numFaces; ++index)
      {
        Scene3D.drawList[index].z = Scene3D.vertexBufferT[Scene3D.indexBuffer[index].a].z + Scene3D.vertexBufferT[Scene3D.indexBuffer[index].b].z + Scene3D.vertexBufferT[Scene3D.indexBuffer[index].c].z + Scene3D.vertexBufferT[Scene3D.indexBuffer[index].d].z >> 2;
        Scene3D.drawList[index].index = index;
      }
      for (int index1 = 0; index1 < Scene3D.numFaces; ++index1)
      {
        for (int index2 = Scene3D.numFaces - 1; index2 > index1; --index2)
        {
          if (Scene3D.drawList[index2].z > Scene3D.drawList[index2 - 1].z)
          {
            int index3 = Scene3D.drawList[index2].index;
            int z = Scene3D.drawList[index2].z;
            Scene3D.drawList[index2].index = Scene3D.drawList[index2 - 1].index;
            Scene3D.drawList[index2].z = Scene3D.drawList[index2 - 1].z;
            Scene3D.drawList[index2 - 1].index = index3;
            Scene3D.drawList[index2 - 1].z = z;
          }
        }
      }
    }

    public static void Draw3DScene(int surfaceNum)
    {
      Quad2D face = new Quad2D();
      for (int index = 0; index < Scene3D.numFaces; ++index)
      {
        Face3D face3D = Scene3D.indexBuffer[Scene3D.drawList[index].index];
        switch (face3D.flag)
        {
          case 0:
            if (Scene3D.vertexBufferT[face3D.a].z > 256 && Scene3D.vertexBufferT[face3D.b].z > 256 && (Scene3D.vertexBufferT[face3D.c].z > 256 && Scene3D.vertexBufferT[face3D.d].z > 256))
            {
              face.vertex[0].x = GlobalAppDefinitions.SCREEN_CENTER + Scene3D.vertexBufferT[face3D.a].x * Scene3D.projectionX / Scene3D.vertexBufferT[face3D.a].z;
              face.vertex[0].y = 120 - Scene3D.vertexBufferT[face3D.a].y * Scene3D.projectionY / Scene3D.vertexBufferT[face3D.a].z;
              face.vertex[1].x = GlobalAppDefinitions.SCREEN_CENTER + Scene3D.vertexBufferT[face3D.b].x * Scene3D.projectionX / Scene3D.vertexBufferT[face3D.b].z;
              face.vertex[1].y = 120 - Scene3D.vertexBufferT[face3D.b].y * Scene3D.projectionY / Scene3D.vertexBufferT[face3D.b].z;
              face.vertex[2].x = GlobalAppDefinitions.SCREEN_CENTER + Scene3D.vertexBufferT[face3D.c].x * Scene3D.projectionX / Scene3D.vertexBufferT[face3D.c].z;
              face.vertex[2].y = 120 - Scene3D.vertexBufferT[face3D.c].y * Scene3D.projectionY / Scene3D.vertexBufferT[face3D.c].z;
              face.vertex[3].x = GlobalAppDefinitions.SCREEN_CENTER + Scene3D.vertexBufferT[face3D.d].x * Scene3D.projectionX / Scene3D.vertexBufferT[face3D.d].z;
              face.vertex[3].y = 120 - Scene3D.vertexBufferT[face3D.d].y * Scene3D.projectionY / Scene3D.vertexBufferT[face3D.d].z;
              face.vertex[0].u = Scene3D.vertexBuffer[face3D.a].u;
              face.vertex[0].v = Scene3D.vertexBuffer[face3D.a].v;
              face.vertex[1].u = Scene3D.vertexBuffer[face3D.b].u;
              face.vertex[1].v = Scene3D.vertexBuffer[face3D.b].v;
              face.vertex[2].u = Scene3D.vertexBuffer[face3D.c].u;
              face.vertex[2].v = Scene3D.vertexBuffer[face3D.c].v;
              face.vertex[3].u = Scene3D.vertexBuffer[face3D.d].u;
              face.vertex[3].v = Scene3D.vertexBuffer[face3D.d].v;
              GraphicsSystem.DrawTexturedQuad(face, surfaceNum);
              break;
            }
            break;
          case 1:
            face.vertex[0].x = Scene3D.vertexBuffer[face3D.a].x;
            face.vertex[0].y = Scene3D.vertexBuffer[face3D.a].y;
            face.vertex[1].x = Scene3D.vertexBuffer[face3D.b].x;
            face.vertex[1].y = Scene3D.vertexBuffer[face3D.b].y;
            face.vertex[2].x = Scene3D.vertexBuffer[face3D.c].x;
            face.vertex[2].y = Scene3D.vertexBuffer[face3D.c].y;
            face.vertex[3].x = Scene3D.vertexBuffer[face3D.d].x;
            face.vertex[3].y = Scene3D.vertexBuffer[face3D.d].y;
            face.vertex[0].u = Scene3D.vertexBuffer[face3D.a].u;
            face.vertex[0].v = Scene3D.vertexBuffer[face3D.a].v;
            face.vertex[1].u = Scene3D.vertexBuffer[face3D.b].u;
            face.vertex[1].v = Scene3D.vertexBuffer[face3D.b].v;
            face.vertex[2].u = Scene3D.vertexBuffer[face3D.c].u;
            face.vertex[2].v = Scene3D.vertexBuffer[face3D.c].v;
            face.vertex[3].u = Scene3D.vertexBuffer[face3D.d].u;
            face.vertex[3].v = Scene3D.vertexBuffer[face3D.d].v;
            GraphicsSystem.DrawTexturedQuad(face, surfaceNum);
            break;
          case 2:
            if (Scene3D.vertexBufferT[face3D.a].z > 256 && Scene3D.vertexBufferT[face3D.b].z > 256 && (Scene3D.vertexBufferT[face3D.c].z > 256 && Scene3D.vertexBufferT[face3D.d].z > 256))
            {
              face.vertex[0].x = GlobalAppDefinitions.SCREEN_CENTER + Scene3D.vertexBufferT[face3D.a].x * Scene3D.projectionX / Scene3D.vertexBufferT[face3D.a].z;
              face.vertex[0].y = 120 - Scene3D.vertexBufferT[face3D.a].y * Scene3D.projectionY / Scene3D.vertexBufferT[face3D.a].z;
              face.vertex[1].x = GlobalAppDefinitions.SCREEN_CENTER + Scene3D.vertexBufferT[face3D.b].x * Scene3D.projectionX / Scene3D.vertexBufferT[face3D.b].z;
              face.vertex[1].y = 120 - Scene3D.vertexBufferT[face3D.b].y * Scene3D.projectionY / Scene3D.vertexBufferT[face3D.b].z;
              face.vertex[2].x = GlobalAppDefinitions.SCREEN_CENTER + Scene3D.vertexBufferT[face3D.c].x * Scene3D.projectionX / Scene3D.vertexBufferT[face3D.c].z;
              face.vertex[2].y = 120 - Scene3D.vertexBufferT[face3D.c].y * Scene3D.projectionY / Scene3D.vertexBufferT[face3D.c].z;
              face.vertex[3].x = GlobalAppDefinitions.SCREEN_CENTER + Scene3D.vertexBufferT[face3D.d].x * Scene3D.projectionX / Scene3D.vertexBufferT[face3D.d].z;
              face.vertex[3].y = 120 - Scene3D.vertexBufferT[face3D.d].y * Scene3D.projectionY / Scene3D.vertexBufferT[face3D.d].z;
              GraphicsSystem.DrawQuad(face, face3D.color);
              break;
            }
            break;
          case 3:
            face.vertex[0].x = Scene3D.vertexBuffer[face3D.a].x;
            face.vertex[0].y = Scene3D.vertexBuffer[face3D.a].y;
            face.vertex[1].x = Scene3D.vertexBuffer[face3D.b].x;
            face.vertex[1].y = Scene3D.vertexBuffer[face3D.b].y;
            face.vertex[2].x = Scene3D.vertexBuffer[face3D.c].x;
            face.vertex[2].y = Scene3D.vertexBuffer[face3D.c].y;
            face.vertex[3].x = Scene3D.vertexBuffer[face3D.d].x;
            face.vertex[3].y = Scene3D.vertexBuffer[face3D.d].y;
            GraphicsSystem.DrawQuad(face, face3D.color);
            break;
        }
      }
    }
  }
}
