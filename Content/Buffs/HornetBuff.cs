using OneSummonArmy.Content.Projectiles.Hornets;
using Terraria.ModLoader;

namespace OneSummonArmy.Content.Buffs
{
    public class HornetBuff() : StandardBuff(ModContent.ProjectileType<HornetCounter>())
    {
        public override int GetProjectileType()
        {
            return ModContent.ProjectileType<HornetCounter>();
        }
    }
}
