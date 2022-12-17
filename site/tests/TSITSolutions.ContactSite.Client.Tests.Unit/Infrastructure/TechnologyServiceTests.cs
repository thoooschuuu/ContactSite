using FluentAssertions;
using TSITSolutions.ContactSite.Client.Infrastructure;

namespace TSITSolutions.ContactSite.Server.Tests.Unit.Infrastructure;

public class TechnologyServiceTests
{
    private readonly TechnologyService _sut;

    public TechnologyServiceTests()
    {
        _sut = new TechnologyService();
    }

    [Fact]
    public void GetColor_ReturnsDifferentColor_WhenGivenDifferentTechnologies()
    {
        var color1 = _sut.GetColor("t1");
        var color2 = _sut.GetColor("t2");

        color1.Should().NotBe(color2);
    }

    [Fact]
    public void GetColor_ReturnsSameColor_WhenGivenSameTechology()
    {
        var color1 = _sut.GetColor("t1");
        var color2 = _sut.GetColor("t1");

        color1.Should().Be(color2);
        
    }
}