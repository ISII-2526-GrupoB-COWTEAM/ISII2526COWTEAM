namespace AppForSEII2526.API.Models
{
    using System.ComponentModel.DataAnnotations;
    using Microsoft.EntityFrameworkCore;

    [Index(nameof(NameModel), IsUnique = true)]
    public class Model
    {
        public Model()
        {
        }

        public Model(string nameModel)
        {
            NameModel = nameModel;
        }

        public int Id { get; set; }

        [StringLength(50, ErrorMessage = "Model name cannot be longer than 50 characters.", MinimumLength = 3)]
        public string NameModel { get; set; }

        // Relación: un modelo puede estar asociado a varios dispositivos
        public IList<Device> Devices { get; set; } = new List<Device>();
    }
}
