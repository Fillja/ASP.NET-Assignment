using Infrastructure.Helpers;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Models.Account;

public class AccountSecurityDeleteModel
{
    [Display(Name = "Yes I want to delete my account.")]
    [CheckBoxRequired(ErrorMessage = "You must confirm your deletion.")]
    [Required(ErrorMessage = "You must confirm your deletion.")]
    public bool IsChecked { get; set; }
}
