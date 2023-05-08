using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Converters;

namespace AdminCustomerAPI.Helpers
{
    public class Converts
    {
        public class CustomDateTimeConvert: IsoDateTimeConverter
        {
            public CustomDateTimeConvert() 
            {
                DateTimeFormat = "yyyy-MM-dd";
            }
        }
    }
}
