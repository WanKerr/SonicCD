// Decompiled with JetBrains decompiler
// Type: Retro_Engine.RenderDevice
// Assembly: Sonic CD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D35AF46A-1892-4F52-B201-E664C9200079
// Assembly location: E:\wamwo\Downloads\Sonic CD Mod Via Rebel\Sonic CD Mod Via Rebel\Sonic CD.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RetroEngine
{
    public static class RenderDevice
    {
        private static RasterizerState rasterState = new RasterizerState();
        public static Texture2D[] gfxTexture = new Texture2D[6];
        public static int highResMode = 0;
        public static bool useFBTexture = true;
        public const int NUM_TEXTURES = 6;
        private static GraphicsDevice gDevice;
        private static BasicEffect effect;
        private static Matrix projection2D;
        private static Matrix projection3D;
        private static RenderTarget2D renderTarget;
        private static SpriteBatch screenSprite;
        private static Rectangle screenRect;
        public static int orthWidth;
        public static int viewWidth;
        public static int viewHeight;
        public static float viewAspect;
        public static int bufferWidth;
        public static int bufferHeight;

        public static void InitRenderDevice(GraphicsDevice graphicsRef)
        {
            gDevice = graphicsRef;
            effect = new BasicEffect(gDevice);
            effect.TextureEnabled = true;
            GraphicsSystem.SetupPolygonLists();
            for (int index = 0; index < 6; ++index)
                gfxTexture[index] = new Texture2D(gDevice, 1024, 1024, false, SurfaceFormat.Bgra5551);
            renderTarget = new RenderTarget2D(gDevice, 400, 240, false, SurfaceFormat.Bgr565, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);
            rasterState.CullMode = CullMode.None;
            gDevice.RasterizerState = rasterState;
            gDevice.PresentationParameters.RenderTargetUsage = RenderTargetUsage.PreserveContents;
            screenSprite = new SpriteBatch(gDevice);
        }

        public static void UpdateHardwareTextures()
        {
            gDevice.Textures[0] = (Texture)null;
            GraphicsSystem.SetActivePalette((byte)0, 0, 240);
            GraphicsSystem.UpdateTextureBufferWithTiles();
            GraphicsSystem.UpdateTextureBufferWithSortedSprites();
            gfxTexture[0].SetData<ushort>(GraphicsSystem.texBuffer);
            for (byte paletteNum = 1; paletteNum < (byte)6; ++paletteNum)
            {
                GraphicsSystem.SetActivePalette(paletteNum, 0, 240);
                GraphicsSystem.UpdateTextureBufferWithTiles();
                GraphicsSystem.UpdateTextureBufferWithSprites();
                gfxTexture[(int)paletteNum].SetData<ushort>(GraphicsSystem.texBuffer);
            }
            GraphicsSystem.SetActivePalette((byte)0, 0, 240);
        }

        public static void SetScreenDimensions(int width, int height)
        {
            InputSystem.touchWidth = width;
            InputSystem.touchHeight = height;
            viewWidth = InputSystem.touchWidth;
            viewHeight = InputSystem.touchHeight;
            bufferWidth = (int)((float)viewWidth / (float)viewHeight * 240f);
            bufferWidth += 8;
            bufferWidth = bufferWidth >> 4 << 4;
            if (bufferWidth > 400)
                bufferWidth = 400;
            viewAspect = 0.75f;
            GlobalAppDefinitions.HQ3DFloorEnabled = viewHeight >= 480;
            //if (RenderDevice.viewHeight >= 480)
            //{
            //  GraphicsSystem.SetScreenRenderSize(RenderDevice.bufferWidth, RenderDevice.bufferWidth);
            //  RenderDevice.bufferWidth *= 2;
            //  RenderDevice.bufferHeight = 480;
            //}
            //else
            //{
            //  RenderDevice.bufferHeight = 240;
            //  GraphicsSystem.SetScreenRenderSize(RenderDevice.bufferWidth, RenderDevice.bufferWidth);
            //}

            GraphicsSystem.SetScreenRenderSize(bufferWidth, bufferWidth);
            orthWidth = GlobalAppDefinitions.SCREEN_XSIZE * 16;
            projection2D = Matrix.CreateOrthographicOffCenter(4f, (float)(orthWidth + 4), 3844f, 4f, 0.0f, 100f);
            projection3D = Matrix.CreatePerspectiveFieldOfView(1.832596f, viewAspect, 0.1f, 2000f) * Matrix.CreateScale(1f, -1f, 1f) * Matrix.CreateTranslation(0.0f, -0.045f, 0.0f);
            screenRect = new Rectangle(0, 0, viewWidth, viewHeight);
        }

        public static void FlipScreen()
        {
            gDevice.SetRenderTarget(renderTarget);
            effect.Texture = gfxTexture[GraphicsSystem.texPaletteNum];
            effect.World = Matrix.Identity;
            effect.View = Matrix.Identity;
            effect.Projection = projection2D;
            effect.LightingEnabled = false;
            effect.VertexColorEnabled = true;
            gDevice.RasterizerState = rasterState;
            if (GraphicsSystem.render3DEnabled)
            {
                foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    gDevice.BlendState = BlendState.Opaque;
                    gDevice.SamplerStates[0] = SamplerState.PointClamp;
                    if (GraphicsSystem.gfxIndexSizeOpaque > (ushort)0)
                        effect.GraphicsDevice.DrawUserIndexedPrimitives<DrawVertex>(PrimitiveType.TriangleList, GraphicsSystem.gfxPolyList, 0, (int)GraphicsSystem.gfxVertexSizeOpaque, GraphicsSystem.gfxPolyListIndex, 0, (int)GraphicsSystem.gfxIndexSizeOpaque);
                }
                gDevice.BlendState = BlendState.NonPremultiplied;
                effect.World = Matrix.CreateTranslation(GraphicsSystem.floor3DPos) * Matrix.CreateRotationY((float)(3.14159274101257 * (180.0 + (double)GraphicsSystem.floor3DAngle) / 180.0));
                effect.Projection = projection3D;
                foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    if (GraphicsSystem.indexSize3D > (ushort)0)
                        effect.GraphicsDevice.DrawUserIndexedPrimitives<DrawVertex3D>(PrimitiveType.TriangleList, GraphicsSystem.polyList3D, 0, (int)GraphicsSystem.vertexSize3D, GraphicsSystem.gfxPolyListIndex, 0, (int)GraphicsSystem.indexSize3D);
                }
                effect.World = Matrix.Identity;
                effect.Projection = projection2D;
                foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    int primitiveCount = (int)GraphicsSystem.gfxIndexSize - (int)GraphicsSystem.gfxIndexSizeOpaque;
                    if (primitiveCount > 0)
                        effect.GraphicsDevice.DrawUserIndexedPrimitives<DrawVertex>(PrimitiveType.TriangleList, GraphicsSystem.gfxPolyList, (int)GraphicsSystem.gfxVertexSizeOpaque, (int)GraphicsSystem.gfxVertexSize - (int)GraphicsSystem.gfxVertexSizeOpaque, GraphicsSystem.gfxPolyListIndex, 0, primitiveCount);
                }
            }
            else
            {
                foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    gDevice.BlendState = BlendState.Opaque;
                    gDevice.SamplerStates[0] = SamplerState.PointClamp;
                    if (GraphicsSystem.gfxIndexSizeOpaque > (ushort)0)
                        effect.GraphicsDevice.DrawUserIndexedPrimitives<DrawVertex>(PrimitiveType.TriangleList, GraphicsSystem.gfxPolyList, 0, (int)GraphicsSystem.gfxVertexSizeOpaque, GraphicsSystem.gfxPolyListIndex, 0, (int)GraphicsSystem.gfxIndexSizeOpaque);
                    gDevice.BlendState = BlendState.NonPremultiplied;
                    int primitiveCount = (int)GraphicsSystem.gfxIndexSize - (int)GraphicsSystem.gfxIndexSizeOpaque;
                    if (primitiveCount > 0)
                        effect.GraphicsDevice.DrawUserIndexedPrimitives<DrawVertex>(PrimitiveType.TriangleList, GraphicsSystem.gfxPolyList, (int)GraphicsSystem.gfxVertexSizeOpaque, (int)GraphicsSystem.gfxVertexSize - (int)GraphicsSystem.gfxVertexSizeOpaque, GraphicsSystem.gfxPolyListIndex, 0, primitiveCount);
                }
            }
            effect.Texture = (Texture2D)null;
            gDevice.SetRenderTarget((RenderTarget2D)null);
            screenSprite.Begin(SpriteSortMode.Immediate, BlendState.Opaque, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone);
            screenSprite.Draw((Texture2D)renderTarget, screenRect, Color.White);
            screenSprite.End();
        }

        public static void FlipScreenHRes()
        {
            effect.Texture = gfxTexture[GraphicsSystem.texPaletteNum];
            effect.World = Matrix.Identity;
            effect.View = Matrix.Identity;
            effect.Projection = projection2D;
            effect.LightingEnabled = false;
            effect.VertexColorEnabled = true;
            gDevice.RasterizerState = rasterState;
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                gDevice.BlendState = BlendState.Opaque;
                gDevice.SamplerStates[0] = SamplerState.LinearClamp;
                if (GraphicsSystem.gfxIndexSizeOpaque > (ushort)0)
                    effect.GraphicsDevice.DrawUserIndexedPrimitives<DrawVertex>(PrimitiveType.TriangleList, GraphicsSystem.gfxPolyList, 0, (int)GraphicsSystem.gfxVertexSizeOpaque, GraphicsSystem.gfxPolyListIndex, 0, (int)GraphicsSystem.gfxIndexSizeOpaque);
                gDevice.BlendState = BlendState.NonPremultiplied;
                int primitiveCount = (int)GraphicsSystem.gfxIndexSize - (int)GraphicsSystem.gfxIndexSizeOpaque;
                if (primitiveCount > 0)
                    effect.GraphicsDevice.DrawUserIndexedPrimitives<DrawVertex>(PrimitiveType.TriangleList, GraphicsSystem.gfxPolyList, (int)GraphicsSystem.gfxVertexSizeOpaque, (int)GraphicsSystem.gfxVertexSize - (int)GraphicsSystem.gfxVertexSizeOpaque, GraphicsSystem.gfxPolyListIndex, 0, primitiveCount);
            }
            effect.Texture = (Texture2D)null;
        }
    }
}
