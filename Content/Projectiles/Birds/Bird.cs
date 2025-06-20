using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using OneSummonArmy.Content.Buffs;
using OneSummonArmy;
using System.Collections.Generic;
using System;
using Terraria.ID;

namespace OneSummonArmy.Content.Projectiles.Birds
{
    public abstract class Bird : StandardProjectile
    {
        protected override string TypeString => "Bird";
        readonly int idleFrame;
        readonly float basicSpeed;
        readonly int movingFramesL, movingFramesR;
        public Bird(int idleFrame = 4, int movingFrameStart = 0, int movingFrameEnd = 4, int basicSpeed = 8)
        {
            this.idleFrame = idleFrame;
            this.basicSpeed = basicSpeed;
            movingFramesL = movingFrameStart;
            movingFramesR = movingFrameEnd;
            sonsTexture = AddDirToPath("Birds");
            buffType = ModContent.BuffType<BirdBuff>();
        }
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
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.minion = true;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.minionSlots = 0f;
            Projectile.penetrate = -1;
            Projectile.hide = false;
            Func.GetMyGroupIndex(Projectile, out var index, out var _);
            Projectile.localAI[0] = index;
            AdditionalDefaults();
        }
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles,
            List<int> overPlayers, List<int> overWires)
        {
            overPlayers.Add(index);
        }
        public override bool? CanCutTiles() { return false; }
        public override bool MinionContactDamage() { return true; }
        
        protected virtual Vector2 GetHomeLocation()
        {
            var home = Projectile.AI_158_GetHomeLocation(Main.player[Projectile.owner], (int) Projectile.localAI[0]);
            return home;
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            General(ref player);
            AnimateMovement();

            int attackTarget = FindTarget(800);
            if (attackTarget != -1)
            {
                Attack(attackTarget);
            }
            else
            {
                GoHome(player);
            }

        }
            
        private void General(ref Player player)
        {
            CheckActive(player);
            int level = player.ownedProjectileCounts[ModContent.ProjectileType<BirdCounter>()];
            if (Projectile.type != Func.ProjIdByLevel("Bird", level))
            {
                var source = player.GetSource_FromThis();
                var proj = Projectile.NewProjectileDirect(source, Projectile.position, Projectile.velocity, Func.ProjIdByLevel("Bird", level), 0, 0);
                proj.Center = Projectile.Center;
                Projectile.Kill();
                return;
            }
            Projectile.damage = 7 * level;
            Projectile.knockBack = 4 * (1 + (float)level * 0.1f);
        }
        private void AnimateMovement()
        {
            int totalIndexesInGroup = ++Projectile.frameCounter;
            if (totalIndexesInGroup >= 6)
            {
                Projectile.frameCounter = 0;
                totalIndexesInGroup = ++Projectile.frame;
                if (totalIndexesInGroup >= movingFramesR || totalIndexesInGroup < movingFramesL)
                {
                    Projectile.frame = movingFramesL;
                }
            }
        }
        private int FindTarget(int range)
        {
            int attackTarget = -1;
            Projectile.Minion_FindTargetInRange(range, ref attackTarget, skipIfCannotHitWithOwnBody: false);
            return attackTarget;
        }
        private void Attack(int attackTarget)
        {
            float maxSpeed = basicSpeed * 1.3f;
            NPC enemy = Main.npc[attackTarget];
            float targetDistance = Projectile.Distance(enemy.Center);
            Rectangle myHitbox = new((int)Projectile.position.X, (int)Projectile.position.Y, Projectile.width, Projectile.height);
            Rectangle enemyHitbox = new((int)enemy.position.X, (int)enemy.position.Y, enemy.width, enemy.height);
            if (myHitbox.Intersects(enemyHitbox))
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
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, direction * basicSpeed, 0.15f);
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
        private void GoHome(Player player) 
        {
            Vector2 home = GetHomeLocation();
            float homeDistance = Projectile.Distance(home);
            bool flag = player.gravDir > 0f && player.fullRotation == 0f && player.headRotation == 0f;
            if (homeDistance > 2000f)
            {
                SitAtHome(home, player);
            }
            else if (homeDistance > 40f)
            {
                float newMaxSpeed = basicSpeed + homeDistance * 0.006f;
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
            else if (homeDistance > 15f + player.velocity.Length())
            {
                Vector2 directionToHome = Projectile.DirectionTo(home);
                Projectile.velocity += new Vector2(Math.Sign(directionToHome.X), Math.Sign(directionToHome.Y)) * 0.05f;
                if (Projectile.velocity.Length() > basicSpeed)
                {
                    Projectile.velocity *= basicSpeed / Projectile.velocity.Length();
                }
                Projectile.rotation = Projectile.velocity.X * 0.1f;
                Projectile.direction = ((Projectile.velocity.X > 0f) ? 1 : (-1));
                Projectile.spriteDirection = ((Projectile.velocity.X > 0f) ? 1 : (-1));
            }
            else if (flag)
            {
                SitAtHome(home, player);
            }
        }
        private void SitAtHome(Vector2 home, Player player)
        {
            Projectile.Center = home;
            Projectile.frame = idleFrame;
            Projectile.frameCounter = 0;
            Projectile.velocity = Vector2.Zero;
            Projectile.direction = (Projectile.spriteDirection = player.direction);
            Projectile.rotation = 0f;
        }
    }
}
