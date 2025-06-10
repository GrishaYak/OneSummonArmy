using OneSummonArmy.Content.Projectiles;
using Terraria;
using Terraria.ModLoader;

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
            int projId = GetProjectileType();
            if (player.ownedProjectileCounts[projId] > 0)
            {
                player.buffTime[buffIndex] = 18000;
                return;
            }
            player.DelBuff(buffIndex);
        }
    }
}
