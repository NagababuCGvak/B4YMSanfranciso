namespace Tar.Core.Caching
{
    public interface ICacheItem
    {
        string Key{ get; set; }
        object Value{ get; set; }
        ICacheValidator Validator { get; set; }
    }
}
