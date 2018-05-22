using System;
using System.ComponentModel.DataAnnotations;
namespace VolunteeringSystem.Models
{   
    public enum Sex
    {
        [Display(Name = "Feminino")] f,
        [Display(Name = "Masculino")] m
    }

    public static class SexMethods
    {
        public static String GetString(this Sex s)
        {
            switch (s)
            {
                case Sex.f:
                    return "Feminino";
                case Sex.m:
                    return "Masculino";
                default:
                    return "";
            }
        }
    }
}
