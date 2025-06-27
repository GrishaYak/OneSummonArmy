using OneSummonArmy.Content.Buffs;
using Terraria;
using Terraria.ModLoader;

namespace OneSummonArmy.Content.Projectiles.Hornets
{
    public class HornetCounter() : Counter()
    {
        protected override bool CheckActive(Player player, int buffType = -1)
        {
            return base.CheckActive(player, ModContent.BuffType<HornetBuff>());
        }
    }
}
