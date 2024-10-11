using System;

public class UpdateUserRequest
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public string? ProfilePhoto { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Address { get; set; } // Address field

    public string? Gender { get; set;}
}

