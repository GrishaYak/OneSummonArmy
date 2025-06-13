using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace OneSummonArmy.Content.Projectiles.Birds

{
    public class Finch : Bird
    {
        public override string Texture => GetPathTo(1);
        protected override Vector2 GetHomeLocation()
        {
            var home = base.GetHomeLocation();
            return home + new Vector2(0, -12);
        }
    }
}
