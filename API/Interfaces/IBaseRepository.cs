using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using API.Helper;

namespace API.Interfaces
{
    // هنا اعطينا للانترفيس اسم عام وجلعناه جنرك وحددنا ان التي عبارة عن كلاس
    public interface IBaseRepository<T> where T : class
    {
/*
 هنا عرفنا هيدر دالة عادية لجلب سجل بواسطة معرفه الرقمي ثم عرفنا نفس الدالة ولكن متزامنة
 وهنا سنحتاج للمعرف فقط لانه سنستخدم دالة 
find
 التي لاتحتاج الا للاي دي
*/
         T GetById(int id);
        Task<T> GetByIdAsync(int id);
// هنا عرفنا هيدر دالة عادية لجلب كل السجلات  ثم عرفنا نفس الدالة ولكن متزامنة

        IEnumerable<T> GetAll();
        Task<IEnumerable<T>> GetAllAsync();

/*
بالتالي عرفنا دالة تعيد لنا عنصر واحد بناء على شرط معين "تعبير" نمرره هنا وسميناه هنا
criteria
 او ممكن نسميه
 match
اضافة الى برميتر اخر في حالة انه نريد السجلات اللي ترتبد فيه بالجدول الاخر اي
includes
وخليناه الافتراضي بنل اي لو ما بعثش
وهنا خلينا كمفصفوفة لانه ممكن يكون انكلود من اكثر من كيان اخر

وهنا حددنا الامر كمتغيرات لانه مش ح نعرف ايش الشرط بايكون اللي على اساسه بانعيد السجل
حيث انه في الكلاس اللي بايعمل امبلمنتيشن لهذه الانترفيس برضه بايكون عام وجنرك
والتحديد للشروط فقط سيكون بالكونترولر كما سنتعلم لاحقا

الدالة الثانية نفس الامر ولكن كدالة متزامنة
*/
        T Find(Expression<Func<T, bool>> criteria, string[] includes = null);
        Task<T> FindAsync(Expression<Func<T, bool>> criteria, string[] includes = null);
/*

التالي عرفنا 3 دوال عادية و 3 دوال متزامنة
والاسماء واحدة ولكن البرميترات مختلفة اي انه طبقنا مبدا الاوفرلودنج

وهنا الدوال تعيد لنا مجموعة من العناصر اي لسته بناء 
على شرط معين  نمرره لها 
criteria
الدالة الاولى لو اردنا جلب حقول مرتبطة من جدول اخر ب
includes
وهنا مصفوفة ممكن تحوي اكثر من قيمة وكل قيمة تمثل كيان اخر قد يرتبط بالجدول المحدد

دالة اخرى حددنا لها شرط نفلتر على اساسه ثم متغيري تيك وسكب الخاصين بالبنجنيشن

اضافة الى برميتر شرطي يحدد على ضوء اي عمود بانقوم بالترتيب
ثم اتجاه الترتيب هذا هل سيكون تصاعدي او تنازلي وهنا اعتمدنا على قيم ثابته انشئناها بكلاس اخر بمجلد هيلبر من باب التنظيم
والافتراضي هنا جعلناه تصاعدي
*/

        IEnumerable<T> FindAll(Expression<Func<T, bool>> criteria, string[] includes = null);
        IEnumerable<T> FindAll(Expression<Func<T, bool>> criteria, int take, int skip);
        IEnumerable<T> FindAll(Expression<Func<T, bool>> criteria, int? take, int? skip, 
            Expression<Func<T, object>> orderBy = null, string orderByDirection = OrderBy.Ascending);

        Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> criteria, string[] includes = null);
        Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> criteria, int skip, int take);
        Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> criteria, int? skip, int? take,
            Expression<Func<T, object>> orderBy = null, string orderByDirection = OrderBy.Ascending);

// دالتي اضافة عادية ومتزامنه وتعيدان العنصر اللي انضاف

        T Add(T entity);
        Task<T> AddAsync(T entity);

// دالتي اضافة لسته عناصر  عادية ومتزامنه وتعيدان العنصر اللي انضاف

        IEnumerable<T> AddRange(IEnumerable<T> entities);
        Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities);

// دوال لعمل تعديل على عنصر او حذفه او حذف لسته عناصر
// اضافة لدوال تخص الاتاتش
        T Update(T entity);
        void Delete(T entity);
        void DeleteRange(IEnumerable<T> entities);
        void Attach(T entity);
        void AttachRange(IEnumerable<T> entities);

/*

دالتين   تعمل احصاء عام لكل السجلات
واحده عادية والثانية متزامنة
يليهما دالتين يعملان احصاء للسجلات بناء على شرط نمرره
criteria
*/
        int Count();

        Task<int> CountAsync();

        int Count(Expression<Func<T, bool>> criteria);
       
        Task<int> CountAsync(Expression<Func<T, bool>> criteria);
    }
}