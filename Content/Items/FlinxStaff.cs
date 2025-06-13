using OneSummonArmy.Content.Buffs;
using OneSummonArmy.Content.Projectiles.Flinxes;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using OneSummonArmy;

namespace OneSummonArmy.Content.Items
{
    public class FlinxStaff : StandardStaff
    {
        public override string Texture => GetPathTo("Flinx");
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.mana = 5;
            Item.damage = 8;
            Item.shootSpeed = 10f;
            Item.shoot = ModContent.ProjectileType<FlinxCounter>();
            Item.buffType = ModContent.BuffType<FlinxBuff>();
            Item.width = 38;
            Item.height = 40;
            Item.rare = ItemRarityID.Orange;
            Item.knockBack = 2f;
            Item.value = Item.sellPrice(silver: 50);
            Item.reuseDelay = 2;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.numMinions == player.maxMinions) { return false; }
            player.AddBuff(Item.buffType, 2);
            int level = player.ownedProjectileCounts[Item.shoot] + 1;
            Projectile.NewProjectileDirect(source, player.Center, Vector2.Zero, Item.shoot, 0, 0f);
            int projId = Func.ProjIdByLevel("Bird", level);
            int prevId = Func.ProjIdByLevel("Bird", level - 1);
            foreach (var proj in Main.ActiveProjectiles)
            {
                if (proj.owner == player.whoAmI && proj.type == prevId)
                {
                    var newProj = Projectile.NewProjectileDirect(source, proj.position, proj.velocity, projId, damage, knockback, player.whoAmI);
                    newProj.Center = proj.Center;
                    proj.Kill();
                    return false;
                }
            }
            Projectile.NewProjectileDirect(source, position, velocity, projId, damage, knockback, player.whoAmI);
            return false;
        }
    }
}
