using Terraria;
using Terraria.ModLoader;
using OneSummonArmy.AI;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace OneSummonArmy.Content.Projectiles
{
    public abstract class Counter : ModProjectile
    {
        int serial;
        protected virtual void SetAdditionalDefaults() { }
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 6;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
        }
        public override void SetDefaults()
        {
            Player player = Main.player[Projectile.owner];
            serial = player.ownedProjectileCounts[Projectile.type];
            Projectile.hide = true;
            Projectile.penetrate = -1;
            Projectile.minion = true;
            Projectile.minionSlots = 1f;
            Projectile.DamageType = DamageClass.Summon;
            SetAdditionalDefaults();
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            int cnt = player.ownedProjectileCounts[Projectile.type];
            Vector2 home = AIs.CounterGetHome(player, serial, cnt);
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
