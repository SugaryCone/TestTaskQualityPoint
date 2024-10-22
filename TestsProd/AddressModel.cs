using AutoMapper;



namespace TestsProd
{
    public class AddressModel
    {
        public string Country { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string House { get; set; }
        public string Flat { get; set; }
    }

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

    public class AddressProfile : Profile
    {
        public AddressProfile()
        {
            CreateMap<AddressResponse, AddressModel>()
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.City) ? src.Region : src.City));


        }
    }
    }

