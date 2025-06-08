using OneSummonArmy.Content.Projectiles.Birds;
using Terraria.ModLoader;

namespace OneSummonArmy.Content.Buffs
{
    public class BirdBuff() : StandardBuff(ModContent.ProjectileType<BirdCounter>())
    {
        public override int GetProjectileType()
        {
            return ModContent.ProjectileType<BirdCounter>();
        }
    }
}
