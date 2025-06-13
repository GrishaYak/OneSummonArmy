using Microsoft.Xna.Framework;

namespace OneSummonArmy.Content.Projectiles.Birds
{
    public class BlueJay : Bird
    {
        public override string Texture => AddToPath(2);
        public BlueJay() : base(basicSpeed: 9) { }
        protected override Vector2 GetHomeLocation()
        {
            var home = base.GetHomeLocation();
            return home + new Vector2(0, -14);
        }
    }
}
