namespace osu__Game;

public class cSlider : cObject
{
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

    private float mXEnd { get; }
    private float mYEnd { get; }
    private double mTimeEnd { get; }
}