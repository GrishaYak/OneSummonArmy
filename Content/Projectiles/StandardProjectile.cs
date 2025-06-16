using OneSummonArmy.Content.Buffs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OneSummonArmy.Content.Projectiles
{
    public abstract class StandardProjectile : ModProjectile
    {
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
        protected bool CheckActive(Player player)
        {
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
    }
}
