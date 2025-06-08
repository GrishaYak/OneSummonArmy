using Microsoft.Xna.Framework;

namespace OneSummonArmy.Content.Projectiles.Birds
{
    public class BlueJay : Bird
    {
        protected override void AdditionalDefaults()
        {
            base.BasicSpeed = 9;
        }
        protected override Vector2 GetHomeLocation()
        {
            var home = base.GetHomeLocation();
            return home + new Vector2(0, -14);
        }
    }
}
