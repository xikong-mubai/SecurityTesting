using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace SecurityTestingServer
{
    public class Hash_IDEA
    {
        static string zero = "00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000";

        /// <summary>
        /// 功能描述    ：循环左移 num 位
        /// 创 建 者    ：叶世林
        /// 创建日期    ：$time$ 
        /// 最后修改者  ：$username$
        /// 最后修改日期：$time$ 
        /// </summary>
        public static String ROL(String lit, int num)
        {
            StringBuilder tmp = new StringBuilder();
            tmp.Append(lit.Remove(0, num));
            return tmp.Append(lit.Substring(0, num)).ToString();
        }

        /// <summary>
        /// 功能描述    ：逐位异或
        /// 创 建 者    ：叶世林
        /// 创建日期    ：$time$ 
        /// 最后修改者  ：$username$
        /// 最后修改日期：$time$ 
        /// </summary>
        public static string idea_xor(string x, string y)
        {
            StringBuilder tmp = new StringBuilder();
            int i = 0;
            while (true)
            {
                try
                {
                    tmp.Append(Convert.ToString(Convert.ToInt32(x[i]) ^ Convert.ToInt32(y[i])));
                    i += 1;
                }
                catch
                {
                    return tmp.ToString();
                }
            }
        }

        /// <summary>
        /// 功能描述    ：模65536相加
        /// 创 建 者    ：叶世林
        /// 创建日期    ：$time$ 
        /// 最后修改者  ：$username$
        /// 最后修改日期：$time$ 
        /// </summary>
        public static string idea_add(string x, string y)
        {
            int tmp_x = 0, tmp_y = 0;
            for (int i = 0; i < x.Length; i++) tmp_x = tmp_x * 2 + Convert.ToInt32(x[i]);
            for (int i = 0; i < y.Length; i++) tmp_y = tmp_y * 2 + Convert.ToInt32(x[i]);
            int z = (tmp_y + tmp_x) % 65536;
            StringBuilder tmp = new StringBuilder();
            while (z != 0)
            {
                tmp.Append(Convert.ToString(z % 2));
                z /= 2;
            }
            char[] tmp_result = tmp.ToString().ToCharArray();
            Array.Reverse(tmp_result);
            string result = zero.Substring(0, 16 - tmp_result.Length % 16) + new string(tmp_result);
            return result;
        }

        /// <summary>
        /// 功能描述    ：模65537相乘
        /// 创 建 者    ：叶世林
        /// 创建日期    ：$time$ 
        /// 最后修改者  ：$username$
        /// 最后修改日期：$time$ 
        /// </summary>
        public static string idea_mul(string x, string y)
        {
            UInt32 tmp_x = 0, tmp_y = 0;
            for (int i = 0; i < x.Length; i++) tmp_x = tmp_x * 2 + Convert.ToUInt32(x[i]);
            for (int i = 0; i < y.Length; i++) tmp_y = tmp_y * 2 + Convert.ToUInt32(y[i]);
            if (tmp_x == 0) tmp_x = 65536;
            if (tmp_y == 0) tmp_y = 65536;
            UInt32 z = (tmp_y * tmp_x) % 65537 % 65536;
            StringBuilder tmp = new StringBuilder();
            while (z != 0)
            {
                tmp.Append(Convert.ToString(z % 2));
                z /= 2;
            }
            char[] tmp_result = tmp.ToString().ToCharArray();
            Array.Reverse(tmp_result);
            string result = zero.Substring(0, 16 - tmp_result.Length) + new string(tmp_result);
            return result;
        }

        /// <summary>
        /// 功能描述    ：哈希函数内部计算流程
        /// 创 建 者    ：叶世林
        /// 创建日期    ：$time$ 
        /// 最后修改者  ：$username$
        /// 最后修改日期：$time$ 
        /// </summary>
        public static string idea_encode(string m, int num, string[] key)
        {
            string[] x_array = new string[4], z_array = new string[6];
            string c = "";
            for (int i = 0; i < 4; i++)
            {
                x_array[i] = m.Substring(i * 16, 16);
            }
            for (int i = 0; i < 6; i++)
            {
                z_array[i] = key[num * 6 + i];
            }

            if (num < 8)
            {
                string out_1 = idea_mul(x_array[0], z_array[0]);
                string out_2 = idea_add(x_array[1], z_array[1]);
                string out_3 = idea_add(x_array[2], z_array[2]);
                string out_4 = idea_mul(x_array[3], z_array[3]);
                string out_5 = idea_mul(z_array[4], idea_xor(out_1, out_3));
                string out_6 = idea_mul(z_array[5], idea_add(idea_xor(out_2, out_4), out_5));
                string out_7 = idea_add(out_5, out_6);

                string w_1 = idea_xor(out_1, out_6);
                string w_2 = idea_xor(out_3, out_6);
                string w_3 = idea_xor(out_2, out_7);
                string w_4 = idea_xor(out_4, out_7);

                c = w_1 + w_2 + w_3 + w_4;

                num += 1;
                c = idea_encode(c, num, key);
            }
            else
            {
                string y_1 = idea_mul(x_array[0], z_array[0]);
                string y_2 = idea_add(x_array[2], z_array[1]);
                string y_3 = idea_add(x_array[1], z_array[2]);
                string y_4 = idea_mul(x_array[3], z_array[3]);
                c = y_1 + y_2 + y_3 + y_4;
            }
            return c;
        }

        /// <summary>
        /// 功能描述    ：哈希函数外部工作流程
        /// 创 建 者    ：叶世林
        /// 创建日期    ：$time$ 
        /// 最后修改者  ：$username$
        /// 最后修改日期：$time$ 
        /// </summary>
        public static string IDEA_Crypto(string cm_str, string key)
        {
            //将明文转化为二元序列
            StringBuilder tmp_m_bin = new StringBuilder();
            for (int i = 0; i < cm_str.Length; i++)
            {
                int tmp = cm_str[i], tmp_x;
                StringBuilder tmp_y = new StringBuilder();
                while (tmp != 0)
                {
                    tmp_x = tmp % 2;
                    tmp_y.Append(tmp_x.ToString());
                    tmp /= 2;
                }
                char[] tmp_result = tmp_y.ToString().ToCharArray();
                Array.Reverse(tmp_result);
                string result = zero.Substring(0, 8 - tmp_result.Length) + new string(tmp_result);
                tmp_m_bin.Append(result);
            }
            string m_bin = tmp_m_bin.ToString();

            //二元序列长度控制为 128 的整数倍，不足时末尾补零
            if (m_bin.Length % 128 != 0) m_bin += zero.Substring(0, 128 - m_bin.Length % 128);

            //将密钥转化为二元序列
            StringBuilder tmp_key_bin = new StringBuilder();
            for (int i = 0; i < key.Length; i++)
            {
                int tmp = key[i], tmp_x;
                StringBuilder tmp_y = new StringBuilder();
                while (tmp != 0)
                {
                    tmp_x = tmp % 2;
                    tmp_y.Append(tmp_x.ToString());
                    tmp /= 2;
                }
                char[] tmp_result = tmp_y.ToString().ToCharArray();
                Array.Reverse(tmp_result);
                string result = zero.Substring(0, 8 - tmp_result.Length) + new string(tmp_result);
                tmp_key_bin.Append(result);
            }
            string key_bin = tmp_key_bin.ToString();

            //二元序列长度控制为 128 的整数倍，不足时末尾补零
            if (key_bin.Length % 128 != 0) key_bin += zero.Substring(0, 128 - key_bin.Length);

            //生成子密钥
            string[] key_encrypt_array = new string[56];
            int num = 0;
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    key_encrypt_array[num] = key_bin.Substring(j * 16, 16);
                    num++;
                }
                key_bin = ROL(key_bin, 25);
            }

            //将明文二元序列每64位一组进行分组
            string[] mc_array = new string[m_bin.Length / 64];
            num = 0;
            for (int i = 0; i < (m_bin.Length / 64); i++)
            {
                mc_array[num] = m_bin.Substring(i * 64, 64);
                num++;
            }

            //求结果二元序列
            string cm = "0000000000000000000000000000000000000000000000000000000000000000";
            for (int i = 0; i < mc_array.Length; i++)
            {
                cm = idea_xor(cm, mc_array[i]);
                cm = idea_encode(cm, 0, key_encrypt_array);
            }

            //将结果二元序列转换为对应的十六进制内容
            UInt64 cm_int = 0;
            for (int i = 0; i < 64; i++)
            {
                cm_int = cm_int * 2 + Convert.ToUInt64(cm[i]);
            }
            cm = string.Format("{0:x8}", cm_int);

            return cm;
        }
    }
}