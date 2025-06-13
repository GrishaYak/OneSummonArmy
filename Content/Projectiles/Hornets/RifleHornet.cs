using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace OneSummonArmy.Content.Projectiles.Hornets
{
    public class RifleHornet : Hornet
    {
        protected override void UpdateShootTimer()
        {
            Projectile.ai[1] += Main.rand.Next(1, 4);
            int shootTimer;
            shootTimer = 25;
            if (Main.player[Projectile.owner].strongBees)
            {
                shootTimer = 20;
            }
            if (Projectile.ai[1] > shootTimer)
            {
                Projectile.ai[1] = 0f;
                Projectile.netUpdate = true;
            }
        }
        protected override void Shoot(float newProjSpeed, int enemyID, int type = 374, Vector2? direction = null)
        {
            base.Shoot(newProjSpeed * 1.5f, enemyID, type, direction);
        }
    }
}
