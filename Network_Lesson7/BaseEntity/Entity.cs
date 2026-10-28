namespace Network_Lesson7_Shared.BaseEntity;

public class Entity
{
    public int Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
    public Entity()
    {
        CreatedDate = DateTime.Now;
    }
}