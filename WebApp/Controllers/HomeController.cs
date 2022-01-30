using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SQLitePCL;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment webHostEnvironment;  


    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context,IWebHostEnvironment hostEnvironment)
    {
        _logger = logger;
        _context = context;
        webHostEnvironment = hostEnvironment;  

    }

    public IActionResult Index()
    {
        return View();
    }
    
    [Authorize(Roles = "admin, user")]
    public IActionResult Privacy()
    {
        string role = User.FindFirst(x => x.Type == ClaimsIdentity.DefaultRoleClaimType).Value;
        return Content($"ваша роль: {role}");
    }

    
   
    
    


    public IActionResult Collections()
    {
        return RedirectToAction("Index", "Topic");
    }
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }
}