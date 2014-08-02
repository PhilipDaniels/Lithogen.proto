using NUnit.Framework;

namespace Lithogen.Core.Tests.Unit
{
    public class BuildContext_Ctor
    {
        [Test]
        public void WhenCompleted_CreatesEmptyTagDataDictionary()
        {
            var bc = new BuildContext();
            Assert.NotNull(bc.TagData);
            Assert.IsEmpty(bc.TagData);
        }
    }
}
