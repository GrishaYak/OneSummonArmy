using OneSummonArmy.Content.Projectiles.Slimes;
using Terraria.ModLoader;

namespace OneSummonArmy.Content.Buffs
{
    public class SlimeBuff() : StandardBuff(ModContent.ProjectileType<SlimeCounter>())
    {
        public override int GetProjectileType()
        {
            return ModContent.ProjectileType<SlimeCounter>();
        }
    }
}
