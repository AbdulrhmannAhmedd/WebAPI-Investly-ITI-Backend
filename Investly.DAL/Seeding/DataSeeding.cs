using Investly.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Investly.DAL.Seeding
{
    public class DataSeeding
    {
        private readonly AppDbContext _appDbContext;
        public DataSeeding(AppDbContext appDbContext)
        {
            this._appDbContext = appDbContext;

        }

        public void SuperAdminSeed()
        {
            var admin = new User
            {
                FirstName = "Super",
                LastName = "Admin",
                Email = "SuperAdmin@gmail.com",
                HashedPassword = BCrypt.Net.BCrypt.HashPassword("123456"),
                UserType = 1,
                Status = 1,
                CreatedAt = DateTime.UtcNow,
                NationalId = "12345678912345",
            };
            if (!_appDbContext.Users.Any(u => u.Email == admin.Email))
            {
                _appDbContext.Users.Add(admin);
                _appDbContext.SaveChanges();
            }
        }

        public void GovernmentCitiesSeed()
        {
            if (_appDbContext.Governments.Any())
            {
                return; // If governments already exist, skip seeding
            }
            var governments = new List<Government>
    {
        new Government
        {

            NameEn = "Cairo",
            NameAr = "القاهرة",
            Cities = new List<City>
            {
                new City { NameEn = "Cairo", NameAr = "القاهرة" },
                new City { NameEn = "Helwan", NameAr = "حلوان" },
            }
        },
        new Government
        {

            NameEn = "Giza",
            NameAr = "الجيزة",
            Cities = new List<City>
            {
                new City { NameEn = "Giza", NameAr = "الجيزة" },
                new City { NameEn = "6th of October City", NameAr = "مدينة السادس من أكتوبر" },
            }
        },
        new Government
        {

            NameEn = "Alexandria",
            NameAr = "الإسكندرية",
            Cities = new List<City>
            {
                new City { NameEn = "Alexandria", NameAr = "الإسكندرية" },
                new City { NameEn = "Borg El Arab", NameAr = "برج العرب" },
            }
        },
        new Government
        {

            NameEn = "Dakahlia",
            NameAr = "الدقهلية",
            Cities = new List<City>
            {
                new City { NameEn = "Mansoura", NameAr = "المنصورة" },
                new City { NameEn = "Talkha", NameAr = "طلخا" },
            }
        },
        new Government
        {
      
            NameEn = "Sharqia",
            NameAr = "الشرقية",
            Cities = new List<City>
            {
                new City { NameEn = "Zagazig", NameAr = "الزقازيق" },
                new City { NameEn = "10th of Ramadan City", NameAr = "العاشر من رمضان" },
            }
        },
        new Government
        {
        
            NameEn = "Gharbia",
            NameAr = "الغربية",
            Cities = new List<City>
            {
                new City { NameEn = "Tanta", NameAr = "طنطا" },
                new City { NameEn = "El Mahalla El Kubra", NameAr = "المحلة الكبرى" },
            }
        },
        new Government
        {

            NameEn = "Monufia",
            NameAr = "المنوفية",
            Cities = new List<City>
            {
                new City { NameEn = "Shibin El Kom", NameAr = "شبين الكوم" },
                new City { NameEn = "Menouf", NameAr = "منوف" },
            }
        },
        new Government
        {
            
            NameEn = "Qalyubia",
            NameAr = "القليوبية",
            Cities = new List<City>
            {
                new City { NameEn = "Banha", NameAr = "بنها" },
                new City { NameEn = "Shubra El Kheima", NameAr = "شبرا الخيمة" },
            }
        },
        new Government
        {

            NameEn = "Beheira",
            NameAr = "البحيرة",
            Cities = new List<City>
            {
                new City { NameEn = "Damanhur", NameAr = "دمنهور" },
                new City { NameEn = "Kafr El Dawwar", NameAr = "كفر الدوار" },
            }
        },
        new Government
        {

            NameEn = "Kafr El Sheikh",
            NameAr = "كفر الشيخ",
            Cities = new List<City>
            {
                new City { NameEn = "Kafr El Sheikh", NameAr = "كفر الشيخ" },
                new City { NameEn = "Desouk", NameAr = "دسوق" },
            }
        },
        new Government
        {

            NameEn = "Damietta",
            NameAr = "دمياط",
            Cities = new List<City>
            {
                new City { NameEn = "Damietta", NameAr = "دمياط" },
                new City { NameEn = "New Damietta", NameAr = "دمياط الجديدة" },
            }
        },
        new Government
        {

            NameEn = "Ismailia",
            NameAr = "الإسماعيلية",
            Cities = new List<City>
            {
                new City { NameEn = "Ismailia", NameAr = "الإسماعيلية" },
                new City { NameEn = "Qantara", NameAr = "القنطرة" },
            }
        },
        new Government
        {
         
            NameEn = "Port Said",
            NameAr = "بورسعيد",
            Cities = new List<City>
            {
                new City { NameEn = "Port Said", NameAr = "بورسعيد" },
                new City { NameEn = "Port Fuad", NameAr = "بورفؤاد" },
            }
        },
        new Government
        {

            NameEn = "Suez",
            NameAr = "السويس",
            Cities = new List<City>
            {
                new City { NameEn = "Suez", NameAr = "السويس" },
                new City { NameEn = "Ain Sokhna", NameAr = "العين السخنة" },
            }
        },
        new Government
        {

            NameEn = "Faiyum",
            NameAr = "الفيوم",
            Cities = new List<City>
            {
                new City { NameEn = "Faiyum", NameAr = "الفيوم" },
                new City { NameEn = "Sinnuris", NameAr = "سنورس" },
            }
        },
        new Government
        {

            NameEn = "Beni Suef",
            NameAr = "بني سويف",
            Cities = new List<City>
            {
                new City { NameEn = "Beni Suef", NameAr = "بني سويف" },
                new City { NameEn = "El Fashn", NameAr = "الفشن" },
            }
        },
        new Government
        {

            NameEn = "Minya",
            NameAr = "المنيا",
            Cities = new List<City>
            {
                new City { NameEn = "Minya", NameAr = "المنيا" },
                new City { NameEn = "Matai", NameAr = "مطاي" },
            }
        },
        new Government
        {

            NameEn = "Assiut",
            NameAr = "أسيوط",
            Cities = new List<City>
            {
                new City { NameEn = "Assiut", NameAr = "أسيوط" },
                new City { NameEn = "Manfalut", NameAr = "منفلوط" },
            }
        },
        new Government
        {

            NameEn = "Sohag",
            NameAr = "سوهاج",
            Cities = new List<City>
            {
                new City { NameEn = "Sohag", NameAr = "سوهاج" },
                new City { NameEn = "Akhmim", NameAr = "أخميم" },
            }
        },
        new Government
        {

            NameEn = "Qena",
            NameAr = "قنا",
            Cities = new List<City>
            {
                new City { NameEn = "Qena", NameAr = "قنا" },
                new City { NameEn = "Nag Hammadi", NameAr = "نجع حمادي" },
            }
        },
        new Government
        {

            NameEn = "Luxor",
            NameAr = "الأقصر",
            Cities = new List<City>
            {
                new City { NameEn = "Luxor", NameAr = "الأقصر" },
                new City { NameEn = "Esna", NameAr = "إسنا" },
            }
        },
        new Government
        {

            NameEn = "Aswan",
            NameAr = "أسوان",
            Cities = new List<City>
            {
                new City { NameEn = "Aswan", NameAr = "أسوان" },
                new City { NameEn = "Edfu", NameAr = "إدفو" },
            }
        },
        new Government
        {

            NameEn = "Red Sea",
            NameAr = "البحر الأحمر",
            Cities = new List<City>
            {
                new City { NameEn = "Hurghada", NameAr = "الغردقة" },
                new City { NameEn = "Safaga", NameAr = "سفاجا" },
            }
        },
        new Government
        {

            NameEn = "New Valley",
            NameAr = "الوادي الجديد",
            Cities = new List<City>
            {
                new City { NameEn = "Kharga", NameAr = "الخارجة" },
                new City { NameEn = "Dakhla", NameAr = "الداخلة" },
            }
        },
        new Government
        {

            NameEn = "Matrouh",
            NameAr = "مطروح",
            Cities = new List<City>
            {
                new City { NameEn = "Marsa Matrouh", NameAr = "مرسى مطروح" },
                new City { NameEn = "Siwa", NameAr = "سيوة" },
            }
        },
        new Government
        {

            NameEn = "North Sinai",
            NameAr = "شمال سيناء",
            Cities = new List<City>
            {
                new City { NameEn = "Arish", NameAr = "العريش" },
                new City { NameEn = "Bir al-Abd", NameAr = "بئر العبد" },
            }
        },
        new Government
        {

            NameEn = "South Sinai",
            NameAr = "جنوب سيناء",
            Cities = new List<City>
            {
                new City { NameEn = "Sharm El Sheikh", NameAr = "شرم الشيخ" },
                new City { NameEn = "Dahab", NameAr = "دهب" },
            }
        },
    };

            _appDbContext.Governments.AddRange(governments);
            _appDbContext.SaveChanges();

        }

    }
}
