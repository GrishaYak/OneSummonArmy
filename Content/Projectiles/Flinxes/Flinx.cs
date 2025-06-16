using Microsoft.Xna.Framework;
using OneSummonArmy.Content.Buffs;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using static System.Net.Mime.MediaTypeNames;

namespace OneSummonArmy.Content.Projectiles.Flinxes
{
    public abstract class Flinx : StandardProjectile
    {
        public Flinx() {
            sonsTexture = AddDirToPath("Flinxes");
            buffType = ModContent.BuffType<FlinxBuff>();
        }
        
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            Main.projFrames[Projectile.type] = 12;
        }
        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.width = 26;
            Projectile.height = 26;
            Projectile.netImportant = true;
            Projectile.minionSlots = 1f;
            Projectile.friendly = true;
            Projectile.decidesManualFallThrough = true;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 10;   
        }

        public override void AI()
        {
            Player player = Main.player[owner];
            if (!player.active)
            {
                active = false;
                return;
            }

            bool flag = type == 393 || type == 394 || type == 395;
            bool flag2 = type == 758;
            bool flag3 = type == 833 || type == 834 || type == 835;
            bool flag4 = type == 834 || type == 835;
            bool flag5 = type == 951;
            int num = 450;
            float num2 = 500f;
            float num3 = 300f;
            int num4 = 15;
            if (flag5)
            {
                if (player.dead)
                    player.flinxMinion = false;

                if (player.flinxMinion)
                    timeLeft = 2;

                num = 800;
            }

            if (flag)
            {
                if (player.dead)
                    player.pirateMinion = false;

                if (player.pirateMinion)
                    timeLeft = 2;

                num = 800;
            }

            if (flag3)
            {
                if (player.dead)
                    player.stormTiger = false;

                if (player.stormTiger)
                    timeLeft = 2;

                num = 800;
                if (ai[0] != 4f)
                {
                    if (velocity != Vector2.Zero && Main.rand.Next(18) == 0)
                    {
                        Dust obj = Main.dust[Dust.NewDust(position, width, height, 269)];
                        obj.fadeIn = 0.5f;
                        obj.scale = 0.3f;
                        obj.noLight = true;
                        obj.velocity += velocity * 0.005f;
                    }

                    if (type == 833)
                        Lighting.AddLight(base.Center, Vector3.One * 0.5f);

                    if (type == 834)
                        Lighting.AddLight(base.Center, Vector3.One * 0.8f);

                    if (type == 835)
                        Lighting.AddLight(base.Center, Color.Lerp(Main.OurFavoriteColor, Color.White, 0.8f).ToVector3() * 1f);
                }

                if (owner == Main.myPlayer)
                {
                    if (localAI[0] <= 0f)
                    {
                        int num5;
                        switch (type)
                        {
                            default:
                                num5 = 360;
                                break;
                            case 834:
                                num5 = 300;
                                break;
                            case 835:
                                num5 = 240;
                                break;
                        }

                        if (damage != 0)
                        {
                            bool flag6 = AI_067_TigerSpecialAttack();
                            localAI[0] = (flag6 ? num5 : 10);
                        }
                    }
                    else
                    {
                        localAI[0] -= 1f;
                    }
                }
            }

            if (flag2)
            {
                if (player.dead)
                    player.vampireFrog = false;

                if (player.vampireFrog)
                    timeLeft = 2;

                num = 800;
            }

            if (type == 500)
            {
                num2 = 200f;
                if (player.dead)
                    player.crimsonHeart = false;

                if (player.crimsonHeart)
                    timeLeft = 2;
            }

            if (type == 653)
            {
                num2 = 300f;
                if (player.dead)
                    player.companionCube = false;

                if (player.companionCube)
                    timeLeft = 2;
            }

            if (type == 1018)
            {
                num2 = 200f;
                if (player.dead)
                    player.petFlagDirtiestBlock = false;

                if (player.petFlagDirtiestBlock)
                    timeLeft = 2;
            }

            if (flag3 && ai[0] == 4f)
            {
                velocity = Vector2.Zero;
                frame = 9;
                if (flag4)
                    frame = 11;

                ai[1] -= 1f;
                if (!(ai[1] <= 0f))
                    return;

                ai[0] = 0f;
                ai[1] = 0f;
                netUpdate = true;
            }

            Vector2 vector = player.Center;
            if (flag5)
            {
                vector.X -= (45 + player.width / 2) * player.direction;
                vector.X -= minionPos * 30 * player.direction;
            }
            else if (flag)
            {
                vector.X -= (15 + player.width / 2) * player.direction;
                vector.X -= minionPos * 20 * player.direction;
            }
            else if (flag3)
            {
                vector.X -= (15 + player.width / 2) * player.direction;
                vector.X -= minionPos * 40 * player.direction;
            }
            else if (flag2)
            {
                vector.X -= (35 + player.width / 2) * player.direction;
                vector.X -= minionPos * 40 * player.direction;
            }
            else if (type == 500)
            {
                vector.X -= (15 + player.width / 2) * player.direction;
                vector.X -= 40 * player.direction;
            }
            else if (type == 1018)
            {
                vector.X = player.Center.X;
            }
            else if (type == 653)
            {
                vector.X = player.Center.X;
            }

            if (type == 500)
            {
                Lighting.AddLight(base.Center, 0.9f, 0.1f, 0.3f);
                int num6 = 6;
                if (frame == 0 || frame == 2)
                    num6 = 12;

                if (++frameCounter >= num6)
                {
                    frameCounter = 0;
                    if (++frame >= Main.projFrames[type])
                        frame = 0;
                }

                rotation += velocity.X / 20f;
                Vector2 vector2 = (-Vector2.UnitY).RotatedBy(rotation).RotatedBy((float)direction * 0.2f);
                int num7 = Dust.NewDust(base.Center + vector2 * 10f - new Vector2(4f), 0, 0, 5, vector2.X, vector2.Y, 0, Color.Transparent);
                Main.dust[num7].scale = 1f;
                Main.dust[num7].velocity = vector2.RotatedByRandom(0.7853981852531433) * 3.5f;
                Main.dust[num7].noGravity = true;
                Main.dust[num7].shader = GameShaders.Armor.GetSecondaryShader(Main.player[owner].cLight, Main.player[owner]);
            }

            if (type == 1018)
                rotation += velocity.X / 20f;

            if (type == 653)
            {
                rotation += velocity.X / 20f;
                bool flag7 = owner >= 0 && owner < 255;
                if (flag7)
                {
                    _CompanionCubeScreamCooldown[owner] -= 1f;
                    if (_CompanionCubeScreamCooldown[owner] < 0f)
                        _CompanionCubeScreamCooldown[owner] = 0f;
                }

                Tile tileSafely = Framing.GetTileSafely(base.Center);
                if (tileSafely.liquid > 0 && tileSafely.lava())
                    localAI[0] += 1f;
                else
                    localAI[0] -= 1f;

                localAI[0] = MathHelper.Clamp(localAI[0], 0f, 20f);
                if (localAI[0] >= 20f)
                {
                    if (flag7 && _CompanionCubeScreamCooldown[owner] == 0f)
                    {
                        _CompanionCubeScreamCooldown[owner] = 3600f;
                        SoundEngine.PlaySound((Main.rand.Next(10) == 0) ? SoundID.NPCDeath61 : SoundID.NPCDeath59, position);
                    }

                    Kill();
                }

                if (flag7 && owner == Main.myPlayer && Main.netMode != 2)
                {
                    Vector3 vector3 = Lighting.GetColor((int)base.Center.X / 16, (int)base.Center.Y / 16).ToVector3();
                    Vector3 vector4 = Lighting.GetColor((int)player.Center.X / 16, (int)player.Center.Y / 16).ToVector3();
                    if (vector3.Length() < 0.15f && vector4.Length() < 0.15f)
                        localAI[1] += 1f;
                    else if (localAI[1] > 0f)
                        localAI[1] -= 1f;

                    localAI[1] = MathHelper.Clamp(localAI[1], -3600f, 120f);
                    if (localAI[1] > (float)Main.rand.Next(30, 120) && !player.immune && player.velocity == Vector2.Zero)
                    {
                        if (Main.rand.Next(5) == 0)
                        {
                            SoundEngine.PlaySound(SoundID.Item16, base.Center);
                            localAI[1] = -600f;
                        }
                        else
                        {
                            SoundEngine.PlaySound(SoundID.Item1, base.Center);
                            player.Hurt(PlayerDeathReason.ByOther(6), 3, 0);
                            player.immune = false;
                            player.immuneTime = 0;
                            localAI[1] = -300 + Main.rand.Next(30) * -10;
                        }
                    }
                }
            }

            bool flag8 = true;
            if (type == 500 || type == 653 || type == 1018)
                flag8 = false;

            shouldFallThrough = player.position.Y + (float)player.height - 12f > position.Y + (float)height;
            friendly = false;
            int num8 = 0;
            int num9 = 15;
            int attackTarget = -1;
            bool flag9 = true;
            bool flag10 = ai[0] == 5f;
            if (flag5)
            {
                flag9 = false;
                friendly = true;
            }

            if (flag2)
            {
                friendly = true;
                num9 = 20;
                num8 = 60;
            }

            if (flag3)
            {
                flag9 = false;
                friendly = true;
                originalDamage = player.highestStormTigerGemOriginalDamage;
            }

            bool flag11 = ai[0] == 0f;
            if (flag3 && flag10)
                flag11 = true;

            if (flag11 && flag8)
                Minion_FindTargetInRange(num, ref attackTarget, skipIfCannotHitWithOwnBody: true, AI_067_CustomEliminationCheck_Pirates);

            if (flag3 && flag10)
            {
                if (attackTarget >= 0)
                {
                    float maxDistance = num;
                    NPC nPC = Main.npc[attackTarget];
                    vector = nPC.Center;
                    if (!IsInRangeOfMeOrMyOwner(nPC, maxDistance, out var _, out var _, out var _))
                    {
                        ai[0] = 0f;
                        ai[1] = 0f;
                        return;
                    }

                    Point point = nPC.Top.ToTileCoordinates();
                    int num10 = 0;
                    int num11 = point.Y;
                    while (num10 < num4)
                    {
                        Tile tile = Main.tile[point.X, num11];
                        if (tile == null || tile.active())
                            break;

                        num10++;
                        num11++;
                    }

                    int num12 = num4 / 2;
                    if (num10 < num12)
                    {
                        ai[0] = 0f;
                        ai[1] = 0f;
                        return;
                    }

                    if (base.Hitbox.Intersects(nPC.Hitbox) && velocity.Y >= 0f)
                    {
                        velocity.Y = -8f;
                        velocity.X = direction * 10;
                    }

                    float num13 = 20f;
                    float maxAmountAllowedToMove = 4f;
                    float num14 = 40f;
                    float num15 = 40f;
                    Vector2 top = nPC.Top;
                    float num16 = (float)Math.Cos(Main.timeForVisualEffects / (double)num14 * 6.2831854820251465);
                    if (num16 > 0f)
                        num16 *= -1f;

                    num16 *= num15;
                    top.Y += num16;
                    Vector2 vector5 = top - base.Center;
                    if (vector5.Length() > num13)
                        vector5 = vector5.SafeNormalize(Vector2.Zero) * num13;

                    velocity = velocity.MoveTowards(vector5, maxAmountAllowedToMove);
                    frame = 8;
                    if (flag4)
                        frame = 10;

                    rotation += 0.6f * (float)spriteDirection;
                }
                else
                {
                    ai[0] = 0f;
                    ai[1] = 0f;
                }

                return;
            }

            if (ai[0] == 1f)
            {
                tileCollide = false;
                float num17 = 0.2f;
                float num18 = 10f;
                int num19 = 200;
                if (num18 < Math.Abs(player.velocity.X) + Math.Abs(player.velocity.Y))
                    num18 = Math.Abs(player.velocity.X) + Math.Abs(player.velocity.Y);

                Vector2 vector6 = player.Center - base.Center;
                float num20 = vector6.Length();
                if (num20 > 2000f)
                    position = player.Center - new Vector2(width, height) / 2f;

                if (num20 < (float)num19 && player.velocity.Y == 0f && position.Y + (float)height <= player.position.Y + (float)player.height && !Collision.SolidCollision(position, width, height))
                {
                    ai[0] = 0f;
                    netUpdate = true;
                    if (velocity.Y < -6f)
                        velocity.Y = -6f;
                }

                if (!(num20 < 60f))
                {
                    vector6.Normalize();
                    vector6 *= num18;
                    if (velocity.X < vector6.X)
                    {
                        velocity.X += num17;
                        if (velocity.X < 0f)
                            velocity.X += num17 * 1.5f;
                    }

                    if (velocity.X > vector6.X)
                    {
                        velocity.X -= num17;
                        if (velocity.X > 0f)
                            velocity.X -= num17 * 1.5f;
                    }

                    if (velocity.Y < vector6.Y)
                    {
                        velocity.Y += num17;
                        if (velocity.Y < 0f)
                            velocity.Y += num17 * 1.5f;
                    }

                    if (velocity.Y > vector6.Y)
                    {
                        velocity.Y -= num17;
                        if (velocity.Y > 0f)
                            velocity.Y -= num17 * 1.5f;
                    }
                }

                if (velocity.X != 0f)
                    spriteDirection = Math.Sign(velocity.X);

                if (flag5)
                {
                    frameCounter++;
                    if (frameCounter > 3)
                    {
                        frame++;
                        frameCounter = 0;
                    }

                    if (frame < 2 || frame >= Main.projFrames[type])
                        frame = 2;

                    rotation = rotation.AngleTowards(rotation + 0.25f * (float)spriteDirection, 0.25f);
                }

                if (flag)
                {
                    frameCounter++;
                    if (frameCounter > 3)
                    {
                        frame++;
                        frameCounter = 0;
                    }

                    if ((frame < 10) | (frame > 13))
                        frame = 10;

                    rotation = velocity.X * 0.1f;
                }

                if (flag2)
                {
                    int num21 = 3;
                    if (++frameCounter >= num21 * 4)
                        frameCounter = 0;

                    frame = 14 + frameCounter / num21;
                    rotation = velocity.X * 0.15f;
                }

                if (flag3)
                {
                    frame = 8;
                    if (flag4)
                        frame = 10;

                    rotation += 0.6f * (float)spriteDirection;
                }

                if (type == 1018 && Main.LocalPlayer.miscCounter % 3 == 0)
                {
                    int num22 = 2;
                    Dust obj2 = Main.dust[Dust.NewDust(position + new Vector2(-num22, -num22), 16 + num22 * 2, 16 + num22 * 2, 0, 0f, 0f, 0, default(Color), 0.8f)];
                    obj2.velocity = -velocity * 0.25f;
                    obj2.velocity = obj2.velocity.RotatedByRandom(0.2617993950843811);
                }
            }

            if (ai[0] == 2f && ai[1] < 0f)
            {
                friendly = false;
                ai[1] += 1f;
                if (num9 >= 0)
                {
                    ai[1] = 0f;
                    ai[0] = 0f;
                    netUpdate = true;
                    return;
                }
            }
            else if (ai[0] == 2f)
            {
                spriteDirection = direction;
                rotation = 0f;
                if (flag)
                {
                    friendly = true;
                    frame = 4 + (int)((float)num9 - ai[1]) / (num9 / 3);
                    if (velocity.Y != 0f)
                        frame += 3;
                }

                if (flag2)
                {
                    float num23 = ((float)num9 - ai[1]) / (float)num9;
                    if ((double)num23 > 0.25 && (double)num23 < 0.75)
                        friendly = true;

                    int num24 = (int)(num23 * 5f);
                    if (num24 > 2)
                        num24 = 4 - num24;

                    if (velocity.Y != 0f)
                        frame = 21 + num24;
                    else
                        frame = 18 + num24;

                    if (velocity.Y == 0f)
                        velocity.X *= 0.8f;
                }

                velocity.Y += 0.4f;
                if (velocity.Y > 10f)
                    velocity.Y = 10f;

                ai[1] -= 1f;
                if (ai[1] <= 0f)
                {
                    if (num8 <= 0)
                    {
                        ai[1] = 0f;
                        ai[0] = 0f;
                        netUpdate = true;
                        return;
                    }

                    ai[1] = -num8;
                }
            }

            if (attackTarget >= 0)
            {
                float maxDistance2 = num;
                float num25 = 20f;
                if (flag2)
                    num25 = 50f;

                NPC nPC2 = Main.npc[attackTarget];
                Vector2 center = nPC2.Center;
                vector = center;
                if (IsInRangeOfMeOrMyOwner(nPC2, maxDistance2, out var _, out var _, out var _))
                {
                    shouldFallThrough = nPC2.Center.Y > base.Bottom.Y;
                    bool flag12 = velocity.Y == 0f;
                    if (wet && velocity.Y > 0f && !shouldFallThrough)
                        flag12 = true;

                    if (center.Y < base.Center.Y - 30f && flag12)
                    {
                        float num26 = (center.Y - base.Center.Y) * -1f;
                        float num27 = 0.4f;
                        float num28 = (float)Math.Sqrt(num26 * 2f * num27);
                        if (num28 > 26f)
                            num28 = 26f;

                        velocity.Y = 0f - num28;
                    }

                    if (flag9 && Vector2.Distance(base.Center, vector) < num25)
                    {
                        if (velocity.Length() > 10f)
                            velocity /= velocity.Length() / 10f;

                        ai[0] = 2f;
                        ai[1] = num9;
                        netUpdate = true;
                        direction = ((center.X - base.Center.X > 0f) ? 1 : (-1));
                    }

                    if (flag3)
                    {
                        Point point2 = nPC2.Top.ToTileCoordinates();
                        int num29 = 0;
                        int num30 = point2.Y;
                        while (num29 < num4)
                        {
                            Tile tile2 = Main.tile[point2.X, num30];
                            if (tile2 == null || tile2.active())
                                break;

                            num29++;
                            num30++;
                        }

                        if (num29 >= num4)
                        {
                            ai[0] = 5f;
                            ai[1] = 0f;
                            netUpdate = true;
                            return;
                        }

                        if (base.Hitbox.Intersects(nPC2.Hitbox) && velocity.Y >= 0f)
                        {
                            velocity.Y = -4f;
                            velocity.X = direction * 10;
                        }
                    }
                }

                if (flag2)
                {
                    int num31 = 1;
                    if (center.X - base.Center.X < 0f)
                        num31 = -1;

                    vector.X += 20 * -num31;
                }
            }

            if (ai[0] == 0f && attackTarget < 0)
            {
                if (Main.player[owner].rocketDelay2 > 0)
                {
                    ai[0] = 1f;
                    netUpdate = true;
                }

                Vector2 vector7 = player.Center - base.Center;
                if (vector7.Length() > 2000f)
                {
                    position = player.Center - new Vector2(width, height) / 2f;
                }
                else if (vector7.Length() > num2 || Math.Abs(vector7.Y) > num3)
                {
                    ai[0] = 1f;
                    netUpdate = true;
                    if (velocity.Y > 0f && vector7.Y < 0f)
                        velocity.Y = 0f;

                    if (velocity.Y < 0f && vector7.Y > 0f)
                        velocity.Y = 0f;
                }
            }

            if (ai[0] == 0f)
            {
                if (attackTarget < 0)
                {
                    if (Distance(player.Center) > 60f && Distance(vector) > 60f && Math.Sign(vector.X - player.Center.X) != Math.Sign(base.Center.X - player.Center.X))
                        vector = player.Center;

                    Rectangle r = Utils.CenteredRectangle(vector, base.Size);
                    for (int i = 0; i < 20; i++)
                    {
                        if (Collision.SolidCollision(r.TopLeft(), r.Width, r.Height))
                            break;

                        r.Y += 16;
                        vector.Y += 16f;
                    }

                    Vector2 vector8 = Collision.TileCollision(player.Center - base.Size / 2f, vector - player.Center, width, height);
                    vector = player.Center - base.Size / 2f + vector8;
                    if (Distance(vector) < 32f)
                    {
                        float num32 = player.Center.Distance(vector);
                        if (player.Center.Distance(base.Center) < num32)
                            vector = base.Center;
                    }

                    Vector2 vector9 = player.Center - vector;
                    if (vector9.Length() > num2 || Math.Abs(vector9.Y) > num3)
                    {
                        Rectangle r2 = Utils.CenteredRectangle(player.Center, base.Size);
                        Vector2 vector10 = vector - player.Center;
                        Vector2 vector11 = r2.TopLeft();
                        for (float num33 = 0f; num33 < 1f; num33 += 0.05f)
                        {
                            Vector2 vector12 = r2.TopLeft() + vector10 * num33;
                            if (Collision.SolidCollision(r2.TopLeft() + vector10 * num33, r.Width, r.Height))
                                break;

                            vector11 = vector12;
                        }

                        vector = vector11 + base.Size / 2f;
                    }
                }

                tileCollide = true;
                float num34 = 0.5f;
                float num35 = 4f;
                float num36 = 4f;
                float num37 = 0.1f;
                if (flag5 && attackTarget != -1)
                {
                    num34 = 0.65f;
                    num35 = 5.5f;
                    num36 = 5.5f;
                }

                if (flag && attackTarget != -1)
                {
                    num34 = 1f;
                    num35 = 8f;
                    num36 = 8f;
                }

                if (flag2 && attackTarget != -1)
                {
                    num34 = 0.7f;
                    num35 = 6f;
                    num36 = 6f;
                }

                if (flag3 && attackTarget != -1)
                {
                    num34 = 1f;
                    num35 = 8f;
                    num36 = 8f;
                }

                if (num36 < Math.Abs(player.velocity.X) + Math.Abs(player.velocity.Y))
                {
                    num36 = Math.Abs(player.velocity.X) + Math.Abs(player.velocity.Y);
                    num34 = 0.7f;
                }

                if (type == 653 || type == 1018)
                {
                    float num38 = player.velocity.Length();
                    if (num38 < 0.1f)
                        num38 = 0f;

                    if (num38 != 0f && num38 < num36)
                        num36 = num38;
                }

                int num39 = 0;
                bool flag13 = false;
                float num40 = vector.X - base.Center.X;
                Vector2 vector13 = vector - base.Center;
                if (type == 1018 && Math.Abs(num40) < 50f)
                {
                    rotation = rotation.AngleTowards(0f, 0.2f);
                    velocity.X *= 0.9f;
                    if ((double)Math.Abs(velocity.X) < 0.1)
                        velocity.X = 0f;
                }
                else if (type == 653 && Math.Abs(num40) < 150f)
                {
                    rotation = rotation.AngleTowards(0f, 0.2f);
                    velocity.X *= 0.9f;
                    if ((double)Math.Abs(velocity.X) < 0.1)
                        velocity.X = 0f;
                }
                else if (Math.Abs(num40) > 5f)
                {
                    if (num40 < 0f)
                    {
                        num39 = -1;
                        if (velocity.X > 0f - num35)
                            velocity.X -= num34;
                        else
                            velocity.X -= num37;
                    }
                    else
                    {
                        num39 = 1;
                        if (velocity.X < num35)
                            velocity.X += num34;
                        else
                            velocity.X += num37;
                    }

                    bool flag14 = true;
                    if (flag)
                        flag14 = false;

                    if (type == 653)
                        flag14 = false;

                    if (type == 1018)
                        flag14 = false;

                    if (flag2 && attackTarget == -1)
                        flag14 = false;

                    if (flag3)
                        flag14 = vector13.Y < -80f;

                    if (flag5)
                        flag14 = attackTarget > -1 && Main.npc[attackTarget].Hitbox.Intersects(base.Hitbox);

                    if (flag14)
                        flag13 = true;
                }
                else
                {
                    velocity.X *= 0.9f;
                    if (Math.Abs(velocity.X) < num34 * 2f)
                        velocity.X = 0f;
                }

                bool flag15 = Math.Abs(vector13.X) >= 64f || (vector13.Y <= -48f && Math.Abs(vector13.X) >= 8f);
                if (num39 != 0 && flag15)
                {
                    int num41 = (int)(position.X + (float)(width / 2)) / 16;
                    int num42 = (int)position.Y / 16;
                    num41 += num39;
                    num41 += (int)velocity.X;
                    for (int j = num42; j < num42 + height / 16 + 1; j++)
                    {
                        if (WorldGen.SolidTile(num41, j))
                            flag13 = true;
                    }
                }

                if (type == 500 && velocity.X != 0f)
                    flag13 = true;

                if (type == 653 && Math.Abs(velocity.X) > 3f)
                    flag13 = true;

                if (type == 1018 && Math.Abs(velocity.X) > 3f)
                    flag13 = true;

                Collision.StepUp(ref position, ref velocity, width, height, ref stepSpeed, ref gfxOffY);
                float num43 = Utils.GetLerpValue(0f, 100f, vector13.Y, clamped: true) * Utils.GetLerpValue(-2f, -6f, velocity.Y, clamped: true);
                if (velocity.Y == 0f)
                {
                    if (flag13)
                    {
                        for (int k = 0; k < 3; k++)
                        {
                            int num44 = (int)(position.X + (float)(width / 2)) / 16;
                            if (k == 0)
                                num44 = (int)position.X / 16;

                            if (k == 2)
                                num44 = (int)(position.X + (float)width) / 16;

                            int num45 = (int)(position.Y + (float)height) / 16;
                            if (!WorldGen.SolidTile(num44, num45) && !Main.tile[num44, num45].halfBrick() && Main.tile[num44, num45].slope() <= 0 && (!TileID.Sets.Platforms[Main.tile[num44, num45].type] || !Main.tile[num44, num45].active() || Main.tile[num44, num45].inActive()))
                                continue;

                            try
                            {
                                num44 = (int)(position.X + (float)(width / 2)) / 16;
                                num45 = (int)(position.Y + (float)(height / 2)) / 16;
                                num44 += num39;
                                num44 += (int)velocity.X;
                                if (!WorldGen.SolidTile(num44, num45 - 1) && !WorldGen.SolidTile(num44, num45 - 2))
                                    velocity.Y = -5.1f;
                                else if (!WorldGen.SolidTile(num44, num45 - 2))
                                    velocity.Y = -7.1f;
                                else if (WorldGen.SolidTile(num44, num45 - 5))
                                    velocity.Y = -11.1f;
                                else if (WorldGen.SolidTile(num44, num45 - 4))
                                    velocity.Y = -10.1f;
                                else
                                    velocity.Y = -9.1f;
                            }
                            catch
                            {
                                velocity.Y = -9.1f;
                            }
                        }

                        if (vector.Y - base.Center.Y < -48f)
                        {
                            float num46 = vector.Y - base.Center.Y;
                            num46 *= -1f;
                            if (num46 < 60f)
                                velocity.Y = -6f;
                            else if (num46 < 80f)
                                velocity.Y = -7f;
                            else if (num46 < 100f)
                                velocity.Y = -8f;
                            else if (num46 < 120f)
                                velocity.Y = -9f;
                            else if (num46 < 140f)
                                velocity.Y = -10f;
                            else if (num46 < 160f)
                                velocity.Y = -11f;
                            else if (num46 < 190f)
                                velocity.Y = -12f;
                            else if (num46 < 210f)
                                velocity.Y = -13f;
                            else if (num46 < 270f)
                                velocity.Y = -14f;
                            else if (num46 < 310f)
                                velocity.Y = -15f;
                            else
                                velocity.Y = -16f;
                        }

                        if (wet && num43 == 0f)
                            velocity.Y *= 2f;
                    }

                    if (type == 1018 && localAI[2] == 0f)
                    {
                        localAI[2] = 1f;
                        for (int l = 0; l < 6; l++)
                        {
                            Dust obj4 = Main.dust[Dust.NewDust(position + velocity, 16, 16, 0, 0f, 0f, 0, default(Color), 0.8f)];
                            obj4.velocity.X = velocity.X * 0.25f;
                            obj4.velocity.Y = -2f + Math.Abs(velocity.Y) * 0.25f;
                            obj4.velocity = obj4.velocity.RotatedByRandom(0.2617993950843811);
                        }
                    }
                }
                else if (type == 1018)
                {
                    localAI[2] = 0f;
                }

                if (velocity.X > num36)
                    velocity.X = num36;

                if (velocity.X < 0f - num36)
                    velocity.X = 0f - num36;

                if (velocity.X < 0f)
                    direction = -1;

                if (velocity.X > 0f)
                    direction = 1;

                if (velocity.X == 0f)
                    direction = ((player.Center.X > base.Center.X) ? 1 : (-1));

                if (velocity.X > num34 && num39 == 1)
                    direction = 1;

                if (velocity.X < 0f - num34 && num39 == -1)
                    direction = -1;

                spriteDirection = direction;
                if (flag5)
                {
                    if (velocity.Y == 0f)
                    {
                        rotation = rotation.AngleTowards(0f, 0.3f);
                        if (velocity.X == 0f)
                        {
                            frame = 0;
                            frameCounter = 0;
                        }
                        else if (Math.Abs(velocity.X) >= 0.5f)
                        {
                            frameCounter += (int)Math.Abs(velocity.X);
                            frameCounter++;
                            if (frameCounter > 10)
                            {
                                frame++;
                                frameCounter = 0;
                            }

                            if (frame < 2 || frame >= Main.projFrames[type])
                                frame = 2;
                        }
                        else
                        {
                            frame = 0;
                            frameCounter = 0;
                        }
                    }
                    else if (velocity.Y != 0f)
                    {
                        rotation = Math.Min(4f, velocity.Y) * -0.1f;
                        if (spriteDirection == -1)
                            rotation -= (float)Math.PI * 2f;

                        frameCounter = 0;
                        frame = 1;
                    }
                }

                if (flag)
                {
                    rotation = 0f;
                    if (velocity.Y == 0f)
                    {
                        if (velocity.X == 0f)
                        {
                            frame = 0;
                            frameCounter = 0;
                        }
                        else if (Math.Abs(velocity.X) >= 0.5f)
                        {
                            frameCounter += (int)Math.Abs(velocity.X);
                            frameCounter++;
                            if (frameCounter > 10)
                            {
                                frame++;
                                frameCounter = 0;
                            }

                            if (frame >= 4)
                                frame = 0;
                        }
                        else
                        {
                            frame = 0;
                            frameCounter = 0;
                        }
                    }
                    else if (velocity.Y != 0f)
                    {
                        frameCounter = 0;
                        frame = 14;
                    }
                }

                if (flag2)
                {
                    rotation = 0f;
                    if (velocity.Y == 0f)
                    {
                        if (velocity.X == 0f)
                        {
                            int num47 = 4;
                            if (++frameCounter >= 7 * num47 && Main.rand.Next(50) == 0)
                                frameCounter = 0;

                            int num48 = frameCounter / num47;
                            if (num48 >= 4)
                                num48 = 6 - num48;

                            if (num48 < 0)
                                num48 = 0;

                            frame = 1 + num48;
                        }
                        else if (Math.Abs(velocity.X) >= 0.5f)
                        {
                            frameCounter += (int)Math.Abs(velocity.X);
                            frameCounter++;
                            int num49 = 15;
                            int num50 = 8;
                            if (frameCounter >= num50 * num49)
                                frameCounter = 0;

                            int num51 = frameCounter / num49;
                            frame = num51 + 5;
                        }
                        else
                        {
                            frame = 0;
                            frameCounter = 0;
                        }
                    }
                    else if (velocity.Y != 0f)
                    {
                        if (velocity.Y < 0f)
                        {
                            if (frame > 9 || frame < 5)
                            {
                                frame = 5;
                                frameCounter = 0;
                            }

                            if (++frameCounter >= 1 && frame < 9)
                            {
                                frame++;
                                frameCounter = 0;
                            }
                        }
                        else
                        {
                            if (frame > 13 || frame < 9)
                            {
                                frame = 9;
                                frameCounter = 0;
                            }

                            if (++frameCounter >= 2 && frame < 11)
                            {
                                frame++;
                                frameCounter = 0;
                            }
                        }
                    }
                }

                if (flag3)
                {
                    int num52 = 8;
                    if (flag4)
                        num52 = 10;

                    rotation = 0f;
                    if (velocity.Y == 0f)
                    {
                        if (velocity.X == 0f)
                        {
                            frame = 0;
                            frameCounter = 0;
                        }
                        else if (Math.Abs(velocity.X) >= 0.5f)
                        {
                            frameCounter += (int)Math.Abs(velocity.X);
                            frameCounter++;
                            if (frameCounter > 10)
                            {
                                frame++;
                                frameCounter = 0;
                            }

                            if (frame >= num52 || frame < 2)
                                frame = 2;
                        }
                        else
                        {
                            frame = 0;
                            frameCounter = 0;
                        }
                    }
                    else if (velocity.Y != 0f)
                    {
                        frameCounter = 0;
                        frame = 1;
                        if (flag4)
                            frame = 9;
                    }
                }

                velocity.Y += 0.4f + num43 * 1f;
                if (velocity.Y > 10f)
                    velocity.Y = 10f;
            }

            if (!flag)
                return;

            localAI[0] += 1f;
            if (velocity.X == 0f)
                localAI[0] += 1f;

            if (localAI[0] >= (float)Main.rand.Next(900, 1200))
            {
                localAI[0] = 0f;
                for (int m = 0; m < 6; m++)
                {
                    int num53 = Dust.NewDust(base.Center + Vector2.UnitX * -direction * 8f - Vector2.One * 5f + Vector2.UnitY * 8f, 3, 6, 216, -direction, 1f);
                    Main.dust[num53].velocity /= 2f;
                    Main.dust[num53].scale = 0.8f;
                }

                int num54 = Gore.NewGore(base.Center + Vector2.UnitX * -direction * 8f, Vector2.Zero, Main.rand.Next(580, 583));
                Main.gore[num54].velocity /= 2f;
                Main.gore[num54].velocity.Y = Math.Abs(Main.gore[num54].velocity.Y);
                Main.gore[num54].velocity.X = (0f - Math.Abs(Main.gore[num54].velocity.X)) * (float)direction;
            }
        }

    }
}
