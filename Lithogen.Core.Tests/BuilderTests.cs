using Lithogen.Interfaces;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lithogen.Core.Tests
{
    [TestFixture]
    public class BuilderTests
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Ctor_WhenBuildContextIsNull_ThrowsArgumentNullException()
        {
            var b = new Builder(null);
        }

        [Test]
        public void Ctor_Executed_CreatesEmptyStepsCollection()
        {
            var bc = new Mock<IBuildContext>();
            var b = new Builder(bc.Object);
            Assert.NotNull(b.Steps);
            Assert.IsEmpty(b.Steps);
        }
    }
}
