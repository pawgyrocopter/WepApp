using WebApp.Data;
using WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Services;

public class RoleCheck
{
    public ApplicationDbContext _context;

    public RoleCheck(ApplicationDbContext context)
    {
        _context = context;
    }

    public bool UserIdCheck(int UserId)
    {

        foreach (var i in _context.Users)
        {
            if (i.Id == UserId)
            {
                return true;
            }
        }
        return false;
    }
}