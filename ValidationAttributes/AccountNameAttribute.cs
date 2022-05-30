using DangerMap.Models;
using DangerMap.Models.db;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DangerMap.ValidationAttributes
{
    public class AccountNameAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            uploadtestv1Context context = (uploadtestv1Context)validationContext.GetService(typeof(uploadtestv1Context));

            var name = (string)value;

            if (name.Length > 16)
            {
                return new ValidationResult("輸入之名稱超過 16 個字");
            }
            return ValidationResult.Success;
        }
    }
}
