
namespace TestsProd
{
    public class AddressResponse
    {
        public int qc { get; set; }

        public int fias_level { get; set; }
        public string Region_type { get; set; }
        public string Region { get; set; }

        public string? Country { get; set; }
        public string? City { get; set; }
        public string? Street { get; set; }
        public int? House { get; set; }
        public int? Flat { get; set; }
    }
}
