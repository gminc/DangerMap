using DangerMap.Dtos;
using DangerMap.Models.db;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;

namespace DangerMap.ValidationAttributes
{
    public class UserMessageAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            uploadtestv1Context context = (uploadtestv1Context)validationContext.GetService(typeof(uploadtestv1Context));

            var eventDescription = (string)value;

            if (IsLegalWord().IsMatch(eventDescription))
            {
                return new ValidationResult("內容包含不合法文字");
            }

            return ValidationResult.Success;
        }

        public Regex IsLegalWord()
        {
            Regex dirtyWord = new Regex("{[(習近平)|(小熊維尼)|(426)]}");
            return dirtyWord;
        }
    }
}
