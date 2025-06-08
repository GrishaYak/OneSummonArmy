using OneSummonArmy.Content.Projectiles.Hornets;
using Terraria.ModLoader;

namespace OneSummonArmy.Content.Buffs
{
    public class HornetBuff : StandardBuff
    {
        HornetBuff() : base(ModContent.ProjectileType<HornetCounter>()) {  }
    }
}
