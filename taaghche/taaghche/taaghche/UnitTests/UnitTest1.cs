using Microsoft.Extensions.Caching.Memory;
using System.Net;
using taaghche;
using taaghche.Models;
using taaghche.services;

namespace UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        private readonly IMemoryCache _memoryCache;
        
        [TestMethod]
        public void GetBook(int bookId)
        {
            var boodas = new Book
            BookService bs = new BookService(null, null);
            bs.GetBook(196039);
            if (!_memoryCache.TryGetValue("196039", out string text))
                Assert.Fail();
        }    

        [TestMethod]
        public void DeleteBook(int bookId)
        {
            BookService bs = new BookService(null, null);
            bs.DeleteFromCache(196039);
            if (_memoryCache.TryGetValue("196039", out string text))
                Assert.Fail();
        }
    }
}