using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace osu__Game;

public class cHit : cObject
{
    private const int Hit100 = 100;
    private const int Hit300 = 300;
    private const int Hit50 = 50;
    private readonly int mHitValue;

    public cHit(float aX, float aY, double aTime, int aHitValue)
    {
        mX = aX;
        mY = aY;
        mTime = aTime;
        mHitValue = aHitValue;
    }

    public static int RhythmHit(double aTime, cObject aCircle)
    {
        // if (aTime >= aCircle.mTime - 100 && aTime <= aCircle.mTime)
        //     return Hit300;
        // if (aTime < aCircle.mTime - 100 && aTime >= aCircle.mTime - 200)
        //     return Hit100;
        // if (aTime < aCircle.mTime - 200 && aTime >= aCircle.mTime - aCircle.mTimeSpan)
        //     return Hit50;
        if (aTime >= aCircle.mTime - (80 - 6 * aCircle.mOverallDiff) &&
            aTime <= aCircle.mTime + (80 - 6 * aCircle.mOverallDiff))
            return Hit300;
        if (aTime >= aCircle.mTime - (140 - 8 * aCircle.mOverallDiff) &&
            aTime <= aCircle.mTime + (140 - 8 * aCircle.mOverallDiff))
            return Hit100;
        if (aTime >= aCircle.mTime - (200 - 10 * aCircle.mOverallDiff) &&
            aTime <= aCircle.mTime + (200 - 10 * aCircle.mOverallDiff))
            return Hit50;
        return 0;
    }

    public Vector2[] BufferC()
    {
        var circleVert = new[]
        {
            new Vector2(mX - mSize / 2, mY - mSize / 2), new Vector2(0, 0),
            new Vector2(mX + mSize / 2, mY - mSize / 2), new Vector2(1, 0),
            new Vector2(mX + mSize / 2, mY + mSize / 2), new Vector2(1, 1),
            new Vector2(mX - mSize / 2, mY + mSize / 2), new Vector2(0, 1)
        };
        return circleVert;
    }

    public override void Draw()
    {
        GL.Enable(EnableCap.Blend);
        GL.Enable(EnableCap.Texture2D);
        GL.BlendFunc((BlendingFactor)BlendingFactorSrc.SrcAlpha,
            (BlendingFactor)BlendingFactorDest.OneMinusSrcAlpha);
        switch (mHitValue)
        {
            case 300:
                GL.BindTexture(TextureTarget.Texture2D, 3);
                break;
            case 100:
                GL.BindTexture(TextureTarget.Texture2D, 4);
                break;
            case 50:
                GL.BindTexture(TextureTarget.Texture2D, 5);
                break;
            case 0:
                GL.BindTexture(TextureTarget.Texture2D, 6);
                break;
        }

        GL.EnableClientState(ArrayCap.VertexArray);
        GL.EnableClientState(ArrayCap.TextureCoordArray);
        GL.BindBuffer(BufferTarget.ArrayBuffer, Vbo);
        GL.VertexPointer(2, VertexPointerType.Float, Vector2.SizeInBytes * 2, 0);
        GL.TexCoordPointer(2, TexCoordPointerType.Float, Vector2.SizeInBytes * 2, Vector2.SizeInBytes);
        GL.Color3(Color.White);
        GL.DrawArrays(PrimitiveType.Quads, 0, mVertCount / 2);
        GL.BindTexture(TextureTarget.Texture2D, 0);
        GL.Disable(EnableCap.Texture2D);
    }
}