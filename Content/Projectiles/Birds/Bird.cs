using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using OneSummonArmy.Content.Buffs;
using System.Collections.Generic;
using System.Reflection;

namespace OneSummonArmy.Content.Projectiles.Birds
{
    public abstract class Bird : ModProjectile
    {
        // Here you can decide if your minion breaks things like grass or pots
        bool isMooving = false;
        private void GetIdleFrames(out int l, out int r) 
        {
            l = 4;
            r = 5;
        }
        private void GetMovingFrames(out int l, out int r)
        {
            l = 0;
            r = 4;
        }
        public override bool? CanCutTiles()
        {
            return false;
        }

        // This is mandatory if your minion deals contact damage (further related stuff in AI() in the Movement region)
        public override bool MinionContactDamage()
        {
            return true;
        }
        private bool CheckActive(Player owner)
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
            if (!CheckActive(player)) { return; }

        }
        private void Visuals()
        {
            // This is a simple "loop through all frames from top to bottom" animation
            int frameSpeed = 5;
            int l = 0, r = Main.projFrames[Projectile.type];
            if (isMooving) 
            {
                GetMovingFrames(out l, out r);
            }
            else
            {
                GetIdleFrames(out l, out r);
            }

            Projectile.frameCounter++;

            if (Projectile.frameCounter >= frameSpeed)
            {
                Projectile.frameCounter = 0;
                Projectile.frame++;

                if (Projectile.frame >= r || Projectile.frame < l)
                {
                    Projectile.frame = l;
                }
            }

        }
    }
}
