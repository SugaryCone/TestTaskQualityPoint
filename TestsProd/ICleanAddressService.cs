
using System.Threading.Tasks;

namespace TestsProd
{
    public interface ICleanAddressService
    {
        public Task<AddressResponse> GetCleanAddressAsync(string address);
       
    }
}
