using Microsoft.AspNetCore.Mvc;

namespace taaghche.Services
{
    public interface IBookService
    {
        public Task<string> GetBookAsync(int bookId);
        public Task DeleteFromCacheAsync(int bookId);
    }
}
