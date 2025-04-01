using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace mvc_todolist.Commons.ViewComponents
{
    public class ActionListViewComponent : ViewComponent
    {
        private readonly IActionDescriptorCollectionProvider _actionDescriptorProvider;

        public ActionListViewComponent(IActionDescriptorCollectionProvider actionDescriptorProvider)
        {
            _actionDescriptorProvider = actionDescriptorProvider;
        }

        public IViewComponentResult Invoke(string controllerName)
        {
            var actions = _actionDescriptorProvider.ActionDescriptors.Items
                .OfType<ControllerActionDescriptor>()
                .Where(a => a.ControllerName == controllerName)
                .Select(a => a.ActionName)
                .Distinct()
                .ToList();
            ViewBag.ControllerName = controllerName;
            return View(actions);
        }
    }
}
