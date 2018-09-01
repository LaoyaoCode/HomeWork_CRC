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


namespace Work
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// 参数枚举体
        /// </summary>
        private enum ParameterEnum
        {
            ITU4 ,
            ITU5,
            ITU6,
            ITU8,
            NONE 
        }

        //选择BOX默认关闭
        private bool IsSelectBoxOpen = false;
        //选择的参数模型 , 默认为无
        private ParameterEnum SelectParameter = ParameterEnum.NONE;
        //当前被选择的TB
        private TextBlock NowSelectTB = null;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// 关闭窗口按钮被点击
        /// </summary>
        private void CloseButton_Click()
        {

            Application.Current.Shutdown();
        }

        /// <summary>
        /// 最小化窗口被点击
        /// </summary>
        //最小化按钮
        private void MinButton_Click()
        {
            this.WindowState = WindowState.Minimized;
        }

        //打开或者关闭选择参数框按钮被点击
        private void SelectPBoxOpenOrCloseButton_Click()
        {
            if(IsSelectBoxOpen)
            {
                IsSelectBoxOpen = false;
                BeginStoryboard((Storyboard)this.Resources["SelectBoxClose"]);
            }
            else
            {
                IsSelectBoxOpen = true;
                BeginStoryboard((Storyboard)this.Resources["SelectBoxOpen"]);
            }
        }

        /// <summary>
        /// 窗口top bar拖动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DargWindowMove(object sender, MouseButtonEventArgs e)
        {
            //move the windows
            this.DragMove();
        }

        /// <summary>
        /// 窗口状态变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void window_StateChanged(object sender, EventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
            {
                BeginStoryboard((Storyboard)this.Resources["MaxAnimation"]);
            }
        }

        /// <summary>
        /// 参数选择鼠标进入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PSelect_MouseEnter(object sender, MouseEventArgs e)
        {
            TextBlock tb = (TextBlock)sender;

            tb.Foreground = new SolidColorBrush(Color.FromRgb(0xE9 , 0x1E , 0x63));
        }

        /// <summary>
        /// 参数选择鼠标离开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PSelect_MouseLeave(object sender, MouseEventArgs e)
        {
            TextBlock tb = (TextBlock)sender;

            if(tb != NowSelectTB)
            {
                tb.Foreground = new SolidColorBrush(Color.FromArgb(0x7F, 0x43, 0x43,0x43));
            }
        }

        /// <summary>
        /// 参数模型选择按钮被点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PSelect_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock tb = (TextBlock)sender;

            //如果点击的是已经选择的，则直接忽略
            if(tb == NowSelectTB)
            {
                return;
            }

            //取消之前的选择状态
            if(NowSelectTB != null)
            {
                NowSelectTB.Foreground = new SolidColorBrush(Color.FromArgb(0x7F, 0x43, 0x43, 0x43));
            }

            //点击自动关闭
            BeginStoryboard((Storyboard)this.Resources["SelectBoxClose"]);
            IsSelectBoxOpen = false;
            NowSelectTB = tb ;

            //字符显示到正确位置
            ParameterModelDisplayTB.Text = tb.Text;

            //选择对应的参数类型
            if(tb.Tag.ToString() == "ITU4")
            {
                SelectParameter = ParameterEnum.ITU4;
            }
            else if(tb.Tag.ToString() == "ITU5")
            {
                SelectParameter = ParameterEnum.ITU5;
            }
            else if (tb.Tag.ToString() == "ITU6")
            {
                SelectParameter = ParameterEnum.ITU6;
            }
            else if (tb.Tag.ToString() == "ITU8")
            {
                SelectParameter = ParameterEnum.ITU8;
            }
        }

        //开始计算按钮被点击
        private void BeginCalculateButton_Click()
        {
            //去除首尾空格，转化为小写
            String data = VerifyData.GetText().ToLower().Trim();

            //如果数据不正确，则显示信息框提示
            if(!VerifyTheData(data))
            {
                WarningText.Text = "校验数据输入格式错误!";
                BeginStoryboard((Storyboard)this.Resources["WarningBoxAppear"]);
                //清空校验码
                VerifyCodeTB.Text = String.Empty;
                return;
            }

            //数据为16进制，需要转化为2进制
            if(data.Contains("0x"))
            {
                char[] temp = data.ToCharArray();
                StringBuilder binBulider = new StringBuilder();
                for(int counter = 2; counter < temp.Length; counter++)
                {
                    binBulider.Append(HexToBin(temp[counter]));
                }

                data = binBulider.ToString();
            }

            switch(SelectParameter)
            {
                case ParameterEnum.ITU4:
                    VerifyCodeTB.Text = CalculateVerifyCode("10011", 4, data);
                    break;
                case ParameterEnum.ITU5:
                    VerifyCodeTB.Text = CalculateVerifyCode("110101", 5, data);
                    break;
                case ParameterEnum.ITU6:
                    VerifyCodeTB.Text = CalculateVerifyCode("1000011", 6, data);
                    break;
                case ParameterEnum.ITU8:
                    VerifyCodeTB.Text = CalculateVerifyCode("100000111", 8, data);
                    break;
                case ParameterEnum.NONE:
                    VerifyCodeTB.Text = String.Empty;
                    WarningText.Text = "参数模型未选择!!";
                    BeginStoryboard((Storyboard)this.Resources["WarningBoxAppear"]);
                    break;
            }
        }

        /// <summary>
        /// 16进制转化为2进制字符串
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        private String HexToBin(char hex)
        {
            switch(hex)
            {
                case '0':
                    return "0000";
                case '1':
                    return "0001";
                case '2':
                    return "0010";
                case '3':
                    return "0011";
                case '4':
                    return "0100";
                case '5':
                    return "0101";
                case '6':
                    return "0110";
                case '7':
                    return "0111";
                case '8':
                    return "1000";
                case '9':
                    return "1001";
                case 'a':
                    return "1010";
                case 'b':
                    return "1011";
                case 'c':
                    return "1100";
                case 'd':
                    return "1101";
                case 'e':
                    return "1110";
                case 'f':
                    return "1111";
                default:
                    throw new Exception();
            }
        }

        /// <summary>
        /// 计算校验码
        /// </summary>
        /// <param name="paraModel">参数模型二进制字符串</param>
        /// <param name="needLength">需要产生的校验码位数</param>
        /// <param name="binData">二进制源需要校验数据</param>
        /// <returns></returns>
        private String CalculateVerifyCode(String paraModel , int needLength , String binData)
        {
            int counter = 0;
            //将二进制字符串和参数模型字符串转化为字符数组
            List<char> binChars = new List<char>(binData.ToCharArray());
            List<char> paraChars = new List<char>(paraModel.ToCharArray());
            StringBuilder result = new StringBuilder() ;

            //左移，留出校验码位数
            for (counter = 0; counter < needLength; counter++)
            {
                binChars.Add('0');
            }

            //清空左边的0
            ClearLeftZero(binChars);
            //数据全为零，无法计算
            if(binChars.Count == 0)
            {
                WarningText.Text = "数据不能全为零!";
                BeginStoryboard((Storyboard)this.Resources["WarningBoxAppear"]);
                return String.Empty;
            }

            while (binChars.Count > needLength)
            {
                //最高位对齐异或
                for (counter = 0; counter < paraChars.Count; counter++)
                {
                    binChars[counter] = BinXOR(binChars[counter], paraChars[counter]);
                }

                //清空左边的0
                ClearLeftZero(binChars);
            }

            //不足位补零
            for (counter = 1;counter <= needLength - binChars.Count; counter++)
            {
                result.Append('0');
            }
            //整合成字符串
            for (counter = 0; counter < binChars.Count; counter++)
            {
                result.Append(binChars[counter]);
            }

            return result.ToString();
        }


        //清除左边的0
        private void ClearLeftZero(List<char> original)
        {
            int counter = 0;
            for (counter = 0; counter < original.Count; counter++)
            {
                if(original[counter] != '0')
                {
                    break;
                }
            }

            original.RemoveRange(0, counter);
        }

        //二进制异或计算
        private char BinXOR(char a , char b)
        {
            if(a == b)
            {
                return '0';
            }
            else
            {
                return '1';
            }
        }

        /// <summary>
        /// 验证是否校验数据输入是否正确
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private bool VerifyTheData(String data)
        {
            //直接转化为小写字母,去掉首尾字符串
            data = data.Trim().ToLower();
            char[] temp = data.ToCharArray();

            //如果包含了Ox则代表想用16进制表示
            if(data.Contains("0x"))
            { 
                //至少得包含三个或者三个以上的字符
                if(temp.Length <= 2)
                {
                    return false;
                }
                //开头出现错误，不是正确的16进制开头
                else if(temp[0] != '0' || temp[1] != 'x')
                {
                    return false;
                }
                else
                {
                    for(int counter = 2; counter < temp.Length; counter++)
                    {
                        //检验数字是否都在 0-9 OR A-F中
                        if((temp[counter] >= '0' && temp[counter] <= '9') || (temp[counter] >= 'a' && temp[counter] <= 'f'))
                        {

                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            //不包含Ox则代表都是二进制
            else
            {
                if(temp.Length == 0)
                {
                    return false;
                }
                else
                {
                    for( int counter = 0; counter < temp.Length; counter ++)
                    {
                        //数据出现了01之外的数字
                        if(temp[counter] != '0' && temp[counter] != '1')
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }
    }
}
