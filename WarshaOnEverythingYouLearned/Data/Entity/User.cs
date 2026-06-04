namespace WarshaOnEverythingYouLearned.Data.Entity
{
    public class User
    { 
        public int Id { get; set; }
        public string Name { get; set; }
        public string email { get; set; }

        public string password { get; set; }
        public bool isActived { get; set; }
        
        public int RoleId { get; set; }
        public Role? roles { get; set; }
       
    }
}
