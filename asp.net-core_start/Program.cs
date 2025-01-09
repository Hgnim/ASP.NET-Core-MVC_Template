using static asp.net_core_start.DataCore;

namespace asp.net_core_start
{
    public class Program
    {
        public static void Main(string[] args)
        {
			if (!Directory.Exists(FilePath.dataDir)) Directory.CreateDirectory(FilePath.dataDir);

			if (File.Exists(FilePath.configFile)) {
				try {
					DataFile.ReadData();
				} catch (Exception ex) { Console.WriteLine($"���������ļ�ʱ���ִ���ԭ��: {ex.Message}"); return; }

				if (DataCore.DataFiles.config.UpdateConfig == true) {
					DataCore.DataFiles.config.UpdateConfig = false;
					DataFile.SaveData();
					Console.WriteLine("�����ļ��Ѹ��£����˳������");
					return;
				}
			}
			else {
				DataFile.SaveData();
			}


			var builder = WebApplication.CreateBuilder(args);
			// Add services to the container.
			builder.Services.AddControllersWithViews();
			builder.WebHost.UseUrls($"{(DataFiles.config.Website.UseHttps?"https":"http")}://{DataFiles.config.Website.Addr}:{DataFiles.config.Website.Port}");
			var app = builder.Build();
			app.UsePathBase(DataFiles.config.Website.UrlRoot);
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
				pattern: "{controller=InputMsg}/{action=Index}"); // /{id?}");

			app.Run();
		}
    }
}
