namespace Identity.ViewModels
{
    public class UserRolesViewModel
    {
        public string UserId { get; set; }
        public string Username { get; set; }  
        public List<RolesSelectedViewModel> Roles { get; set; }
    }
}
