using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria.GameContent;
using Terraria;
using OneSummonArmy.Content.Projectiles.Birds;

namespace OneSummonArmy.DrawLayers
{
    public class DrawNest : PlayerDrawLayer
    {
        public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.Head);
        protected override void Draw(ref PlayerDrawSet drawinfo)
        {
            Player player = drawinfo.drawPlayer;
            if (player.ownedProjectileCounts[ModContent.ProjectileType<BirdCounter>()] == 0) { return; }

            Rectangle bodyFrame = drawinfo.drawPlayer.bodyFrame;
            bodyFrame.Y = 0;
            Vector2 vector = Vector2.Zero;
            Color color = drawinfo.colorArmorHead;
            if (drawinfo.drawPlayer.mount.Active && drawinfo.drawPlayer.mount.Type == 52)
            {
                Vector2 mountedCenter = drawinfo.drawPlayer.MountedCenter;
                color = drawinfo.drawPlayer.GetImmuneAlphaPure(Lighting.GetColorClamped((int)mountedCenter.X / 16, (int)mountedCenter.Y / 16, Color.White), drawinfo.shadow);
                vector = new Vector2(0f, 6f) * drawinfo.drawPlayer.Directions;
            }
            DrawData item = new(TextureAssets.Extra[100].Value, vector + new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - drawinfo.drawPlayer.bodyFrame.Width / 2 + drawinfo.drawPlayer.width / 2), (int)(drawinfo.Position.Y - Main.screenPosition.Y + drawinfo.drawPlayer.height - drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect + Main.OffsetsPlayerHeadgear[drawinfo.drawPlayer.bodyFrame.Y / drawinfo.drawPlayer.bodyFrame.Height] * drawinfo.drawPlayer.gravDir, bodyFrame, color, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
            drawinfo.DrawDataCache.Add(item);
            
        }
    }
}
