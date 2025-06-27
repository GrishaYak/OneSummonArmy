using OneSummonArmy.Content.Buffs;
using Terraria;
using Terraria.ModLoader;

namespace OneSummonArmy.Content.Projectiles.Slimes
{
    public class SlimeCounter() : Counter()
    {
        protected override bool CheckActive(Player player, int buffType = -1)
        {
            return base.CheckActive(player, ModContent.BuffType<SlimeBuff>());
        }
    }
}
