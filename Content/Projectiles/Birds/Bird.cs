using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using OneSummonArmy.Content.Buffs;
using OneSummonArmy.ID;
using OneSummonArmy.AI;
using System.Collections.Generic;
using System;

namespace OneSummonArmy.Content.Projectiles.Birds
{
    public abstract class Bird : ModProjectile
    {
        // Here you can decide if your minion breaks things like grass or pots
        bool isMooving = false;
        protected float BasicSpeed {  get; set; }
        protected float BasicInertia {  get; set; }

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
            int totalIndexesInGroup;
            CheckActive(player);
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
            float standardSpeed = 6f;
            float maxSpeed = 8f;
            int attackRange = 800;
            int attackTarget = -1;
            Projectile.Minion_FindTargetInRange(attackRange, ref attackTarget, skipIfCannotHitWithOwnBody: false); /*
            if (attackTarget != -1)
            {
                NPC nPC = Main.npc[attackTarget];
                if (player.Distance(nPC.Center) > (float)attackRange)
                {
                    attackTarget = -1;
                }
            }
            */
            if (attackTarget != -1)
            {
                if (!Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
                {
                    Projectile.tileCollide = true;
                }
                NPC nPC2 = Main.npc[attackTarget];
                float targetDistance = Projectile.Distance(nPC2.Center);
                Rectangle rectangle = new Rectangle((int)Projectile.position.X, (int)Projectile.position.Y, Projectile.width, Projectile.height);
                Rectangle value = new Rectangle((int)nPC2.position.X, (int)nPC2.position.Y, nPC2.width, nPC2.height);
                if (rectangle.Intersects(value))
                {
                    Projectile.tileCollide = false;
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
                    Vector2 direction = Projectile.DirectionTo(nPC2.Center);
                    Projectile.velocity = Vector2.Lerp(Projectile.velocity, direction * standardSpeed, 0.15f);
                }
                else
                {
                    Projectile.tileCollide = false;
                    Vector2 direction = Projectile.DirectionTo(nPC2.Center);
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
            Projectile.tileCollide = false;
            List<int> ai158_blacklistedTargets = new List<int>();
            AIs.AI_GetMyGroupIndexAndFillBlackList(Projectile, ai158_blacklistedTargets, out var index, out var _);
            Projectile.localAI[0] = index;
            Vector2 home = Projectile.AI_158_GetHomeLocation(player, index);
            float homeDistance = Projectile.Distance(home);
            bool flag = player.gravDir > 0f && player.fullRotation == 0f && player.headRotation == 0f;
            if (homeDistance > 2000f || flag)
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
        }
    }
}
