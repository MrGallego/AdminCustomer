using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel.DataAnnotations;
using static AdminCustomerAPI.Helpers.Converts;

namespace AdminCustomerAPI.Repository.Dto
{
    public class CustomerUpdateDto
    {
        [Required]
        public string TipoIdentificacion { get; set; }
        [Required]
        public int NumeroIdentificacion { get; set; }
        [Required]
        public string Nombres { get; set; }
        [Required]
        public string Apellidos { get; set; }
        [Required]
        public string Correo { get; set; }

        [JsonConverter(typeof(CustomDateTimeConvert))]
        [Required]
        public DateTime FechaNacimiento { get; set; }
    }


}
