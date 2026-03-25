using NUnit.Framework;
using SametGuzelDev.HMS.Ads;

namespace SametGuzelDev.HMS.Ads.Tests.Editor
{
    [TestFixture]
    public class RewardTests
    {
        [Test]
        public void Constructor_SetsNameAndAmount()
        {
            var reward = new Reward("coins", 100);

            Assert.AreEqual("coins", reward.Name);
            Assert.AreEqual(100, reward.Amount);
        }

        [Test]
        public void Constructor_WithNullName_SetsNull()
        {
            var reward = new Reward(null, 50);

            Assert.IsNull(reward.Name);
            Assert.AreEqual(50, reward.Amount);
        }

        [Test]
        public void Constructor_WithZeroAmount_SetsZero()
        {
            var reward = new Reward("gems", 0);

            Assert.AreEqual(0, reward.Amount);
        }

        [Test]
        public void ToString_ReturnsExpectedFormat()
        {
            var reward = new Reward("coins", 100);

            Assert.AreEqual("Reward(Name=coins, Amount=100)", reward.ToString());
        }
    }
}
