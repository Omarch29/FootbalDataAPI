using System.ComponentModel.DataAnnotations.Schema;

namespace Solstice.API.models
{
    [ComplexType]
    public class Address
    {
        public int Number { get; set; }
        
        public string Street { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public int ZipCode { get; set; }

        public override string ToString() {
            return $"{Number} {Street}, {City}, {State}, ZIPCODE: {ZipCode}";
        }
    }
}