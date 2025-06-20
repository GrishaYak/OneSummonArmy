using Microsoft.Xna.Framework;
using OneSummonArmy.Content.Buffs;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static OneSummonArmy.Func;

namespace OneSummonArmy.Content.Projectiles.Flinxes
{
    public abstract class Flinx : StandardProjectile
    {
        protected override string TypeString => "Flinx";
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
        public override bool MinionContactDamage()
        {
            return true;
        }
        public override bool? CanCutTiles()
        {
            return false;
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (!player.active)
            {
                Projectile.active = false;
                return;
            }
            float num43 = 0f;
            float maxMod = 500f;
            float maxDif = 300f;
            Basics(player, player.ownedProjectileCounts[ModContent.ProjectileType<FlinxCounter>()]);
            Vector2 idlePos = player.Center;

            idlePos.X -= (45 + player.width / 2) * player.direction;
            idlePos.X -= Projectile.minionPos * 30 * player.direction;

            
            Projectile.shouldFallThrough = player.position.Y + (float)player.height - 12f > Projectile.position.Y + (float)Projectile.height;
            int attackTarget = -1;

            Projectile.friendly = true;
            
            float playerDistance;
            float myDistance;
            bool closerIsMe;
            if (Projectile.ai[0] == 1f)
            {
                Projectile.tileCollide = false;
                float num17 = 0.2f;
                float num18 = Max(10f, Math.Abs(player.velocity.X) + Math.Abs(player.velocity.Y));
                int num19 = 200;
                Vector2 toPlayer = player.Center - Projectile.Center;
                float num20 = toPlayer.Length();
                if (num20 > 2000f)
                {
                    Projectile.position = player.Center - new Vector2(Projectile.width, Projectile.height) / 2f;
                }
                if (num20 < (float)num19 && player.velocity.Y == 0f && Projectile.position.Y + (float)Projectile.height <= player.position.Y + (float)player.height && !Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
                {
                    Projectile.ai[0] = 0f;
                    Projectile.netUpdate = true;
                    if (Projectile.velocity.Y < -6f)
                    {
                        Projectile.velocity.Y = -6f;
                    }
                }
                if (!(num20 < 60f))
                {
                    toPlayer.Normalize();
                    toPlayer *= num18;
                    if (Projectile.velocity.X < toPlayer.X)
                    {
                        Projectile.velocity.X += num17;
                        if (Projectile.velocity.X < 0f)
                        {
                            Projectile.velocity.X += num17 * 1.5f;
                        }
                    }
                    if (Projectile.velocity.X > toPlayer.X)
                    {
                        Projectile.velocity.X -= num17;
                        if (Projectile.velocity.X > 0f)
                        {
                            Projectile.velocity.X -= num17 * 1.5f;
                        }
                    }
                    if (Projectile.velocity.Y < toPlayer.Y)
                    {
                        Projectile.velocity.Y += num17;
                        if (Projectile.velocity.Y < 0f)
                        {
                            Projectile.velocity.Y += num17 * 1.5f;
                        }
                    }
                    if (Projectile.velocity.Y > toPlayer.Y)
                    {
                        Projectile.velocity.Y -= num17;
                        if (Projectile.velocity.Y > 0f)
                        {
                            Projectile.velocity.Y -= num17 * 1.5f;
                        }
                    }
                }
                if (Projectile.velocity.X != 0f)
                {
                    Projectile.spriteDirection = Math.Sign(Projectile.velocity.X);
                }
                Projectile.frameCounter++;
                if (Projectile.frameCounter > 3)
                {
                    Projectile.frame++;
                    Projectile.frameCounter = 0;
                }
                if (Projectile.frame < 2 || Projectile.frame >= Main.projFrames[Projectile.type])
                {
                    Projectile.frame = 2;
                }
                Projectile.rotation = Projectile.rotation.AngleTowards(Projectile.rotation + 0.25f * (float)Projectile.spriteDirection, 0.25f);

            }
            if (Projectile.ai[0] == 2f && Projectile.ai[1] < 0f)
            {
                Projectile.friendly = false;
                Projectile.ai[1] = 0f;
                Projectile.ai[0] = 0f;
                Projectile.netUpdate = true;
                return;
                
            }
            else if (Projectile.ai[0] == 2f)
            {
                Projectile.spriteDirection = Projectile.direction;
                Projectile.rotation = 0f;
                Projectile.velocity.Y += 0.4f;
                if (Projectile.velocity.Y > 10f)
                {
                    Projectile.velocity.Y = 10f;
                }
                Projectile.ai[1] -= 1f;
                if (Projectile.ai[1] <= 0f)
                {
                    
                    Projectile.ai[1] = 0f;
                    Projectile.ai[0] = 0f;
                    Projectile.netUpdate = true;
                    return;
                }
            }
            if (attackTarget >= 0)
            {
                float maxDistance2 = 800;
                float num25 = 20f;
                NPC nPC2 = Main.npc[attackTarget];
                Vector2 center = nPC2.Center;
                idlePos = center;
                if (Projectile.IsInRangeOfMeOrMyOwner(nPC2, maxDistance2, out myDistance, out playerDistance, out closerIsMe))
                {
                    Projectile.shouldFallThrough = nPC2.Center.Y > Projectile.Bottom.Y;
                    bool flag = Projectile.velocity.Y == 0f;
                    if (Projectile.wet && Projectile.velocity.Y > 0f && !Projectile.shouldFallThrough)
                    {
                        flag = true;
                    }
                    if (center.Y < Projectile.Center.Y - 30f && flag)
                    {
                        float num26 = (center.Y - Projectile.Center.Y) * -1f;
                        float num27 = 0.4f;
                        float num28 = (float)Math.Sqrt(num26 * 2f * num27);
                        if (num28 > 26f)
                        {
                            num28 = 26f;
                        }
                        Projectile.velocity.Y = 0f - num28;
                    }
                    if (Vector2.Distance(Projectile.Center, idlePos) < num25)
                    {
                        if (Projectile.velocity.Length() > 10f)
                        {
                            Projectile.velocity /= Projectile.velocity.Length() / 10f;
                        }
                        Projectile.ai[0] = 2f;
                        Projectile.ai[1] = 15;
                        Projectile.netUpdate = true;
                        Projectile.direction = ((center.X - Projectile.Center.X > 0f) ? 1 : (-1));
                    }
                }
            }
            if (Projectile.ai[0] == 0f && attackTarget < 0)
            {
                if (Main.player[Projectile.owner].rocketDelay2 > 0)
                {
                    Projectile.ai[0] = 1f;
                    Projectile.netUpdate = true;
                }
                Vector2 toPlayer = player.Center - Projectile.Center;
                if (toPlayer.Length() > 2000f)
                {
                    Projectile.position = player.Center - new Vector2(Projectile.width, Projectile.height) / 2f;
                }
                else if (toPlayer.Length() > maxMod || Math.Abs(toPlayer.Y) > maxDif)
                {
                    Projectile.ai[0] = 1f;
                    Projectile.netUpdate = true;
                    if (Projectile.velocity.Y > 0f && toPlayer.Y < 0f)
                    {
                        Projectile.velocity.Y = 0f;
                    }
                    if (Projectile.velocity.Y < 0f && toPlayer.Y > 0f)
                    {
                        Projectile.velocity.Y = 0f;
                    }
                }
            }
            if (Projectile.ai[0] == 0f)
            {
                if (attackTarget < 0)
                {
                    if (Projectile.Distance(player.Center) > 60f && Projectile.Distance(idlePos) > 60f && Math.Sign(idlePos.X - player.Center.X) != Math.Sign(Projectile.Center.X - player.Center.X))
                    {
                        idlePos = player.Center;
                    }
                    Rectangle r = Utils.CenteredRectangle(idlePos, Projectile.Size);
                    for (int i = 0; i < 20; i++)
                    {
                        if (Collision.SolidCollision(r.TopLeft(), r.Width, r.Height))
                        {
                            break;
                        }
                        r.Y += 16;
                        idlePos.Y += 16f;
                    }
                    Vector2 vector8 = Collision.TileCollision(player.Center - Projectile.Size / 2f, idlePos - player.Center, Projectile.width, Projectile.height);
                    idlePos = player.Center - Projectile.Size / 2f + vector8;
                    float distToProj = player.Center.Distance(Projectile.Center);
                    float distToIdle = player.Center.Distance(idlePos);
                    if (Projectile.Distance(idlePos) < 32f && distToProj < distToIdle)
                    {
                        idlePos = Projectile.Center;
                    }
                    Vector2 playerToIdle = player.Center - idlePos;
                    if (playerToIdle.Length() > maxMod || Math.Abs(playerToIdle.Y) > maxDif)
                    {
                        Rectangle r2 = Utils.CenteredRectangle(player.Center, Projectile.Size);
                        Vector2 vector10 = idlePos - player.Center;
                        Vector2 vector11 = r2.TopLeft();
                        for (float num33 = 0f; num33 < 1f; num33 += 0.05f)
                        {
                            Vector2 vector12 = r2.TopLeft() + vector10 * num33;
                            if (Collision.SolidCollision(r2.TopLeft() + vector10 * num33, r.Width, r.Height))
                            {
                                break;
                            }
                            vector11 = vector12;
                        }
                        idlePos = vector11 + Projectile.Size / 2f;
                    }
                }
                Projectile.tileCollide = true;
                float num34 = 0.5f;
                float num35 = 4f;
                float num36 = 4f;
                float num37 = 0.1f;
                if (attackTarget != -1)
                {
                    num34 = 0.65f;
                    num35 = 5.5f;
                    num36 = 5.5f;
                }
                if (num36 < Math.Abs(player.velocity.X) + Math.Abs(player.velocity.Y))
                {
                    num36 = Math.Abs(player.velocity.X) + Math.Abs(player.velocity.Y);
                    num34 = 0.7f;
                }
                int num39 = 0;
                bool flag5 = false;
                float num40 = idlePos.X - Projectile.Center.X;
                Vector2 vector13 = idlePos - Projectile.Center;
                if (Math.Abs(num40) > 5f)
                {
                    if (num40 < 0f)
                    {
                        num39 = -1;
                        if (Projectile.velocity.X > 0f - num35)
                        {
                            Projectile.velocity.X -= num34;
                        }
                        else
                        {
                            Projectile.velocity.X -= num37;
                        }
                    }
                    else
                    {
                        num39 = 1;
                        if (Projectile.velocity.X < num35)
                        {
                            Projectile.velocity.X += num34;
                        }
                        else
                        {
                            Projectile.velocity.X += num37;
                        }
                    }
                    flag5 = flag5 || attackTarget > -1 && Main.npc[attackTarget].Hitbox.Intersects(Projectile.Hitbox);
                }
                else
                {
                    Projectile.velocity.X *= 0.9f;
                    if (Math.Abs(Projectile.velocity.X) < num34 * 2f)
                    {
                        Projectile.velocity.X = 0f;
                    }
                }
                bool flag7 = Math.Abs(vector13.X) >= 64f || (vector13.Y <= -48f && Math.Abs(vector13.X) >= 8f);
                if (num39 != 0 && flag7)
                {
                    int num41 = (int)(Projectile.position.X + (float)(Projectile.width / 2)) / 16;
                    int num42 = (int)Projectile.position.Y / 16;
                    num41 += num39;
                    num41 += (int)Projectile.velocity.X;
                    for (int j = num42; j < num42 + Projectile.height / 16 + 1; j++)
                    {
                        if (WorldGen.SolidTile(num41, j))
                        {
                            flag5 = true;
                        }
                    }
                }
                Collision.StepUp(ref Projectile.position, ref Projectile.velocity, Projectile.width, Projectile.height, ref Projectile.stepSpeed, ref Projectile.gfxOffY);
                num43 = Utils.GetLerpValue(0f, 100f, vector13.Y, clamped: true) * Utils.GetLerpValue(-2f, -6f, Projectile.velocity.Y, clamped: true);
                if (Projectile.velocity.Y == 0f)
                {
                    if (flag5)
                    {
                        for (int k = 0; k < 3; k++)
                        {
                            int num44 = (int)(Projectile.position.X + (float)(Projectile.width / 2)) / 16;
                            if (k == 0)
                            {
                                num44 = (int)Projectile.position.X / 16;
                            }
                            if (k == 2)
                            {
                                num44 = (int)(Projectile.position.X + (float)Projectile.width) / 16;
                            }
                            int num45 = (int)(Projectile.position.Y + (float)Projectile.height) / 16;
                            if (!WorldGen.SolidTile(num44, num45) && !Main.tile[num44, num45].IsHalfBlock && Main.tile[num44, num45].Slope <= 0 && (!TileID.Sets.Platforms[Main.tile[num44, num45].TileType] || Main.tile[num44, num45].IsActuated))
                            {
                                continue;
                            }
                            try
                            {
                                num44 = (int)(Projectile.position.X + (float)(Projectile.width / 2)) / 16;
                                num45 = (int)(Projectile.position.Y + (float)(Projectile.height / 2)) / 16;
                                num44 += num39;
                                num44 += (int)Projectile.velocity.X;
                                if (!WorldGen.SolidTile(num44, num45 - 1) && !WorldGen.SolidTile(num44, num45 - 2))
                                {
                                    Projectile.velocity.Y = -5.1f;
                                }
                                else if (!WorldGen.SolidTile(num44, num45 - 2))
                                {
                                    Projectile.velocity.Y = -7.1f;
                                }
                                else if (WorldGen.SolidTile(num44, num45 - 5))
                                {
                                    Projectile.velocity.Y = -11.1f;
                                }
                                else if (WorldGen.SolidTile(num44, num45 - 4))
                                {
                                    Projectile.velocity.Y = -10.1f;
                                }
                                else
                                {
                                    Projectile.velocity.Y = -9.1f;
                                }
                            }
                            catch
                            {
                                Projectile.velocity.Y = -9.1f;
                            }
                        }
                        if (idlePos.Y - Projectile.Center.Y < -48f)
                        {
                            float num46 = idlePos.Y - Projectile.Center.Y;
                            num46 *= -1f;
                            if (num46 < 60f)
                            {
                                Projectile.velocity.Y = -6f;
                            }
                            else if (num46 < 80f)
                            {
                                Projectile.velocity.Y = -7f;
                            }
                            else if (num46 < 100f)
                            {
                                Projectile.velocity.Y = -8f;
                            }
                            else if (num46 < 120f)
                            {
                                Projectile.velocity.Y = -9f;
                            }
                            else if (num46 < 140f)
                            {
                                Projectile.velocity.Y = -10f;
                            }
                            else if (num46 < 160f)
                            {
                                Projectile.velocity.Y = -11f;
                            }
                            else if (num46 < 190f)
                            {
                                Projectile.velocity.Y = -12f;
                            }
                            else if (num46 < 210f)
                            {
                                Projectile.velocity.Y = -13f;
                            }
                            else if (num46 < 270f)
                            {
                                Projectile.velocity.Y = -14f;
                            }
                            else if (num46 < 310f)
                            {
                                Projectile.velocity.Y = -15f;
                            }
                            else
                            {
                                Projectile.velocity.Y = -16f;
                            }
                        }
                        if (Projectile.wet && num43 == 0f)
                        {
                            Projectile.velocity.Y *= 2f;
                        }
                    }
                }
                if (Projectile.velocity.X > num36)
                {
                    Projectile.velocity.X = num36;
                }
                if (Projectile.velocity.X < 0f - num36)
                {
                    Projectile.velocity.X = 0f - num36;
                }
                if (Projectile.velocity.X < 0f)
                {
                    Projectile.direction = -1;
                }
                if (Projectile.velocity.X > 0f)
                {
                    Projectile.direction = 1;
                }
                if (Projectile.velocity.X == 0f)
                {
                    Projectile.direction = ((player.Center.X > Projectile.Center.X) ? 1 : (-1));
                }
                if (Projectile.velocity.X > num34 && num39 == 1)
                {
                    Projectile.direction = 1;
                }
                if (Projectile.velocity.X < 0f - num34 && num39 == -1)
                {
                    Projectile.direction = -1;
                }
                Projectile.spriteDirection = Projectile.direction;
                
                if (Projectile.velocity.Y == 0f)
                {
                    Projectile.rotation = Projectile.rotation.AngleTowards(0f, 0.3f);
                    if (Projectile.velocity.X == 0f)
                    {
                        Projectile.frame = 0;
                        Projectile.frameCounter = 0;
                    }
                    else if (Math.Abs(Projectile.velocity.X) >= 0.5f)
                    {
                        Projectile.frameCounter += (int)Math.Abs(Projectile.velocity.X);
                        Projectile.frameCounter++;
                        if (Projectile.frameCounter > 10)
                        {
                            Projectile.frame++;
                            Projectile.frameCounter = 0;
                        }
                        if (Projectile.frame < 2 || Projectile.frame >= Main.projFrames[Projectile.type])
                        {
                            Projectile.frame = 2;
                        }
                    }
                    else
                    {
                        Projectile.frame = 0;
                        Projectile.frameCounter = 0;
                    }
                }
                else if (Projectile.velocity.Y != 0f)
                {
                    Projectile.rotation = Math.Min(4f, Projectile.velocity.Y) * -0.1f;
                    if (Projectile.spriteDirection == -1)
                    {
                        Projectile.rotation -= (float)Math.PI * 2f;
                    }
                    Projectile.frameCounter = 0;
                    Projectile.frame = 1;
                }
            }
            Projectile.velocity.Y += 0.4f + num43;
            if (Projectile.velocity.Y > 10f)
            {
                Projectile.velocity.Y = 10f;
            }
        }
    }
}
