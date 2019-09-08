namespace PTMS.BusinessLogic.Models.User
{
    public class UserLightModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        public string LongName => $"{LastName} {FirstName} {MiddleName}";

        public string Description { get; set; }

        public int RoleId { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public int? ProjectId { get; set; }

        public UserStatusModel Status { get; set; }
    }
}
