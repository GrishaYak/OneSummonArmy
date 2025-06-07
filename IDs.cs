using OneSummonArmy.Content.Projectiles.Birds;
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
    }
}
