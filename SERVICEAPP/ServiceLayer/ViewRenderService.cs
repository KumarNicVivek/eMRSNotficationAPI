using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVICEAPP.ServiceLayer
{
    public class ViewRenderService : IViewRenderService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ICompositeViewEngine _viewEngine;
        private readonly ITempDataProvider _tempDataProvider;

        public ViewRenderService(
        IServiceProvider serviceProvider,
        ICompositeViewEngine viewEngine,
        ITempDataProvider tempDataProvider)
        {
            _serviceProvider = serviceProvider;
            _viewEngine = viewEngine;
            _tempDataProvider = tempDataProvider;
        }
        public async Task<string> RenderToStringAsync(string viewName, object model)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var httpContext = new DefaultHttpContext
                {
                    RequestServices = scope.ServiceProvider
                };

                var actionContext = new ActionContext(
                    httpContext,
                    new RouteData(),
                    new ActionDescriptor()
                );

                using (var writer = new StringWriter())
                {
                    var viewResult = _viewEngine.GetView(null, viewName, false);

                    if (viewResult.View == null)
                        throw new Exception($"View '{viewName}' not found.");

                    // ✅ Fresh ViewData
                    var viewData = new ViewDataDictionary(
                        new EmptyModelMetadataProvider(),
                        new ModelStateDictionary())
                    {
                        Model = model
                    };

                    // ✅ Fresh TempData
                    var tempData = new TempDataDictionary(
                        httpContext,
                        _tempDataProvider
                    );

                    var viewContext = new ViewContext(
                        actionContext,
                        viewResult.View,
                        viewData,
                        tempData,
                        writer,
                        new HtmlHelperOptions()
                    );

                    await viewResult.View.RenderAsync(viewContext);

                    return writer.ToString();
                }
            }
        }
    }
}
