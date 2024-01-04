using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WiFiAutoConnector
{
    internal class ColorTable : ProfessionalColorTable
    {
        private Color Background_Base = Color.FromArgb(0x1e, 0x1e, 0x2b);   // #1e1e2e
        private Color Secondary_Crust = Color.FromArgb(0x11, 0x11, 0x1b);   // #11111b
        private Color Secondary_Mantle = Color.FromArgb(0x18, 0x18, 0x25);  // #181825
        private Color Selection = Color.FromArgb(0x58, 0x5b, 0x70);         // #585b70

        public override Color MenuItemSelectedGradientBegin => Selection;
        public override Color MenuItemSelectedGradientEnd => Selection;

        public override Color ButtonSelectedGradientBegin => Selection;
        public override Color ButtonSelectedGradientMiddle => Selection;
        public override Color ButtonSelectedGradientEnd => Selection;

        public override Color MenuItemBorder => Secondary_Mantle;
        public override Color ToolStripDropDownBackground => Background_Base;
        public override Color ImageMarginGradientBegin => Secondary_Crust;
        public override Color ImageMarginGradientMiddle => Secondary_Mantle;
        public override Color ImageMarginGradientEnd => Secondary_Crust;
    }
}
