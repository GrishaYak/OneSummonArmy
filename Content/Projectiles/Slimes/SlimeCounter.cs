using OneSummonArmy.Content.Buffs;
using Terraria;
using Terraria.ModLoader;
using OneSummonArmy.Content.Projectiles;

namespace OneSummonArmy.Content.Projectiles.Slimes
{
    public class SlimeCounter: Counter
    {
        protected override void SetAdditionalDefaults()
        {
            base.buffType = ModContent.BuffType<SlimeBuff>();
        }
    }
}
