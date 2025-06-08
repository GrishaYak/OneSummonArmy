using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using OneSummonArmy.Content.Buffs;

namespace OneSummonArmy.Content.Projectiles.Hornets
{
    public abstract class Hornet : ModProjectile
    {
        protected virtual void AdditionalStaticDefaults() { }
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 3;
            Main.projPet[Projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
            AdditionalStaticDefaults();
        }
        protected virtual void AdditionalDefaults() { }
        public override void SetDefaults()
        {
            Projectile.netImportant = true;
            Projectile.width = 24;
            Projectile.height = 26;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 2;
            Projectile.minion = true;
            Projectile.minionSlots = 0f;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            AdditionalDefaults();
        }
        bool CheckActive(Player player)
        {
            if (player.dead || !player.active)
            {
                player.ClearBuff(ModContent.BuffType<HornetBuff>());
                return false;
            }
            if (player.HasBuff<HornetBuff>())
            {
                Projectile.timeLeft = 2;
            }
            return true;
        }
        public override void AI()
        {
            {
                float num = 40f;
                Player player = Main.player[Projectile.owner];
                CheckActive(player);

                Vector2 myPos = Projectile.position;
                float num12 = 2000f;
                bool flag = false;
                int num13 = -1;
                Projectile.tileCollide = true;
                NPC enemy = Projectile.OwnerMinionAttackTargetNPC;
                if (enemy != null && enemy.CanBeChasedBy(Projectile))
                {
                    float num17 = Vector2.Distance(enemy.Center, Projectile.Center);
                    float num18 = num12 * 3f;
                    if (num17 < num18 && !flag && ((Projectile.type != 963) ? Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, enemy.position, enemy.width, enemy.height) : Collision.CanHit(Projectile.Center, 1, 1, enemy.Center, 1, 1)))
                    {
                        num12 = num17;
                        myPos = enemy.Center;
                        flag = true;
                        num13 = enemy.whoAmI;
                    }
                }
                if (!flag)
                {
                    for (int num19 = 0; num19 < 200; num19++)
                    {
                        NPC nPC2 = Main.npc[num19];
                        if (nPC2.CanBeChasedBy(Projectile))
                        {
                            float num20 = Vector2.Distance(nPC2.Center, Projectile.Center);
                            if (!(num20 >= num12) && ((Projectile.type != 963) ? Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, nPC2.position, nPC2.width, nPC2.height) : Collision.CanHit(Projectile.Center, 1, 1, nPC2.Center, 1, 1)))
                            {
                                num12 = num20;
                                myPos = nPC2.Center;
                                flag = true;
                                num13 = num19;
                            }
                        }
                    }
                }
                
                int num21 = 500;
                if (Vector2.Distance(player.Center, Projectile.Center) > (float)num21)
                {
                    Projectile.ai[0] = 1f;
                    Projectile.netUpdate = true;
                }
                if (Projectile.ai[0] == 1f)
                {
                    Projectile.tileCollide = false;
                }
                bool flag4 = false;
            
                bool flag5 = false;
                if (Projectile.ai[0] >= 2f)
                {
                    if (Projectile.ai[0] == 2f && Projectile.type == 963)
                    {
                        SoundEngine.PlaySound(in SoundID.AbigailAttack, Projectile.Center);
                    }
                    Projectile.ai[0] += 1f;
                    if (flag4)
                    {
                        Projectile.localAI[1] = Projectile.ai[0] / num;
                    }
                    if (!flag)
                    {
                        Projectile.ai[0] += 1f;
                    }
                    if (Projectile.ai[0] > num)
                    {
                        Projectile.ai[0] = 0f;
                        Projectile.netUpdate = true;
                    }
                    Projectile.velocity *= 0.69f;
                }
                else if (flag && (flag5 || Projectile.ai[0] == 0f))
                {
                    Vector2 v = myPos - Projectile.Center;
                    float num22 = v.Length();
                    v = v.SafeNormalize(Vector2.Zero);
                    if (num22 > 200f)
                    {
                        float num26 = 6f;
                        v *= num26;
                        float num27 = 40f;
                        Projectile.velocity.X = (Projectile.velocity.X * num27 + v.X) / (num27 + 1f);
                        Projectile.velocity.Y = (Projectile.velocity.Y * num27 + v.Y) / (num27 + 1f);
                    }
                    else if (Projectile.velocity.Y > -1f)
                    {
                        Projectile.velocity.Y -= 0.1f;
                    }
                }
                else
                {
                    if (Projectile.type != 963 && !Collision.CanHitLine(Projectile.Center, 1, 1, Main.player[Projectile.owner].Center, 1, 1))
                    {
                        Projectile.ai[0] = 1f;
                    }
                    float num31 = 6f;
                    if (Projectile.ai[0] == 1f)
                    {
                        num31 = 15f;
                    }
                 
                    Vector2 center2 = Projectile.Center;
                    Vector2 v2 = player.Center - center2 + new Vector2(0f, -60f);
                    
                    
                    float num34 = v2.Length();
                    if (num34 > 200f && num31 < 9f)
                    {
                        num31 = 9f;
                    }
                    if (num34 < 100f && Projectile.ai[0] == 1f && !Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
                    {
                        Projectile.ai[0] = 0f;
                        Projectile.netUpdate = true;
                    }
                    if (num34 > 2000f)
                    {
                        Projectile.position.X = Main.player[Projectile.owner].Center.X - (float)(Projectile.width / 2);
                        Projectile.position.Y = Main.player[Projectile.owner].Center.Y - (float)(Projectile.width / 2);
                    }
                    if (num34 > 70f)
                    {
                        v2 = v2.SafeNormalize(Vector2.Zero);
                        v2 *= num31;
                        Projectile.velocity = (Projectile.velocity * 20f + v2) / 21f;
                    }
                    else
                    {
                        if (Projectile.velocity.X == 0f && Projectile.velocity.Y == 0f)
                        {
                            Projectile.velocity.X = -0.15f;
                            Projectile.velocity.Y = -0.05f;
                        }
                        Projectile.velocity *= 1.01f;
                    }
                   
                }
                Projectile.rotation = Projectile.velocity.X * 0.05f;
                Projectile.frameCounter++;

                if (Projectile.frameCounter > 1)
                {
                    Projectile.frame++;
                    Projectile.frameCounter = 0;
                }
                if (Projectile.frame > 2)
                {
                    Projectile.frame = 0;
                }
                if (Projectile.velocity.X > 0f)
                {
                    Projectile.spriteDirection = (Projectile.direction = -1);
                }
                else if (Projectile.velocity.X < 0f)
                {
                    Projectile.spriteDirection = (Projectile.direction = 1);
                }
                if (Projectile.ai[1] > 0f)
                {
                    Projectile.ai[1] += Main.rand.Next(1, 4);
                }
                int num44 = 90;
                if (Main.player[Projectile.owner].strongBees)
                {
                    num44 = 70;
                }
                if (Projectile.ai[1] > (float)num44)
                {
                    Projectile.ai[1] = 0f;
                    Projectile.netUpdate = true;
                }
                if (!flag5 && Projectile.ai[0] != 0f)
                {
                    return;
                }
                float num45 = 0f;
                int num46 = 0;

                num45 = 10f;
                num46 = 374;
                if (!flag)
                {
                    return;
                }
                if (Projectile.ai[1] == 0f)
                {
                    Vector2 v5 = myPos - Projectile.Center;
                    Projectile.ai[1] += 1f;
                    if (Main.myPlayer == Projectile.owner)
                    {
                        v5 = v5.SafeNormalize(Vector2.Zero);
                        v5 *= num45;
                        int num51 = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, v5.X, v5.Y, num46, Projectile.damage, Projectile.knockBack, Main.myPlayer);
                        Main.projectile[num51].timeLeft = 300;
                        Main.projectile[num51].netUpdate = true;
                        Projectile.netUpdate = true;
                    }
                }
            }
        }
    }
}
