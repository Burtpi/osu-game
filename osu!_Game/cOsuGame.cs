using System;
using System.Collections.Generic;
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
        private readonly GameWindow mOsuWindow;
        private readonly List<cText> mText = new List<cText>();
        private int mCombo;
        private int mHit;
        private cCircle mLastObj;
        private SoundPlayer mPlayer;
        private int mScoreFinal;
        private double mTime;

        public cOsuGame(GameWindow aWindow)
        {
            mOsuWindow = aWindow;
            mOsuWindow.Load += Osu_Window_Load;
            mOsuWindow.RenderFrame += Osu_Window_RenderFrame;
            mOsuWindow.MouseDown += IfMouseDown;
        }

        private void IfMouseDown(object aSender, MouseButtonEventArgs aEvent)
        {
            foreach (var obj in mHitObjects)
            {
                obj.IsVisible(mTime);
                if (!obj.mVisible) continue;
                if (aEvent.X >= obj.mX - obj.mSize / 2 && aEvent.X <= obj.mX + obj.mSize / 2 && aEvent.Y >= obj.mY - obj.mSize / 2 &&
                    aEvent.Y <= obj.mY + obj.mSize / 2)
                    obj.mHover = true;
            }
        }

        private void Osu_Window_Load(object aSender, EventArgs aEvent)
        {
            mOsuWindow.CursorVisible = true;
            mPlayer = new SoundPlayer { SoundLocation = "map/audio.wav" };
            mPlayer.Play();
            cTextureLoad.Load("hitcirclewith.png");
            cTextureLoad.Load("approachcircle.png");
            cTextureLoad.Load("300.png");
            cTextureLoad.Load("100.png");
            cTextureLoad.Load("50.png");
            cTextureLoad.Load("miss.png");
            cTextureLoad.Load("text.png");
            var lines = File.ReadAllLines("map/map.txt");
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
            mHitObjects.Add(new cHitObject(0, 0, mLastObj.mTime+2000));
            mHitObjects.Reverse();
            foreach (var obj in mHitObjects)
            {
                obj.SetSizeHb(4);
                obj.SetTimeSpanHb(9);
            }
        }

        private void Osu_Window_RenderFrame(object aSender, FrameEventArgs aEvent)
        {
            var renderObject = new HashSet<cObject>();
            var removeObject = new HashSet<cObject>();
            mTime += aEvent.Time * 1000;
            GL.ClearColor(Color.Black);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            var pixelMap = Matrix4.CreateOrthographicOffCenter(0, 1600, 900, 0, 0, 1);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref pixelMap);
            foreach (var hitObject in mHitObjects)
            {
                hitObject.IsVisible(mTime);
                if (!hitObject.mVisible) continue;
                hitObject.CreateObject(aEvent.Time);
                renderObject.Add(hitObject);
            }
            
            var deleteObj = mHitObjects[mHitObjects.Count-1];
            if (deleteObj.mHover)
            {
                mCombo += 1;
                mHit = cHit.RhythmHit(mTime, deleteObj);
                mHits.Add(new cHit(deleteObj.mX, deleteObj.mY, mTime, mHit));
                var scorePre = new cScoreCalculation(mCombo, mHit);
                mHitObjects[mHitObjects.Count-2].mHover = false;
                mScoreFinal = scorePre + mScoreFinal;
                removeObject.Add(deleteObj);
                Console.WriteLine($"Hit is now:{mHit}, Combo is now:{mCombo}, Score is now:{mScoreFinal}");
            }
            else if (mTime >= deleteObj.mTime)
            {
                mHits.Add(new cHit(deleteObj.mX, deleteObj.mY, mTime, 0));
                removeObject.Add(deleteObj);
                Console.WriteLine("Miss");
                mCombo = 0;
            }

            foreach (var obj in mHits)
                if (mTime < obj.mTime + 500)
                {
                    obj.SetSize(4);
                    obj.Create(obj.BufferC());
                    renderObject.Add(obj);
                }
                else if (mTime >= obj.mTime + 500)
                {
                    removeObject.Add(obj);
                }

            var score = mScoreFinal.ToString();
            var combo = mCombo.ToString();
            for (var i = 0; i < score.Length; i++)
                mText.Add(new cText(50 + 25 * (i + 1), 50, score[i] - '0'));

            for (var i = 0; i < combo.Length; i++)
                mText.Add(new cText(25 + 25 * (i + 1), 800, combo[i] - '0'));

            foreach (var obj in mText)
            {
                obj.Create(obj.BufferText(25, obj.Value));
                renderObject.Add(obj);
            }

            foreach (var obj in renderObject) obj.DrawObject();
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

            mText.Clear();
            mOsuWindow.SwapBuffers();
            var b1 = new cBeatmap((int)mLastObj.mTime + 2500);
            if (mTime >= b1.Length) mOsuWindow.Exit();
        }
    }
}