using System.Collections.Generic;
using System.Linq;

using FluentAssertions;

using Xunit;

namespace AptlyParser.Tests
{
    public class VersionOrderTests
    {
        [Theory]
        [InlineData("2.3-1.0", "2.3.8-1.0")]
        [InlineData("0.9.12.3.1173-937aac3", "0.9.12.18.1520-6833552")]
        [InlineData("1.1.0.2611-ba905d2", "1.1.2.2680-09e98fb")]
        [InlineData("1.1~pre9-1", "1.1~pre11-1")]
        public void Can_oder_versions(string _1, string _2)
        {
            var versions = new List<string>
            {
                _2,
                _1
            };

            // Act
            versions = versions.OrderBy(x => x, new VersionComparer()).ToList();

            // Assert
            versions.Should().StartWith(_1);

            versions = new List<string>
            {
                _1,
                _2
            };

            // Act
            versions = versions.OrderBy(x => x, new VersionComparer()).ToList();

            // Assert
            versions.Should().StartWith(_1);
        }
    }
}