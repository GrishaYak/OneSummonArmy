using Microsoft.Xna.Framework;
using OneSummonArmy.Content.Buffs;
using static OneSummonArmy.Func;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OneSummonArmy.Content.Projectiles
{
    public abstract class StandardProjectile : ModProjectile
    {
        protected virtual string TypeString => "_";
        protected virtual int DefaultDamage { get => 7; }
        protected string sonsTexture = "OneSummonArmy/Assets/Textures/Projectiles/";
        public override string Texture => GetPathTo("Counter");
        protected string GetPathTo(string s) => string.Concat(sonsTexture, s);
        protected string GetPathTo(int n) => GetPathTo($"{n}");
        protected string AddDirToPath(string s) => GetPathTo($"{s}/");
        protected int buffType = ModContent.BuffType<StandardBuff>();
        public override void SetStaticDefaults()
        {
            Main.projPet[Projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
        }
        protected virtual bool CheckActive(Player player, int buffType=-1)
        {
            buffType = buffType != -1 ? buffType : this.buffType;
            if (player.dead || !player.active)
            {
                player.ClearBuff(buffType);
                return false;
            }
            if (player.HasBuff(buffType))
            {
                Projectile.timeLeft = 2;
            }
            return true;
        }

        public override void SetDefaults()
        {
            Projectile.minion = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 2;
        }
        protected void ScaleDamage(double scale)
        {
            Projectile.damage = (int)(Projectile.damage * scale);
        }
        /// <summary>
        ///  You have to set a value to DefaultDamage first!
        /// </summary>
        protected void SetDamageToDefault()
        {
            Projectile.damage = DefaultDamage;
        }
        protected void Move(Vector2 direction, float speed, float inertia)
        {
            Move(direction * speed, inertia);
        }
        protected void Move(Vector2 velocity, float inertia)
        {
            Projectile.velocity = (Projectile.velocity * inertia + velocity) / (inertia + 1f);
        }
        protected int GetTarget(float range, ref Vector2 targetPos)
        {
            int target = GetTarget(range);
            if (target != -1)
            {
                targetPos = Main.npc[target].Center;
            }
            return target;
        }
        protected int GetTarget(float range)
        {
            int target = -1;
            Projectile.Minion_FindTargetInRange((int)range, ref target, skipIfCannotHitWithOwnBody: false);
            return target;
        }
        /// <summary>
        /// It runs method CheckActive and checks if your projectile type is corresponding to 
        /// type that it should have on its level and replaces this projectile with the right one
        /// if needed.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="level"></param>
        protected void Basics(Player player, int level)
        {
            CheckActive(player);
            if (Projectile.type != ProjIdByLevel(TypeString, level))
            {
                var source = player.GetSource_FromThis();
                var proj = Projectile.NewProjectileDirect(source, Projectile.position, Projectile.velocity, ProjIdByLevel(TypeString, level), DefaultDamage, Projectile.knockBack);
                proj.Center = Projectile.Center;
                Projectile.Kill();
                return;
            }
        }
    }
}
