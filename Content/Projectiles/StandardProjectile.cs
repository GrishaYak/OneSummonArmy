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
        protected string sunsTexture = "OneSummonArmy/Assets/Textures/Projectiles/";
        public override string Texture => AddToPath("Counter");
        protected string AddToPath(string s) => string.Concat(sunsTexture, s);
        protected string AddToPath(int n) => AddToPath($"{n}");
        protected string AddDirToPath(string s) => AddToPath($"{s}/");
    }
}
