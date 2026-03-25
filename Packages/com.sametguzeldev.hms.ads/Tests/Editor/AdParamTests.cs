using NUnit.Framework;
using SametGuzelDev.HMS.Ads;

namespace SametGuzelDev.HMS.Ads.Tests.Editor
{
    [TestFixture]
    public class AdParamTests
    {
        [Test]
        public void Default_ReturnsNonNull()
        {
            var adParam = AdParam.Default;

            Assert.IsNotNull(adParam);
        }

        [Test]
        public void Builder_Build_ReturnsNonNull()
        {
            var adParam = new AdParam.Builder().Build();

            Assert.IsNotNull(adParam);
        }

        [Test]
        public void Default_ReturnsFreshInstanceEachCall()
        {
            var a = AdParam.Default;
            var b = AdParam.Default;

            Assert.AreNotSame(a, b);
        }
    }
}
