using Lithogen.Core.FileSystem;
using Lithogen.Interfaces;
using NSubstitute;
using NUnit.Framework;
using System;

namespace Lithogen.Core.Tests.Unit
{
    public class BuilderTests
    {
        // TODO: Fix this
        //[SetUp]
        //public void Setup()
        //{
        //    TheMockFS = Substitute.For<ICountingFileSystem>();
        //    TheMockBuildContext = Substitute.For<IBuildContext>();
        //}
        //public ICountingFileSystem TheMockFS;
        //public IBuildContext TheMockBuildContext;

        //[Test]
        //public void Ctor_ArgumentsAreNull_ThrowsArgumentNullException()
        //{
        //    Assert.Throws<ArgumentNullException>(() => new Builder(null, TheMockFS));
        //    Assert.Throws<ArgumentNullException>(() => new Builder(TheMockBuildContext, null));
        //}

        //[Test]
        //public void Ctor_WhenCompleted_CreatesEmptyStepsCollectionAndSetsContextAndFileSystem()
        //{
        //    var builder = new Builder(TheMockBuildContext, TheMockFS);
        //    Assert.NotNull(builder.Steps);
        //    Assert.IsEmpty(builder.Steps);
        //    Assert.AreSame(TheMockBuildContext, builder.BuildContext);
        //    Assert.AreSame(TheMockFS, builder.FileSystem);
        //}
    }
}
