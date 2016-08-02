using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Harry.Extensions;

namespace Harry.Common
{
    public static class HttpHelper
    {
#if !COREFX
        public static string Get(string strUrl, Encoding encoding, int timeout = 120000)
        {
            HttpWebRequest myReq = (HttpWebRequest)HttpWebRequest.Create(strUrl);
            myReq.Method = "GET";
            myReq.Timeout = timeout;
            HttpWebResponse HttpWResp = (HttpWebResponse)myReq.GetResponse();
            using (Stream myStream = HttpWResp.GetResponseStream())
            using (StreamReader sr = new StreamReader(myStream, encoding))
            {
                StringBuilder strBuilder = new StringBuilder();
                while (-1 != sr.Peek())
                {
                    strBuilder.Append(sr.ReadLine());
                }
                return strBuilder.ToString();
            }
        }

        public static string Post(string strUrl, Encoding encoding, int timeout = 120000)
        {
            HttpWebRequest myReq = (HttpWebRequest)HttpWebRequest.Create(strUrl);
            myReq.Method = "POST";
            myReq.Timeout = timeout;
            HttpWebResponse HttpWResp = (HttpWebResponse)myReq.GetResponse();
            using (Stream myStream = HttpWResp.GetResponseStream())
            using (StreamReader sr = new StreamReader(myStream, encoding))
            {
                StringBuilder strBuilder = new StringBuilder();
                while (-1 != sr.Peek())
                {
                    strBuilder.Append(sr.ReadLine());
                }
                return strBuilder.ToString();
            }
        }

        public static string Post(string strUrl, string body, Encoding encoding, int timeout = 120000)
        {
            HttpWebRequest myReq = (HttpWebRequest)HttpWebRequest.Create(strUrl);
            myReq.Method = "POST";
            myReq.Timeout = timeout;
            byte[] btBodys = encoding.GetBytes(body);
            myReq.ContentLength = btBodys.Length;
            myReq.GetRequestStream().Write(btBodys, 0, btBodys.Length);
            HttpWebResponse HttpWResp = (HttpWebResponse)myReq.GetResponse();
            using (Stream myStream = HttpWResp.GetResponseStream())
            using (StreamReader sr = new StreamReader(myStream, encoding))
            {
                StringBuilder strBuilder = new StringBuilder();
                while (-1 != sr.Peek())
                {
                    strBuilder.Append(sr.ReadLine());
                }
                return strBuilder.ToString();
            }
        }
#endif

        /// <summary>
        /// 除去数组中的指定key及空值
        /// </summary>
        /// <param name="dicArrayPre">过滤前的参数组</param>
        /// <returns>过滤后的参数组</returns>
        public static Dictionary<string, string> FilterPara(IDictionary<string, string> dicArrayPre, params string[] strArray)
        {
            Dictionary<string, string> dicArray = new Dictionary<string, string>();
            foreach (KeyValuePair<string, string> temp in dicArrayPre)
            {
                if (temp.Value.HasValue() && !Common.Utils.Contains(strArray, temp.Key))
                {
                    dicArray.Add(temp.Key, temp.Value);
                }
            }

            return dicArray;
        }

        /// <summary>
        /// 除去数组中的空值和签名参数并以字母a到z的顺序排序
        /// </summary>
        /// <param name="dicArrayPre">过滤前的参数组</param>
        /// <returns>过滤后的参数组</returns>
        public static Dictionary<string, string> FilterPara(IDictionary<string, string> dicArrayPre)
        {
            return FilterPara(dicArrayPre, "sign", "sign_type");
        }

        /// <summary>
        /// 把数组所有元素，按照“参数=参数值”的模式用“&”字符拼接成字符串
        /// </summary>
        /// <param name="sArray">需要拼接的数组</param>
        /// <returns>拼接完成以后的字符串</returns>
        public static string CreateLinkString(IDictionary<string, string> dicArray
            , Func<string, string> encoderFun = null
            )
        {
            StringBuilder prestr = new StringBuilder();

            if (encoderFun != null)
            {
                foreach (KeyValuePair<string, string> temp in dicArray)
                {
                    prestr.Append(temp.Key + "=" + encoderFun(temp.Value) + "&");
                }
            }
            else
            {
                foreach (KeyValuePair<string, string> temp in dicArray)
                {
                    prestr.Append(temp.Key + "=" + temp.Value + "&");
                }
            }

            //去掉最后一个&字符
            int nLen = prestr.Length;
            prestr.Remove(nLen - 1, 1);

            return prestr.ToString();
        }
    }
}
