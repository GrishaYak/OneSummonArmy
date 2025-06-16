using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using OneSummonArmy.Content.Buffs;
using OneSummonArmy;
using OneSummonArmy.Content.Projectiles.Birds;

namespace OneSummonArmy.Content.Projectiles.Hornets
{
    public abstract class Hornet : StandardProjectile
    {
        public Hornet()
        {
            sonsTexture = AddDirToPath("Hornets");
            buffType = ModContent.BuffType<HornetBuff>();
        }
        private int reloadCounter = 0;
        protected int ReloadCounter
        {
            get => reloadCounter;
            set => reloadCounter = value;
        }
        protected bool ReadyToShoot
        {
            get => reloadCounter == 0;
            set => reloadCounter = value ? 0 : 1;
        }
        private int stage = 0;
        protected bool ReadyToAttack
        {
            get { return stage == 0; }
            set { stage = value ? 0 : 1; }
        }
        protected bool ShouldGoHome 
        { 
            get => stage == 1; 
            set => stage = value ? 1 : 0; 
        }
        readonly int deafaultDamage = 12;
        protected void ScaleDamage(double scale)
        {
            Projectile.damage = (int)(Projectile.damage * scale);
        }
        protected void SetDamageToDefault()
        {
            Projectile.damage = deafaultDamage;
        }
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            Main.projFrames[Projectile.type] = 3;
        }
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

            ReloadCounter += Main.rand.Next(1, 4);
            
            int shootTimer = Main.player[Projectile.owner].strongBees ? 70 : 90;
            if (ReloadCounter > shootTimer)
            {
                ReadyToShoot = true;
                Projectile.netUpdate = true;
            }
        }
        protected Projectile DirectProj(Vector2 direction, float speed, int type = 374, Vector2 position=new Vector2())
        {
            return DirectProj(direction * speed, type, position);
        }
        protected Projectile DirectProj(Vector2 velocity, int type = 374, Vector2 position= new Vector2())
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
                return shot;
            }
            return null;
        }
        
        protected virtual void Shoot(float newProjSpeed, int enemyID, int type=374, Vector2? direction=null)
        {
            NPC enemy = Main.npc[enemyID];
            Vector2 toEnemy = enemy.Center - Projectile.Center;
            direction ??= toEnemy.SafeNormalize(Vector2.Zero);
            ReadyToShoot = false;
            DirectProj((Vector2) direction, newProjSpeed, type);
            
        }
        protected virtual void GetToAttackPosition(Vector2 enemyPos, float speed, float inertia, float dist=200f)
        {
            Vector2 direction = enemyPos - Projectile.Center;
            float enemyDistance = direction.Length();
            direction = direction.SafeNormalize(Vector2.Zero);
            if (enemyDistance > dist)
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
                ShouldGoHome = true;
            }
            if (ShouldGoHome)
            {
                speed = 15f;
            }
            Vector2 toIdlePos = player.Center - Projectile.Center + new Vector2(0f, -60f);
            float distanceToHome = toIdlePos.Length();
            if (distanceToHome > 200f && speed < 9f)
            {
                speed = 9f;
            }
            if (distanceToHome < 100f && ShouldGoHome && !Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
            {
                ReadyToAttack = true;
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
        void Basics(Player player, int level)
        {
            CheckActive(player);
            if (Projectile.type != Func.ProjIdByLevel("Hornet", level))
            {
                var source = player.GetSource_FromThis();
                var proj = Projectile.NewProjectileDirect(source, Projectile.position, Projectile.velocity, Func.ProjIdByLevel("Hornet", level), 12, 2f);
                proj.Center = Projectile.Center;
                Projectile.Kill();
                return;
            }
        }

        public override void AI()
        {
            // Projectile.ai[1] is used to support frequency of shooting
            // ai[0] == 1 means "should get to idlePos
            // ai[0] == 0 means "ready to attack"
            
            Player player = Main.player[Projectile.owner];
            int level = player.ownedProjectileCounts[ModContent.ProjectileType<HornetCounter>()];
            Basics(player, level);
            float speed = 6f, inertia = 40f, targetRange = 2000f;
            Vector2 enemyPos = Projectile.position;
            int target = FindTarget(ref targetRange, ref enemyPos);

            if (Vector2.Distance(player.Center, Projectile.Center) > 500)
            {
                ShouldGoHome = true;
                Projectile.netUpdate = true;
            }
            Projectile.tileCollide = !ShouldGoHome;

            if (target != -1 && ReadyToAttack)
            {
                GetToAttackPosition(enemyPos, speed, inertia);
            }
            else
            {
                General(player, speed);
            }
            Visuals();
            UpdateShootTimer();
            if (!ReadyToAttack || target == -1 || !ReadyToShoot)
            {
                return;
            }
            Shoot(10, target);
        }
    }
}
