﻿using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace MediWeb.Models;

public class MedicalEmployeeDetailsViewModel
{
    public long Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string ClinicName { get; set; }
    public string ClinicId { get; set; }

    public string FullName { get => FirstName + " " + LastName; }
}
