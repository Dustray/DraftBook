using DraftBook.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.ViewManagement;

namespace DraftBook.Navigation
{
    class TittleBarConfiguration
    {
        public Color BackgroundColor { get; set; }
        public Color ForegroundColor { get; set; }
        public Color InactiveBackgroundColor { get; set; }
        public Color InactiveForegroundColor { get; set; }
        private ApplicationView view;
        public TittleBarConfiguration()
        {
            BackgroundColor = ThemeColor.TransColor;
            ForegroundColor = ThemeColor.GrayStandard;

            InactiveBackgroundColor = ThemeColor.GrayStandardL;
            InactiveForegroundColor = ThemeColor.MainColorD3;
        }
        public void Effect()
        {
            //设置内容延伸至标题栏
            CoreApplicationView coreappview = CoreApplication.GetCurrentView();
            coreappview.TitleBar.ExtendViewIntoTitleBar = false;
            //标题栏颜色
            view = ApplicationView.GetForCurrentView();
            // active
            view.TitleBar.BackgroundColor = BackgroundColor;
            view.TitleBar.ForegroundColor = ForegroundColor;

            // inactive
            view.TitleBar.InactiveBackgroundColor = InactiveBackgroundColor;
            view.TitleBar.InactiveForegroundColor = InactiveForegroundColor;
            SetThreeBtn();
        }
        private void SetThreeBtn()
        {
            // button
            view.TitleBar.ButtonBackgroundColor = ThemeColor.TransColor;
            view.TitleBar.ButtonForegroundColor = ThemeColor.GrayStandard;

            view.TitleBar.ButtonHoverBackgroundColor = ThemeColor.MainColorL2;
            view.TitleBar.ButtonHoverForegroundColor = ThemeColor.WhiteStandard;

            view.TitleBar.ButtonPressedBackgroundColor = ThemeColor.MainColor;
            view.TitleBar.ButtonPressedForegroundColor = ThemeColor.WhiteStandard;

            view.TitleBar.ButtonInactiveBackgroundColor = ThemeColor.GrayStandardL;
            view.TitleBar.ButtonInactiveForegroundColor = ThemeColor.WhiteStandard;
        }
    }
}
