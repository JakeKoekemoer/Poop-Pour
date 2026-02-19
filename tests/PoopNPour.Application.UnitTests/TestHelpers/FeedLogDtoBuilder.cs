using PoopNPour.Abstractions.FeedLog;
using PoopNPour.Domain.Enums.FeedLog;

namespace PoopNPour.Application.UnitTests.TestHelpers;

/// <summary>
/// Fluent builder for FeedLogDto in tests.
/// </summary>
public class FeedLogDtoBuilder
{
    private Guid _feedLogId = Guid.NewGuid();
    private Guid _dependentId = Guid.NewGuid();
    private FeedLogType _feedType = FeedLogType.BREAST_MILK;
    private DateTimeOffset _timeFed = DateTimeOffset.UtcNow;
    private decimal? _mililitersFed = 120m;
    private ICollection<string>? _notes = null;
    private DateTimeOffset _createdOn = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero);
    private string? _createdBy = "test-user";
    private DateTimeOffset _lastModifiedOn = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero);
    private string? _lastModifiedBy = "test-user";

    public FeedLogDtoBuilder WithFeedLogId(Guid feedLogId)
    {
        _feedLogId = feedLogId;
        return this;
    }

    public FeedLogDtoBuilder WithDependentId(Guid dependentId)
    {
        _dependentId = dependentId;
        return this;
    }

    public FeedLogDtoBuilder WithFeedType(FeedLogType feedType)
    {
        _feedType = feedType;
        return this;
    }

    public FeedLogDtoBuilder WithTimeFed(DateTimeOffset timeFed)
    {
        _timeFed = timeFed;
        return this;
    }

    public FeedLogDtoBuilder WithMililitersFed(decimal? mililitersFed)
    {
        _mililitersFed = mililitersFed;
        return this;
    }

    public FeedLogDtoBuilder WithNotes(ICollection<string>? notes)
    {
        _notes = notes;
        return this;
    }

    public FeedLogDtoBuilder WithCreatedOn(DateTimeOffset createdOn)
    {
        _createdOn = createdOn;
        return this;
    }

    public FeedLogDtoBuilder WithCreatedBy(string? createdBy)
    {
        _createdBy = createdBy;
        return this;
    }

    public FeedLogDtoBuilder WithLastModifiedOn(DateTimeOffset lastModifiedOn)
    {
        _lastModifiedOn = lastModifiedOn;
        return this;
    }

    public FeedLogDtoBuilder WithLastModifiedBy(string? lastModifiedBy)
    {
        _lastModifiedBy = lastModifiedBy;
        return this;
    }

    public FeedLogDto Build()
    {
        return new FeedLogDto
        {
            FeedLogId = _feedLogId,
            DependentId = _dependentId,
            FeedType = _feedType,
            TimeFed = _timeFed,
            MililitersFed = _mililitersFed,
            Notes = _notes,
            CreatedOn = _createdOn,
            CreatedBy = _createdBy,
            LastModifiedOn = _lastModifiedOn,
            LastModifiedBy = _lastModifiedBy
        };
    }
}
