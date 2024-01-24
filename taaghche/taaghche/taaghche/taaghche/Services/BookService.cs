namespace taaghche.Services
{
    public class BookService : IBookService
    {
        private readonly IInMemoryCacheService _inMemoryCacheService;
        private readonly IDistributedCacheService _distributedCacheService;
        private readonly ITaghcheService _taghcheService;

        public BookService(IInMemoryCacheService inMemoryCacheService,
            IDistributedCacheService distributedCacheService,
            ITaghcheService taghcheService)
        {
            _inMemoryCacheService = inMemoryCacheService;
            _distributedCacheService = distributedCacheService;
            _taghcheService = taghcheService;
        }
        public async Task<string> GetBookAsync(int bookId)
        {
            if (_inMemoryCacheService.GetValue(bookId.ToString()) is string fromMemory)
            {
                return fromMemory;
            }
            var result = await _distributedCacheService.GetValueAsync(bookId.ToString()) as string;
            if (result  == null)
            {
                result = await _taghcheService.GetBookDataAsync(bookId);
                await _distributedCacheService.SetValueAsync($"{bookId}", result);
            }

            _inMemoryCacheService.SetValue(bookId.ToString(), result);
            return result;  
        }
        public async Task DeleteFromCacheAsync(int bookId)
        {
            _inMemoryCacheService.Remove(bookId.ToString());
            await _distributedCacheService.RemoveAsync(bookId.ToString());
        }
    }
}
