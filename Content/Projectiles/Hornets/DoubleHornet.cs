using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace OneSummonArmy.Content.Projectiles.Hornets
{
    public class DoubleHornet : Hornet
    {
        bool firstShot = true;
        Vector2 direction;
        protected override void UpdateShootTimer()
        {
            Projectile.ai[1] += Main.rand.Next(1, 4);
            int shootTimer;
            if (firstShot)
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
                firstShot = !firstShot;
            }
        }
        protected override void Shoot(float newProjSpeed, int enemyID, int type = 374, Vector2? direction=null)
        {
            if (firstShot)
            {
                var enemy = Main.npc[enemyID];
                this.direction = enemy.Center - Projectile.Center;
                this.direction = this.direction.SafeNormalize(Vector2.Zero);
                base.Shoot(newProjSpeed, enemyID, type, this.direction);
                return;
            }
            base.Shoot(newProjSpeed, enemyID, type, this.direction);
        }

    }
}
