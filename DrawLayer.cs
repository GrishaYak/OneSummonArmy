using System;
using Terraria.ModLoader;
using Terraria.DataStructures;
using ReLogic.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using OneSummonArmy.ID;
using Terraria.GameContent;
using Terraria;
using System.Collections.Generic;

namespace OneSummonArmy
{
    public class DrawLayer : PlayerDrawLayer
    {
        public static void DrawPlayer_FinchNest(ref PlayerDrawSet drawinfo)
        {
            Player player = drawinfo.drawPlayer;
            int[] projIds = GetIds.GetBirdsIds();
            bool ok = false;
            foreach (int projId in projIds)
            {
                if (player.ownedProjectileCounts[projId] > 0)
                {
                    ok = true; break;
                }
            }
            if (ok)
            {
                Rectangle bodyFrame5 = drawinfo.drawPlayer.bodyFrame;
                bodyFrame5.Y = 0;
                Vector2 vector6 = Vector2.Zero;
                Color color8 = drawinfo.colorArmorHead;
                if (drawinfo.drawPlayer.mount.Active && drawinfo.drawPlayer.mount.Type == 52)
                {
                    Vector2 mountedCenter = drawinfo.drawPlayer.MountedCenter;
                    color8 = drawinfo.drawPlayer.GetImmuneAlphaPure(Lighting.GetColorClamped((int)mountedCenter.X / 16, (int)mountedCenter.Y / 16, Color.White), drawinfo.shadow);
                    vector6 = new Vector2(0f, 6f) * drawinfo.drawPlayer.Directions;
                }
                DrawData item = new(TextureAssets.Extra[100].Value, vector6 + new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect + Main.OffsetsPlayerHeadgear[drawinfo.drawPlayer.bodyFrame.Y / drawinfo.drawPlayer.bodyFrame.Height] * drawinfo.drawPlayer.gravDir, bodyFrame5, color8, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
                drawinfo.DrawDataCache.Add(item);
            }

        }
        public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.Head);
        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            DrawPlayer_FinchNest(ref drawInfo);
        }
    }
}
