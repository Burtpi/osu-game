using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace osu__Game
{
    public class cText : cObject
    {
        public readonly int mValue;

        public cText(float aX, float aY, int aValue)
        {
            mX = aX;
            mY = aY;
            mValue = aValue;
        }

        ~cText()
        {
        }

        public Vector2[] mBufferText(int aWidth, int i)
        {
            var offset1 = (float) (0.1 * i);
            var offset2 = (float) (0.1 * (i + 1));
            var textVert = new[]
            {
                new Vector2(mX, mY), new Vector2(offset1, 0),
                new Vector2(mX + aWidth, mY), new Vector2(offset2, 0),
                new Vector2(mX + aWidth, mY + 50), new Vector2(offset2, 1),
                new Vector2(mX, mY + 50), new Vector2(offset1, 1)
            };
            return textVert;
        }
        
        public override void mDraw()
        {
            GL.Enable(EnableCap.Blend);
            GL.Enable(EnableCap.Texture2D);
            GL.BlendFunc((BlendingFactor) BlendingFactorSrc.SrcAlpha,
                (BlendingFactor) BlendingFactorDest.OneMinusSrcAlpha);
            GL.BindTexture(TextureTarget.Texture2D, 7);
            GL.EnableClientState(ArrayCap.VertexArray);
            GL.EnableClientState(ArrayCap.TextureCoordArray);
            GL.BindBuffer(BufferTarget.ArrayBuffer, mVbo);
            GL.VertexPointer(2, VertexPointerType.Float, Vector2.SizeInBytes * 2, 0);
            GL.TexCoordPointer(2, TexCoordPointerType.Float, Vector2.SizeInBytes * 2, Vector2.SizeInBytes);
            GL.Color3(Color.White);
            GL.DrawArrays(PrimitiveType.Quads, 0, mVertCount / 2);
            GL.BindTexture(TextureTarget.Texture2D, 0);
            GL.Disable(EnableCap.Texture2D);
        }
    }
}