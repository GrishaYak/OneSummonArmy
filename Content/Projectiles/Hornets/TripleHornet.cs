using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace OneSummonArmy.Content.Projectiles.Hornets
{
    public class TripleHornet : Hornet
    {
        short shotCnt = 0;
        Vector2 direction;
        protected override void UpdateShootTimer()
        {
            Projectile.ai[1] += Main.rand.Next(1, 4);
            int shootTimer;
            if (shotCnt == 0)
            {
                shootTimer = 90;
                if (Main.player[Projectile.owner].strongBees)
                {
                    shootTimer = 70;
                }
            }
            else
            {
                shootTimer = 15;
                if (Main.player[Projectile.owner].strongBees)
                {
                    shootTimer = 12;
                }
            }
            if (Projectile.ai[1] > shootTimer)
            {
                Projectile.ai[1] = 0f;
                Projectile.netUpdate = true;
                ++shotCnt;
                if (shotCnt >= 3)
                {
                    shotCnt = 0;
                }
            }
        }
        protected override void Shoot(float newProjSpeed, int enemyID, int type = 374, Vector2? direction = null)
        {
            if (shotCnt == 1)
            {
                var enemy = Main.npc[enemyID];
                this.direction = (enemy.Center - Projectile.Center).SafeNormalize(Vector2.Zero);
                base.Shoot(newProjSpeed, enemyID, type, this.direction);
                return;
            }
            base.Shoot(newProjSpeed, enemyID, type, this.direction);
        }
    }
}
