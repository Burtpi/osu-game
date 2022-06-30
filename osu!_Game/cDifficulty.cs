namespace osu__Game
{
    public abstract class cDifficulty
    {
        public void SetSize(float aCircleSize)
        {
            var v = 70 - 5 * aCircleSize;
        }

        public void SetTimeSpan(double aApproachRate)
        {
            if (aApproachRate <= 4 && aApproachRate >= 0)
            {
                var v = 1800 - 120 * aApproachRate;
            }
            else if (aApproachRate > 4 && aApproachRate <= 11)
            {
                var v = 1200 - 150 * (aApproachRate - 5);
            }
        }
    }
}