﻿using Microsoft.AspNetCore.Identity;

namespace BlazorEcommerce.Identity;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
}
