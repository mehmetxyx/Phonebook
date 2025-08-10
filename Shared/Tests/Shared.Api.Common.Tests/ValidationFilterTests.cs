using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using NSubstitute;
using Shared.Api.Common;

public class ValidationFilterTests
{
    [Fact]
    public void OnActionExecuting_WithValidModelState_DoesNothing()
    {
        var modelState = new ModelStateDictionary();
        var actionContext = new ActionContext
        {
            HttpContext = new DefaultHttpContext(),
            RouteData = new RouteData(),
            ActionDescriptor = new ActionDescriptor()
        };

        actionContext.ModelState.Merge(modelState);

        var context = Substitute.For<ActionExecutingContext>(
            actionContext,
            new List<IFilterMetadata>(),
            new Dictionary<string, object>(),
            new object()
        );

        var filter = new ValidationFilter();

        filter.OnActionExecuting(context);

        Assert.True(context.ModelState.IsValid);
    }

    [Fact]
    public void OnActionExecuting_WithInvalidModelState_ReturnsBadRequest()
    {
        var modelState = new ModelStateDictionary();
        modelState.AddModelError("Name", "Name is required");
        modelState.AddModelError("Company", "Company is required");

        var actionContext = new ActionContext
        {
            HttpContext = new DefaultHttpContext(),
            RouteData = new RouteData(),
            ActionDescriptor = new ActionDescriptor()
        };

        actionContext.ModelState.Merge(modelState);

        var context = Substitute.For<ActionExecutingContext>(
            actionContext,
            new List<IFilterMetadata>(),
            new Dictionary<string, object>(),
            new object()
        );

        var filter = new ValidationFilter();

        filter.OnActionExecuting(context);

        var result = Assert.IsType<BadRequestObjectResult>(context.Result);
        var response = Assert.IsType<ApiResponse<string>>(result.Value);

        Assert.False(response.IsSuccessful);
    }
}