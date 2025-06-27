using Terraria;
using Terraria.ModLoader;
using OneSummonArmy;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace OneSummonArmy.Content.Projectiles
{
    public abstract class Counter : StandardProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 6;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            Main.projPet[Projectile.type] = true;
        }
        public override void SetDefaults()
        {
            Projectile.timeLeft = 2;
            Projectile.hide = true;
            Projectile.penetrate = -1;
            Projectile.minion = true;
            Projectile.minionSlots = 1f;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.tileCollide = false;
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            CheckActive(player);
            int cnt = player.ownedProjectileCounts[Projectile.type];

            int serial = Main.player[Projectile.owner].ownedProjectileCounts[Projectile.type];
            Vector2 home = Func.CounterGetHome(player, serial, cnt);
            Projectile.Center = home;
            if (Projectile.frameCounter >= 5)
            {
                Projectile.frameCounter = 0;
                ++Projectile.frame;
                if (Projectile.frame >= Main.projFrames[Projectile.type])
                {
                    Projectile.frame = 0;
                }
            }
        }
    }
}
