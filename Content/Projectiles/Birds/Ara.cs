using Microsoft.Xna.Framework;
using Terraria;

namespace OneSummonArmy.Content.Projectiles.Birds
{
    public class Ara : Bird
    {
        public Ara() : base(idleFrame: 0, movingFrameStart: 1, movingFrameEnd: 6, basicSpeed: 12) { }


        protected override void AdditionalStaticDefaults ()
        {
            Main.projFrames[Projectile.type] = 6;
        }
        protected override void AdditionalDefaults()
        {
            Projectile.width = 46;
            Projectile.height = 40;
        }
        protected override Vector2 GetHomeLocation()
        {
            var player = Main.player[Projectile.owner];
            var home =  base.GetHomeLocation();
            home.Y += 18;
            home.X -= player.direction * 10;
            return home;
        }
    }
}
