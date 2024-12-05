﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace ddac_bookmate.Areas.Identity.Data;

// Add profile data for application users by adding properties to the ddac_bookmateUser class
public class ddac_bookmateUser : IdentityUser
{
    [PersonalData]
    public string? CustomerFullName { get; set; }
    [PersonalData]
    public int CustomerAge { get; set; }
    [PersonalData]
    public string? CustomerAddress { get; set; }
    [PersonalData]
    public DateTime CustomerDOB { get; set; }  
}

