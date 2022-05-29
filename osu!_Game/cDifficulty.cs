namespace osu__Game
{
    public abstract class cDifficulty
    {
        public void mSetSize(float aCircle_size)
        {
            var v = 70 - 5 * aCircle_size;
        }

        public void mSetTimeSpan(double aApproach_rate)
        {
            if (aApproach_rate <= 4 && aApproach_rate >= 0)
            {
                var v = 1800 - 120 * aApproach_rate;
            }
            else if (aApproach_rate > 4 && aApproach_rate <= 11)
            {
                var v = 1200 - 150 * (aApproach_rate - 5);
            }
        }
    }
}