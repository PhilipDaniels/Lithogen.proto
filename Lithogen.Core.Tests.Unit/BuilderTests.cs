using Lithogen.Core.FileSystem;
using NSubstitute;
using NUnit.Framework;
using System;

namespace Lithogen.Core.Tests.Unit
{
    public class BuilderTests
    {
        [SetUp]
        public void Setup()
        {
            TheMockFS = Substitute.For<ICountingFileSystem>();
            TheMockLogger = Substitute.For<ILogger>();
            TheSettings = new Settings();
        }
        public ICountingFileSystem TheMockFS;
        public ILogger TheMockLogger;
        public Settings TheSettings;

        [Test]
        public void Ctor_WhenArgumentsAreNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new Builder(null, TheMockLogger, TheSettings));
            Assert.Throws<ArgumentNullException>(() => new Builder(TheMockFS, null, TheSettings));
            Assert.Throws<ArgumentNullException>(() => new Builder(TheMockFS, TheMockLogger, null));
        }

        [Test]
        public void Ctor_WhenCompleted_CreatesStepsCollectionAndSetsContextAndFileSystem()
        {
            var builder = new Builder(TheMockFS, TheMockLogger, TheSettings);
            Assert.NotNull(builder.Steps);
            Assert.AreSame(TheMockFS, builder.FileSystem);
            Assert.AreSame(TheMockLogger, builder.Logger);
            Assert.AreSame(TheSettings, builder.Settings);
        }
    }
}
