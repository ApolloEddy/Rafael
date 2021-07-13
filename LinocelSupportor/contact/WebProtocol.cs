using System;
using System.IO;
using System.Net;
using System.Text;

namespace Aries
{
	/// <summary>
	/// 提供网络交互的方法
	/// </summary>
	public class WebProtocol
	{
		// 构造函数
		public WebProtocol()
		{
			Timeout = 1000;
		}
		public WebProtocol(string url, bool computerUA = true)
		{
			this.url = url;
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
			request = (HttpWebRequest)WebRequest.Create(url);
			computerUAsign = computerUA;
			Timeout = 1000;
		}

		// 字段、属性
		/// <summary>
		/// 指定是否使用PC端的UA标识
		/// </summary>
		public bool computerUAsign = true;
		/// <summary>
		/// 指定是否使用随机UA标识
		/// </summary>
		public bool autoUA = true;
		/// <summary>
		/// 获取每次请求的时间间隔
		/// </summary>
		public int requestInterval
		{
			get
			{
				return rand.Next(15, 20);
			}
		}
		protected HttpWebRequest request;
		protected HttpWebResponse response;
		protected RandomUserAgent randUA = new RandomUserAgent();

		/// <summary>
		/// 获取或设置请求的目标URL链接
		/// </summary>
		public string url { get; set; }
		/// <summary>
		/// 获取或设置当前的UA标识
		/// </summary>
		public string UserAgent
		{
			get
			{
				return request.UserAgent;
			}
			set
			{
				request.UserAgent = value;
			}
		}
		/// <summary>
		/// 获取或设置当前请求的引用来源
		/// </summary>
		public string Referer
		{
			get
			{
				return request.Referer;
			}
			set
			{
				request.Referer = value;
			}
		}
		public string Host
		{
			get { return request.Host; }
			set { request.Host = value; }
		}
		/// <summary>
		/// 获取或设置当前请求接受的数据格式
		/// </summary>
		public string Accept
		{
			get
			{
				return request.Accept;
			}
			set
			{
				request.Accept = value;
			}
		}
		/// <summary>
		/// 获取或设置当前请求的Cookies缓存信息
		/// </summary>
		public CookieContainer Cookies
		{
			get
			{
				return request.CookieContainer;
			}
			set
			{
				request.CookieContainer = value;
			}
		}
		/// <summary>
		/// 获取或设置当前请求的Connection值
		/// </summary>
		public string Connection
		{
			get
			{
				return request.Connection;
			}
			set
			{
				request.Connection = value;
			}
		}
		public bool KeepAlive
		{
			get
			{
				return request.KeepAlive;
			}
			set
			{
				request.KeepAlive = value;
			}
		}
		/// <summary>
		/// 获取或设置当前的请求方法
		/// </summary>
		public string Method
		{
			get
			{
				return request.Method;
			}
			set
			{
				request.Method = value;
			}
		}
		/// <summary>
		/// 获取或设置当前请求的普通请求报头
		/// </summary>
		public WebHeaderCollection Headers
		{
			get
			{
				return request.Headers;
			}
			set
			{
				request.Headers = value;
			}
		}
		/// <summary>
		/// 获取或设置当前请求允许等待的最多响应时长
		/// </summary>
		public int Timeout
		{
			get
			{
				return request.Timeout;
			}
			set
			{
				request.Timeout = value;
			}
		}
		private Random rand = new Random(DateTime.Now.GetHashCode());

		/// <summary>
		/// 获取请求返回的响应流
		/// </summary>
		/// <returns>数据流：<seealso cref="Stream"/></returns>
		public Stream content
		{
			get
			{
				using (MemoryStream ms = new MemoryStream() { Position = 0 })
				{
					getContentStream().CopyTo(ms);
					return ms;
				}					
			}
		}
		/// <summary>
		/// 获取请求返回的字符串报文
		/// </summary>
		/// <returns>数据报文：<seealso cref="string"/></returns>
		public string contentDocument
		{
			get
			{
				return new StreamReader(getContentStream()).ReadToEnd();
			}
		}
		/// <summary>
		/// 获取请求返回的响应字节数组
		/// </summary>
		/// <returns>字节流数组：<seealso cref="byte[]"/></returns>
		public byte[] contentBytes
		{
			get
			{
				var ms = (MemoryStream)content;
				return ms.ToArray();
			}
		}

		// 函数
		/// <summary>
		/// 将字符串转义为URI字符串
		/// </summary>
		/// <param name="input">待转换的字符串</param>
		/// <returns>转义后的字符串：<seealso cref="string"/></returns>
		public static string EscapeDataString(string input)
		{
			return Uri.EscapeDataString(input);
		}
		/// <summary>
		/// 将URI字符串反转义为文本字符串
		/// </summary>
		/// <param name="input">待反转换的字符串</param>
		/// <returns>反转义后的字符串：<seealso cref="string"/></returns>
		public static string UnescapeDataString(string input)
		{
			return Uri.UnescapeDataString(input);
		}
		public void post(string content)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(content);
			request.Method = "POST";
			request.ContentLength = bytes.Length;
			var requestStream = request.GetRequestStream();
			requestStream.Write(bytes, 0, bytes.Length);
			requestStream.Close();
		}

		/// <summary>
		/// 获取请求返回的响应流
		/// </summary>
		/// <returns>数据流：<seealso cref="Stream"/></returns>
		private Stream getContentStream()
		{
			for(int i = 1; i <= 6; i ++)
			{
				if (autoUA) { UserAgent = computerUAsign ? randUA.nextComputerUA : randUA.nextCellphoneUA;}
				try 
				{
					response = (HttpWebResponse)request.GetResponse();
					break;
				}
				catch (Exception)
				{
					System.Threading.Thread.Sleep(requestInterval);
					if(! (i == 6)) { continue; };
					throw;
				}
			}
			return response.GetResponseStream();
		}
		/// <summary>
		/// 返回 1970年1月1日 到现在的协调世界时(UTC)毫秒数的差值
		/// </summary>
		/// <returns>间隔毫秒数<see cref="long"/></returns>
		public long getTime()
		{
			var origin = DateTime.Parse("1970/01/01").ToUniversalTime().AddHours(8.0);
			long diff = DateTime.Now.ToUniversalTime().Ticks - origin.Ticks;
			return diff;
		}
		public HttpWebResponse getResponse()
		{ return (HttpWebResponse)request.GetResponse(); }
	}
}
