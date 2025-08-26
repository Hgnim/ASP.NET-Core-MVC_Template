using Microsoft.AspNetCore.Mvc;

namespace ASP.NET_Core_MVC_Template.Controllers {
	public class StartPageController : Controller {
		public IActionResult Index() {
			return View();
		}
	}
}
