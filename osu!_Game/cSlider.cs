using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace osu__Game
{
    public class cSlider : cObject
    {
        private float mXEnd { get; set; }
        private float mYEnd { get; set; }
        private double mTimeEnd { get; set; }

        public cSlider(float aX, float aY, double aTime, float aXEnd, float aYEnd, double aTimeEnd)
        {
            mX = aX;
            mY = aY;
            mTime = aTime;
            mXEnd = aXEnd;
            mYEnd = aYEnd;
            mTimeEnd = aTimeEnd;
            mHover = false;
        }

    }
}