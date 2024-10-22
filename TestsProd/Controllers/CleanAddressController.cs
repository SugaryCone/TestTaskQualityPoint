using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

namespace TestsProd
{
    [Route("api/[controller]")]
    [ApiController]
    public class CleanAddressController : ControllerBase
    {
        private readonly ICleanAddressService _addressService;
        private readonly ILogger<CleanAddressController> _Logger;
        private readonly IMapper _mapper;



        public CleanAddressController(ICleanAddressService addressService, ILogger<CleanAddressController> Logger, IMapper mapper)
        {
            _addressService = addressService;
            _Logger = Logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<AddressModel>> GetAddress([FromQuery] string input)
        {
            _Logger.LogInformation("GET address");
            var addressResponse = await _addressService.GetCleanAddressAsync(input);

            if(addressResponse.qc != 0)
            {
                throw new GarbageAdressException();
            }
            if (addressResponse.fias_level < 9)
            {
                
                throw new FiasLevelException();
            }

            var addressModel = _mapper.Map<AddressModel>(addressResponse);

            return Ok(addressModel);
        }
    }
}
