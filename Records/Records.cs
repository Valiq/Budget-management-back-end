﻿using Budget_management_back_end.Models;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;

namespace Budget_management_back_end.Records
{
    public class Records
    {
        public record UserRequest(
           [Required]
           [MaxLength(255)]
           string name,

           [Required]
           [EmailAddress]
           [MaxLength(255)]
           string email,

           [Required]
           [MinLength(8)]
           [MaxLength(255)]
           [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$")]
           string password
        );

        public record Login(
           [Required]
           [EmailAddress]
           [MaxLength(255)]
           string email,

           [Required]
           [MinLength(8)]
           [MaxLength(255)]
           [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$")]
           string password
        );

        public record AccountRequest(
            [Required]
            [MaxLength(255)]
            string name,
            string description
        );

        public record FinanceEntityRequest(
            [Required]
            long accountId,

            [Required]
            [MaxLength(255)]
            string name,
            string description
        );

        public record BalanceRequest(
            [Required]
            long financeEntityId,

            [Required]
            [MinLength(3)]
            [MaxLength(30)]
            string currencyCode,

            [Required]
            [MaxLength(255)]
            string name,

            [Required]
            decimal sum
        );

        public record UpdateEmailRequest(
            [Required]
            [EmailAddress]
            [MaxLength(320)]
            string email
        );

        public record UpdatePasswordRequest(
            [Required]
            [MinLength(8)]
            [MaxLength(255)]
            [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$")]
            string password
        );

        public record UpdaeUserRequest(
            [MaxLength(255)]
            string name
        );

        public record UpdateAccountRequest(
            [MaxLength(255)]
            string name,
            string description
        );

        public record UpdateFinanceEntityRequest(
            [MaxLength(255)]
            string name,
            string description
        );
    }
}