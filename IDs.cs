using OneSummonArmy.Content.Projectiles.Birds;
using OneSummonArmy.Content.Projectiles.Slimes;
using Terraria.ModLoader;

namespace OneSummonArmy.ID
{
    public class GetIds
    {
        public static int[] GetBirdsIds()
        {
            int[] a = [ModContent.ProjectileType<Ara>(),
                ModContent.ProjectileType<Toucan>(),
                ModContent.ProjectileType<BlueJay>(),
                ModContent.ProjectileType<Finch>(),
                ModContent.ProjectileType<GoldenBird>()];
            return a;
        }


        public static int[] GetSlimesIds()
        {
            int[] s = [ModContent.ProjectileType<GreenS>(),
                ModContent.ProjectileType<PinkS>(),
                ModContent.ProjectileType<YellowS>(),
                ModContent.ProjectileType<CopperS>(),
                ModContent.ProjectileType<KingS>(),
                ModContent.ProjectileType<RandomS>()];
            return s;
        }
    }
}
