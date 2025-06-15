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
        protected override string TypeString { get => "Flinx"; }

    }
}
