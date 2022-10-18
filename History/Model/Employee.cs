namespace History.Model
{
    public class Employee
    {
        public int SrNo { get; set; }
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? City { get; set; }
        public int? isdeleted { get; set; }
    }
    public class Employeehis
    {
        public int HId { get; set; }
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? City { get; set; }
        public string? Operation { get; set; }
    }

}
