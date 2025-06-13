using OneSummonArmy.Content.Projectiles.Slimes;
using Terraria.ModLoader;

namespace OneSummonArmy.Content.Buffs
{
    public class SlimeBuff : StandardBuff
    {
        public override string Texture => GetPathTo("Slime");
        public override int GetProjectileType()
        {
            return ModContent.ProjectileType<SlimeCounter>();
        }
    }
}
