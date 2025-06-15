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
        protected override string TypeString { get => "Bird"; }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddRecipeGroup("Birds");
            recipe.AddIngredient(ItemID.LivingWoodWand);
        }
    }
}
