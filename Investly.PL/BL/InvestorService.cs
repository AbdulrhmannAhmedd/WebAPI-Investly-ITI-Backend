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
        public int Add(InvestorDto investor)
        {
            int res = 0;
            try
            {
                if (investor == null)
                {
                    return 0; // Invalid input
                }
                var existedUser =_unitOfWork.UserRepo.GetAll(u => u.Email == investor.User.Email).FirstOrDefault();
                if (existedUser != null)
                {
                    return -1; //user exist
                }
                var newInvestor = _mapper.Map<Investor>(investor);
                newInvestor.User.HashedPassword = BCrypt.Net.BCrypt.HashPassword(investor.User.Password);
                newInvestor.User.UserType=(int)UserType.Investor;
                newInvestor.User.Status =(int)UserStatus.Active;
                newInvestor.User.CreatedAt = DateTime.UtcNow;
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
                var investor = _unitOfWork.InvestorRepo.FirstOrDefault(u=>u.Id==id,"User");
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

    }
}
