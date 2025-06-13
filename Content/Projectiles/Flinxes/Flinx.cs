using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneSummonArmy.Content.Projectiles.Flinxes
{
    public abstract class Flinx : StandardProjectile
    {
        public Flinx() {
            sunsTexture = AddDirToPath("Flinxes");
        }

    }
}
