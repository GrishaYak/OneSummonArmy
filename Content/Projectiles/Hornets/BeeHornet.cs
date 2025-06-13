using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace OneSummonArmy.Content.Projectiles.Hornets
{
    public class BeeHornet : Hornet
    {
        public override string Texture => AddToPath(4);
        protected override void UpdateShootTimer()
        {
            ReloadCounter += Main.rand.Next(1, 4);
            int shootTimer =  Main.player[Projectile.owner].strongBees ? 70 : 90;

            if (ReloadCounter > shootTimer)
            {
                ReadyToShoot = true;
                Projectile.netUpdate = true;
            }
        }
        protected override void Shoot(float newProjSpeed, int enemyID, int type = 374, Vector2? direction=null)
        { 
            int level = Main.player[Projectile.owner].ownedProjectileCounts[ModContent.ProjectileType<HornetCounter>()];
            for (int i = 0; i < Main.rand.Next(0, level); ++i)
            {
                type = 181;
                if (Main.player[Projectile.owner].strongBees && Main.rand.NextBool(4))
                {
                    type = 566;
                    ScaleDamage(1.5);
                }
                base.Shoot(newProjSpeed, enemyID, type, direction);
                SetDamageToDefault();
            }

        }

    }
}
