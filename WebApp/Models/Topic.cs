using System.ComponentModel.DataAnnotations;

namespace WebApp.Models;

public class Topic
{
    [Key]
    public int TopicId { get; set; }
    public string Name { get; set; }
    public string Info { get; set; }

    public int UserId { get; set; }
}