using System.ComponentModel.DataAnnotations;

namespace WebApp.Models;

public class TopicViewModel
{
    [Key]
    public int TopicId { get; set; }
    public string Name { get; set; }
    public string Info { get; set; }
    public int UserId { get; set; }
    public List<TopicItem> Items { get; set; }
}