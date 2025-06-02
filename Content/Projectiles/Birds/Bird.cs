using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using OneSummonArmy.Content.Buffs;
using OneSummonArmy.ID;
using OneSummonArmy.AI;
using System.Collections.Generic;
using System;
using Terraria.DataStructures;
using Terraria.ID;
using log4net.Core;

namespace OneSummonArmy.Content.Projectiles.Birds
{
    public abstract class Bird : ModProjectile
    {
        bool shouldWriteStats = true;
        protected float BasicSpeed { get; set; }
        protected float BasicInertia { get; set; }
        protected virtual void AdditionalStaticDefaults() { }
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 5;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
            Main.projPet[Projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
            AdditionalStaticDefaults();
        }
        protected virtual void AdditionalDefaults() { }
        public override void SetDefaults()
        {
            Projectile.timeLeft = 2;
            Projectile.width = 30;
            Projectile.height = 20;
            BasicSpeed = 8;
            BasicInertia = 20;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.minion = true;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.minionSlots = 0f;
            Projectile.penetrate = -1;
            AdditionalDefaults();
        }

        protected virtual int GetIdleFrame() { return 4; }

        protected virtual void GetMovingFrames(out int l, out int r)
        {
            l = 0;
            r = 4;
        }
        public override bool? CanCutTiles() { return false; }
        public override bool MinionContactDamage() { return true; }
        bool CheckActive(Player owner)
        {
            if (owner.dead || !owner.active)
            {
                owner.ClearBuff(ModContent.BuffType<BirdBuff>());

                return false;
            }

            if (owner.HasBuff(ModContent.BuffType<BirdBuff>()))
            {
                Projectile.timeLeft = 2;
            }

            return true;
        }
        
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            CheckActive(player);
            int level = player.ownedProjectileCounts[ModContent.ProjectileType<BirdCounter>()];
            Projectile.damage = 7 * level;
            Projectile.knockBack = 4 * (1 + (float)level * 0.1f);
            shouldWriteStats = false;
            int totalIndexesInGroup;
            totalIndexesInGroup = ++Projectile.frameCounter;
            if (totalIndexesInGroup >= 6)
            {
                Projectile.frameCounter = 0;
                GetMovingFrames(out var l, out var r);
                totalIndexesInGroup = ++Projectile.frame;
                if (totalIndexesInGroup >= r || totalIndexesInGroup < l)
                {
                    Projectile.frame = 0;
                }
            }
            float standardSpeed = BasicSpeed;
            float maxSpeed = standardSpeed * 1.3f;
            int attackRange = 800;
            int attackTarget = -1;
            Projectile.Minion_FindTargetInRange(attackRange, ref attackTarget, skipIfCannotHitWithOwnBody: false); 
            if (attackTarget != -1)
            {
                NPC enemy = Main.npc[attackTarget];
                float targetDistance = Projectile.Distance(enemy.Center);
                Rectangle rectangle = new((int)Projectile.position.X, (int)Projectile.position.Y, Projectile.width, Projectile.height);
                Rectangle value = new((int)enemy.position.X, (int)enemy.position.Y, enemy.width, enemy.height);
                if (rectangle.Intersects(value))
                {
                    if (Math.Abs(Projectile.velocity.X) + Math.Abs(Projectile.velocity.Y) < maxSpeed)
                    {
                        Projectile.velocity *= 1.1f;
                    }
                    if (Projectile.velocity.Length() > maxSpeed)
                    {
                        Projectile.velocity *= maxSpeed / Projectile.velocity.Length();
                    }
                }
                else if (targetDistance > 150f)
                {
                    Vector2 direction = Projectile.DirectionTo(enemy.Center);
                    Projectile.velocity = Vector2.Lerp(Projectile.velocity, direction * standardSpeed, 0.15f);
                }
                else
                {
                    Vector2 direction = Projectile.DirectionTo(enemy.Center);
                    Projectile.velocity += new Vector2(Math.Sign(direction.X), Math.Sign(direction.Y)) * 0.35f;
                    if (Projectile.velocity.Length() > maxSpeed)
                    {
                        Projectile.velocity *= maxSpeed / Projectile.velocity.Length();
                    }
                }
                Projectile.rotation = Projectile.velocity.X * 0.1f;
                Projectile.direction = ((Projectile.velocity.X > 0f) ? 1 : (-1));
                Projectile.spriteDirection = ((Projectile.velocity.X > 0f) ? 1 : (-1));
                return;
            }
            List<int> ai158_blacklistedTargets = new List<int>();
            AIs.AI_GetMyGroupIndexAndFillBlackList(Projectile, ai158_blacklistedTargets, out var index, out var _);
            Projectile.localAI[0] = index;
            Vector2 home = Projectile.AI_158_GetHomeLocation(player, index);
            float homeDistance = Projectile.Distance(home);
            bool flag = player.gravDir > 0f && player.fullRotation == 0f && player.headRotation == 0f;
            if (homeDistance > 2000f)
            {
                Projectile.Center = home;
                Projectile.frame = GetIdleFrame();
                Projectile.frameCounter = 0;
                Projectile.velocity = Vector2.Zero;
                Projectile.direction = (Projectile.spriteDirection = player.direction);
                Projectile.rotation = 0f;
            }
            else if (homeDistance > 40f)
            {
                float newMaxSpeed = standardSpeed + homeDistance * 0.006f;
                Vector2 directionToHome = Projectile.DirectionTo(home);
                directionToHome *= MathHelper.Lerp(1f, 5f, Utils.GetLerpValue(40f, 800f, homeDistance, clamped: true));
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, directionToHome * newMaxSpeed, 0.025f);
                if (Projectile.velocity.Length() > newMaxSpeed)
                {
                    Projectile.velocity *= newMaxSpeed / Projectile.velocity.Length();
                }
                Projectile.rotation = Projectile.velocity.X * 0.04f;
                Projectile.direction = ((Projectile.velocity.X > 0f) ? 1 : (-1));
                Projectile.spriteDirection = ((Projectile.velocity.X > 0f) ? 1 : (-1));
            }
            else if (homeDistance > 8f + player.velocity.Length())
            {
                Vector2 directionToHome = Projectile.DirectionTo(home);
                Projectile.velocity += new Vector2(Math.Sign(directionToHome.X), Math.Sign(directionToHome.Y)) * 0.05f;
                if (Projectile.velocity.Length() > standardSpeed)
                {
                    Projectile.velocity *= standardSpeed / Projectile.velocity.Length();
                }
                Projectile.rotation = Projectile.velocity.X * 0.1f;
                Projectile.direction = ((Projectile.velocity.X > 0f) ? 1 : (-1));
                Projectile.spriteDirection = ((Projectile.velocity.X > 0f) ? 1 : (-1));
            }
            else if (flag)
            {
                Projectile.Center = home;
                Projectile.frame = GetIdleFrame();
                Projectile.frameCounter = 0;
                Projectile.velocity = Vector2.Zero;
                Projectile.direction = (Projectile.spriteDirection = player.direction);
                Projectile.rotation = 0f;
            }
        }
    }
}
