namespace LuckySlots.Services.Models
{
    public class UserListViewModel
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public bool IsAdmin { get; set; }

        public bool IsSupport { get; set; }

        public bool IsAccountLocked { get; set; }
    }
}
