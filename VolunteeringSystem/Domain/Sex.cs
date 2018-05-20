using System;
using System.ComponentModel.DataAnnotations;
namespace VolunteeringSystem.Models
{   
    public enum Sex
    {
        [Display(Name = "Feminino")] Female,
        [Display(Name = "Masculino")] Male
    }
}
