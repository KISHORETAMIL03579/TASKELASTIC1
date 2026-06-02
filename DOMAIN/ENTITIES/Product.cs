using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DOMAIN.Entities
{
    public class Product : IValidatableObject
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public double Price { get; set; }

        public int Stock { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var errors = new List<ValidationResult>();

            if (string.IsNullOrWhiteSpace(Name))
            {
                errors.Add(new ValidationResult("Name is required", new[] { nameof(Name) }));
            }

            if (Price <= 0)
            {
                errors.Add(new ValidationResult("Price must be greater than 0", new[] { nameof(Price) }));
            }

            if (Stock < 0)
            {
                errors.Add(new ValidationResult("Stock cannot be negative", new[] { nameof(Stock) }));
            }

            return errors;
        }
    }
}