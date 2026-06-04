namespace WarshaOnEverythingYouLearned.Data.Entity
{
    public class Role
    { 
        public int Id { get; set; }
        public string Name { get; set; }    

  public  ICollection<RolePermission> ?rolePermissions { get; set; } =new HashSet<RolePermission>();
    }
}
