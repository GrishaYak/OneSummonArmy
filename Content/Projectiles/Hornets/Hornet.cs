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
        void Move(Vector2 direction, float speed, float inertia)
        {
            Projectile.velocity = (Projectile.velocity * inertia + direction * speed) / (inertia + 1f);
        }
        int FindTarget(ref float range, ref Vector2 targetPos)
        {
            int target = -1;
            Projectile.Minion_FindTargetInRange((int) range, ref target, skipIfCannotHitWithOwnBody: false);
            if (target != -1)
            {
                targetPos = Main.npc[target].Center;
                range = Projectile.Distance(targetPos);
            }
            return target;
        }
        void Visuals(int counterMax = 1)
        {
            Projectile.frameCounter++;

            if (Projectile.frameCounter > counterMax)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
                if (Projectile.frame >= Main.projFrames[Projectile.type])
                {
                    Projectile.frame = 0;
                }
            }

        }
        public override void AI()
        {
            // Projectile.ai[1] is used to support frequency of shooting
            float num = 40f;
            Player player = Main.player[Projectile.owner];
            CheckActive(player);
            float speed = 6f;
            float inertia = 40f;
            Vector2 enemyPos = Projectile.position;
            float targetRange = 2000f;
            int target = FindTarget(ref targetRange, ref enemyPos); 
            bool foundTarget = target != -1;
            Projectile.tileCollide = true;
            if (Vector2.Distance(player.Center, Projectile.Center) > 500)
            {
                Projectile.ai[0] = 1f;
                Projectile.netUpdate = true;
            }
            if (Projectile.ai[0] == 1f)
            {
                Projectile.tileCollide = false;
            }

            if (Projectile.ai[0] >= 2f)
            {
                Projectile.ai[0] += 1f;

                if (target == -1)
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
            else if (target != -1 && Projectile.ai[0] == 0f)
            {
                Vector2 direction = enemyPos - Projectile.Center;
                float enemyDistance = direction.Length();
                direction = direction.SafeNormalize(Vector2.Zero);
                if (enemyDistance > 200f)
                {
                    Move(direction, speed, inertia);
                }
                else if (Projectile.velocity.Y > -1f)
                {
                    Projectile.velocity.Y -= 0.1f;
                }
            }
            else
            {
                if (!Collision.CanHitLine(Projectile.Center, 1, 1, Main.player[Projectile.owner].Center, 1, 1))
                {
                    Projectile.ai[0] = 1f;
                }
                if (Projectile.ai[0] == 1f)
                {
                    speed = 15f;
                }
                Vector2 toIdlePos = player.Center - Projectile.Center + new Vector2(0f, -60f);


                float distanceToHome = toIdlePos.Length();
                if (distanceToHome > 200f && speed < 9f)
                {
                    speed = 9f;
                }
                if (distanceToHome < 100f && Projectile.ai[0] == 1f && !Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
                {
                    Projectile.ai[0] = 0f;
                    Projectile.netUpdate = true;
                }
                if (distanceToHome > 2000f)
                {
                    Projectile.Center = player.Center;
                }
                if (distanceToHome > 70f)
                {
                    Move(toIdlePos.SafeNormalize(Vector2.Zero), speed, 20);
                }
                else
                {
                    if (Projectile.velocity == Vector2.Zero)
                    {
                        Projectile.velocity.X = -0.15f;
                        Projectile.velocity.Y = -0.05f;
                    }
                    Projectile.velocity *= 1.01f;
                }

            }
            Projectile.rotation = Projectile.velocity.X * 0.05f;
            Visuals();
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
            if (Projectile.ai[0] != 0f || !foundTarget)
            {
                return;
            }
            speed = 10f;
            int type = 374;
            if (Projectile.ai[1] == 0f)
            {
                Vector2 toEnemy = enemyPos - Projectile.Center;
                Projectile.ai[1] += 1f;
                if (Main.myPlayer == Projectile.owner)
                {
                    toEnemy = toEnemy.SafeNormalize(Vector2.Zero);
                    toEnemy *= speed;
                    var shot = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, toEnemy * speed, type, Projectile.damage, Projectile.knockBack, Main.myPlayer);
                    shot.timeLeft = 300;
                    shot.netUpdate = true;
                    Projectile.netUpdate = true;
                }
            }
            

        }
    }
}
