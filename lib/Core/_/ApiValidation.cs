using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace InnateGlory.Api
{
    class xxxValidationAttribute : ValidationAttribute, IClientModelValidator
    {
        public override bool RequiresValidationContext => base.RequiresValidationContext;

        public override bool IsValid(object value)
        {
            return base.IsValid(value);
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            return base.IsValid(value, validationContext);
        }

        void IClientModelValidator.AddValidation(ClientModelValidationContext context)
        {
            throw new NotImplementedException();
        }
    }

    class xxxModel : IValidatableObject
    {
        IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }
    }
}
