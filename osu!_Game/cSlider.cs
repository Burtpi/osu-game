using System;
using System.Collections.Generic;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace osu__Game;

public class cSlider : cObject
{
    private cSlider(float aX, float aY, double aTime)
    {
        mX = aX;
        mY = aY;
        mTime = aTime;
        mHover = false;
    }
    public cSlider(float aX, float aY, double aTime, float aXEnd, float aYEnd)
    {
        mDiff = Math.Sqrt(Math.Pow(aXEnd - aX, 2) + Math.Pow(aYEnd - aY, 2))/10;
        mCount = mDiff/10;
        for (var i = 0; i < mCount; i++)
        {
            mSlider.Add(new cSlider(aX + 10 * i, aY + 10 * i, aTime));
        }
    }

    private double mDiff{ get; }
    private double mCount { get; }
    private readonly List<cSlider> mSlider = new();

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
    public virtual void Draw()
    {
        GL.Enable(EnableCap.Blend);
        GL.Enable(EnableCap.Texture2D);
        GL.BlendFunc((BlendingFactor)BlendingFactorSrc.SrcAlpha,
            (BlendingFactor)BlendingFactorDest.OneMinusSrcAlpha);
        GL.BindTexture(TextureTarget.Texture2D, 8);
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