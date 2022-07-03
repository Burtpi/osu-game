using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using NAudio.Wave;

namespace osu__Game
{
    internal class cOsuGame
    {
        private readonly List<cHitObject> mHitObjects = new();
        private readonly List<cHit> mHits = new();
        private readonly GameWindow mOsuWindow;
        private readonly List<cText> mText = new();
        private int mCombo;
        private int mHit;
        private int mScoreFinal;
        private double mTime;
        private string mAudioPath;
        private cBeatmap mBeatmap = new();

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
            var mapNumber = 0;
            var intValue = false;
            cTextureLoad.Load("hitcirclewith.png");
            cTextureLoad.Load("approachcircle.png");
            cTextureLoad.Load("300.png");
            cTextureLoad.Load("100.png");
            cTextureLoad.Load("50.png");
            cTextureLoad.Load("miss.png");
            cTextureLoad.Load("text.png");
            var dirs = Directory.GetDirectories("map/", "*", SearchOption.TopDirectoryOnly);
            for (var i = 0; i < dirs.Length; i++)
                Console.WriteLine($"{i+1}. {dirs[i][(dirs[i].Split()[0].Length + 1)..]}");
            while (mapNumber > dirs.Length || mapNumber <= 0 || intValue == false)
                intValue = int.TryParse(Console.ReadLine(), out mapNumber);
            mAudioPath = mBeatmap.ReadFile(mHitObjects, mapNumber);
            var reader = new Mp3FileReader($"{dirs[mapNumber-1]}/{mAudioPath}");
            var waveOut = new WaveOutEvent();
            waveOut.Init(reader); 
            waveOut.Play();
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
                foreach (var obj in mHitObjects)
                    obj.mHover = false;
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
            if (mTime >= mBeatmap.Length+3000) mOsuWindow.Exit();
        }
    }
}