using Microsoft.Xna.Framework;

namespace OneSummonArmy.Content.Projectiles.Birds

{
    public class Finch : Bird
    {
        protected override Vector2 GetHomeLocation()
        {
            var home = base.GetHomeLocation();
            return home + new Vector2(0, -12);
        }
    }
}
