using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace asp.net_core_start {
	internal struct DataCore {
		internal struct DataFiles {
			static internal DataFile.ConfigFile config=new();
		}
	}
	public struct DataFile {
		/// <summary>
		/// 将配置数据保存至配置文件中
		/// </summary>
		internal static void SaveData() {
			ISerializer yamlS = new SerializerBuilder()
				.WithNamingConvention(CamelCaseNamingConvention.Instance)
				.Build();
			File.WriteAllText(FilePath.configFile, yamlS.Serialize(DataCore.DataFiles.config));
		}
		/// <summary>
		/// 读取数据文件并将数据写入实例中
		/// </summary>
		internal static void ReadData() {
			IDeserializer yamlD = new DeserializerBuilder()
				.WithNamingConvention(CamelCaseNamingConvention.Instance)
					.Build();

			DataCore.DataFiles.config = yamlD.Deserialize<ConfigFile>(File.ReadAllText(FilePath.configFile));
		}
		public struct ConfigFile {
			public struct WebsiteModel {
				public required string Addr { get; set; }
				private string urlRoot;
				public required string UrlRoot {
					readonly get => urlRoot;
					//对UrlRoot的值进行格式化
					set {
						string urlRoot_;
						if (value == "/") urlRoot_ = "";//urlRoot不能只包含单独的斜杠
						else if (value == "") { urlRoot_ = value; }//如果为空则直接输出
						else {
							if (value[..1] == "/")
								urlRoot_ = value;
							else//如果开头没有斜杠则加上斜杠
								urlRoot_ = "/" + value;
							if (urlRoot_.Substring(urlRoot_.Length - 1, 1) == "/")//如果末尾包含斜杠则去掉
								urlRoot_ = urlRoot_[..(urlRoot_.Length - 1)];
						}
						urlRoot = urlRoot_;
					}
				}
				
				public required int Port { get; set; }
				public required bool UseHttps { get; set; }
				public required bool UseXFFRequestHeader { get; set; }
				/* UseXFFRequestHeader
				 * 后续用于获取客户端IP时可能会用到的选项
				 string GetClientIP() {
					if (Config.UseXFFRequestHeader)
						return Request.Headers["X-Forwarded-For"].ToString();
					else
						return HttpContext.Connection.RemoteIpAddress!.ToString();
				 }
				 */
			}
			public required WebsiteModel Website { get; set; }
			public required int DebugOutput { get; set; }
			public required bool UpdateConfig { get; set; }

			[SetsRequiredMembers]
			public ConfigFile() {
				Website = new() {
					Addr = "*",
					UrlRoot = "/",
					Port = 80,
					UseHttps = false,
					UseXFFRequestHeader = false
				};
				DebugOutput = 0;
				UpdateConfig = false;
			}
		}
	}
	internal struct FilePath {
		internal const string dataDir = "xxx_data/";//此处为服务端的数据文件目录
		internal const string configFile = dataDir + "config.yml";
	}
}
