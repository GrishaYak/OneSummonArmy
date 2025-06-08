using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using OneSummonArmy.Content.Buffs;
using OneSummonArmy.Content.Projectiles.Birds;
using OneSummonArmy.AI;

namespace OneSummonArmy.Content.Items
{
    public class BirdStaff : ModItem
	{
        public override void SetStaticDefaults()
        {
            ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true; // This lets the player target anywhere on the whole screen while using a controller
            ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;

            ItemID.Sets.StaffMinionSlotsRequired[Type] = 1f; // The default value is 1, but other values are supported. See the docs for more guidance. 
        }
        public override void SetDefaults()
        {
            Item.damage = 7;
            Item.knockBack = 4f;
            Item.mana = 10;
            Item.width = 26;
            Item.height = 28;
            Item.useTime = 36;
            Item.useAnimation = 36;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item44;
            Item.value = Item.sellPrice(gold: 1);
            Item.rare = ItemRarityID.Blue;
            Item.noMelee = true;
            Item.DamageType = DamageClass.Summon;
            Item.shootSpeed = 10f;
            Item.shoot = ModContent.ProjectileType<BirdCounter>();
            Item.buffType = ModContent.BuffType<BirdBuff>();
            Item.autoReuse = true;
            Item.reuseDelay = 1;
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            position = Main.MouseWorld;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.numMinions == player.maxMinions) { return false; }
            player.AddBuff(Item.buffType, 2);
            int level = player.ownedProjectileCounts[Item.shoot] + 1;
            Projectile.NewProjectileDirect(source, player.Center, Vector2.Zero, Item.shoot, 0, 0f);
            int projId = AIs.BirdIdByLevel(level);
            int prevId = AIs.BirdIdByLevel(level - 1);
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
