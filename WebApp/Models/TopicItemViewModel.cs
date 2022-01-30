using System.ComponentModel.DataAnnotations;

namespace WebApp.Models;

public class TopicItemViewModel
{
    [Key] 
    public int ItemId { get; set; }
    public string Name { get; set; }
    public int TopicId { get; set; }
    public IFormFile ProfileImage { get; set; } 
}