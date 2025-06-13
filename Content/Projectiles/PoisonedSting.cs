using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;

namespace OneSummonArmy.Content.Projectiles
{
    public class PoisonedSting : ModProjectile
    {
        readonly int myType = ModContent.ProjectileType<PoisonedSting>();
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.aiStyle = 1;
            Projectile.tileCollide = true;
            Projectile.scale *= 0.9f;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Main.rand.NextBool(4))
            {
                target.AddBuff(20, 60 * 5);
            }
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (Main.rand.NextBool(4))
            {
                target.AddBuff(20, 60 * 5);
            }
        }
        public override bool PreAI()
        {
            Projectile.type = 374;
            return base.PreAI();
        }
        public override void PostAI()
        {
            Projectile.type = myType;
            base.PostAI();
        }
    }
}
