using NUnit.Framework;

namespace Lithogen.Core.Tests.Unit
{
    public class BuildContext_Ctor : BuildContextBase
    {
        [Test]
        public void WhenCompleted_CreatesEmptyTagDataDictionary()
        {
            Assert.NotNull(TheContext.TagData);
            Assert.IsEmpty(TheContext.TagData);
        }
    }
}
