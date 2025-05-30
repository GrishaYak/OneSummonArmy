using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using OneSummonArmy.Content.Buffs;
using OneSummonArmy.Content.Projectiles.Birds;

namespace OneSummonArmy.Content.Items
{
    public class BirdStaff : ModItem
	{
        int Level {  get; set; }
        private int minionDamage;
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
            Item.shoot = ModContent.ProjectileType<Bird>();
            Item.buffType = ModContent.BuffType<BirdBuff>();
            Item.autoReuse = true;
            Item.reuseDelay = 1;

        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            position = Main.MouseWorld;
        }
        private void ProjIdAndDamageByLevel(int level, out int id, out int damage)
        {
            damage = Item.damage * level;
            id = level switch
            {
                1 => ModContent.ProjectileType<Finch>(),
                2 => ModContent.ProjectileType<BlueJay>(),
                3 => ModContent.ProjectileType<GoldenBird>(),
                4 => ModContent.ProjectileType<Toucan>(),
                _ => ModContent.ProjectileType<Ara>(),
            };
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            // This is needed so the buff that keeps your minion alive and allows you to despawn it properly applies
            if (!player.HasBuff<BirdBuff>())
            {
                Level = 0;
            }
            Level += 1;
            ProjIdAndDamageByLevel(Level, out int projId, out minionDamage);
            Item.shoot = projId;
            player.AddBuff(Item.buffType, 2);
            Vector2 newPos = position, v = velocity;
            IEntitySource src = source;
            foreach (var proj in Main.ActiveProjectiles)
            {
                if (proj.owner == player.whoAmI && proj.type == ModContent.ProjectileType<Bird>())
                {
                    src = proj.GetSource_FromThis();
                    newPos = proj.position;
                    v = proj.velocity;
                    proj.Kill();
                    break;
                }
            }
            // Minions have to be spawned manually, then have originalDamage assigned to the damage of the summon item
            Projectile.NewProjectileDirect(src, newPos, v, type, minionDamage, knockback, player.whoAmI);
            // Since we spawned the projectile manually already, we do not need the game to spawn it for ourselves anymore, so return false
            return false;
        }
    }
}
