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
using SecurityTestingClient.ServiceReference1;

namespace SecurityTestingClient
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        //客户端代理类
        Service1Client client = new Service1Client();
        /*
         * 传出：将防伪码提交的服务端
         * 传入：产品的相关信息，创建StringBuilder对象接受传回来的对象，然后输出到检测界面
         * 参考界面显示.PNG的文件样式创建个性的界面。
             
             */
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string securityCode = SecurityCode.Text.ToString();
            //创建StringBuilder对象接收结果
            StringBuilder sb = new StringBuilder();
            sb = client.Detection(securityCode);
            Result.Text = sb.ToString();
        }
    }
}
