using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TimeOrganizer.Utilities
{
    public class FutureDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ValidationResult result = ValidationResult.Success;

            if ((DateTime)value < DateTime.Now)
            {
                return new ValidationResult("You are not allowed to set tasks in past.");
            }

            return result;
        }
    }
}
