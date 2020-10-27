using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TimeOrganizer.Utilities
{
    public class DatesAreSameDayAttribute : ValidationAttribute
    {
        private string otherProperty;

        public DatesAreSameDayAttribute(string otherProperty)
        {
            if (otherProperty == null)
            {
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

            if (startTimeValue.Day == endTimeValue.Day && startTimeValue.Month == endTimeValue.Month && startTimeValue.Year == endTimeValue.Year) {
                return result;
            }

            return new ValidationResult("Start time and End time are not on same day");
        }
    }
}
