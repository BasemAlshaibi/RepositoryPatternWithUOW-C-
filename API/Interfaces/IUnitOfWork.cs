using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;

namespace API.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        // نعمل الاثنين الريبو لدينا كخصائص
        IBaseRepository<Author> Authors { get; }
  //      IBaseRepository<Book> Books { get; }

          IBooksRepository Books { get; }


/*
الدالة الاخيرة هي لامبلنتيشن
SaveChanges  
وهي ستعيد بيانات رقمية اللي هي عدد الحقول اللي تم التاثير عليها
ونقدر نعيد بوليان 
اي ترو في حالة تمت عملية الحفظ بنجاح
*/
        int Complete();

    }
}