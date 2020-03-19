using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Library.Entities;
using Library.Models.DTO;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Xunit;

namespace XUnitTestProject1.Controllers
{
    public class BookBorrowInitTests
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;

        public BookBorrowInitTests()
        {
            _server = ServerFactory.GetServerInstance();
            _client = _server.CreateClient();

            using (var scope = _server.Host.Services.CreateScope())
            {
                var _db = scope.ServiceProvider.GetRequiredService<LibraryContext>();

                _db.BookBorrow.Add(new BookBorrow
                {
                    IdUser = 1,
                    IdBook = 1,
                    Comments = "Initial"
                });

                _db.SaveChanges();

            }
        }

        [Fact]
        public async Task PostBookBorrow_200Ok()
        {
            var newBookBorrow = new BookBorrow()
            {
                IdUser = 1,
                IdBook = 1,
                Comments = "TEST"
            };

            var postResponse = await _client.PostAsync($"{_client.BaseAddress.AbsoluteUri}api/book-borrows", new StringContent(JsonConvert.SerializeObject(newBookBorrow), Encoding.UTF8, "application/json"));

            postResponse.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task UpdateBookBorrow_200Ok()
        {

            var updatedBookBorrow = new BookBorrow()
            {
                Comments = "Updated"
            };

            var postResponse = await _client.PostAsync($"{_client.BaseAddress.AbsoluteUri}api/book-borrows/1", new StringContent(JsonConvert.SerializeObject(updatedBookBorrow), Encoding.UTF8, "application/json"));

            postResponse.EnsureSuccessStatusCode();
        }
    }
}
