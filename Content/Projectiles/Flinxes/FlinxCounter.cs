using OneSummonArmy.Content.Buffs;
using Terraria.ModLoader;

namespace OneSummonArmy.Content.Projectiles.Flinxes
{
    public class FlinxCounter : Counter
    {
        public FlinxCounter()
        {
            buffType = ModContent.BuffType<FlinxBuff>();
        }
    }
}
