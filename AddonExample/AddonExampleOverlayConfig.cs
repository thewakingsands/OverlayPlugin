using System;
using RainbowMage.OverlayPlugin;

namespace AddonExample
{
    public class AddonExampleOverlayConfig : OverlayConfigBase
    {
        public AddonExampleOverlayConfig(string name) : base(name)
        {

        }

        private AddonExampleOverlayConfig() : base(null)
        {

        }

        public override Type OverlayType
        {
            get { return typeof(AddonExampleOverlay); }
        }
    }
}
