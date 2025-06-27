using OneSummonArmy.Content.Buffs;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace OneSummonArmy.Content.Projectiles.Birds
{
    public class BirdCounter() : Counter()
    {
        protected override bool CheckActive(Player player, int buffType = -1)
        {
            return base.CheckActive(player, ModContent.BuffType<BirdBuff>());
        }

    }
}
