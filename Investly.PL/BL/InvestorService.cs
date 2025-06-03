using AutoMapper;
using Investly.DAL.Entities;
using Investly.DAL.Repos.IRepos;
using Investly.PL.Dtos;
using Investly.PL.General;
using Investly.PL.IBL;

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
                newInvestor.User.Status = (int)UserStatus.Active;
                newInvestor.User.CreatedAt = DateTime.UtcNow;
                newInvestor.User.HashedPassword = BCrypt.Net.BCrypt.HashPassword("123456");
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
                _unitOfWork.InvestorRepo.Update(investor);
                return _unitOfWork.Save();

            }
            catch (Exception ex)
            {
                return -1; // Exception occurred

            }
        }
    }
}
