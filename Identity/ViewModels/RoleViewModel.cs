using System.ComponentModel.DataAnnotations;

namespace Identity.ViewModels
{
    public class RoleViewModel
    {
        [Required]
        public string Name { get; set; }
    }
}
