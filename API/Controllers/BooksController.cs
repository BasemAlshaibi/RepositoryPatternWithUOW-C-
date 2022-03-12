using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using API.Helper;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        // عملية حقن السيرفس الخاصة باليونت اوف ورك
        private readonly IUnitOfWork _unitOfWork;

        public BooksController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
   // بالدوال سنتعامل مع مثيل اليونت اوف ورك ثم الريبو وهنا هي البوكس ثم الدالة المطلوبة     
// هنا سيجلب لنا الكتاب اللي معرفه واحد
 //ولكنه لن يجلب المؤلف حقه لان ذي داله لا تمرر انكلود للريبو وكذلك الدالة اللي بعدها تعيد لسته كتب بدون ريبو
        [HttpGet]
        public IActionResult GetById()
        {
            return Ok(_unitOfWork.Books.GetById(1));
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            return Ok(_unitOfWork.Books.GetAll());
        }
/*
بهذه الاند بوينت سنتعامل مع دالة 
Find
والتي تستقبل برميترين الاول هو الشرط اللي على ضوءه ستتم الفلترة
واللي عنوانه يكون 
title book 1
والبرميتر الثاني الخاص بالانكلود اي انه سيضمن لنا سجلات المؤلف الذي يرتبط بهذا الكتاب
*/
        [HttpGet("GetByName")]
        public IActionResult GetByName()
        {
            return Ok(_unitOfWork.Books.Find(b => b.Title == "title book 1", new[] { "Author" }));
        }
        /*
        هنا سنتعامل مع دالة جلب كل السجلات بناء على شرط والشرط هنا ان العنوان يحتوي على كلمة بوك
        وكمان نريد المؤلفين اللي مرتبطين بكل السجلات الراجعة
        */


        [HttpGet("GetAllWithAuthors")]
        public IActionResult GetAllWithAuthors()
        {
            return Ok(_unitOfWork.Books.FindAll(b => b.Title.Contains("book"), new[] { "Author" }));
        }

        /*
        هنا نستدعي دالة جلب كل السجلات بناء على شرط
        ونريط الترتيب يكون بناء على رقم الاي دي ولكن بترتيب تنازلي
        
        */

        [HttpGet("GetOrdered")]
        public IActionResult GetOrdered()
        {
            return Ok(_unitOfWork.Books.FindAll(b => b.Title.Contains("book"), null, null, b => b.Id, OrderBy.Descending));
        }
   /*
      هنا دالة ستضيف لنا سجل معين ونحن هنا مررنا الاوبجكت اللي نشتي نضيف مباشرة بشكل ستاتك
        
        */
        [HttpPost("AddOne")]
        public IActionResult AddOne()
        {
            var book = _unitOfWork.Books.Add(new Book { Title = "Test book", AuthorId = 1 });
            _unitOfWork.Complete();

            return Ok(book);
        }
    }
}