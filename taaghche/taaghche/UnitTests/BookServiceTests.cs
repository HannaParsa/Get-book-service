using Microsoft.VisualStudio.TestTools.UnitTesting;
using taaghche.Services;
using NSubstitute;
using System.Threading.Tasks;

namespace UnitTests
{

    [TestClass]
    public class BookServiceTests
    {
        [TestMethod]
        public void If_Book_Exists_In_Memory_cache_It_must_not_Call_Destributed_Cache()
        {
            //arrange
            var inMemoryCacheServiceMock = Substitute.For<IInMemoryCacheService>();
            inMemoryCacheServiceMock.GetValue(Arg.Any<string>()).Returns("some book data");

            var destributedCacheMock = Substitute.For<IDistributedCacheService>();
            var bookService = new BookService(inMemoryCacheServiceMock, destributedCacheMock, null);

            //act
            bookService.GetBookAsync(3);

            //assrt
            destributedCacheMock.DidNotReceive().GetValueAsync(Arg.Any<string>());
        }
        
        [TestMethod]
        public void If_Book_Not_Exists_In_Memory_cache_It_must_Call_Destributed_Cache()
        {
            //arrange
            var inMemoryCacheServiceMock = Substitute.For<IInMemoryCacheService>();
            inMemoryCacheServiceMock.GetValue(Arg.Any<string>()).Returns(null);

            var destributedCacheMock = Substitute.For<IDistributedCacheService>();
            var bookService = new BookService(inMemoryCacheServiceMock, destributedCacheMock, null);

            //act
            bookService.GetBookAsync(3);

            //assrt
            destributedCacheMock.Received().GetValueAsync(Arg.Any<string>());
        }
        
        [TestMethod]
        public void If_Book_Not_Exists_In_Memory_And_Destributed_cache_It_must_Get_From_TaghcheService()
        {
            //arrange
            var inMemoryCacheServiceMock = Substitute.For<IInMemoryCacheService>();
            inMemoryCacheServiceMock.GetValue(Arg.Any<string>()).Returns(null);

            var destributedCacheMock = Substitute.For<IDistributedCacheService>();
            destributedCacheMock.GetValueAsync(Arg.Any<string>()).Returns(Task.FromResult<string>(null));

            var taghcheServiceMock = Substitute.For<ITaghcheService>();

            var bookService = new BookService(inMemoryCacheServiceMock, destributedCacheMock, taghcheServiceMock);

            //act
            bookService.GetBookAsync(3);

            //assrt
            taghcheServiceMock.Received().GetBookDataAsync(Arg.Any<int>());
        }
        
        [TestMethod]
        public async Task If_Book_Not_Exists_In_Memory_And_Exists_In_Destributed_cache_It_must_Noy_Get_From_TaghcheService()
        {
            //arrange
            var inMemoryCacheServiceMock = Substitute.For<IInMemoryCacheService>();
            inMemoryCacheServiceMock.GetValue(Arg.Any<string>()).Returns(null);

            var destributedCacheMock = Substitute.For<IDistributedCacheService>();
            destributedCacheMock.GetValueAsync(Arg.Any<string>()).Returns(Task.FromResult<string>("some book data").Result);
            
            var taghcheServiceMock = Substitute.For<ITaghcheService>();

            var bookService = new BookService(inMemoryCacheServiceMock, destributedCacheMock, taghcheServiceMock);

            //act
            await bookService.GetBookAsync(3);

            //assrt
            taghcheServiceMock.DidNotReceive().GetBookDataAsync(Arg.Any<int>());
        }
    }
}