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
            Move(direction * speed, inertia);
        }
        void Move(Vector2 velocity, float inertia)
        {
            Projectile.velocity = (Projectile.velocity * inertia + velocity) / (inertia + 1f);
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
            Projectile.rotation = Projectile.velocity.X * 0.05f;
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
            if (Projectile.velocity.X > 0f)
            {
                Projectile.spriteDirection = (Projectile.direction = -1);
            }
            else if (Projectile.velocity.X < 0f)
            {
                Projectile.spriteDirection = (Projectile.direction = 1);
            }
        }
        protected virtual void UpdateShootTimer()
        {

            Projectile.ai[1] += Main.rand.Next(1, 4);
            
            int shootTimer = 90;
            if (Main.player[Projectile.owner].strongBees)
            {
                shootTimer = 70;
            }
            if (Projectile.ai[1] > shootTimer)
            {
                Projectile.ai[1] = 0f;
                Projectile.netUpdate = true;
            }
        }
        protected void DirectProj(Vector2 direction, float speed, int type = 374, Vector2 position=new Vector2())
        {
            DirectProj(direction * speed, type, position);
        }
        protected void DirectProj(Vector2 velocity, int type = 374, Vector2 position= new Vector2())
        {
            if (Main.myPlayer == Projectile.owner)
            {
                if (position == new Vector2())
                {
                    position = Projectile.Center;
                }
                var shot = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), position, velocity, type, Projectile.damage, Projectile.knockBack, Main.myPlayer);
                shot.timeLeft = 300;
                shot.netUpdate = true;
                Projectile.netUpdate = true;
            }
        }
        protected virtual void Shoot(float newProjSpeed, Vector2 enemyPos, int type=374)
        {            
            Vector2 toEnemy = enemyPos - Projectile.Center;
            Projectile.ai[1] += 1f;
            DirectProj(toEnemy, newProjSpeed, type);
            
        }
        void GetToAttackPosition(Vector2 enemyPos, float speed, float inertia)
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
        void General(Player player, float speed)
        {
            if (!Collision.CanHitLine(Projectile.Center, 1, 1, player.Center, 1, 1))
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
        public override void AI()
        {
            // Projectile.ai[1] is used to support frequency of shooting
            // ai[0] == 1 means "should get to idlePos
            // ai[0] == 0 means "ready to attack"
            Player player = Main.player[Projectile.owner];
            if (!CheckActive(player))
            {
                return;
            }
            float speed = 6f, inertia = 40f, targetRange = 2000f;
            Vector2 enemyPos = Projectile.position;
            int target = FindTarget(ref targetRange, ref enemyPos);
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

            if (target != -1 && Projectile.ai[0] == 0f)
            {
                GetToAttackPosition(enemyPos, speed, inertia);
            }
            else
            {
                General(player, speed);
            }
            Visuals();
            UpdateShootTimer();
            if (Projectile.ai[0] != 0f || target == -1 || Projectile.ai[1] != 0f)
            {
                return;
            }
            Shoot(10, enemyPos);
            

        }
    }
}
