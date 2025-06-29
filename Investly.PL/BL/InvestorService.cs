using AutoMapper;
using Investly.DAL.Entities;
using Investly.DAL.Repos;
using Investly.DAL.Repos.IRepos;
using Investly.PL.Dtos;
using Investly.PL.General;
using Investly.PL.IBL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;

namespace Investly.PL.BL
{
    public class InvestorService : IInvestorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public InvestorService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public int Add(InvestorDto investor, int? loggedInUser)
        {
            int res = 0;
            try
            {
                if (investor == null)
                {
                    return 0; // Invalid input
                }
                var existedUser = _unitOfWork.UserRepo.GetAll(u => u.Email == investor.User.Email).FirstOrDefault();
                if (existedUser != null)
                {
                    return -1; //user exist
                }
                var newInvestor = _mapper.Map<Investor>(investor);
                newInvestor.User.UserType = (int)UserType.Investor;
                newInvestor.User.CreatedBy = loggedInUser;
                newInvestor.User.Status =loggedInUser!=null? (int)UserStatus.Active:(int)UserStatus.Pending;
                newInvestor.User.CreatedAt = DateTime.UtcNow;
                newInvestor.User.HashedPassword = loggedInUser != null? BCrypt.Net.BCrypt.HashPassword("123456"): BCrypt.Net.BCrypt.HashPassword(investor.User.Password);
                _unitOfWork.InvestorRepo.Insert(newInvestor);
                res = _unitOfWork.Save();
                if (res > 0)
                {
                    return newInvestor.Id;
                }
            }
            catch (Exception ex)
            {

                return -3;//exception
            }

            return res;
        }
        public InvestorDto? GetById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return null; // Invalid ID
                }
                var investor = _unitOfWork.InvestorRepo.FirstOrDefault(u => u.Id == id, "User");
                if (investor == null)
                {
                    return null; // Not found
                }
                return _mapper.Map<InvestorDto>(investor);
            }
            catch (Exception ex)
            {
                return null; // Exception occurred

            }
        }
        public InvestorDtoWithPagination GetPaginatedData(InvestorSearchDto investorSearch)
        {
            try
            {
                var investorsQuery = _unitOfWork.InvestorRepo.GetAll(
                    item =>
                  (string.IsNullOrEmpty(investorSearch.SearchInput) ||
                item.User.Email.Contains(investorSearch.SearchInput) ||
                item.User.FirstName.Contains(investorSearch.SearchInput) ||
                item.User.LastName.Contains(investorSearch.SearchInput)) &&
                (investorSearch.GovernmentId == null || investorSearch.GovernmentId == 0 || item.User.GovernmentId == investorSearch.GovernmentId) &&
                (investorSearch.Gender == null || item.User.Gender == investorSearch.Gender)&&
                (investorSearch.Status==null || investorSearch.Status==0||item.User.Status==investorSearch.Status)&&
                (item.User.Status!=(int)UserStatus.Deleted)

                ,includeProperties: "User"
                 ).OrderByDescending(u=>u.User.CreatedAt);

                int skip= (investorSearch.PageSize * (investorSearch.PageNumber > 0 ? investorSearch.PageNumber - 1 : 1));
                var paginateData = investorsQuery
                    .Skip(skip)
                    .Take(investorSearch.PageSize)
                    .ToList();
                return new InvestorDtoWithPagination
                {
                    List = _mapper.Map<List<InvestorDto>>(paginateData),
                    TotalCount = investorsQuery.Count(),
                    PageSize = investorSearch.PageSize,
                    CurrentPage = investorSearch.PageNumber
                };
            }
            catch (Exception ex)
            {
                return null; // Exception occurred
            }

        }

        public int Update(InvestorDto investorDto, int? loggedInUser)
        {
            int res = 0;
            try
            {

                if (investorDto == null || investorDto.Id <= 0)
                {
                    return 0; // Invalid input
                }
                var existingInvestor = _unitOfWork.InvestorRepo.FirstOrDefault(i => i.Id == investorDto.Id,includeProperties:"User");
                if (existingInvestor == null)
                {
                    return -1; // Investor not found
                }
                // Update properties
                existingInvestor.User.Email = investorDto.User.Email;
                existingInvestor.User.FirstName = investorDto.User.FirstName;
                existingInvestor.InvestingType = investorDto.InvestingType ?? 0;
                existingInvestor.User.LastName = investorDto.User.LastName;
                existingInvestor.User.Address = investorDto.User.Address;
                existingInvestor.User.PhoneNumber = investorDto.User.PhoneNumber;
                existingInvestor.User.NationalId = investorDto.User.NationalId;
                existingInvestor.User.GovernmentId = investorDto.User.GovernmentId;
                existingInvestor.User.CityId = investorDto.User.CityId;
                existingInvestor.User.DateOfBirth = investorDto.User.DateOfBirth;
                existingInvestor.User.UpdatedAt = DateTime.UtcNow;
                existingInvestor.User.ProfilePicPath = investorDto.User.ProfilePicPath ?? existingInvestor.User.ProfilePicPath;
                existingInvestor.User.FrontIdPicPath = investorDto.User.FrontIdPicPath ?? existingInvestor.User.FrontIdPicPath;
                existingInvestor.User.BackIdPicPath = investorDto.User.BackIdPicPath ?? existingInvestor.User.BackIdPicPath;
                existingInvestor.User.Status=investorDto.User.Status??existingInvestor.User.Status;
                existingInvestor.InvestingType = investorDto.InvestingType ?? existingInvestor.InvestingType;
                existingInvestor.InterestedBusinessStages = investorDto.InterestedBusinessStages ?? existingInvestor.InterestedBusinessStages;
                existingInvestor.MaxFunding = investorDto.MaxFunding ?? existingInvestor.MaxFunding;
                existingInvestor.MinFunding = investorDto.MinFunding ?? existingInvestor.MinFunding;
                _unitOfWork.InvestorRepo.Update(existingInvestor);
                res = _unitOfWork.Save();
                return res;
            }
            catch (Exception ex)
            {
                return -1; // Exception occurred    
            }
        }
        public InvestorTotalActiveIactiveDto GetTotalActiveInactiveInvestors()
        {
            try
            {
                var result = _unitOfWork.InvestorRepo.GetTotalActiveInactive((int)UserStatus.Active,(int)UserStatus.Inactive);
                return new InvestorTotalActiveIactiveDto
                {
                    TotalActive = result.Item1,
                    TotalInactive = result.Item2
                };

            }
            catch (Exception ex)
            {
                return null;

            }
        }
        public int ChangeStatus(int id, int status, int? loggedUser)
        {
            try
            {
                if (id <= 0 )
                {
                    return 0; // Invalid input
                }
                var investor = _unitOfWork.InvestorRepo.FirstOrDefault(i => i.Id == id, "User");
                if (investor == null)
                {
                    return -1; // Investor not found
                }
                investor.User.Status = status;
                investor.User.UpdatedAt = DateTime.UtcNow;
                investor.User.UpdatedBy = loggedUser;
                investor.User.TokenVersion= status == (int)UserStatus.Inactive ? (investor.User.TokenVersion ?? 0) + 1 : investor.User.TokenVersion;
                _unitOfWork.InvestorRepo.Update(investor);
                return _unitOfWork.Save();

            }
            catch (Exception ex)
            {
                return -1; // Exception occurred

            }
        }

        public async Task<List<DropdownDto>> GetInvestorsForDropdownAsync()
        {
            return await _unitOfWork.InvestorRepo.FindAll(properties: "User")
                .Select(i => new DropdownDto
                {
                    Id = i.Id,
                    Name = $"{i.User.FirstName} {i.User.LastName}"
                }).ToListAsync();
        }
        public InvestorDto GetInvestorByUserId(int? loggedInUser)
        {
            try
            {
                var investor = _unitOfWork.InvestorRepo.FirstOrDefault(i => i.UserId == loggedInUser, "User.Government,User.City");
                var res = _mapper.Map<InvestorDto>(investor);
                return res;
            }
            catch (Exception ex)
            {
                return null;

            }
        }

        public int UpdateProfilePicture(string ProfilePicPath, int? loggedInUser)
        {
            int res = 0;
            try
            {

                if (ProfilePicPath == null || loggedInUser <= 0)
                {
                    return 0; // Invalid input
                }
                var existingInvestor = _unitOfWork.InvestorRepo.FirstOrDefault(i => i.UserId == loggedInUser, includeProperties: "User");
                if (existingInvestor == null)
                {
                    return -1; // Investor not found
                }
                // Update properties
                existingInvestor.User.UpdatedBy = loggedInUser.Value;
                existingInvestor.User.UpdatedAt = DateTime.Now;
                existingInvestor.User.ProfilePicPath = ProfilePicPath;
                existingInvestor.User.Status = (int)UserStatus.Pending;
                _unitOfWork.InvestorRepo.Update(existingInvestor);
                res = _unitOfWork.Save();
                return res;
            }
            catch (Exception ex)
            {
                return -1; // Exception occurred    
            }
        }
    
     public int UpdateNationalId(string FrontIdPicPath, string BackIdPicPath, int? loggedInUser)
        {
            int res = 0;
            try
            {

                if (loggedInUser <= 0)
                {
                    return 0; // Invalid input
                }
                var existingInvestor = _unitOfWork.InvestorRepo.FirstOrDefault(i => i.UserId == loggedInUser, includeProperties: "User");
                if (existingInvestor == null)
                {
                    return -1; // Investor not found
                }
                // Update properties
                existingInvestor.User.UpdatedBy = loggedInUser.Value;
                existingInvestor.User.UpdatedAt = DateTime.Now;
                existingInvestor.User.Status = (int)UserStatus.Pending;
                existingInvestor.User.FrontIdPicPath = FrontIdPicPath;
                existingInvestor.User.BackIdPicPath = BackIdPicPath;
                _unitOfWork.InvestorRepo.Update(existingInvestor);
                res = _unitOfWork.Save();
                return res;
            }
            catch (Exception ex)
            {
                return -1; // Exception occurred    
            }
        }
        public int ChangePassword(ChangePasswordDto passwordDto, int? loggedInUser)
        {
            try
            {
                var user = _unitOfWork.InvestorRepo.FirstOrDefault(i => i.UserId == loggedInUser, "User");
                if(user == null)
                {
                    return 0;
                }
                if(!BCrypt.Net.BCrypt.Verify(passwordDto.CurrentPassword,user.User.HashedPassword))
                {
                    return -2;
                }
                user.User.UpdatedBy=loggedInUser.Value;
                user.User.UpdatedAt = DateTime.Now;
                user.User.HashedPassword = BCrypt.Net.BCrypt.HashPassword(passwordDto.NewPassword);
                _unitOfWork.InvestorRepo.Update(user);
             var res=_unitOfWork.Save();
                return res;

            }
            catch (Exception ex)
            {
                return -1; // Exception occurred    
            }
        }
    }
}
