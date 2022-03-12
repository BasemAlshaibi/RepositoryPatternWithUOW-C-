using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using API.EF;
using API.Helper;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    // هنا الكلاس هذا هو بدوره جنرك ويعمل امبلنتيشن لدوال الانترفيس اللي هي كمان جنرك
    // حيث ان قيم متغير التي ستسند بالكونترولر لاحقا
   public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        // اول خطوة هنا هي عمل حقن للداتا كونتكست لانه سنحتاج له لما نعمل امبلنتيشن للدوال 
        protected ApplicationDbContext _context;

        public BaseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

/*
سنبدا بعمل الامبلمنتيشن لدالتي جلب عنصر بواسطة الاي دي 
المشكلة التي ستصادفنا انه لما نستخدم مثيل الكونتكست
عادة يكون بعدها اسم التيبل اي المؤلفين مثلا او الكتب
ولان هذا الريبو جنيرك ينفع مع كل كيان سنسنتخدم دالة 
set
اللي باتسند بمكانها نوع الكيان 
T
عندما نحدده 
*/
        public T GetById(int id)
        {
            return _context.Set<T>().Find(id);
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }
// هنا نفس الامر ولكن نعيد لسته من العناصر اي كل السجلات
        public IEnumerable<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

/*
هنا دوال الفلترة او الدوال اللي تعيد سجل او سجلات بناء على شرط معين
في الدالة الاولى ستستقبل شرط معين نمرره كبرميتر داخل
SingleOrDefault
وهنا سنجل او ديفلوت لانه سيرجع لنا سجل واحد او لا شيء خالص
وبعده لدينا برميتر مصفوفة باسم
includes
يحمل اسماء باسماء الكيانات اللي ممكن يرتبط بها الكيان الحالي
ولانه لانعرف عددها علمنا لووب لتلف عليها
-- ملاحظة
استخدمنا هنا
IQueryable
لانه لسا العمليات جارية على البيانات اي نخزنها ككويري ونضيف لها انكلود ثم سنجل اور ديفولت وهكذا

*/
        public T Find(Expression<Func<T, bool>> criteria, string[] includes = null)
        {
            IQueryable<T> query = _context.Set<T>();

            if (includes != null)
                foreach (var incluse in includes)
                    query = query.Include(incluse);

            return query.SingleOrDefault(criteria);
        }
// نفس الدالة السابقة ولكن استخدمنا دوال لينكيو متزامنة
        public async Task<T> FindAsync(Expression<Func<T, bool>> criteria, string[] includes = null)
        {
            IQueryable<T> query = _context.Set<T>();

            if (includes != null)
                foreach (var incluse in includes)
                    query = query.Include(incluse);

            return await query.SingleOrDefaultAsync(criteria);
        }

/*
هنا النتيجة اللي نريدها ستكون اكثر من سجل بناء على شرط واحتمال يكون في ربط بانكلود لجداول اخرى
ولذلك استخدمنا نفس الاكواد للدالة السابقة ولكن الفلترة هنا بالدالة 
Where
اللي نمرر لها
criteria
*/

        public IEnumerable<T> FindAll(Expression<Func<T, bool>> criteria, string[] includes = null)
        {
            IQueryable<T> query = _context.Set<T>();

            if (includes != null)
                foreach (var include in includes)
                    query = query.Include(include);

            return query.Where(criteria).ToList();
        }
// هنا نمرر برميترات النفجيشن للدوال القرينة بها بعد تمرير الشرط لدالة الوير
        public IEnumerable<T> FindAll(Expression<Func<T, bool>> criteria, int skip, int take)
        {
            return _context.Set<T>().Where(criteria).Skip(skip).Take(take).ToList();
        }
/*

هنا اضافة للبرميترات السابقة لدينا برميتران خاصين بالترتيب الاول فيه شرط الترتيب بناء على ايش ؟
والثاني يحددنا لنا نوع الترتيب تصاعدي او تنازلي

نبدا اولا بالفلترة بناء على الشرط في دالة الوير 
ثم نتحقق من قيم برميترات النفجيشن لو هلها
ثم نتحقق هل هناك شرط للترتيب بناء على ايه
فلو في فهنا سنتحقق هل الترتيب تصاعدي او تنازلي
وبناء على ذلك نكتب الكود المناسب ونطبقه على الكويري
ثم نرجعه من الداله بعدما نقرنه بداله تو لست .
*/
        public IEnumerable<T> FindAll(Expression<Func<T, bool>> criteria, int? skip, int? take,
            Expression<Func<T, object>> orderBy = null, string orderByDirection = OrderBy.Ascending)
        {
            IQueryable<T> query = _context.Set<T>().Where(criteria);

            if (skip.HasValue)
                query = query.Skip(skip.Value);

            if (take.HasValue)
                query = query.Take(take.Value);

            if(orderBy != null)
            {
                if(orderByDirection == OrderBy.Ascending)
                    query = query.OrderBy(orderBy);
                else
                    query = query.OrderByDescending(orderBy);
            }

            return query.ToList();
        }
// الدوال الثلاث التالية نفس ما سبق ولكن باستخدام دوال لينكيو متزامنة
        public async Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> criteria, string[] includes = null)
        {
            IQueryable<T> query = _context.Set<T>();

            if (includes != null)
                foreach (var include in includes)
                    query = query.Include(include);

            return await query.Where(criteria).ToListAsync();
        }

        public async Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> criteria, int take, int skip)
        {
            return await _context.Set<T>().Where(criteria).Skip(skip).Take(take).ToListAsync();
        }

        public async Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> criteria, int? take, int? skip,
            Expression<Func<T, object>> orderBy = null, string orderByDirection = OrderBy.Ascending)
        {
            IQueryable<T> query = _context.Set<T>().Where(criteria);

            if (take.HasValue)
                query = query.Take(take.Value);

            if (skip.HasValue)
                query = query.Skip(skip.Value);

            if (orderBy != null)
            {
                if (orderByDirection == OrderBy.Ascending)
                    query = query.OrderBy(orderBy);
                else
                    query = query.OrderByDescending(orderBy);
            }

            return await query.ToListAsync();
        }
// هنا دالة اضافة
        public T Add(T entity)
        {
            _context.Set<T>().Add(entity);
             return entity;
        }

        public async Task<T> AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
             return entity;
        }

        public IEnumerable<T> AddRange(IEnumerable<T> entities)
        {
            _context.Set<T>().AddRange(entities);
             return entities;
        }

        public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities)
        {
            await _context.Set<T>().AddRangeAsync(entities);
             return entities;
        }
//
        public T Update(T entity)
        {
            _context.Update(entity);
             return entity;
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
         }

        public void DeleteRange(IEnumerable<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
         }

        public void Attach(T entity)
        {
            _context.Set<T>().Attach(entity);
         }

        public void AttachRange(IEnumerable<T> entities)
        {
            _context.Set<T>().AttachRange(entities);
         }
//
        public int Count()
        {
            return _context.Set<T>().Count();
        }

           public async Task<int> CountAsync()
        {
            return await _context.Set<T>().CountAsync();
        }


        public int Count(Expression<Func<T, bool>> criteria)
        {
            return _context.Set<T>().Count(criteria);
        }

     
        public async Task<int> CountAsync(Expression<Func<T, bool>> criteria)
        {
            return await _context.Set<T>().CountAsync(criteria);
        }
    }
}