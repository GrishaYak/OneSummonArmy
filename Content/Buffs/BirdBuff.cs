using OneSummonArmy.ID;
using Terraria.ModLoader;

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
