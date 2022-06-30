
namespace osu__Game
{
    public class cHitObject : cObject
    {
        private readonly cApproachCircle mApproachCircle;
        private readonly cCircle mCircle;

        public cHitObject(float aX, float aY, double aTime)
        {
            mCircle = new cCircle((aX + 192) * 1.7857f, (aY + 96) * 1.7578125f, aTime + 1500);
            mApproachCircle = new cApproachCircle((aX + 192) * 1.7857f, (aY + 96) * 1.7578125f, aTime + 1500);
            mX = (aX + 192) * 1.7857f;
            mY = (aY + 96) * 1.7578125f;
            mTime = aTime + 1500;
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