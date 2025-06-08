using OneSummonArmy.Content.Projectiles.Birds;
using Terraria.ModLoader;

namespace OneSummonArmy.Content.Buffs
{
    public class BirdBuff : StandardBuff
    {
        public BirdBuff() : base(ModContent.ProjectileType<BirdCounter>()) { }
    }
}
