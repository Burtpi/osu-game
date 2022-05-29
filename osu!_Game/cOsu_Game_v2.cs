using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Media;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace osu__Game
{
    internal class cOsuGame
    {
        private readonly List<cHitObject> mHitObjects = new List<cHitObject>();
        private readonly List<cHit> mHits = new List<cHit>();
        private readonly GameWindow mOsu_Window;
        private readonly List<cText> mText = new List<cText>();
        private int mCombo;
        private int mHit;
        private cCircle mLastObj;
        private SoundPlayer mPlayer;
        private int mScoreFinal;
        private double mTime;

        public cOsuGame(GameWindow aWindow)
        {
            mOsu_Window = aWindow;
            mOsu_Window.Load += MOsu_Window_Load;
            mOsu_Window.RenderFrame += MOsu_Window_RenderFrame;
            mOsu_Window.MouseDown += mIfMouseDown;
        }

        private void mIfMouseDown(object sender, MouseButtonEventArgs e)
        {
            foreach (var obj in mHitObjects)
            {
                obj.mIsVisible(mTime);
                if (!obj.mVisible) continue;
                if (e.X >= obj.mX - obj.mSize / 2 && e.X <= obj.mX + obj.mSize / 2 && e.Y >= obj.mY - obj.mSize / 2 &&
                    e.Y <= obj.mY + obj.mSize / 2)
                    obj.mHover = true;
            }
        }

        private void MOsu_Window_Load(object sender, EventArgs e)
        {
            mOsu_Window.CursorVisible = true;
            var currentDirectory = System.IO.Directory.GetCurrentDirectory(); 
            mPlayer = new SoundPlayer {SoundLocation = "audio.wav"};
            mPlayer.Play();
            cTextureLoad.mLoad("hitcirclewith.png");
            cTextureLoad.mLoad("approachcircle.png");
            cTextureLoad.mLoad("300.png");
            cTextureLoad.mLoad("100.png");
            cTextureLoad.mLoad("50.png");
            cTextureLoad.mLoad("miss.png");
            cTextureLoad.mLoad("text.png");
            var lines = File.ReadAllLines("map.txt");
            var isHitObjects = false;
            foreach (var obj in lines)
            {
                if (obj == "[HitObjects]")
                {
                    isHitObjects = true;
                    continue;
                }

                if (!isHitObjects) continue;
                var a = obj.Split(',');
                mHitObjects.Add(new cHitObject(Convert.ToInt32(a[0]), Convert.ToInt32(a[1]), Convert.ToInt32(a[2])));
                mLastObj = new cCircle(Convert.ToInt32(a[0]), Convert.ToInt32(a[1]), Convert.ToInt32(a[2]));
            }

            mHitObjects.Add(new cHitObject(5000, 5000, (int) mLastObj.mTime + 5000));
            foreach (var obj in mHitObjects)
            {
                obj.mSetSizeHb(4);
                obj.mSetTimeSpanHb(8);
            }
        }

        private void MOsu_Window_RenderFrame(object sender, FrameEventArgs e)
        {
            var renderObject = new HashSet<cObject>();
            var removeObject = new HashSet<cObject>();
            mTime += e.Time * 1000;
            GL.ClearColor(Color.Black);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            var pixelMap = Matrix4.CreateOrthographicOffCenter(0, 1600, 900, 0, 0, 1);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref pixelMap);
            var prevHb = new cHitObject {mVisible = false};
            foreach (var obj in mHitObjects)
            {
                obj.mIsVisible(mTime);
                prevHb.mIsVisible(mTime);
                if (obj.mVisible)
                {
                    obj.mCreateObject(e.Time);
                    renderObject.Add(obj);
                }

                if (prevHb.mVisible && prevHb.mHover)
                {
                    mCombo += 1;
                    mHit = cHit.mRhythmHit(mTime, prevHb);
                    mHits.Add(new cHit(prevHb.mX, prevHb.mY, mTime, mHit));
                    var scorePre = new cScoreCalculation(mCombo, mHit);
                    mScoreFinal = scorePre + mScoreFinal;
                    obj.mHover = false;
                    var i = -prevHb;
                    removeObject.Add(prevHb);
                    if (prevHb.mVisible && obj.mHover && obj.mVisible) continue;
                    Debug.WriteLine($"Hit is now:{mHit}, Combo is now:{mCombo}, Score is now:{mScoreFinal}");
                }
                else if (mTime >= obj.mTime)
                {
                    mHits.Add(new cHit(obj.mX, obj.mY, mTime, 0));
                    var i = -obj;
                    removeObject.Add(obj);
                    Debug.WriteLine("Miss");
                    mCombo = 0;
                }

                prevHb = obj;
            }

            foreach (var obj in mHits)
                if (mTime < obj.mTime + 500)
                {
                    obj.mSetSize(4);
                    obj.mCreate(obj.mBufferC());
                    renderObject.Add(obj);
                }
                else if (mTime >= obj.mTime + 500)
                {
                    var i = -obj;
                    removeObject.Add(obj);
                }

            var score = mScoreFinal.ToString();
            var combo = mCombo.ToString();
            for (var i = 0; i < score.Length; i++)
                switch (score[i] - '0')
                {
                    case 0:
                        mText.Add(new cText(50 + 25 * (i + 1), 50, 0));
                        break;
                    case 1:
                        mText.Add(new cText(50 + 25 * (i + 1), 50, 1));
                        break;
                    case 2:
                        mText.Add(new cText(50 + 25 * (i + 1), 50, 2));
                        break;
                    case 3:
                        mText.Add(new cText(50 + 25 * (i + 1), 50, 3));
                        break;
                    case 4:
                        mText.Add(new cText(50 + 25 * (i + 1), 50, 4));
                        break;
                    case 5:
                        mText.Add(new cText(50 + 25 * (i + 1), 50, 5));
                        break;
                    case 6:
                        mText.Add(new cText(50 + 25 * (i + 1), 50, 6));
                        break;
                    case 7:
                        mText.Add(new cText(50 + 25 * (i + 1), 50, 7));
                        break;
                    case 8:
                        mText.Add(new cText(50 + 25 * (i + 1), 50, 8));
                        break;
                    case 9:
                        mText.Add(new cText(50 + 25 * (i + 1), 50, 9));
                        break;
                }

            for (var i = 0; i < combo.Length; i++)
                switch (combo[i] - '0')
                {
                    case 0:
                        mText.Add(new cText(25 + 25 * (i + 1), 800, 0));
                        break;
                    case 1:
                        mText.Add(new cText(25 + 25 * (i + 1), 800, 1));
                        break;
                    case 2:
                        mText.Add(new cText(25 + 25 * (i + 1), 800, 2));
                        break;
                    case 3:
                        mText.Add(new cText(25 + 25 * (i + 1), 800, 3));
                        break;
                    case 4:
                        mText.Add(new cText(25 + 25 * (i + 1), 800, 4));
                        break;
                    case 5:
                        mText.Add(new cText(25 + 25 * (i + 1), 800, 5));
                        break;
                    case 6:
                        mText.Add(new cText(25 + 25 * (i + 1), 800, 6));
                        break;
                    case 7:
                        mText.Add(new cText(25 + 25 * (i + 1), 800, 7));
                        break;
                    case 8:
                        mText.Add(new cText(25 + 25 * (i + 1), 800, 8));
                        break;
                    case 9:
                        mText.Add(new cText(25 + 25 * (i + 1), 800, 9));
                        break;
                }

            foreach (var obj in mText)
            {
                switch (obj.mValue)
                {
                    case 0:
                        obj.mCreate(obj.mBufferText(25, 0));
                        break;
                    case 1:
                        obj.mCreate(obj.mBufferText(25, 1));
                        break;
                    case 2:
                        obj.mCreate(obj.mBufferText(25, 2));
                        break;
                    case 3:
                        obj.mCreate(obj.mBufferText(25, 3));
                        break;
                    case 4:
                        obj.mCreate(obj.mBufferText(25, 4));
                        break;
                    case 5:
                        obj.mCreate(obj.mBufferText(25, 5));
                        break;
                    case 6:
                        obj.mCreate(obj.mBufferText(25, 6));
                        break;
                    case 7:
                        obj.mCreate(obj.mBufferText(25, 7));
                        break;
                    case 8:
                        obj.mCreate(obj.mBufferText(25, 8));
                        break;
                    case 9:
                        obj.mCreate(obj.mBufferText(25, 9));
                        break;
                }

                renderObject.Add(obj);
            }

            foreach (var obj in renderObject) obj.mDrawObject();
            foreach (var obj in removeObject)
            {
                obj.Dispose();
                switch (obj)
                {
                    case cHitObject hitObject:
                    {
                        mHitObjects.Remove(hitObject);
                        break;
                    }
                    case cHit hit:
                    {
                        mHits.Remove(hit);
                        break;
                    }
                }
            }

            foreach (var obj in mText)
            {
                var i = -obj;
            }

            mText.Clear();
            mOsu_Window.SwapBuffers();
            var b1 = new cBeatmap((int) mLastObj.mTime + 2500);
            if (mTime >= b1.mLength) mOsu_Window.Exit();
        }
    }
}