using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace OneSummonArmy.Content.Projectiles.Birds
{
    public class BlueJay : Bird
    {
        protected override void AdditionalDefaults()
        {
            base.BasicSpeed = 9;
        }
    }
}
