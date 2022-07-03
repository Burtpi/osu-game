namespace osu__Game;

public class cScoreCalculation
{
    public cScoreCalculation(int aCombo, int aHit)
    {
        mCombo = aCombo;
        mHit = aHit;
    }

    private int mCombo { get; }

    private int mHit { get; }

    public static int operator +(cScoreCalculation aComboHit, int aScore)
    {
        aScore += aComboHit.mHit + aComboHit.mHit / 10 * (aComboHit.mCombo - 1);
        return aScore;
    }
}