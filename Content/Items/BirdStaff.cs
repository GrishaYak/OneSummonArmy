using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using OneSummonArmy.Content.Buffs;
using OneSummonArmy.Content.Projectiles.Birds;
using OneSummonArmy;

namespace OneSummonArmy.Content.Items
{
    public class BirdStaff : StandardStaff
	{
        public override string Texture => GetPathTo("Bird");
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 7;
            Item.knockBack = 4f;
            Item.mana = 10;
            Item.width = 26;
            Item.height = 28;
            Item.value = Item.sellPrice(gold: 1);
            Item.rare = ItemRarityID.Blue;
            Item.shootSpeed = 10f;
            Item.shoot = ModContent.ProjectileType<BirdCounter>();
            Item.buffType = ModContent.BuffType<BirdBuff>();
            Item.reuseDelay = 1;
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
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddRecipeGroup("Birds");
            recipe.AddIngredient(ItemID.LivingWoodWand);
        }
    }
}
