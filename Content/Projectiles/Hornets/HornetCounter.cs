using OneSummonArmy.Content.Buffs;
using Terraria.ModLoader;

namespace OneSummonArmy.Content.Projectiles.Hornets
{
    internal class HornetCounter : Counter
    {
        public HornetCounter() : base(ModContent.BuffType<HornetBuff>()) { }
    }
}
