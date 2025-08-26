using ASP.NET_Core_MVC_Template.Models;
using static ASP.NET_Core_MVC_Template.DataCore;
using static ASP.NET_Core_MVC_Template.Const.FilePath;

namespace ASP.NET_Core_MVC_Template
{
    public class Program
    {
        public static void Main(string[] args)
        {
			Console.WriteLine(
@$"服务端启动"
				);
			try {
				static void saveData() => ConfigModel.SaveData(configFile, config);

				if (!Directory.Exists(dataDir)) Directory.CreateDirectory(dataDir);

				if (!File.Exists(configFile))
					saveData();
				else
					config=ConfigModel.ReadData(configFile);

				if (config.UpdateConfig == true) {
					config.UpdateConfig = false;
					saveData();
					Console.WriteLine("配置文件已更新，已退出服务端");
					return;
				}
			} catch { Console.WriteLine("处理配置文件时出现错误!"); return; }


			var builder = WebApplication.CreateBuilder(args);
			builder.Services.AddControllersWithViews();

			builder.WebHost.UseUrls(config.Website.Url.Get());
			builder.Services.AddHttpContextAccessor();
			builder.Services.AddSession();

			var app = builder.Build();

			app.UsePathBase(config.Website.Url.UrlRoot);
			app.UseSession();

			/*// Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }*/
			app.UseHttpsRedirection();
			app.UseStaticFiles();
			app.UseRouting();
			app.UseAuthorization();
			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=StartPage}/{action=Index}"); // /{id?}");
			app.Run();
		}
    }
}
