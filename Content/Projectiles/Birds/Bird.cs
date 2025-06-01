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

        protected virtual void GetIdleFrames(out int l, out int r) 
        {
            l = 4;
            r = 5;
        }
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
            if (Projectile.type == 759)
            {
                if (player.dead)
                {
                    player.babyBird = false;
                }
                if (player.babyBird)
                {
                    Projectile.timeLeft = 2;
                }
                totalIndexesInGroup = ++Projectile.frameCounter;
                if (totalIndexesInGroup >= 6)
                {
                    Projectile.frameCounter = 0;
                    totalIndexesInGroup = ++Projectile.frame;
                    if (totalIndexesInGroup >= Main.projFrames[Projectile.type] - 1)
                    {
                        Projectile.frame = 0;
                    }
                }
            }
            float num = 6f;
            float num2 = 8f;
            int num3 = 800;
            float num4 = 150f;
            int attackTarget = -1;
            Projectile.Minion_FindTargetInRange(num3, ref attackTarget, skipIfCannotHitWithOwnBody: false);
            if (attackTarget != -1)
            {
                NPC nPC = Main.npc[attackTarget];
                if (player.Distance(nPC.Center) > (float)num3)
                {
                    attackTarget = -1;
                }
            }
            if (attackTarget != -1)
            {
                if (!Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
                {
                    Projectile.tileCollide = true;
                }
                NPC nPC2 = Main.npc[attackTarget];
                float num5 = Projectile.Distance(nPC2.Center);
                Rectangle rectangle = new Rectangle((int)Projectile.position.X, (int)Projectile.position.Y, Projectile.width, Projectile.height);
                Rectangle value = new Rectangle((int)nPC2.position.X, (int)nPC2.position.Y, nPC2.width, nPC2.height);
                if (rectangle.Intersects(value))
                {
                    Projectile.tileCollide = false;
                    if (Math.Abs(Projectile.velocity.X) + Math.Abs(Projectile.velocity.Y) < num2)
                    {
                        Projectile.velocity *= 1.1f;
                    }
                    if (Projectile.velocity.Length() > num2)
                    {
                        Projectile.velocity *= num2 / Projectile.velocity.Length();
                    }
                }
                else if (num5 > num4)
                {
                    Vector2 vector = Projectile.DirectionTo(nPC2.Center);
                    Projectile.velocity = Vector2.Lerp(Projectile.velocity, vector * num, 0.15f);
                }
                else
                {
                    Projectile.tileCollide = false;
                    Vector2 vector2 = Projectile.DirectionTo(nPC2.Center);
                    Projectile.velocity += new Vector2(Math.Sign(vector2.X), Math.Sign(vector2.Y)) * 0.35f;
                    if (Projectile.velocity.Length() > num2)
                    {
                        Projectile.velocity *= num2 / Projectile.velocity.Length();
                    }
                }
                float num6 = 0.025f;
                float num7 = Projectile.width * 3;
                for (int i = 0; i < 1000; i++)
                {
                    if (i != Projectile.whoAmI && Main.projectile[i].active && Main.projectile[i].owner == Projectile.owner && Main.projectile[i].type == Projectile.type && Math.Abs(Projectile.position.X - Main.projectile[i].position.X) + Math.Abs(Projectile.position.Y - Main.projectile[i].position.Y) < num7)
                    {
                        if (Projectile.position.X < Main.projectile[i].position.X)
                        {
                            Projectile.velocity.X -= num6;
                        }
                        else
                        {
                            Projectile.velocity.X += num6;
                        }
                        if (Projectile.position.Y < Main.projectile[i].position.Y)
                        {
                            Projectile.velocity.Y -= num6;
                        }
                        else
                        {
                            Projectile.velocity.Y += num6;
                        }
                    }
                }
                Projectile.rotation = Projectile.velocity.X * 0.1f;
                Projectile.direction = ((Projectile.velocity.X > 0f) ? 1 : (-1));
                Projectile.spriteDirection = ((Projectile.velocity.X > 0f) ? 1 : (-1));
                return;
            }
            Projectile.tileCollide = false;
            List<int> ai158_blacklistedTargets = new List<int>();
            AIs.AI_GetMyGroupIndexAndFillBlackList(Projectile, ai158_blacklistedTargets, out var index, out totalIndexesInGroup);
            Projectile.localAI[0] = index;
            Vector2 vector3 = Projectile.AI_158_GetHomeLocation(player, index);
            float num8 = Projectile.Distance(vector3);
            bool flag = player.gravDir > 0f && player.fullRotation == 0f && player.headRotation == 0f;
            if (num8 > 2000f)
            {
                Projectile.Center = vector3;
                Projectile.frame = Main.projFrames[Projectile.type] - 1;
                Projectile.frameCounter = 0;
                Projectile.velocity = Vector2.Zero;
                Projectile.direction = (Projectile.spriteDirection = player.direction);
                Projectile.rotation = 0f;
            }
            else if (num8 > 40f)
            {
                float num9 = num + num8 * 0.006f;
                Vector2 vector4 = Projectile.DirectionTo(vector3);
                vector4 *= MathHelper.Lerp(1f, 5f, Utils.GetLerpValue(40f, 800f, num8, clamped: true));
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, vector4 * num9, 0.025f);
                if (Projectile.velocity.Length() > num9)
                {
                    Projectile.velocity *= num9 / Projectile.velocity.Length();
                }
                float num10 = 0.05f;
                float num11 = Projectile.width;
                for (int j = 0; j < 1000; j++)
                {
                    if (j != Projectile.whoAmI && Main.projectile[j].active && Main.projectile[j].owner == Projectile.owner && Main.projectile[j].type == Projectile.type && Math.Abs(Projectile.position.X - Main.projectile[j].position.X) + Math.Abs(Projectile.position.Y - Main.projectile[j].position.Y) < num11)
                    {
                        if (Projectile.position.X < Main.projectile[j].position.X)
                        {
                            Projectile.velocity.X -= num10;
                        }
                        else
                        {
                            Projectile.velocity.X += num10;
                        }
                        if (Projectile.position.Y < Main.projectile[j].position.Y)
                        {
                            Projectile.velocity.Y -= num10;
                        }
                        else
                        {
                            Projectile.velocity.Y += num10;
                        }
                    }
                }
                Projectile.rotation = Projectile.velocity.X * 0.04f;
                Projectile.direction = ((Projectile.velocity.X > 0f) ? 1 : (-1));
                Projectile.spriteDirection = ((Projectile.velocity.X > 0f) ? 1 : (-1));
            }
            else if (num8 > 8f + player.velocity.Length())
            {
                Vector2 vector5 = Projectile.DirectionTo(vector3);
                Projectile.velocity += new Vector2(Math.Sign(vector5.X), Math.Sign(vector5.Y)) * 0.05f;
                if (Projectile.velocity.Length() > num)
                {
                    Projectile.velocity *= num / Projectile.velocity.Length();
                }
                Projectile.rotation = Projectile.velocity.X * 0.1f;
                Projectile.direction = ((Projectile.velocity.X > 0f) ? 1 : (-1));
                Projectile.spriteDirection = ((Projectile.velocity.X > 0f) ? 1 : (-1));
            }
            else if (flag)
            {
                Projectile.Center = vector3;
                Projectile.frame = Main.projFrames[Projectile.type] - 1;
                Projectile.frameCounter = 0;
                Projectile.velocity = Vector2.Zero;
                Projectile.direction = (Projectile.spriteDirection = player.direction);
                Projectile.rotation = 0f;
            }
        }
    }
}
