using DangerMap.Models;
using DangerMap.Models.db;
using System.ComponentModel.DataAnnotations;

namespace DangerMap.ValidationAttributes
{
    public class AccountPasswordAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            uploadtestv1Context context = (uploadtestv1Context)validationContext.GetService(typeof(uploadtestv1Context));

            var password = (string)value;

            if (!Global.LegalIdAndPassword.IsMatch(password))
            {
                return new ValidationResult("輸入之密碼不符合規範");
            }
            return ValidationResult.Success;
        }

    }
}
