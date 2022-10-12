using FluentAssertions;
using linqPlusPlus;

namespace LinqPlusPlusTest
{
    public class GeneralExtensionTest
    {
        [Fact]
        public void IsSameSiteTest()
        {
            var first = new Uri("https://sub1.domain.com/path?query=value");
            var second = new Uri("https://sub2.domain.com/path?query=value");
            first.IsSameSite(second).Should().Be(true);

            second = new Uri("https://sub2.Domain.com/path?query=value");
            first.IsSameSite(second).Should().Be(true);

            second = new Uri("https://Domain.com/path?query=value");
            first.IsSameSite(second).Should().Be(true);

            second = new Uri("https://sub2.anotherDomain.com/path?query=value");
            first.IsSameSite(second).Should().Be(false);


        }
    }
}