using DraftBook.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Input.Inking;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace DraftBook
{
    public class CalligraphicPen : InkToolbarCustomPen
    {
        public CalligraphicPen()
        {
        }

        protected override InkDrawingAttributes CreateInkDrawingAttributesCore(Brush brush, double strokeWidth)
        {

            InkDrawingAttributes inkDrawingAttributes = new InkDrawingAttributes();
            inkDrawingAttributes.PenTip = PenTipShape.Circle;
            inkDrawingAttributes.IgnorePressure = false;
            SolidColorBrush solidColorBrush = (SolidColorBrush)brush;

            if (solidColorBrush != null)
            {
                inkDrawingAttributes.Color = solidColorBrush.Color;
            }

            inkDrawingAttributes.Size = new Size(strokeWidth, 2.0f * strokeWidth);
            inkDrawingAttributes.PenTipTransform = System.Numerics.Matrix3x2.CreateRotation((float)(Math.PI * 45 / 180));

            return inkDrawingAttributes;
        }
    }

    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class InkPage : Page
    {
        private Polyline lasso;
        private Rect boundingRect;
        private MainPage rootPage = MainPage.Current;
        private bool isBoundRect;

        Symbol LassoSelect = (Symbol)0xEF20;
        Symbol TouchWriting = (Symbol)0xED5F;
        Symbol ShowMoreBtn = (Symbol)0xE09A;
        Symbol HideMoreBtn = (Symbol)0xE09B;
        string mainColor;

        public InkPage()
        {
            this.InitializeComponent();
            UISettings uiSettings = new UISettings();
            mainColor = uiSettings.GetColorValue(UIColorType.AccentLight1).ToString();
            //设置鼠标和触摸可用
            MainInkCanvas.InkPresenter.InputDeviceTypes = CoreInputDeviceTypes.Mouse | CoreInputDeviceTypes.Touch;

            // Handlers to clear the selection when inking or erasing is detected
            MainInkCanvas.InkPresenter.StrokeInput.StrokeStarted += StrokeInput_StrokeStarted;
            MainInkCanvas.InkPresenter.StrokesErased += InkPresenter_StrokesErased;
        }
        private void StrokeInput_StrokeStarted(InkStrokeInput sender, PointerEventArgs args)
        {
            ClearSelection();
            MainInkCanvas.InkPresenter.UnprocessedInput.PointerPressed -= UnprocessedInput_PointerPressed;
            MainInkCanvas.InkPresenter.UnprocessedInput.PointerMoved -= UnprocessedInput_PointerMoved;
            MainInkCanvas.InkPresenter.UnprocessedInput.PointerReleased -= UnprocessedInput_PointerReleased;
        }

        private void InkPresenter_StrokesErased(InkPresenter sender, InkStrokesErasedEventArgs args)
        {
            ClearSelection();
            MainInkCanvas.InkPresenter.UnprocessedInput.PointerPressed -= UnprocessedInput_PointerPressed;
            MainInkCanvas.InkPresenter.UnprocessedInput.PointerMoved -= UnprocessedInput_PointerMoved;
            MainInkCanvas.InkPresenter.UnprocessedInput.PointerReleased -= UnprocessedInput_PointerReleased;
        }

        //private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        //{
        //    HelperFunctions.UpdateCanvasSize(RootGrid, outputGrid, MainInkCanvas);
        //}

        // This is the recommended way to implement inking with touch on Windows.
        // Since touch is reserved for navigation (pan, zoom, rotate, etc.),
        // if you抎 like your app to have inking with touch, it is recommended
        // that it is enabled via CustomToggle like in this scenario, with the
        // same icon and tooltip.
        private void Toggle_Custom(object sender, RoutedEventArgs e)
        {
            if (toggleButton.IsChecked == true)
            {
                MainInkCanvas.InkPresenter.InputDeviceTypes |= CoreInputDeviceTypes.Touch;
            }
            else
            {
                MainInkCanvas.InkPresenter.InputDeviceTypes &= ~CoreInputDeviceTypes.Touch;
            }


        }
        private void ShowMoreBtn_Click(object sender, RoutedEventArgs e)
        {
            if (hideOrShowMoreButton.IsChecked == true)
            {
                hideOrShowMoreBtnIcon.Symbol = HideMoreBtn;
                ToolTipService.SetToolTip(hideOrShowMoreButton, "隐藏更多工具");
                toggleButton.Visibility = Visibility.Visible;
            }
            else
            {
                hideOrShowMoreBtnIcon.Symbol = ShowMoreBtn;
                ToolTipService.SetToolTip(hideOrShowMoreButton, "显示更多工具");
                toggleButton.Visibility = Visibility.Collapsed;
            }


        }
        private void UnprocessedInput_PointerPressed(InkUnprocessedInput sender, PointerEventArgs args)
        {
            lasso = new Polyline()
            {
                Stroke = new SolidColorBrush(Windows.UI.Colors.Blue),
                StrokeThickness = 1,
                StrokeDashArray = new DoubleCollection() { 5, 2 },
            };

            lasso.Points.Add(args.CurrentPoint.RawPosition);
            selectionCanvas.Children.Add(lasso);
            isBoundRect = true;
        }

        private void UnprocessedInput_PointerMoved(InkUnprocessedInput sender, PointerEventArgs args)
        {
            if (isBoundRect)
            {
                lasso.Points.Add(args.CurrentPoint.RawPosition);
            }
        }

        private void UnprocessedInput_PointerReleased(InkUnprocessedInput sender, PointerEventArgs args)
        {
            lasso.Points.Add(args.CurrentPoint.RawPosition);

            boundingRect = MainInkCanvas.InkPresenter.StrokeContainer.SelectWithPolyLine(lasso.Points);
            isBoundRect = false;
            DrawBoundingRect();
        }

        private void DrawBoundingRect()
        {
            selectionCanvas.Children.Clear();

            if (boundingRect.Width <= 0 || boundingRect.Height <= 0)
            {
                return;
            }

            var rectangle = new Rectangle()
            {
                Stroke = new SolidColorBrush(Windows.UI.Colors.Blue),
                StrokeThickness = 1,
                StrokeDashArray = new DoubleCollection() { 5, 2 },
                Width = boundingRect.Width,
                Height = boundingRect.Height
            };

            Canvas.SetLeft(rectangle, boundingRect.X);
            Canvas.SetTop(rectangle, boundingRect.Y);

            selectionCanvas.Children.Add(rectangle);
        }

        private void ToolButton_Lasso(object sender, RoutedEventArgs e)
        {
            // By default, pen barrel button or right mouse button is processed for inking
            // Set the configuration to instead allow processing these input on the UI thread
            MainInkCanvas.InkPresenter.InputProcessingConfiguration.RightDragAction = InkInputRightDragAction.LeaveUnprocessed;

            MainInkCanvas.InkPresenter.UnprocessedInput.PointerPressed += UnprocessedInput_PointerPressed;
            MainInkCanvas.InkPresenter.UnprocessedInput.PointerMoved += UnprocessedInput_PointerMoved;
            MainInkCanvas.InkPresenter.UnprocessedInput.PointerReleased += UnprocessedInput_PointerReleased;
        }

        private void ClearDrawnBoundingRect()
        {
            if (selectionCanvas.Children.Count > 0)
            {
                selectionCanvas.Children.Clear();
                boundingRect = Rect.Empty;
            }
        }

        private void OnCopy(object sender, RoutedEventArgs e)
        {
            MainInkCanvas.InkPresenter.StrokeContainer.CopySelectedToClipboard();
        }

        private void OnCut(object sender, RoutedEventArgs e)
        {
            MainInkCanvas.InkPresenter.StrokeContainer.CopySelectedToClipboard();
            MainInkCanvas.InkPresenter.StrokeContainer.DeleteSelected();
            //MainInkCanvas.InkPresenter.StrokeContainer.MoveSelected();//移动涂鸦
            ClearDrawnBoundingRect();
        }
        private void OnPaste(object sender, RoutedEventArgs e)
        {
            if (MainInkCanvas.InkPresenter.StrokeContainer.CanPasteFromClipboard())
            {
                MainInkCanvas.InkPresenter.StrokeContainer.PasteFromClipboard(new Point((scrollViewer.HorizontalOffset + 10) / scrollViewer.ZoomFactor, (scrollViewer.VerticalOffset + 10) / scrollViewer.ZoomFactor));
            }
            else
            {
                // rootPage.NotifyUser("Cannot paste from clipboard.", NotifyType.ErrorMessage);
            }
        }
        private void ClearSelection()
        {
            var strokes = MainInkCanvas.InkPresenter.StrokeContainer.GetStrokes();
            foreach (var stroke in strokes)
            {
                stroke.Selected = false;
            }
            ClearDrawnBoundingRect();
        }


        private void CurrentToolChanged(Windows.UI.Xaml.Controls.InkToolbar sender, object args)
        {
            bool enabled = sender.ActiveTool.Equals(toolButtonLasso);
            ToolButtonPanel.Visibility = enabled ? Visibility.Visible : Visibility.Collapsed;
            ButtonCut.IsEnabled = enabled;
            ButtonCopy.IsEnabled = enabled;
            ButtonPaste.IsEnabled = enabled;
        }

        private void HideOrShowToolbar_Click(object sender, RoutedEventArgs e)
        {
            if (HideOrShowToolbar.IsChecked==true)
            {
                MainInkToolbar.Visibility = Visibility.Collapsed;
            }
            else
            {
                MainInkToolbar.Visibility = Visibility.Visible;
            }
        }
    }
}