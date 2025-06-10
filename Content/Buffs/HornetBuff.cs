using OneSummonArmy.Content.Projectiles.Hornets;
using Terraria.ModLoader;

namespace OneSummonArmy.Content.Buffs
{
    public class HornetBuff : StandardBuff
    {
        public override int GetProjectileType()
        {
            return ModContent.ProjectileType<HornetCounter>();
        }
    }
}
