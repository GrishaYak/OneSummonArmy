using Terraria;
using Terraria.ModLoader;
using OneSummonArmy.AI;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace OneSummonArmy.Content.Projectiles
{
    public abstract class Counter(int buffType) : ModProjectile()
    {
        int serial;
        protected virtual void SetAdditionalDefaults() { }
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 6;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            Main.projPet[Projectile.type] = true;
        }
        public override void SetDefaults()
        {
            Projectile.timeLeft = 2;
            Projectile.hide = false;
            Projectile.penetrate = -1;
            Projectile.minion = true;
            Projectile.minionSlots = 1f;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.tileCollide = false;
            SetAdditionalDefaults();
        }
        public override void OnSpawn(IEntitySource source)
        {
            serial = Main.player[Projectile.owner].ownedProjectileCounts[Projectile.type];
        }
        public virtual bool CheckActive(Player owner)
        {
            if (owner.dead || !owner.active)
            {
                owner.ClearBuff(buffType);

                return false;
            }

            if (owner.HasBuff(buffType))
            {
                Projectile.timeLeft = 2;
            }

            return true;
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            CheckActive(player);
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
