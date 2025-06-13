using OneSummonArmy.Content.Projectiles.Birds;
using Terraria.ModLoader;

namespace OneSummonArmy.Content.Buffs
{
    public class BirdBuff : StandardBuff
    {
        public override string Texture => GetPathTo("Bird");
        public override int GetProjectileType()
        {
            return ModContent.ProjectileType<BirdCounter>();
        }
    }
}
