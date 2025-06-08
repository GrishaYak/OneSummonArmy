using Microsoft.Xna.Framework;
using OneSummonArmy.Content.Buffs;
using OneSummonArmy.Content.Projectiles.Hornets;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;


namespace OneSummonArmy.Content.Items
{
    internal class HornetStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true;
            ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
        }
        public override void SetDefaults()
        {
            Item.mana = 10;
            Item.damage = 12;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.shootSpeed = 10f;
            Item.shoot = ModContent.ProjectileType<HornetCounter>();
            Item.width = 26;
            Item.height = 28;
            Item.UseSound = SoundID.Item76;
            Item.useAnimation = 22;
            Item.useTime = 22;
            Item.rare = ItemRarityID.Orange;
            Item.noMelee = true;
            Item.knockBack = 2f;
            Item.buffType = ModContent.BuffType<HornetBuff>();
            Item.value = Item.sellPrice(silver: 70);
            Item.DamageType = DamageClass.Summon;
            Item.autoReuse = true;
            Item.reuseDelay = 2;
        }
        int HornetIdByLevel(int level)
        {
            return level switch
            {
                1 => ModContent.ProjectileType<VanilaHornet>(),
                2 => ModContent.ProjectileType<DoubleHornet>(),
                3 => ModContent.ProjectileType<TripleHornet>(),
                4 => ModContent.ProjectileType<RifleHornet>(),
                _ => ModContent.ProjectileType<ShotgunHornet>()
            };
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.numMinions == player.maxMinions) { return false; }
            player.AddBuff(Item.buffType, 2);
            int level = player.ownedProjectileCounts[Item.shoot] + 1;
            Projectile.NewProjectileDirect(source, player.Center, Vector2.Zero, Item.shoot, 0, 0f);
            int projId = HornetIdByLevel(level);
            int prevId = HornetIdByLevel(level - 1);
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
