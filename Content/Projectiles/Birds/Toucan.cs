using Microsoft.Xna.Framework;
using Terraria;

namespace OneSummonArmy.Content.Projectiles.Birds
{
    public class Toucan : Bird
    {
        protected override int GetIdleFrame() { return 0; }

        protected override void GetMovingFrames(out int l, out int r) { l = 1; r = 6; }

        protected override void AdditionalStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 6;
        }
        protected override void AdditionalDefaults()
        {
            Projectile.width = 46;
            Projectile.height = 40;
            base.BasicSpeed = 11;
        }
        protected override Vector2 GetHomeLocation()
        {
            var home =  base.GetHomeLocation();
            home.Y -= 11;
            return home;
        }

    }
}
