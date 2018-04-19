using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace DraftBook.Helper
{
    class Configuration
    {
    }
    static class HelperFunctions
    {
        public static void UpdateCanvasSize(FrameworkElement root, FrameworkElement output, FrameworkElement inkCanvas)
        {
            output.Width = root.ActualWidth;
            output.Height = root.ActualHeight / 2;
            inkCanvas.Width = root.ActualWidth;
            inkCanvas.Height = root.ActualHeight / 2;
        }
    }

    static class MoreSymbols
    {
        static public Symbol CalligraphyPen = (Symbol)0xEDFB;
        static public Symbol LassoSelect = (Symbol)0xEF20;
        static public Symbol TouchWriting = (Symbol)0xED5F;
    }
}
