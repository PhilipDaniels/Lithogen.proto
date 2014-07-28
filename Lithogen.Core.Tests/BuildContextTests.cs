using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Method_StateUnderTest_ExpectedBehavior.
// It is nice to Assert.NotNull before checking more specific criteria as it results in a more meaningful error message.

namespace Lithogen.Core.Tests
{
    [TestFixture]
    public class BuildContextTests
    {
        [Test]
        public void Ctor_Executed_CreatesEmptyTagDataDictionary()
        {
            var bc = new BuildContext();
            Assert.NotNull(bc.TagData);
            Assert.IsEmpty(bc.TagData);
        }
    }
}
