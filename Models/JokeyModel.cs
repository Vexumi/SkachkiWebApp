using System.ComponentModel;

public class JokeyModel
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public DateTime CreationDate { get; set; }

    public string? Name { get; set; }
    [DisplayName("Date Of Birth")]
    public DateOnly? DOB { get; set; } // date of birth day
    public int? Rating { get; set; }

}