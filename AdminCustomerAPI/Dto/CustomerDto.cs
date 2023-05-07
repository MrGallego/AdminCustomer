using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel.DataAnnotations;
using static AdminCustomerAPI.Helpers.Converts;

namespace AdminCustomerAPI.Dto
{
    public class CustomerDto
    {
        [Required]
        public string TipoIdentificacion { get; set; }
        [Required]
        public int NumeroIdentificacion { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Correo { get; set; }

        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime FechaNacimiento { get; set; }
    }


}
