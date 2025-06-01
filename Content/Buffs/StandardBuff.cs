using OneSummonArmy.Content.Projectiles;
using Terraria;
using Terraria.ModLoader;

namespace OneSummonArmy.Content.Buffs
{
    public abstract class StandardBuff : ModBuff
    {
        public virtual int GetProjectileType() { return ModContent.ProjectileType<StandardMinion>(); }
        public virtual int[] GetProjectileIds() 
        { 
            int[] a = [ModContent.ProjectileType<StandardMinion>()];
            return a;
        }
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = false; // This buff will save when you exit the world
            Main.buffNoTimeDisplay[Type] = true; // The time remaining won't display on this buff
        }

        public override void Update(Player player, ref int buffIndex)
        {
            int[] projIds = GetProjectileIds();
            foreach (int projId in projIds)
            {
                if (player.ownedProjectileCounts[projId] > 0)
                {
                    player.buffTime[buffIndex] = 18000;
                    return;
                }
            }
            player.DelBuff(buffIndex);
            buffIndex--;
        }
    }
}
