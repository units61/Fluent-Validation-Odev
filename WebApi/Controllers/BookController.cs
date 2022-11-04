using System.Linq;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using WebApi.BookOperations.CreateBook;
using WebApi.BookOperations.DeleteBook;
using WebApi.BookOperations.GetBookDetail;
using WebApi.BookOperations.GetBooks;
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
        private readonly IMapper _mapper;
        public BookController(BookStoreDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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
           GetBooksQuery query = new GetBooksQuery(_context, _mapper);
           var result = query.Handle();
           return Ok(result);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            BookDetailViewModel result;
            try
            {
                GetBookDetailQuery query = new GetBookDetailQuery(_context, _mapper);
                query.BookId = id;
                GetBookDetailQeuryValidator validator = new GetBookDetailQeuryValidator();
                validator.ValidateAndThrow(query);

                result = query.Handle();
            }
            catch (Exception ex)
            {
                
               return BadRequest(ex.Message);
            }
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
            CreateBookCommand command = new CreateBookCommand(_context, _mapper);
            var book = _context.Books.SingleOrDefault(x=> x.Title == newbook.Title);
            try
            {
                command.Model = newbook;
                CreateBookCommandValidator validator = new CreateBookCommandValidator();
                validator.ValidateAndThrow(command);
                command.Handle();
                // if(!result.IsValid)
                // foreach (var item in result.Errors)
                //     Console.WriteLine("Özellik " + item.PropertyName + " - Error Message: " + item.ErrorMessage);
                // else
                // command.Handle();
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
            
            try
            {
                UpdateBookCommand command = new UpdateBookCommand(_context);
                command.BookId = id;
                command.Model = updatebook;
                UpdateBookCommandValidator validator = new UpdateBookCommandValidator();
                validator.ValidateAndThrow(command);
                command.Handle();
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
            try
            {   
                DeleteBookCommand command = new DeleteBookCommand(_context);
                command.BookId = id;
                DeleteBookCommandValidator validator = new DeleteBookCommandValidator();
                validator.ValidateAndThrow(command);
                command.Handle();
                
            }
            catch (Exception ex)
            {
                
               return BadRequest(ex.Message);
            }
            return Ok();
           
        }
    }
}