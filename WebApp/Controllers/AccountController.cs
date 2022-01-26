using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Controllers;

public class AccountController : Controller
{
    private ApplicationDbContext _context;

    public AccountController(ApplicationDbContext context)
    {
        _context = context;
    }
    
    [Authorize]
    public IActionResult Profile()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterModel model)
    {
        if (ModelState.IsValid)
        {
            User user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
            if (user == null)
            {
                // добавляем пользователя в бд
                user = new User {Email = model.Email, Password = model.Password, FirstName = model.FirstName, LastName = model.LastName};
                Role userRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "user");
                if (userRole != null)
                    user.Role = userRole;
                user.Topics = new List<Topic>();
                user.Topics.Add(new Topic(){Name = "qweqe", Info = "qqqq"});
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                await Authenticate(user); // аутентификация

                return RedirectToAction("Index", "Home");
            }
            else
                ModelState.AddModelError("", "Некорректные логин и(или) пароль");
        }

        return View(model);
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginModel model)
    {
        if (ModelState.IsValid)
        {
            User user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == model.Email && u.Password == model.Password);
            if (user != null)
            {
                await Authenticate(user); // аутентификация

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Некорректные логин и(или) пароль");
        }

        return View(model);
    }

    private async Task Authenticate(User user)
    {
        // создаем один claim
        var claims = new List<Claim>
        {
            new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
            new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role?.Name)
        };
        // создаем объект ClaimsIdentity
        ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType,
            ClaimsIdentity.DefaultRoleClaimType);
        // установка аутентификационных куки
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
    }

    public void ShowInfo()
    {
       
    } 
}