using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace TimeOrganizer.Utilities
{
    public class EndGreatedThenStartDateAttribute : ValidationAttribute
    {
        private string otherProperty;

        public EndGreatedThenStartDateAttribute(string otherProperty, string errorMessage) : base(errorMessage)
        {
            if (otherProperty == null) {
                throw new ArgumentException("otherProperty");
            }

            this.otherProperty = otherProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ValidationResult result = ValidationResult.Success;

            var field = validationContext.ObjectType.GetProperty(otherProperty);
            object otherPropertyValue = field.GetValue(validationContext.ObjectInstance, null);
            
            DateTime startTimeValue = (DateTime)value;
            DateTime endTimeValue = (DateTime)otherPropertyValue;

            if (startTimeValue == null) {
                return new ValidationResult("Start time is empty");    
            }
            if (endTimeValue == null) {
                return new ValidationResult("End time is empty");
            }

            if (startTimeValue >= endTimeValue) {
                return new ValidationResult("Start date must be earlier then end date");
            }

            return result;
        }
    }
}
