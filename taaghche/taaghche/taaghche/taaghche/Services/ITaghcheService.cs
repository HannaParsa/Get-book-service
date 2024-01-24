namespace taaghche.Services
{
    public interface ITaghcheService
    {
        Task<string> GetBookDataAsync(int bookId);
    }
}