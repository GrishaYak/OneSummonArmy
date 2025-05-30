using OneSummonArmy.Content.Projectiles.Birds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace OneSummonArmy.Content.Buffs
{
    public class BirdBuff : StandardBuff
    {
        public override int GetProjectileType() { return ModContent.ProjectileType<Bird>(); }
    }
}
