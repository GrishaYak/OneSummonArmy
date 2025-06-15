using Microsoft.Xna.Framework;
using OneSummonArmy.Content.Buffs;
using OneSummonArmy.Content.Projectiles.Hornets;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using OneSummonArmy;


namespace OneSummonArmy.Content.Items
{
    internal class HornetStaff : StandardStaff
    {
        public override string Texture => GetPathTo("Hornet");
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.mana = 10;
            Item.damage = 12;
            Item.shootSpeed = 10f;
            Item.shoot = ModContent.ProjectileType<HornetCounter>();
            Item.width = 26;
            Item.height = 28;
            Item.useAnimation = 22;
            Item.useTime = 22;
            Item.rare = ItemRarityID.Orange;
            Item.knockBack = 2f;
            Item.buffType = ModContent.BuffType<HornetBuff>();
            Item.value = Item.sellPrice(silver: 70);
            Item.reuseDelay = 2;
        }
        protected override string TypeString { get => "Hornet"; }

    }
}
