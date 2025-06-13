
using Microsoft.Xna.Framework;

namespace OneSummonArmy.Content.Projectiles.Birds
{
    public class GoldenBird : Bird
    {
        public override string Texture => GetPathTo(3);
        public GoldenBird() : base(basicSpeed: 10) { }
        protected override Vector2 GetHomeLocation()
        {
            var home = base.GetHomeLocation();
            return home + new Vector2(0, -14);
        }
    }
}
