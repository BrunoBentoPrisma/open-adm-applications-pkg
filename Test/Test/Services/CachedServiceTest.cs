using application_pkg.Services;
using Bogus;
using Microsoft.Extensions.Caching.Distributed;
using Moq;

namespace Test.Test.Services;

public class CachedServiceTest
{
    private readonly Faker _faker;
    private readonly Mock<IDistributedCache> _mock;
    private readonly CachedService<string> _cachedService;

    public CachedServiceTest()
    {
        _faker = new();
        _mock = new();
        _cachedService = new(_mock.Object);
    }
    //[Fact]
    //public async Task DeveSetarCached()
    //{
    //    var key = _faker.Lorem.Text();
    //    var value = _faker.Lorem.Paragraph();
    //    _mock.Setup(x => x.SetStringAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(true));
    //    var result = await _cachedService.SetItemAsync(key, value);
    //    Assert.True(result);
    //}

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task NaoDeveSetarCachedComKeyInvalida(string key)
    {
        await Assert.ThrowsAsync<ArgumentException>(() => _cachedService.SetItemAsync(key, _faker.Lorem.Paragraph()));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task NaoDeveSetarCollectionComKeyInvalida(string key)
    {
        await Assert.ThrowsAsync<ArgumentException>(() => _cachedService.SetCollectionAsync(key,new() { _faker.Lorem.Paragraph(), _faker.Lorem.Paragraph() }));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task NaoDeveRetornarItemCachedComKeyInvalida(string key)
    {
        await Assert.ThrowsAsync<ArgumentException>(() => _cachedService.GetItemAsync(key));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task NaoDeveRetornarCollectionCachedComKeyInvalida(string key)
    {
        await Assert.ThrowsAsync<ArgumentException>(() => _cachedService.GetCollectionAsync(key));
    }

    [Theory]
    [InlineData(null)]
    public async Task NaoDeveSetarCachedComValueNull(string? value)
    {
        await Assert.ThrowsAsync<ArgumentException>(() => _cachedService.SetItemAsync(_faker.Lorem.Paragraph(), value));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task NaoDeveRemoverCachedComKeyInvalida(string key)
    {
        await Assert.ThrowsAsync<ArgumentException>(() => _cachedService.RemoveCached(key));
    }
}
