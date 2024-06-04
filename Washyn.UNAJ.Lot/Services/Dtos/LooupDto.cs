namespace Washyn.UNAJ.Lot
{
    public class LookupDto<TKey>
    {
        public TKey Id { get; set; }
        public string DisplayName { get; set; }
        public string? AlternativeText { get; set; }
    }

    public class LookupDto : LookupDto<Guid>
    {
    }
}