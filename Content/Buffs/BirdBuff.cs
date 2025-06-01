using OneSummonArmy.ID;

namespace OneSummonArmy.Content.Buffs
{
    public class BirdBuff : StandardBuff
    {
        public override int[] GetProjectileIds()
        {
            return GetIds.GetBirdsIds();
        }
    }
}
