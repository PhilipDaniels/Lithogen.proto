using Lithogen.Interfaces;
using NSubstitute;
using NUnit.Framework;
using System;

namespace Lithogen.Core.Tests.Unit
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
            var bc = Substitute.For<IBuildContext>();
            var b = new Builder(bc);
            Assert.NotNull(b.Steps);
            Assert.IsEmpty(b.Steps);
        }
    }
}
