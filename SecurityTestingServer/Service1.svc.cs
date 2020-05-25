using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace SecurityTestingServer
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“Service1”。
    // 注意: 为了启动 WCF 测试客户端以测试此服务，请在解决方案资源管理器中选择 Service1.svc 或 Service1.svc.cs，然后开始调试。
    public class Service1 : IService1
    {
        //选取密钥,类型自己选择
       private string key;
        public StringBuilder Detection(string SecurityCode)
        {
            //创建一个StringBuilder对象
            StringBuilder sb = new StringBuilder();
            //防伪码中的前六位为产品编号
            string number = SecurityCode.Substring(0,6);
            //计算编号的杂凑值
            string MACNumber = MAC(key,number);
            //数据库搜索产品检测表判断是否存在于数据库中
            if (true)
            {
                //搜索时间记录表判断是否有查询记录
                if (true)
                {
                    sb.AppendLine("您所查询的产品为正品但已被检测，检测时间如下：");
                    //然后将每一次时间记录表里的时间进行输入到sb中。
                    //将本次查询时间记录到数据库中
                    string time = DateTime.Now.ToString();
                }
                else
                {
                    //将本次产品记录到数据库中
                    sb.AppendLine("您好，您所查询的***产品是正品，请放心购买，谢谢您的惠顾！");
                }
            }
            else
            {
                sb.AppendLine("对不起，你搜索的产品不存在。");
            }
            return sb;
        }
        //带密钥的杂凑函数算法实现
        public string MAC(string key,string number)
        {
            return "编号的杂凑值";
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }
    }
}
