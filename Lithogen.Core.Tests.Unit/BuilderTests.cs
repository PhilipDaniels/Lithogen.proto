using Lithogen.Interfaces;
using Moq;
using NUnit.Framework;
using System;

namespace Lithogen.Core.Tests
{
    public class BuilderTests
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Ctor_WhenBuildContextIsNull_ThrowsArgumentNullException()
        {
            var b = new Builder(null);
        }

        [Test]
        public void Ctor_WhenCompleted_CreatesEmptyStepsCollection()
        {
            var bc = new Mock<IBuildContext>();
            var b = new Builder(bc.Object);
            Assert.NotNull(b.Steps);
            Assert.IsEmpty(b.Steps);
        }
    }
}
