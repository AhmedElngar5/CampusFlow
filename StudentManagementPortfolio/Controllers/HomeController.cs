using Microsoft.AspNetCore.Mvc;

namespace StudentManagementPortfolio.Controllers;

public class HomeController : Controller
{
    [Route("Home/Error/{statusCode?}")]
    public IActionResult Error(int? statusCode)
    {
        ViewBag.StatusCode = statusCode ?? 500;
        ViewBag.Message = statusCode switch
        {
            404 => "The page you're looking for doesn't exist or has been moved.",
            403 => "You don't have permission to access this resource.",
            500 => "Something went wrong on our end. Please try again later.",
            _ => "An unexpected error occurred."
        };
        ViewBag.Title = statusCode switch
        {
            404 => "Page not found",
            403 => "Access denied",
            _ => "Error"
        };
        Response.StatusCode = statusCode ?? 500;
        return View("Error");
    }
}
