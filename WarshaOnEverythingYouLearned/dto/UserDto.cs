namespace WarshaOnEverythingYouLearned.dto
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public string Password { get; set; }
        public bool IsActived { get; set; }

        public int RoleId { get; set; }
    }
}
