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
        protected override void GetToAttackPosition(Vector2 enemyPos, float speed, float inertia, float dist)
        {
            base.GetToAttackPosition(enemyPos, speed, inertia, 100);
        }
        protected override void UpdateShootTimer()
        {
            ReloadCounter += Main.rand.Next(1, 4);

            int shootTimer = Main.player[Projectile.owner].strongBees ? 110 : 140;
            if (ReloadCounter > shootTimer)
            {
                ReadyToShoot = true;
                Projectile.netUpdate = true;
            }
        }
        protected override void Shoot(float newProjSpeed, int enemyID, int type = 374, Vector2? direction = null)
        {
            ReadyToShoot = false;
            ScaleDamage(0.6);
            NPC enemy = Main.npc[enemyID];
            direction ??= (enemy.Center - Projectile.Center).SafeNormalize(Vector2.Zero);
            Vector2 projPos = Projectile.position;
            if (Projectile.direction < 0 )
            {
                projPos.X += 3;
            }
            else
            {
                projPos.X += Projectile.width - 3;
            }
            projPos.Y += 32;
            Random rand = new();
            double spread = double.Pi / 12;
            newProjSpeed *= 1.5f;
            if (Main.player[Projectile.owner].strongBees)
            {
                spread *= 0.8;
            }
            for (int i = 0; i < 5; i++)
            {
                double radians = spread * (rand.NextDouble() * 2 - 1);
                DirectProj(((Vector2) direction).RotatedBy(radians) * newProjSpeed, position: projPos);
            }
        }
    }
}
