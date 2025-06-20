using OneSummonArmy.Content.Buffs;
using Terraria.ModLoader;

namespace OneSummonArmy.Content.Projectiles.Hornets
{
    public class HornetCounter : Counter
    {
        public HornetCounter()
        {
            buffType = ModContent.BuffType<HornetBuff>();
        }
    }
}
