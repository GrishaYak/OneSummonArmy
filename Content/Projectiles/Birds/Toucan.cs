using Microsoft.Xna.Framework;
using Terraria;

namespace OneSummonArmy.Content.Projectiles.Birds
{
    public class Toucan : Bird
    {
        public override string Texture => AddToPath(4);
        public Toucan() : base(idleFrame: 0, movingFrameStart: 1, movingFrameEnd: 6, basicSpeed: 11) { }

        protected override void AdditionalStaticDefaults()
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
            var home =  base.GetHomeLocation();
            home.Y -= 11;
            return home;
        }

    }
}
