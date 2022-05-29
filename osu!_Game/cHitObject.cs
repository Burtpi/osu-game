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

        ~cHitObject()
        {
        }

        public void mSetSizeHb(float aCircle_size)
        {
            mCircle.mSetSize(aCircle_size);
            mApproachCircle.mSetSize(aCircle_size);
            mSetSize(aCircle_size);
        }

        public void mSetTimeSpanHb(double aApproach_rate)
        {
            mCircle.mSetTimeSpan(aApproach_rate);
            mApproachCircle.mSetTimeSpan(aApproach_rate);
            mSetTimeSpan(aApproach_rate);
        }

        public void mCreateObject(double aTime)
        {
            mCircle.mCreate(mCircle.mBufferC());
            mApproachCircle.mCreate(mApproachCircle.mBufferAc(aTime));
        }

        public override void mDrawObject()
        {
            mCircle.mDraw();
            mApproachCircle.mDraw();
        }
    }
}