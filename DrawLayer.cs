using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Terraria.ModLoader.PlayerDrawLayer;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace OneSummonArmy
{
    internal class ModDrawLayer : PlayerDrawLayer
    {
        /// <summary> The delegate of this method, which can either do the actual drawing or add draw data, depending on what kind of layer this is. </summary>
        public delegate void DrawFunc(ref PlayerDrawSet info);

        /// <summary> The delegate of this method, which can either do the actual drawing or add draw data, depending on what kind of layer this is. </summary>
        public delegate bool Condition(PlayerDrawSet info);

        private readonly DrawFunc drawFunc;

        private readonly Condition condition;

        private Transformation _transform;

        private readonly string _name;

        private readonly bool _isHeadLayer;

        private readonly Position position;

        public override Transformation Transform => this._transform;

        public override string Name => this._name;

        public override bool IsHeadLayer => this._isHeadLayer;

        /// <summary> Creates a LegacyPlayerLayer with the given mod name, identifier name, and drawing action. </summary>
        public ModDrawLayer(string name, DrawFunc drawFunc, Transformation transform = null, bool isHeadLayer = false, Condition condition = null, Position position = null)
        {
            this._name = name;
            this.drawFunc = drawFunc;
            this.condition = condition;
            this.position = position;
            this._transform = transform;
            this._isHeadLayer = isHeadLayer;
        }
        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            this.drawFunc(ref drawInfo);
        }
        public override Position GetDefaultPosition()
        {
            if (this.position != null)
            {
                return this.position;
            }
            int index;
            for (index = 0; FixedVanillaLayers[index] != this; index++)
            {
            }
            return new Between((index > 0) ? FixedVanillaLayers[index - 1] : null, (index < FixedVanillaLayers.Count - 1) ? FixedVanillaLayers[index + 1] : null);
        }
        internal static IReadOnlyList<PlayerDrawLayer> FixedVanillaLayers => new PlayerDrawLayer[45]
    {
        PlayerDrawLayers.JimsCloak,
        PlayerDrawLayers.MountBack,
        PlayerDrawLayers.Carpet,
        PlayerDrawLayers.PortableStool,
        PlayerDrawLayers.ElectrifiedDebuffBack,
        PlayerDrawLayers.ForbiddenSetRing,
        PlayerDrawLayers.SafemanSun,
        PlayerDrawLayers.WebbedDebuffBack,
        PlayerDrawLayers.LeinforsHairShampoo,
        PlayerDrawLayers.Backpacks,
        PlayerDrawLayers.Tails,
        PlayerDrawLayers.Wings,
        PlayerDrawLayers.HairBack,
        PlayerDrawLayers.BackAcc,
        PlayerDrawLayers.HeadBack,
        PlayerDrawLayers.BalloonAcc,
        PlayerDrawLayers.Skin,
        PlayerDrawLayers.Leggings,
        PlayerDrawLayers.Shoes,
        PlayerDrawLayers.Robe,
        PlayerDrawLayers.SkinLongCoat,
        PlayerDrawLayers.ArmorLongCoat,
        PlayerDrawLayers.Torso,
        PlayerDrawLayers.OffhandAcc,
        PlayerDrawLayers.WaistAcc,
        PlayerDrawLayers.NeckAcc,
        PlayerDrawLayers.Head,
        PlayerDrawLayers.FinchNest,
        PlayerDrawLayers.FaceAcc,
        PlayerDrawLayers.MountFront,
        PlayerDrawLayers.Pulley,
        PlayerDrawLayers.JimsDroneRadio,
        PlayerDrawLayers.FrontAccBack,
        PlayerDrawLayers.Shield,
        PlayerDrawLayers.SolarShield,
        PlayerDrawLayers.ArmOverItem,
        PlayerDrawLayers.HandOnAcc,
        PlayerDrawLayers.BladedGlove,
        PlayerDrawLayers.ProjectileOverArm,
        PlayerDrawLayers.FrozenOrWebbedDebuff,
        PlayerDrawLayers.ElectrifiedDebuffFront,
        PlayerDrawLayers.IceBarrier,
        PlayerDrawLayers.CaptureTheGem,
        PlayerDrawLayers.BeetleBuff,
        PlayerDrawLayers.EyebrellaCloud
    };
    }
}
