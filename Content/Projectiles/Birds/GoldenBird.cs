
using Microsoft.Xna.Framework;

namespace OneSummonArmy.Content.Projectiles.Birds
{
    public class GoldenBird : Bird
    {
        protected override void AdditionalDefaults()
        {
            base.BasicSpeed = 10;
        }
        protected override Vector2 GetHomeLocation()
        {
            var home = base.GetHomeLocation();
            return home + new Vector2(0, -14);
        }
    }
}
