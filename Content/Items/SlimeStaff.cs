﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using OneSummonArmy.Content.Buffs;
using OneSummonArmy.Content.Projectiles.Slimes;

namespace OneSummonArmy.Content.Items
{
    public class SlimeStaff : StandardStaff
    {
        public override string Texture => GetPathTo("Slime");
        int Level { get; set; }
        private int minionDamage;
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 8;
            Item.knockBack = 4f;
            Item.mana = 10;
            Item.width = 26;
            Item.height = 28;
            Item.value = Item.sellPrice(gold: 1);
            Item.rare = ItemRarityID.Blue;
            Item.shootSpeed = 10f;
            Item.shoot = ModContent.ProjectileType<SlimeCounter>();
            Item.buffType = ModContent.BuffType<SlimeBuff>();
            Item.reuseDelay = 1;
            Level = 1;
        }
        private void ProjIdByLevel(int level, out int id)
        {
            
            id = level switch
            {
                1 => ModContent.ProjectileType<GreenS>(),
                2 => ModContent.ProjectileType<PinkS>(),
                3 => ModContent.ProjectileType<YellowS>(),
                4 => ModContent.ProjectileType<CopperS>(),
                _ => ModContent.ProjectileType<KingS>(),
            };
        }
        protected override string TypeString { get => "Slime"; }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            player.AddBuff(Item.buffType, 2);
            int level = player.ownedProjectileCounts[Item.shoot] + 1;
            Projectile.NewProjectileDirect(source, player.Center, Vector2.Zero, Item.shoot, 0, 0f);
            ProjIdByLevel(level, out int projId);
            if (level == 1)
            {
                Projectile.NewProjectileDirect(source, position, velocity, projId, damage, knockback, player.whoAmI);
               
                return false;
            }
            ProjIdByLevel(level - 1, out int prevId);
            foreach (var proj in Main.ActiveProjectiles)
            {
                if (proj.owner == player.whoAmI && proj.type == prevId)
                {
                    Projectile.NewProjectileDirect(source, proj.position, proj.velocity, projId, damage, knockback, player.whoAmI);
                    proj.Kill();
                    break;
                }
            }
            return false;
        }
    }
}
