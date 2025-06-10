using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace OneSummonArmy.Content.Projectiles.Hornets
{
    public class DoubleHornet : Hornet
    {
        bool firstShot = true;
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
                shootTimer = 30;
                if (Main.player[Projectile.owner].strongBees)
                {
                    shootTimer = 23;
                }
            }
            if (Projectile.ai[1] > shootTimer)
            {
                Projectile.ai[1] = 0f;
                Projectile.netUpdate = true;
                firstShot = !firstShot;
            }
        }
    }
}
