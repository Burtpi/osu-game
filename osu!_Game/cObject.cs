using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace osu__Game
{
    public abstract class cObject : IDisposable
    {
        private bool mIsCreated;
        protected int mVbo;
        public float mX { get; protected set; }
        public float mY { get; protected set; }
        public double mTime { get; protected set; }
        public bool mVisible { get; set; }
        public bool mHover { get; set; }
        public float mSize { get; private set; }
        public double mTimeSpan { get; private set; }
        protected int mVertCount { get; private set; }
        private static int mCount = 0;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void mSetSize(float aCircle_size)
        {
            var v = 70 - 5 * aCircle_size;
            mSize = 2.5f * v;
        }

        public void mSetTimeSpan(double aApproach_rate)
        {
            if (aApproach_rate <= 4 && aApproach_rate >= 0)
            {
                var v = 1800 - 120 * aApproach_rate;
                mTimeSpan = v;
            }
            else if (aApproach_rate > 4 && aApproach_rate <= 11)
            {
                var v = 1200 - 150 * (aApproach_rate - 5);
                mTimeSpan = v;
            }
        }
        public static int operator -(cObject aObject)
        {
            
            aObject.Dispose();
            mCount++;
            return mCount;
        }
        public void mIsVisible(double aTimer)
        {
            mVisible = aTimer >= mTime - mTimeSpan && aTimer <= mTime;
        }

        public void mCreate(Vector2[] aVertices)
        {
            mVertCount = aVertices.Length;

            mVbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, mVbo);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr) (Vector2.SizeInBytes * mVertCount), aVertices,
                BufferUsageHint.StreamDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindTexture(TextureTarget.Texture2D, 0);
            mIsCreated = true;
        }

        public virtual void mDrawObject()
        {
            mDraw();
        }

        public virtual void mDraw()
        {
            GL.Enable(EnableCap.Blend);
            GL.Enable(EnableCap.Texture2D);
            GL.BlendFunc((BlendingFactor) BlendingFactorSrc.SrcAlpha,
                (BlendingFactor) BlendingFactorDest.OneMinusSrcAlpha);
            GL.BindTexture(TextureTarget.Texture2D, 0);
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

        private void Dispose(bool aShouldDispose)
        {
            if (!aShouldDispose) return;
            if (!mIsCreated) return;
            GL.DeleteBuffer(mVbo);
            mIsCreated = false;
        }
    }
}