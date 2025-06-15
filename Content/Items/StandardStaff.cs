using Microsoft.Xna.Framework;
using OneSummonArmy.Content.Buffs;
using OneSummonArmy.Content.Projectiles.Birds;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace OneSummonArmy.Content.Items
{
    public class StandardStaff : ModItem
    {
        protected string sonsTexture = "OneSummonArmy/Assets/Textures/Items/";
        public override string Texture => GetPathTo("Standard");
        protected string GetPathTo(string s) => string.Concat(sonsTexture, s);
        protected string GetPathTo(int n) => GetPathTo($"{n}");
        protected string AddDirToPath(string s) => GetPathTo($"{s}/");
        protected virtual string TypeString { get; }
        public override void SetStaticDefaults()
        {
            ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true;
            ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
            ItemID.Sets.StaffMinionSlotsRequired[Type] = 1f;
        }
        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item44;
            Item.DamageType = DamageClass.Summon;
            Item.autoReuse = true;
            Item.useTime = 36;
            Item.useAnimation = 36;
            Item.noMelee = true;
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            position = Main.MouseWorld;
        }
        /// <summary>
        /// If you want to use this method, make sure to create a field named "typeString".
        /// It must contain the name of minions your staff summons.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="source"></param>
        /// <param name="position"></param>
        /// <param name="velocity"></param>
        /// <param name="type"></param>
        /// <param name="damage"></param>
        /// <param name="knockback"></param>
        /// <returns></returns>
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.numMinions == player.maxMinions) { return false; }
            player.AddBuff(Item.buffType, 2);
            int level = player.ownedProjectileCounts[Item.shoot] + 1;
            Projectile.NewProjectileDirect(source, player.Center, Vector2.Zero, Item.shoot, 0, 0f);
            int projId = Func.ProjIdByLevel(TypeString, level);
            int prevId = Func.ProjIdByLevel(TypeString, level - 1);
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
