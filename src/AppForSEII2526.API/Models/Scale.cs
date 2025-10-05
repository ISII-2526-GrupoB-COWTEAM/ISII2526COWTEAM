namespace AppForSEII2526.API.Models
{
    public class Scale
    {


        public Scale() { }

        public Scale (string Name, int Id)
        {
            this.Name = Name;
            this.Id = Id;
        }

        public int Id { get; set; }


        [StringLength(50, ErrorMessage = "Scale name cannot be longer than 50 characters.", MinimumLength = 3)]
        public string Name { get; set; }


        public IList<Repair> Repairs { get; set; } = new List<Repair>();

    }
}
