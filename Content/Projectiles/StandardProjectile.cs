using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace OneSummonArmy.Content.Projectiles
{
    public abstract class StandardProjectile : ModProjectile
    {
        protected string sonsTexture = "OneSummonArmy/Assets/Textures/Projectiles/";
        public override string Texture => GetPathTo("Counter");
        protected string GetPathTo(string s) => string.Concat(sonsTexture, s);
        protected string GetPathTo(int n) => GetPathTo($"{n}");
        protected string AddDirToPath(string s) => GetPathTo($"{s}/");
    }
}
