using OneSummonArmy.Content.Buffs;
using Terraria.ModLoader;

namespace OneSummonArmy.Content.Projectiles.Birds
{
    public class BirdCounter : Counter
    {
        public BirdCounter() : base(ModContent.BuffType<BirdBuff>()) { }
    }
}
