using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;

namespace Work.MControls
{
    /// <summary>
    /// LineAnimationTextInput.xaml 的交互逻辑
    /// </summary>
    public partial class LineAnimationTextInput : UserControl
    {
        /// <summary>
        /// 底线颜色
        /// </summary>
        public SolidColorBrush BaseLineColor
        {
            get { return (SolidColorBrush)GetValue(BaseLineColorProperty); }
            set { SetValue(BaseLineColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BaseLineColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BaseLineColorProperty =
            DependencyProperty.Register("BaseLineColor", typeof(SolidColorBrush), typeof(LineAnimationTextInput), new PropertyMetadata(null));


        /// <summary>
        /// 活跃线条颜色
        /// </summary>
        public SolidColorBrush ActiveLineColor
        {
            get { return (SolidColorBrush)GetValue(ActiveLineColorProperty); }
            set { SetValue(ActiveLineColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ActiveLineColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ActiveLineColorProperty =
            DependencyProperty.Register("ActiveLineColor", typeof(SolidColorBrush), typeof(LineAnimationTextInput), new PropertyMetadata(null));


        /// <summary>
        /// 文字大小尺寸
        /// </summary>
        public double TextSize
        {
            get { return (double)GetValue(TextSizeProperty); }
            set { SetValue(TextSizeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TextSize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextSizeProperty =
            DependencyProperty.Register("TextSize", typeof(double), typeof(LineAnimationTextInput), new PropertyMetadata(0.0));


        /// <summary>
        /// 文字颜色
        /// </summary>
        public SolidColorBrush TextColor
        {
            get { return (SolidColorBrush)GetValue(TextColorProperty); }
            set { SetValue(TextColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TextColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextColorProperty =
            DependencyProperty.Register("TextColor", typeof(SolidColorBrush), typeof(LineAnimationTextInput), new PropertyMetadata(null));



        /// <summary>
        /// 被选择的颜色
        /// </summary>
        public SolidColorBrush SelectionColor
        {
            get { return (SolidColorBrush)GetValue(SelectionColorProperty); }
            set { SetValue(SelectionColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectionColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectionColorProperty =
            DependencyProperty.Register("SelectionColor", typeof(SolidColorBrush), typeof(LineAnimationTextInput), new PropertyMetadata(null));


        private bool MIsFocused = false;

        public LineAnimationTextInput()
        {
            InitializeComponent();
        }



        private void InputArea_LostFocus(object sender, RoutedEventArgs e)
        {
            BeginStoryboard((Storyboard)this.Resources["DisActive"]);
            MIsFocused = false;
        }

        private void InputArea_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (!IsFocused)
            {
                //开始活跃动画
                BeginStoryboard((Storyboard)this.Resources["FoucsActive"]);
                MIsFocused = true;
            }

        }

        public string GetText()
        {
            return InputArea.Text;
        }
    }
}
