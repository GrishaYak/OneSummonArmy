using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using OneSummonArmy.Content.Items;
using OneSummonArmy.Content.Projectiles;

namespace OneSummonArmy.Content.Buffs
{
    public abstract class StandardBuff : ModBuff
    {
        public virtual int GetProjectileType() { return ModContent.ProjectileType<StandardMinion>(); }
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = false; // This buff will save when you exit the world
            Main.buffNoTimeDisplay[Type] = true; // The time remaining won't display on this buff
        }

        public override void Update(Player player, ref int buffIndex)
        {
            // If the minions exist reset the buff time, otherwise remove the buff from the player
            if (player.ownedProjectileCounts[GetProjectileType()] > 0)
            {
                player.buffTime[buffIndex] = 18000;
            }
            else
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
        }
    }
}
