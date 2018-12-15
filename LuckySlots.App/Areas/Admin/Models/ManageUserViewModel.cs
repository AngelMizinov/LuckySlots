namespace LuckySlots.App.Areas.Admin.Models
{
    public class ManageUserViewModel
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public bool IsAdmin { get; set; }

        public bool IsSupport { get; set; }

        public bool IsLocked { get; set; }
    }
}
