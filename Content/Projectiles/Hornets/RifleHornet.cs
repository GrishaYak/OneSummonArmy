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
        public override string Texture => AddToPath(2);
        protected override void UpdateShootTimer()
        {
            ReloadCounter += Main.rand.Next(1, 4);
            int shootTimer = Main.player[Projectile.owner].strongBees ? 20 : 25;

            if (ReloadCounter > shootTimer)
            {
                ReadyToShoot = true;
                Projectile.netUpdate = true;
            }
        }
        protected override void Shoot(float newProjSpeed, int enemyID, int type = 374, Vector2? direction = null)
        {
            ScaleDamage(0.6);
            base.Shoot(newProjSpeed * 1.5f, enemyID, type, direction);
        }
    }
}
