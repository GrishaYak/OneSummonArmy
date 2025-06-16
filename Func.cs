using Microsoft.Xna.Framework;
using OneSummonArmy.Content.Buffs;
using OneSummonArmy.Content.Projectiles.Birds;
using OneSummonArmy.Content.Projectiles.Flinxes;
using OneSummonArmy.Content.Projectiles.Hornets;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace OneSummonArmy
{
    public class Func
    {
        public static Vector2 CounterGetHome(Player master, int stackedIndex, int totalIndexes)
        {
            int playerHeight = master.bodyFrame.Height;
            if (playerHeight == 0)
            {
                playerHeight = 1;
            }
            Vector2 vector = Main.OffsetsPlayerHeadgear[master.bodyFrame.Y / playerHeight];
            vector.Y -= 2f;
            float num2 = master.miscCounterNormalized * 2f;
            int num3 = stackedIndex / 4;
            int num4 = (totalIndexes + 3) / 4;
            int num5 = (totalIndexes - num3 * 4) % 4;
            if (num5 == 0 || num4 - 1 != num3)
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
        public static void GetMyGroupIndex(Projectile proj, out int index, out int totalIndexesInGroup)
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
        public static int ProjIdByLevel(string minion, int level)
        {
            switch (minion)
            {
                case "Bird":
                    return level switch
                    {
                        1 => ModContent.ProjectileType<Finch>(),
                        2 => ModContent.ProjectileType<BlueJay>(),
                        3 => ModContent.ProjectileType<GoldenBird>(),
                        4 => ModContent.ProjectileType<Toucan>(),
                        _ => ModContent.ProjectileType<Ara>()
                    };
                case "Slime":
                    return level switch
                    {
                        1 => ModContent.ProjectileType<Finch>(),
                        2 => ModContent.ProjectileType<BlueJay>(),
                        3 => ModContent.ProjectileType<GoldenBird>(),
                        4 => ModContent.ProjectileType<Toucan>(),
                        _ => ModContent.ProjectileType<Ara>()
                    };
                case "Hornet":
                    return level switch
                    {
                        1 => ModContent.ProjectileType<VanilaHornet>(),
                        2 => ModContent.ProjectileType<RifleHornet>(),
                        3 => ModContent.ProjectileType<ShotgunHornet>(),
                        _ => ModContent.ProjectileType<BeeHornet>()
                    };
                case "Fllinx":
                    return level switch
                    {
                        1 => ModContent.ProjectileType<Flinx1>(),
                        2 => ModContent.ProjectileType<Flinx2>(),
                        3 => ModContent.ProjectileType<Flinx3>(),
                        _ => ModContent.ProjectileType<Flinx4>()
                    };
                default:
                    throw new NotImplementedException();

            }
        }

    }
}
