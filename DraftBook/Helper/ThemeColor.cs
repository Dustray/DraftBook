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
        public static Color mainColor = uiSettings.GetColorValue(UIColorType.Accent);
        public static Color mainColorD1 = uiSettings.GetColorValue(UIColorType.AccentDark1);
        public static Color mainColorD2 = uiSettings.GetColorValue(UIColorType.AccentDark2);
        public static Color mainColorD3 = uiSettings.GetColorValue(UIColorType.AccentDark3);
        public static Color mainColorL1 = uiSettings.GetColorValue(UIColorType.AccentLight1);
        public static Color mainColorL2 = uiSettings.GetColorValue(UIColorType.AccentLight2);
        public static Color mainColorL3 = uiSettings.GetColorValue(UIColorType.AccentLight3);

    }
}
