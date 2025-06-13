using Microsoft.Xna.Framework;
using OneSummonArmy;
using OneSummonArmy.Content.Buffs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace OneSummonArmy.Content.Projectiles.Slimes
{
    public abstract class Slime : StandardProjectile
    {
        public Slime() {
            sunsTexture = AddDirToPath("Slimes");
        }
        bool onTildeCollide_checker = true;
        float n = 0f;
        int onGroundCounter = 0;
        protected float BasicSpeed { get; set; }
        protected float BasicInertia { get; set; }
        protected virtual void AdditionalStaticDefaults() { }
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 6;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
            Main.projPet[Projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
            AdditionalStaticDefaults();
        }
        protected virtual void AdditionalDefaults() { }
        public override void SetDefaults()
        {
            //Projectile.extraUpdates = 1;
            Projectile.timeLeft = 2;
            Projectile.width = 44;
            Projectile.height = 28;
            BasicSpeed = 8;
            BasicInertia = 20;
            Projectile.tileCollide = true;
            Projectile.friendly = true;
            Projectile.minion = true;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.minionSlots = 0f;
            Projectile.penetrate = -1;
            AdditionalDefaults();
        }

        

        protected virtual void GetMovingFramesOnEarth(out int l, out int r)
        {
            l = 0;
            r = 2;
        }

        protected virtual void GetMovingFramesInFly(out int l, out int r)
        {
            l = 2;
            r = 6;
        }
        
        public override bool? CanCutTiles() { return false; }
        public override bool MinionContactDamage() { return true; }
        bool CheckActive(Player player)
        {
            if (player.dead || !player.active)
            {
                player.ClearBuff(ModContent.BuffType<SlimeBuff>());

                return false;
            }

            if (player.HasBuff(ModContent.BuffType<SlimeBuff>()))
            {
                Projectile.timeLeft = 2;
            }

            return true;
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            CheckActive(player);
            int level = player.ownedProjectileCounts[ModContent.ProjectileType<SlimeCounter>()];
            Projectile.damage = 8 * level;
            Projectile.knockBack = 4 * (1 + (float)level * 0.1f);


            if (Math.Abs(player.position.Y - Projectile.position.Y) >= 480)
            {
                Projectile.width = 44;
                Projectile.tileCollide = false;
                onTildeCollide_checker = false;
                
                int totalIndexesInGroup;
                totalIndexesInGroup = ++Projectile.frameCounter;
                GetMovingFramesInFly(out var l, out var r);
                if (totalIndexesInGroup >= 6)
                {

                    Projectile.frameCounter = 0;

                    totalIndexesInGroup = ++Projectile.frame;
                    if (totalIndexesInGroup >= r || totalIndexesInGroup < l)
                    {
                        Projectile.frame = l;
                    }
                }
                Vector2 idlePosition = player.Center;
                idlePosition.Y -= 48f;

                idlePosition.X += 50 * -player.direction;

                Vector2 vectorToIdlePosition = idlePosition - Projectile.Center;
                float distanceToIdlePosition = vectorToIdlePosition.Length();

                if (Main.myPlayer == player.whoAmI && distanceToIdlePosition > 2000f)
                {
                    Projectile.position = idlePosition;
                    Projectile.velocity *= 0.1f;
                    Projectile.netUpdate = true;
                }

                float overlapVelocity = 0.04f;
                
                for (int i = 0; i < Main.maxProjectiles; i++)
                {
                    Projectile other = Main.projectile[i];
                    if (
                        i != Projectile.whoAmI &&
                        other.active &&
                        other.owner == Projectile.owner &&
                        Math.Abs(Projectile.position.X - other.position.X) + Math.Abs(Projectile.position.Y - other.position.Y) < Projectile.width
                    )
                    {
                        if (Projectile.position.X < other.position.X)
                        {
                            Projectile.velocity.X -= overlapVelocity;
                        }
                        else
                        {
                            Projectile.velocity.X += overlapVelocity;
                        }

                        if (Projectile.position.Y < other.position.Y)
                        {
                            Projectile.velocity.Y -= overlapVelocity;
                        }
                        else
                        {
                            Projectile.velocity.Y += overlapVelocity;
                        }
                    }

                }

                float speed = BasicSpeed;
                float inertia = BasicInertia;

                if (distanceToIdlePosition > 600f)
                {
                    speed = 48f;
                    inertia = 20f;
                }
                else
                {
                    speed = 16f;
                    inertia = 20f;
                }

                if (distanceToIdlePosition > 20f)
                {
                    vectorToIdlePosition.Normalize();
                    vectorToIdlePosition *= speed;

                    Projectile.velocity = (Projectile.velocity * (inertia - 1) + vectorToIdlePosition) / inertia;
                }
                else if (Projectile.velocity == Vector2.Zero)
                {
                    Projectile.velocity.X = -0.15f;
                    Projectile.velocity.Y = -0.05f;
                }
                if (Projectile.velocity.X != 0)
                {
                    Projectile.direction = ((Projectile.velocity.X > 0f) ? -1 : (1));
                    Projectile.spriteDirection = ((Projectile.velocity.X > 0f) ? -1 : (1));
                }
                else { Projectile.direction = - player.direction; Projectile.spriteDirection = - player.direction; }
            }

            else
            {
                Projectile.width = 26;
                Projectile.tileCollide = true;
                onTildeCollide_checker = true;
                int totalIndexesInGroup;
                totalIndexesInGroup = ++Projectile.frameCounter;
                GetMovingFramesOnEarth(out var l, out var r);
                if (totalIndexesInGroup >= 24)
                {
                    Projectile.frameCounter = 0;

                    totalIndexesInGroup = ++Projectile.frame;
                    if (totalIndexesInGroup >= r || totalIndexesInGroup < l)
                    {
                        Projectile.frame = l;
                    }
                }
                Projectile.velocity.X = player.velocity.X * 1.1f;
                if (Projectile.velocity.X != 0f)
                {
                    Projectile.direction = ((Projectile.velocity.X > 0f) ? -1 : (1));
                    Projectile.spriteDirection = ((Projectile.velocity.X > 0f) ? -1 : (1));
                }
                else { Projectile.direction = - player.direction;  Projectile.spriteDirection = - player.direction; }
                    Projectile.velocity.Y += 0.25f; 
                n += 1f; 
                
                
            }


        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (onTildeCollide_checker && Main.player[Projectile.owner].velocity.X != 0 && onGroundCounter >= 1)
            {
                Projectile.velocity.Y = -4f;
                n = 0f;
                onGroundCounter = 0;
            }
            else { onGroundCounter++; } // Projectile.velocity.X = 0; }
            return false;
        }
    }
}
