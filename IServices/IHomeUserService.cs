using MyBackendApp.CommonResult;
using MyBackendApp.Dto.Home;

namespace MyBackendApp.IServices;

public interface IHomeUserService
{
    public Task<ResponseModel<string>> RegisterUser(HomeUserDto user);
    public bool ActivateUser(ActivationRequestDto request);
}
