using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Controllers;

public class ItemController : Controller
{
    // GET
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment webHostEnvironment;  

    public ItemController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
    {
        _context = context;
        webHostEnvironment = hostEnvironment;
    }
    
    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]  
    [ValidateAntiForgeryToken]  
    public async Task<IActionResult> Create(int id,TopicItemViewModel model)  
    {  
        if (ModelState.IsValid)  
        {  
            string uniqueFileName = UploadedFile(model);  
  
            TopicItem topicItem = new TopicItem  
            {  
                Name = model.Name,  
                TopicId = id,
                ProfilePicture = uniqueFileName,  
            };  
            _context.TopicItems.Add(topicItem);  
            await _context.SaveChangesAsync();  
            return RedirectToAction("Edit", "Topic", new {id = topicItem.TopicId});  
        }  
        return View();  
    }  
    private string UploadedFile(TopicItemViewModel model)  
    {  
        string uniqueFileName = null;  
  
        if (model.ProfileImage != null)  
        {  
            string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");  
            uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ProfileImage.FileName;  
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);  
            using (var fileStream = new FileStream(filePath, FileMode.Create))  
            {  
                model.ProfileImage.CopyTo(fileStream);  
            }  
        }  
        return uniqueFileName;  
    }  
    [Authorize]
    public async Task<IActionResult> Delete(string? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        
        var i = id.Split("_");
        int? itemId = int.Parse(i[0]);
        int? topicId = int.Parse(i[1]);
        
        var item = await _context.TopicItems.FindAsync(itemId);
        _context.TopicItems.Remove(item);
        _context.SaveChanges();
        if (topicId == null)
        {
            return NotFound();
        }
        return RedirectToAction("Edit", "Topic", new {id = topicId});
    }
}