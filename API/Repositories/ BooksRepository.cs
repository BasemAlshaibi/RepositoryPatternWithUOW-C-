using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.EF;
using API.Entities;
using API.Interfaces;

namespace API.Repositories
{
    /*
    الريبو سيرص مت ريبو  
    BaseRepository
    وكذلك  يعمل امبلمنتشن للدالة المضافة في الانترفيس
    IBooksRepository
    */
  public class BooksRepository : BaseRepository<Book>, IBooksRepository
    {
        // لانه سيتعامل مع الكونتكست فسنحتاج لحمل انجكت لها
        //  base(context)ولانه مستخدمه في البيس ريبستوري فحددنا بالطريقة  
        private new readonly ApplicationDbContext _context;

        public BooksRepository(ApplicationDbContext context) : base(context)
        {
        }

// عمل الامبلنتيشن للدالة المضافة في الانترفيس 
//IBooksRepository
        public IEnumerable<Book> SpecialMethod()
        {
            // نكتب الكود اللي نريده هنا
            throw new NotImplementedException();
        }
    }
}