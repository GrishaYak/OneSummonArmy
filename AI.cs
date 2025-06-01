using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;

namespace OneSummonArmy.AI
{
    public class AIs
    {
        public static Vector2 CounterGetHome(Player master, int stackedIndex, int totalIndexes)
        {
            int num = master.bodyFrame.Height;
            if (num == 0)
            {
                num = 1;
            }
            Vector2 vector = Main.OffsetsPlayerHeadgear[master.bodyFrame.Y / num];
            vector.Y -= 2f;
            float num2 = master.miscCounterNormalized * 2f;
            int num3 = stackedIndex / 4;
            int num4 = totalIndexes / 4;
            if (totalIndexes % 4 > 0)
            {
                num4++;
            }
            int num5 = (totalIndexes - num3 * 4) % 4;
            if (num5 == 0)
            {
                num5 = 4;
            }
            if (num4 - 1 != num3)
            {
                num5 = 4;
            }
            int num6 = stackedIndex % num5;
            float num7 = (float)num6 / (float)num5;
            num2 += (float)num3 / 8f;
            if (stackedIndex >= (num4 - 1) * 4 && num3 > 0)
            {
                num2 = 0f;
                switch (num5)
                {
                    case 1:
                        num7 = 0f;
                        break;
                    case 2:
                        num7 = 0.25f + (float)num6 * 0.5f;
                        break;
                    case 3:
                        num7 = (float)(num6 - 1) / 6f;
                        break;
                    case 4:
                        num7 = ((float)num6 - 1.5f) / 6f;
                        break;
                }
            }
            Vector2 vector2 = new Vector2(0f, -8 - 12 * num3).RotatedBy((num2 + num7) * ((float)Math.PI * 2f));
            vector += vector2 + new Vector2(0f, master.gravDir * -40f);
            Vector2 mountedCenter = master.MountedCenter;
            _ = master.direction;
            Vector2 vec = mountedCenter + new Vector2(0f, master.gravDir * -21f) + vector;
            vec.Y += master.gfxOffY;
            return vec.Floor();
        }
        public static void AI_GetMyGroupIndexAndFillBlackList(Projectile proj, List<int> blackListedTargets, out int index, out int totalIndexesInGroup)
        {
            index = 0;
            totalIndexesInGroup = 0;
            for (int i = 0; i < 1000; i++)
            {
                Projectile projectile = Main.projectile[i];
                if (projectile.active && projectile.owner == proj.owner && projectile.type == proj.type && (projectile.type != 759 || projectile.frame == Main.projFrames[projectile.type] - 1))
                {
                    if (proj.whoAmI > i)
                    {
                        index++;
                    }
                    totalIndexesInGroup++;
                }
            }
        }
        public void BirdAI(Projectile proj)
        {
            Player player = Main.player[proj.owner];
            int totalIndexesInGroup;
            if (proj.type == 759)
            {
                if (player.dead)
                {
                    player.babyBird = false;
                }
                if (player.babyBird)
                {
                    proj.timeLeft = 2;
                }
                totalIndexesInGroup = ++proj.frameCounter;
                if (totalIndexesInGroup >= 6)
                {
                    proj.frameCounter = 0;
                    totalIndexesInGroup = ++proj.frame;
                    if (totalIndexesInGroup >= Main.projFrames[proj.type] - 1)
                    {
                        proj.frame = 0;
                    }
                }
            }
            float num = 6f;
            float num2 = 8f;
            int num3 = 800;
            float num4 = 150f;
            int attackTarget = -1;
            proj.Minion_FindTargetInRange(num3, ref attackTarget, skipIfCannotHitWithOwnBody: false);
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
                if (!Collision.SolidCollision(proj.position, proj.width, proj.height))
                {
                    proj.tileCollide = true;
                }
                NPC nPC2 = Main.npc[attackTarget];
                float num5 = proj.Distance(nPC2.Center);
                Rectangle rectangle = new Rectangle((int)proj.position.X, (int)proj.position.Y, proj.width, proj.height);
                Rectangle value = new Rectangle((int)nPC2.position.X, (int)nPC2.position.Y, nPC2.width, nPC2.height);
                if (rectangle.Intersects(value))
                {
                    proj.tileCollide = false;
                    if (Math.Abs(proj.velocity.X) + Math.Abs(proj.velocity.Y) < num2)
                    {
                        proj.velocity *= 1.1f;
                    }
                    if (proj.velocity.Length() > num2)
                    {
                        proj.velocity *= num2 / proj.velocity.Length();
                    }
                }
                else if (num5 > num4)
                {
                    Vector2 vector = proj.DirectionTo(nPC2.Center);
                    proj.velocity = Vector2.Lerp(proj.velocity, vector * num, 0.15f);
                }
                else
                {
                    proj.tileCollide = false;
                    Vector2 vector2 = proj.DirectionTo(nPC2.Center);
                    proj.velocity += new Vector2(Math.Sign(vector2.X), Math.Sign(vector2.Y)) * 0.35f;
                    if (proj.velocity.Length() > num2)
                    {
                        proj.velocity *= num2 / proj.velocity.Length();
                    }
                }
                float num6 = 0.025f;
                float num7 = proj.width * 3;
                for (int i = 0; i < 1000; i++)
                {
                    if (i != proj.whoAmI && Main.projectile[i].active && Main.projectile[i].owner == proj.owner && Main.projectile[i].type == proj.type && Math.Abs(proj.position.X - Main.projectile[i].position.X) + Math.Abs(proj.position.Y - Main.projectile[i].position.Y) < num7)
                    {
                        if (proj.position.X < Main.projectile[i].position.X)
                        {
                            proj.velocity.X -= num6;
                        }
                        else
                        {
                            proj.velocity.X += num6;
                        }
                        if (proj.position.Y < Main.projectile[i].position.Y)
                        {
                            proj.velocity.Y -= num6;
                        }
                        else
                        {
                            proj.velocity.Y += num6;
                        }
                    }
                }
                proj.rotation = proj.velocity.X * 0.1f;
                proj.direction = ((proj.velocity.X > 0f) ? 1 : (-1));
                proj.spriteDirection = ((proj.velocity.X > 0f) ? 1 : (-1));
                return;
            }
            proj.tileCollide = false;
            List<int> ai158_blacklistedTargets = new List<int>();
            ai158_blacklistedTargets.Clear();
            AI_GetMyGroupIndexAndFillBlackList(proj, ai158_blacklistedTargets, out var index, out totalIndexesInGroup);
            proj.localAI[0] = index;
            Vector2 vector3 = Projectile.AI_158_GetHomeLocation(player, index);
            float num8 = proj.Distance(vector3);
            bool flag = player.gravDir > 0f && player.fullRotation == 0f && player.headRotation == 0f;
            if (num8 > 2000f)
            {
                proj.Center = vector3;
                proj.frame = Main.projFrames[proj.type] - 1;
                proj.frameCounter = 0;
                proj.velocity = Vector2.Zero;
                proj.direction = (proj.spriteDirection = player.direction);
                proj.rotation = 0f;
            }
            else if (num8 > 40f)
            {
                float num9 = num + num8 * 0.006f;
                Vector2 vector4 = proj.DirectionTo(vector3);
                vector4 *= MathHelper.Lerp(1f, 5f, Utils.GetLerpValue(40f, 800f, num8, clamped: true));
                proj.velocity = Vector2.Lerp(proj.velocity, vector4 * num9, 0.025f);
                if (proj.velocity.Length() > num9)
                {
                    proj.velocity *= num9 / proj.velocity.Length();
                }
                float num10 = 0.05f;
                float num11 = proj.width;
                for (int j = 0; j < 1000; j++)
                {
                    if (j != proj.whoAmI && Main.projectile[j].active && Main.projectile[j].owner == proj.owner && Main.projectile[j].type == proj.type && Math.Abs(proj.position.X - Main.projectile[j].position.X) + Math.Abs(proj.position.Y - Main.projectile[j].position.Y) < num11)
                    {
                        if (proj.position.X < Main.projectile[j].position.X)
                        {
                            proj.velocity.X -= num10;
                        }
                        else
                        {
                            proj.velocity.X += num10;
                        }
                        if (proj.position.Y < Main.projectile[j].position.Y)
                        {
                            proj.velocity.Y -= num10;
                        }
                        else
                        {
                            proj.velocity.Y += num10;
                        }
                    }
                }
                proj.rotation = proj.velocity.X * 0.04f;
                proj.direction = ((proj.velocity.X > 0f) ? 1 : (-1));
                proj.spriteDirection = ((proj.velocity.X > 0f) ? 1 : (-1));
            }
            else if (num8 > 8f + player.velocity.Length())
            {
                Vector2 vector5 = proj.DirectionTo(vector3);
                proj.velocity += new Vector2(Math.Sign(vector5.X), Math.Sign(vector5.Y)) * 0.05f;
                if (proj.velocity.Length() > num)
                {
                    proj.velocity *= num / proj.velocity.Length();
                }
                proj.rotation = proj.velocity.X * 0.1f;
                proj.direction = ((proj.velocity.X > 0f) ? 1 : (-1));
                proj.spriteDirection = ((proj.velocity.X > 0f) ? 1 : (-1));
            }
            else if (flag)
            {
                proj.Center = vector3;
                proj.frame = Main.projFrames[proj.type] - 1;
                proj.frameCounter = 0;
                proj.velocity = Vector2.Zero;
                proj.direction = (proj.spriteDirection = player.direction);
                proj.rotation = 0f;
            }
        }
    }
}
