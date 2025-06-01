using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace OneSummonArmy.Content.Extras
{
    public class Nest : ModProjectile
    {
        public sealed override void SetDefaults()
        {
            Projectile.tileCollide = false;
            Projectile.velocity = Vector2.Zero;

        }
        public override void AI()
        {

            Player owner = Main.player[Projectile.owner];
            Projectile.position = owner.position;
            Projectile.velocity = owner.velocity;
        }
    }
}
