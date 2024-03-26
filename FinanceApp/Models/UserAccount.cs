using System.ComponentModel.DataAnnotations;

public class UserAccount
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Email { get; set; }

    public bool IsSetup { get; set; }
}