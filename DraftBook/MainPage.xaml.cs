using DraftBook.Helper;
using DraftBook.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace DraftBook
{

    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {

        public static MainPage Current;
        string mainColor = ThemeColor.MainColor.ToString();
        // 为不同的菜单创建不同的List类型
        private List<NavMenuItem> navMenuPrimaryItem = new List<NavMenuItem>(
            new[]
            {
                new NavMenuItem()
                {
                    FontFamily = new FontFamily("Segoe MDL2 Assets"),
                    Icon = "\xE160",
                    Label = "新一页",
                    Selected = Visibility.Visible,
                    DestPage = typeof(InkPage)
                },

                new NavMenuItem()
                {
                    FontFamily = new FontFamily("Segoe MDL2 Assets"),
                    Icon = "\xE16F",
                    Label = "新建草稿本",
                    Selected = Visibility.Collapsed,
                    DestPage = typeof(SettingPage)
                },

                new NavMenuItem()
                {
                    FontFamily = new FontFamily("Segoe MDL2 Assets"),
                    Icon = "\xE105",
                    Label = "保存",
                    Selected = Visibility.Collapsed,
                    DestPage = typeof(SettingPage)
                },

                new NavMenuItem()
                {
                    FontFamily = new FontFamily("Segoe MDL2 Assets"),
                    Icon = "\xE176",
                    Label = "另存为",
                    Selected = Visibility.Collapsed,
                    DestPage = typeof(InkPage)
                }

            });

        private List<NavMenuItem> navMenuSecondaryItem = new List<NavMenuItem>(
            new[]
            {
                new NavMenuItem()
                {
                    FontFamily = new FontFamily("Segoe MDL2 Assets"),
                    Icon = "\xE72B",
                    Label = "上一页",
                    Selected = Visibility.Collapsed,
                    DestPage = typeof(SettingPage)
                },new NavMenuItem()
                {
                    FontFamily = new FontFamily("Segoe MDL2 Assets"),
                    Icon = "\xE72A",
                    Label = "下一页",
                    Selected = Visibility.Collapsed,
                    DestPage = typeof(SettingPage)
                },new NavMenuItem()
                {
                    FontFamily = new FontFamily("Segoe MDL2 Assets"),
                    Icon = "\xE713",
                    Label = "设置",
                    Selected = Visibility.Collapsed,
                    DestPage = typeof(SettingPage)
                },
            new NavMenuItem()
                {
                    FontFamily = new FontFamily("Segoe MDL2 Assets"),
                    Icon = "\xE7E8",
                    Label = "退出",
                    Selected = Visibility.Collapsed,
                    DestPage = typeof(SettingPage)
                }
            });
        public MainPage()
        {
            this.InitializeComponent();
            // This is a static public property that allows downstream pages to get a handle to the MainPage instance
            // in order to call methods that are in this class.
            Current = this;
            // SampleTitle.Text = FEATURE_NAME;

            //设置tittleBar
            TittleBarConfiguration conf = new TittleBarConfiguration();
            conf.Effect();
            // 绑定导航菜单
            NavMenuPrimaryListView.ItemsSource = navMenuPrimaryItem;
            NavMenuSecondaryListView.ItemsSource = navMenuSecondaryItem;
            // SplitView 开关
            PaneOpenButton.Click += (sender, args) =>
            {
                RootSplitView.IsPaneOpen = !RootSplitView.IsPaneOpen;
            };
            // 导航事件
            NavMenuPrimaryListView.ItemClick += NavMenuListView_ItemClick;
            NavMenuSecondaryListView.ItemClick += NavMenuListView_ItemClick;
            // 默认页
            RootFrame.SourcePageType = typeof(InkPage);
            SystemNavigationManager.GetForCurrentView().BackRequested += BackRequested;
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = RootFrame.CanGoBack ? AppViewBackButtonVisibility.Visible : AppViewBackButtonVisibility.Collapsed;
        }
        private void BackRequested(object sender, BackRequestedEventArgs e)
        {
            if (RootFrame.CanGoBack)
            {
                RootFrame.GoBack();
            }
        }

        
        private void NavMenuListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            // 遍历，将选中Rectangle隐藏
            foreach (var np in navMenuPrimaryItem)
            {
                np.Selected = Visibility.Collapsed;
            }
            foreach (var ns in navMenuSecondaryItem)
            {
                ns.Selected = Visibility.Collapsed;
            }

            NavMenuItem item = e.ClickedItem as NavMenuItem;
            // Rectangle显示并导航
            item.Selected = Visibility.Visible;
            if (item.DestPage != null)
            {
                RootFrame.Navigate(item.DestPage);
            }

            RootSplitView.IsPaneOpen = false;
        }

       
        private void RootFrame_Navigated(object sender, NavigationEventArgs e)
        {
SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = RootFrame.CanGoBack ? AppViewBackButtonVisibility.Visible : AppViewBackButtonVisibility.Collapsed;

        }
    }
}

