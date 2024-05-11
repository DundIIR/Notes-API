namespace Notes_API.Contracts
{
    public record GetNotesRequest(string? Search, bool SearchAll, string? SortItem, string? SortOrder);
}
