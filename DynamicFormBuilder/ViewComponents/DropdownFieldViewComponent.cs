using Microsoft.AspNetCore.Mvc;
using DynamicFormBuilder.Models;
using Newtonsoft.Json;


namespace DynamicFormBuilder.ViewComponents
{
    public class DropdownFieldViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(FormFieldModel field)
        {
            return View(field);
        }
    }
}