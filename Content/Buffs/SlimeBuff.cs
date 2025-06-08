using OneSummonArmy.Content.Projectiles.Slimes;
using Terraria.ModLoader;

namespace OneSummonArmy.Content.Buffs
{
    public class SlimeBuff : StandardBuff
    {
        public SlimeBuff() : base(ModContent.ProjectileType<SlimeCounter>()) { }
    }
}
