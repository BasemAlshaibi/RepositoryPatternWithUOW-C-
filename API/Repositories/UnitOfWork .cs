using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.EF;
using API.Entities;
using API.Interfaces;

namespace API.Repositories
{
  public class UnitOfWork : IUnitOfWork
    {
// هنا اشبه بعمل مركزية للداتا كونتكست بحيث نعملها هنا ونمرر المثيل منها الى الريبوستوريس
// ثم نعمل امبلنتيشن للخصائص التي تمثل الريبستوريس ونحط لها قيم اوليه ونمرر لها الكونتكست
        public IBaseRepository<Author> Authors { get; private set; }

    //    public IBaseRepository<Book> Books  { get; private set; }

      public IBooksRepository Books { get; private set; }


        private readonly ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;

            Authors = new BaseRepository<Author>(_context);
            
            Books = new BooksRepository(_context);

           // Books = new BaseRepository<Book>(_context);

        }
// هنا دالة السيف تشنج وبعدها دالة تحرير الريسورس
        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}