namespace Core
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<Student> Students { get; set; } = new List<Student>();

        public override string ToString()
        {
            return $"Group: {Name}; Id: {Id}; Students: {Students.Count}";
        }
    }
}
