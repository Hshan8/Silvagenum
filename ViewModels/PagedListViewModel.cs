namespace SilvagenumWebApp.ViewModels
{
    public class PagedListViewModel<T> : List<T>
    {
        public int PageIndex { get; private set; }
        public int PageCount { get; private set; }
        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < PageCount;
        public string? Title { get; private set; }

        public PagedListViewModel(List<T> items, int itemCount, int pageIndex, int pageSize, string title = "")
        {
            PageIndex = pageIndex;
            PageCount = (int)Math.Ceiling(itemCount / (double)pageSize);
            AddRange(items);
            Title = title;
        }
    }
}
