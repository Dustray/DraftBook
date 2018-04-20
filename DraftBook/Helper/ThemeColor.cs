using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.ViewManagement;

namespace DraftBook.Helper
{
    class ThemeColor
    {
        public static UISettings uiSettings = new UISettings();
        public static Color MainColor = uiSettings.GetColorValue(UIColorType.Accent);
        public static Color MainColorD1 = uiSettings.GetColorValue(UIColorType.AccentDark1);
        public static Color MainColorD2 = uiSettings.GetColorValue(UIColorType.AccentDark2);
        public static Color MainColorD3 = uiSettings.GetColorValue(UIColorType.AccentDark3);
        public static Color MainColorL1 = uiSettings.GetColorValue(UIColorType.AccentLight1);
        public static Color MainColorL2 = uiSettings.GetColorValue(UIColorType.AccentLight2);
        public static Color MainColorL3 = uiSettings.GetColorValue(UIColorType.AccentLight3);
        public static Color TransColor = Colors.Transparent;
        public static Color GrayStandard = Colors.Gray;
        public static Color GrayStandardD = Colors.DarkGray;
        public static Color GrayStandardL = Colors.LightGray;
        public static Color WhiteStandard = Colors.White;
    }
}
