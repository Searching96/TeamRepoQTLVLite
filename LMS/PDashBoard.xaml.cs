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

namespace LMS
{
    /// <summary>
    /// Interaction logic for PDashBoard.xaml
    /// </summary>
    public partial class PDashBoard : Page
    {
        public PDashBoard()
        {
            InitializeComponent();
        }


        private void media_Loaded(object sender, RoutedEventArgs e)
        {
            media.Play();  // Bắt đầu phát video ngay sau khi tải
        }
        private void MuteButton_Click(object sender, RoutedEventArgs e)
        {
            media.IsMuted = !media.IsMuted;
            MuteButton.Content = media.IsMuted ? "Unmute" : "Mute";        }
        bool mediaisPause = false;
        private void media_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (mediaisPause == false && media.Position != media.NaturalDuration.TimeSpan)
            {
                // Nếu video có thể tạm dừng và chưa kết thúc
                media.Pause();
                mediaisPause = true;// Tạm dừng video nếu đang phát
            }
            else if(mediaisPause == true)
            {
                // Nếu video đã tạm dừng hoặc kết thúc, tiếp tục phát
                media.Play();
                mediaisPause = false;// Tiếp tục phát video
            }
        }
    }
}
