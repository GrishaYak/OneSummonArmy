using OneSummonArmy.Content.Buffs;
using Terraria.ModLoader;

namespace OneSummonArmy.Content.Projectiles.Slimes
{
    public class SlimeCounter: Counter
    {
        public SlimeCounter() : base(ModContent.BuffType<SlimeBuff>()) { }
    }
}
