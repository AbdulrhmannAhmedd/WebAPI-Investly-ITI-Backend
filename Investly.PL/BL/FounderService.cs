using AutoMapper;
using Investly.DAL.Entities;
using Investly.DAL.Repos.IRepos;
using Investly.PL.Dtos;
using Investly.PL.General;
using Investly.PL.General.Services.IServices;
using Investly.PL.IBL;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Investly.PL.BL
{
    public class FounderService : IFounderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IHelper _helper;

        public FounderService(
            IUnitOfWork unitOfWork, 
            IMapper mapper,
            IUserService userService,
            IHelper helper
            )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userService = userService;
            _helper = helper;
        }
        public int ChangeFounderStatus(int Id, int Status,int ?LoggedInUser)
        {
            try
            {
                if(Id<=0||Status<=0)
                {
                    return -3;
                }
                var founder=_unitOfWork.FounderRepo.FirstOrDefault(x => x.Id == Id,"User");
                if (founder == null)
                {
                    return -2;
                }else
                {
                    founder.User.UpdatedBy = LoggedInUser;
                    founder.User.UpdatedAt = DateTime.Now;
                    founder.User.Status = Status;
                    founder.User.TokenVersion = Status == (int)UserStatus.Inactive ? (founder.User.TokenVersion ??0)+1 :founder.User.TokenVersion;
                    _unitOfWork.FounderRepo.Update(founder);
                   var res= _unitOfWork.Save();
                    if(res>0)
                    {
                        return 1;
                    }else
                    {
                        return 0;
                    }
                }
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public FoundersPaginatedDto GetAllPaginatedFounders(FounderSearchDto search)
        {
            try
            {
                var FoundersList = _unitOfWork.FounderRepo.GetAll(
             f => (string.IsNullOrEmpty(search.SearchInput) ||
                     f.User.Email.Contains(search.SearchInput) ||
                     f.User.FirstName.Contains(search.SearchInput) ||
                     f.User.LastName.Contains(search.SearchInput) ||
                     f.User.Address.Contains(search.SearchInput) ||
                     f.User.PhoneNumber.Contains(search.SearchInput))&&
                 (search.Gender == null || f.User.Gender == search.Gender) &&
                 (search.GovernmentId == null || search.GovernmentId == 0|| f.User.GovernmentId == search.GovernmentId) &&
                 ( (search.Status == null || search.Status == 0) && f.User.Status != (int)UserStatus.Deleted)||(search.Status != null && search.Status != 0 && f.User.Status == search.Status)

                , "User" ).OrderByDescending(f => f.User.CreatedAt);
                var PaginatedData = FoundersList
                  .Skip(((search.PageNumber > 0 ? search.PageNumber : 1) - 1) * (search.PageSize > 0 ? search.PageSize : 5))
                  .Take(search.PageSize > 0 ? search.PageSize : 10)
                  .ToList();


                var FoundersPaginted = new FoundersPaginatedDto
                {
                    founders=_mapper.Map<List<FounderDto>>(PaginatedData),
                    CurrentPage = (search.PageNumber > 0) ? search.PageNumber : 1,
                    PageSize = (search.PageSize>0)?search.PageSize:5,
                    TotalCount = FoundersList.Count(),

                };
                return FoundersPaginted;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public FounderDto GetFounderById(int Id)
        {
            try
            {
                var founder = _unitOfWork.FounderRepo.FirstOrDefault(x => x.Id == Id, "User");
                if (founder == null)
                {
                    return null;
                }
                var FounderDto = _mapper.Map<FounderDto>(founder);
                return FounderDto; 
               
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public FoundersTotalActiveIactiveDto GetTotalFoundersActiveIactive()
        {
            try
            {
                var founders = _unitOfWork.FounderRepo.GetAll(null,"User").ToList();
                var FounderActive = founders.Count(f => f.User.Status ==(int)UserStatus.Active);
                var FounderInActive= founders.Count(f => f.User.Status == (int)UserStatus.Inactive);
                var ActiveIactiveFounderDto = new FoundersTotalActiveIactiveDto { TotalActive = FounderActive, TotalInactive = FounderInActive };
                return ActiveIactiveFounderDto;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public int Add(FounderDto founder, int? loggedInUser)
        {
            try
            {
                if (founder == null || founder.User == null)
                {
                    return 0; // Invalid input
                }
                var existedUser = _unitOfWork.UserRepo.GetAll(u => u.Email == founder.User.Email).FirstOrDefault();
                if (existedUser != null)
                {
                    return -1; // User already exists
                }
                var newFounder = _mapper.Map<Founder>(founder);
                newFounder.User.UserType = (int)UserType.Founder;
                newFounder.User.CreatedBy = loggedInUser;
                newFounder.User.Status = loggedInUser != null ? (int)UserStatus.Active : (int)UserStatus.Pending;
                newFounder.User.CreatedAt = DateTime.UtcNow;
                newFounder.User.HashedPassword = loggedInUser != null ? BCrypt.Net.BCrypt.HashPassword("123456") : BCrypt.Net.BCrypt.HashPassword(founder.User.Password);
                _unitOfWork.FounderRepo.Insert(newFounder);
                int res = _unitOfWork.Save();
                if (res > 0)
                {
                    return newFounder.Id;
                }
                return res;

            }
            catch (Exception ex)
            {
                return -3; // Exception occurred
            }
        }


        public async Task<List<DropdownDto>> GetFoundersForDropdownAsync()
        {
            return await _unitOfWork.FounderRepo.FindAll(properties: "User")
                .Select(i => new DropdownDto
                {
                    Id = i.Id,
                    Name = $"{i.User.FirstName} {i.User.LastName}"
                }).ToListAsync();
        }

        public FounderDto GetFounderByUserId(int LoggedInUserId)
        {
            try
            {
                var founder = _unitOfWork.FounderRepo.FirstOrDefault(f => f.UserId == LoggedInUserId, "User.Government,User.City");
                return _mapper.Map<FounderDto>(founder);


            }
            catch (Exception ex)
            {
                return null;

            }
        }

        public Tuple<bool, FounderDto> UpdateFounderData(string email, UpdateFounderDto founderDto)
        {
            var founder = _unitOfWork.FounderRepo.FirstOrDefault(f => f.User.Email == email, includeProperties: "User");
            if (founder == null)
                throw new KeyNotFoundException("No Founder Found");

            var currentDto = founder.ToUpdateDto();
            if (founderDto.Equals(currentDto))
                return new Tuple<bool, FounderDto>(false, _mapper.Map<FounderDto>(founder));

            if (!string.IsNullOrWhiteSpace(founderDto.PhoneNumber))
            {
                var existingUser = _unitOfWork.UserRepo.FirstOrDefault(u =>
                    u.PhoneNumber == founderDto.PhoneNumber &&
                    u.Id != founder.User.Id);

                if (existingUser != null)
                    throw new ArgumentException("Phone number must be unique.");
            }

            if (founderDto.DateOfBirth.HasValue)
            {
                var dob = founderDto.DateOfBirth.Value;
                var today = DateOnly.FromDateTime(DateTime.Today);
                int age = today.Year - dob.Year;
                if (today < dob.AddYears(age))
                    age--;

                if (age < 21)
                    throw new ArgumentException("Age must be at least 21 years.");
            }

            var user = founder.User;

            user.FirstName = founderDto.FirstName;
            user.LastName = founderDto.LastName;
            user.PhoneNumber = founderDto.PhoneNumber;
            user.Gender = founderDto.Gender;
            user.GovernmentId = founderDto.GovernmentId;
            user.CityId = founderDto.CityId;
            user.Address = founderDto.Address;
            user.DateOfBirth = founderDto.DateOfBirth;
            user.Status = (int)UserStatus.Inactive;

            _unitOfWork.Save();

            return new Tuple<bool, FounderDto>(true, _mapper.Map<FounderDto>(founder));
        }

        public bool ChangePassword(ChangePasswordDto model)
        {
            var user = _unitOfWork.UserRepo.FirstOrDefault(user => user.Email == model.email);

            if (user == null)
                throw new KeyNotFoundException("User not found");

            if (!BCrypt.Net.BCrypt.Verify(model.CurrentPassword, user.HashedPassword))
                throw new ArgumentException("Current password is incorrect");

            user.HashedPassword = BCrypt.Net.BCrypt.HashPassword(model.NewPassword);

            _unitOfWork.UserRepo.Update(user);

            _unitOfWork.Save();
            return true;
        }

        public bool UpdateProfilePicture(UpdateProfilePicDto model)
        {
            if (string.IsNullOrWhiteSpace(model.Email))
                throw new ArgumentException("Email is required.");

            var user = _unitOfWork.UserRepo.FirstOrDefault(user => user.Email == model.Email);
            if (user == null)
                throw new KeyNotFoundException("User not found.");

            user.ProfilePicPath = HandleImageUpload(
                model.PicFile,
                user.ProfilePicPath,
                "profilePic"
            );

            _unitOfWork.UserRepo.Update(user);
            _unitOfWork.Save();

            return true;
        }


        public UpdateNationalIdResponseDto UpdateNationalIdImages(UpdateNationalIdDto model)
        {
            var user = _unitOfWork.UserRepo.FirstOrDefault(u => u.Email == model.Email);
            if (user == null)
                throw new KeyNotFoundException("User not found.");

            if (model.FrontIdFile != null)
            {
                user.FrontIdPicPath = HandleImageUpload(
                    model.FrontIdFile,
                    user.FrontIdPicPath,
                    "nationalIdPic"
                );
            }

            if (model.BackIdFile != null)
            {
                user.BackIdPicPath = HandleImageUpload(
                    model.BackIdFile,
                    user.BackIdPicPath,
                    "nationalIdPic"
                );
            }

            _unitOfWork.UserRepo.Update(user);
            _unitOfWork.Save();

            UpdateNationalIdResponseDto res = new UpdateNationalIdResponseDto()
            {
                FrontIdPicPath = user.FrontIdPicPath,
                BackIdPicPath = user.BackIdPicPath
            };

            return res;

        }


        private string HandleImageUpload(IFormFile file, string? oldPath, string folderName)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("Image file is required.");

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var extension = Path.GetExtension(file.FileName);

            if (string.IsNullOrWhiteSpace(extension) ||
                !allowedExtensions.Contains(extension.ToLowerInvariant()))
            {
                throw new ArgumentException("Only JPG and PNG files are allowed.");
            }

            var path = _helper.UploadFile(file, "founder", folderName);
            if (string.IsNullOrWhiteSpace(path))
                throw new InvalidOperationException("Failed to upload image.");

            if (!string.IsNullOrWhiteSpace(oldPath))
                _helper.DeleteFile(oldPath);

            return path;
        }




    }
}
