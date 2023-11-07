namespace HunterX.Trader.Domain.Common;

public class PaginatedList<T>
{
    public required IReadOnlyList<T> Items { get; init; }
    public required long Count { get; init; }
    public required int PageNumber { get; init; }
    public required int PageSize { get; init; }

    public int TotalPages => (int)Math.Ceiling(this.Count / (double)this.PageSize);
    public bool HasPreviousPage => this.PageNumber > 1;
    public bool HasNextPage => this.PageNumber < this.TotalPages;
}
