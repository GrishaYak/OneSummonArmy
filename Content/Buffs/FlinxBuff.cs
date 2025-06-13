
using OneSummonArmy.Content.Projectiles.Flinxes;
using Terraria.ModLoader;

namespace OneSummonArmy.Content.Buffs
{
    public class FlinxBuff : StandardBuff
    {
        public override int GetProjectileType()
        {
            return ModContent.ProjectileType<FlinxCounter>();
        }
    }
}
