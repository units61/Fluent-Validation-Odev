using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WebApi.BookOperations.CreateBook;
using WebApi.BookOperations.GetBooks;
using WebApi.BookOperations.GetById;
using WebApi.BookOperations.UpdateBook;
using WebApi.DBOperations;
using static WebApi.BookOperations.CreateBook.CreateBookCommand;
using static WebApi.BookOperations.UpdateBook.UpdateBookCommand;

namespace WebApi.AddControllers
{
    [ApiController]
    [Route("[controller]s")]
    public class BookController : ControllerBase
    {

        private readonly BookStoreDbContext _context;
        public BookController (BookStoreDbContext context)
        {
            _context = context;
        }
        // private static List<Book> BookList = new List<Book>()
        // {
        //     new Book 
        //     {
        //         Id= 1, 
        //         Title="Lean Statrup",
        //         GenreId=1,//Personal Growth
        //         PageCount = 200,
        //         PublishDate = new DateTime(2001,06,12)
        //     },
        //     new Book 
        //     {
        //         Id= 2, 
        //         Title="Herland",
        //         GenreId=2,//Science Fiction
        //         PageCount = 250,
        //         PublishDate = new DateTime(2010,05,23)
        //     },
        //     new Book 
        //     {
        //         Id= 3, 
        //         Title="Dune",
        //         GenreId=2,//Science Fiction
        //         PageCount = 540,
        //         PublishDate = new DateTime(2001,12,21)
        //     }
        // };

        [HttpGet]
        public IActionResult GetBooks()
        {
           GetBooksQuery query = new GetBooksQuery(_context);
           var result = query.Handle();
           return Ok(result);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            GetByIdQuery.BooksViewModel result;
            GetByIdQuery query = new GetByIdQuery(_context,id);
            result=query.Handle();
            return Ok(result);
        }  

        // [HttpGet]
        // public Book Get([FromQuery] string id)
        // {
        //     var book = BookList.Where(book=> book.Id == Convert.ToInt32(id)).SingleOrDefault();
        //     return book;
        // }

        [HttpPost]
        public IActionResult AddBook([FromBody] CreateBookModel newbook)
        {
            CreateBookCommand command = new CreateBookCommand(_context);
            var book = _context.Books.SingleOrDefault(x=> x.Title == newbook.Title);
            try
            {
                command.Model = newbook;
                command.Handle();
            }
            catch (Exception ex)
            {
                
                return BadRequest(ex.Message);
            }
                return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult UpdateBook(int id,[FromBody] UpdateBookModel updatebook)
        {
            UpdateBookCommand command = new UpdateBookCommand(_context);
            try
            {
                command.Model= updatebook;
                command.Handle(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok();
        
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteBook(int id)
        {
            var book = _context.Books.SingleOrDefault(x=> x.Id == id);
            if(book is null)
                return BadRequest();
            
            _context.Books.Remove(book);
            _context.SaveChanges();
                return Ok();
        }
    }
}