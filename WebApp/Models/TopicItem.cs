using System.ComponentModel.DataAnnotations;

namespace WebApp.Models;

public class TopicItem
{
    [Key] 
    public int ItemId { get; set; }
    public string Name { get; set; }
    public int TopicId { get; set; }
    public string ProfilePicture { get; set; } 
}