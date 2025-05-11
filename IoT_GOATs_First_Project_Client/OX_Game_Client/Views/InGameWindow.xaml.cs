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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OX_Game_Client.Views
{
    /// <summary>
    /// InGameWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class InGameWindow : UserControl
    {
        private double moveStep = 30;
        private double animationTime = 0.1;
        public InGameWindow()
        {
            InitializeComponent();
            this.Loaded += InGameWindow_Loaded;
        }
        public InGameWindow(string name)
        {
            CharName.Text = name;
        }
        private void InGameWindow_Loaded(object sender, RoutedEventArgs e)
        {
            GameCanvas.Focus();
            GameCanvas.KeyDown += GameCanvas_KeyDown;
        }
        private void GameCanvas_KeyDown(object sender, KeyEventArgs e)
        {
            double currentLeft = Canvas.GetLeft(Character);
            double currentTop = Canvas.GetTop(Character);
            double characterWidth = 50;
            double characterHeight = 56;

            double moveStep = 40;

            double newLeft = currentLeft;
            double newTop = currentTop;

            switch (e.Key)
            {
                case Key.Left:
                    newLeft -= moveStep;
                    break;
                case Key.Right:
                    newLeft += moveStep;
                    break;
                case Key.Up:
                    newTop -= moveStep;
                    break;
                case Key.Down:
                    newTop += moveStep;
                    break;
                default:
                    return;
            }

            // 경계 충돌 처리
            double canvasWidth = GameCanvas.ActualWidth;
            double canvasHeight = GameCanvas.ActualHeight;

            // 좌측 경계
            if (newLeft < 0) newLeft = 0;

            // 상단 경계
            if (newTop < 0) newTop = 0;

            // 우측 경계
            if (newLeft + characterWidth > canvasWidth) newLeft = canvasWidth - characterWidth;

            // 하단 경계
            if (newTop + characterHeight > canvasHeight) newTop = canvasHeight - characterHeight - characterHeight;

            // 새로운 위치 설정
            Canvas.SetLeft(Character, newLeft);
            Canvas.SetTop(Character, newTop);
        }
        //private void ToggleChatButton_Click(object sender, RoutedEventArgs e)
        //{
        //    if (ChatPanel.Visibility == Visibility.Visible)
        //    {
        //        ChatPanel.Visibility = Visibility.Collapsed;
        //        ToggleChatButton.Content = "채팅창 열기";
        //    }
        //    else
        //    {
        //        ChatPanel.Visibility = Visibility.Visible;
        //        ToggleChatButton.Content = "채팅창 닫기";
        //    }
        //}


    }
}
