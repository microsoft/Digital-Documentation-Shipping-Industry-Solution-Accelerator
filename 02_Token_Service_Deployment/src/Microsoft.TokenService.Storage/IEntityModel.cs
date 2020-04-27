namespace Microsoft.TokenService.Storage
{
    public interface IEntityModel<TIdentifier>
    {
        TIdentifier Id { get; set; }
    }
}