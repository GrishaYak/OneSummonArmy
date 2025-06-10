using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace OneSummonArmy.Content.Projectiles.Hornets
{
    public class TripleHornet : Hornet
    {
        short shotCnt = 0;
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
                shootTimer = 25;
                if (Main.player[Projectile.owner].strongBees)
                {
                    shootTimer = 20;
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
    }
}
