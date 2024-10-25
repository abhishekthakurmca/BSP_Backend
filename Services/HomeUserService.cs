using AutoMapper;
using MyBackendApp.CommonResult;
using MyBackendApp.Data;
using MyBackendApp.Dto.Home;
using MyBackendApp.IServices;
using MyBackendApp.Models;
using MyBackendApp.Models.HomeProfile;
using MyBackendApp.Utils;

namespace MyBackendApp.Services;

public class HomeUserService : IHomeUserService
{
    private readonly IEmailService _emailService;
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public HomeUserService(IEmailService emailService, IMapper mapper, ApplicationDbContext context)
    {
        _emailService = emailService;
        _mapper = mapper;
        _context = context;
    }

    public bool ActivateUser(ActivationRequestDto request)
    {
        if (!string.IsNullOrEmpty(request.Email) && !string.IsNullOrEmpty(request.ActivationCode))
        {
            var activationCode = _context.HomeUsers.FirstOrDefault(e => e.Email == request.Email)!.ActivationToken;
            if (activationCode.ToString() == request.ActivationCode)
                return true;
        }
        return false;
    }

    public async Task<ResponseModel<string>> RegisterUser(HomeUserDto user)
    {
        var response = new ResponseModel<string>();
        user.ActivationToken = Guid.NewGuid();
        user.Pwd = SharedResource.HashPassword(user.Pwd);

        if (_context.HomeUsers.Any(e => e.Email == user.Email))
        {
            response.Message.Add(SharedResource.UserAlreadyExist);
            return response;
        }

        if (response.Message.Count == 0)
        {
            await _context.HomeUsers.AddAsync(_mapper.Map<HomeUser>(user));
            await _context.SaveChangesAsync();

            var homeUserProfile = new HomeUserProfileModel()
            {
                UserId = _context.HomeUsers.FirstOrDefault(e => e.Email == user.Email)!.user_id
            };
            await _context.HomeUserProfile.AddAsync(homeUserProfile);
            await _context.SaveChangesAsync();

            if (user.ActivationToken != Guid.Empty && user.Email != null)
            {
                await _emailService.SendActivationEmailAsync(user.Email, user.ActivationToken);
                response.Message.Add(SharedResource.UserRegisteredCheckEmail);
            }
        }
        return response;
    }
}
