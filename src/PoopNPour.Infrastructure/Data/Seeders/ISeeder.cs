namespace PoopNPour.Infrastructure.Data.Seeders;

public interface ISeeder
{
    Task SeedAsync(CancellationToken cancellationToken = default);
}
