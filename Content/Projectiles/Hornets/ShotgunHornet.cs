using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace OneSummonArmy.Content.Projectiles.Hornets
{
    public class ShotgunHornet : Hornet
    {

        protected override void Shoot(float speed, Vector2 enemyPos, int type = 374)
        {
            Projectile.ai[1] += 1f;
            Vector2 direction = enemyPos - Projectile.Center;
            direction = direction.SafeNormalize(Vector2.Zero);
            DirectProj(direction * speed);
            
        }
    }
}
