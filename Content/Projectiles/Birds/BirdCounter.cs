using OneSummonArmy.Content.Buffs;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace OneSummonArmy.Content.Projectiles.Birds
{
    public class BirdCounter : Counter
    {
        public BirdCounter()
        {
            buffType = ModContent.BuffType<BirdBuff>();
        }
        
    }
}
