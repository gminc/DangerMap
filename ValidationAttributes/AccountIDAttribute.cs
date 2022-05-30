﻿using DangerMap.Dtos;
using DangerMap.Models.db;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DangerMap.ValidationAttributes
{
    public class AccountIDAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            uploadtestv1Context context = (uploadtestv1Context)validationContext.GetService(typeof(uploadtestv1Context));

            var AccountID = (string)value;

            if (!Global.LegalIdAndPassword.IsMatch(AccountID))
            {
                return new ValidationResult("輸入之 ID 不符合規範");
            }

            var findId = from a in context.AccountProfiles
                         where a.AccountId == AccountID
                         select a;

            var dto = validationContext.ObjectInstance;

            if (dto.GetType() == typeof(AccountProfileInputDto))
            {
                var updateDto = (AccountProfileInputDto)dto;
                findId = findId.Where(a => a.AccountId != updateDto.AccountId);
            }

            if (findId.FirstOrDefault() == null)
            {
                return new ValidationResult("查無此ID");
            }

            return ValidationResult.Success;
        }
    }
}
