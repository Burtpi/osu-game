
namespace osu__Game
{
    public class cHitObject : cObject
    {
        private readonly cApproachCircle mApproachCircle;
        private readonly cCircle mCircle;
        private readonly cSlider mSlider;

        public cHitObject(float aX, float aY, double aTime)
        {
            mCircle = new cCircle((aX + 192) * 1.7857f, (aY + 96) * 1.7578125f, aTime);
            mApproachCircle = new cApproachCircle((aX + 192) * 1.7857f, (aY + 96) * 1.7578125f, aTime);
            mX = (aX + 192) * 1.7857f;
            mY = (aY + 96) * 1.7578125f;
            mTime = aTime;
        }
        public cHitObject(float aX, float aY, double aTime, float aXEnd, float aYEnd, double aTimeEnd)
        {
            mSlider= new cSlider((aX + 192) * 1.7857f, (aY + 96) * 1.7578125f, aTime, (aXEnd + 192) * 1.7857f, (aYEnd + 96) * 1.7578125f, aTimeEnd);
            mApproachCircle = new cApproachCircle((aX + 192) * 1.7857f, (aY + 96) * 1.7578125f, aTime);
            mX = (aX + 192) * 1.7857f;
            mY = (aY + 96) * 1.7578125f;
            mTime = aTime;
        }

        public cHitObject()
        {
        }

        public void SetSizeHb(float aCircleSize)
        {
            mCircle.SetSize(aCircleSize);
            mApproachCircle.SetSize(aCircleSize);
            SetSize(aCircleSize);
        }

        public void SetTimeSpanHb(double aApproachRate)
        {
            mCircle.SetTimeSpan(aApproachRate);
            mApproachCircle.SetTimeSpan(aApproachRate);
            SetTimeSpan(aApproachRate);
        }

        public void CreateObject(double aTime)
        {
            mCircle.Create(mCircle.BufferC());
            mApproachCircle.Create(mApproachCircle.BufferAc(aTime));
        }
        

        public override void DrawObject()
        {
            mCircle.Draw();
            mApproachCircle.Draw();
        }
    }
}