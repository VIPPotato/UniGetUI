namespace UniGetUI.Core.Data.Tests
{
    public class CoreTests
    {
        public static object[][] Data =>
            [
                [CoreData.UniGetUIDataDirectory],
                [CoreData.UniGetUIInstallationOptionsDirectory ],
                [CoreData.UniGetUICacheDirectory_Data ],
                [CoreData.UniGetUICacheDirectory_Icons ],
                [CoreData.UniGetUICacheDirectory_Lang ],
                [CoreData.UniGetUI_DefaultBackupDirectory ]
             ];

        [Theory]
        [MemberData(nameof(Data))]
        public void CheckDirectoryAttributes(string directory)
        {
            Assert.True(Directory.Exists(directory), $"Directory ${directory} does not exist, but it should have been created automatically");
        }

        [Fact]
        public void CheckOtherAttributes()
        {
            Assert.NotEmpty(CoreData.VersionName);
            Assert.NotEqual(0, CoreData.BuildNumber);
            Assert.NotEqual(0, CoreData.UpdatesAvailableNotificationTag);

            Assert.True(Directory.Exists(CoreData.UniGetUIExecutableDirectory), "Directory where the executable is located does not exist");
            Assert.True(File.Exists(CoreData.UniGetUIExecutableFile), "The executable file does not exist");
        }

        [Fact]
        public void ExecutablePathAndDirectoryAreConsistent()
        {
            string executableDirectory = CoreData.UniGetUIExecutableDirectory;
            string? parentDirectory = Path.GetDirectoryName(CoreData.UniGetUIExecutableFile);

            Assert.False(string.IsNullOrWhiteSpace(parentDirectory), "Executable parent directory should be resolvable");
            Assert.True(
                string.Equals(
                    Path.GetFullPath(executableDirectory).TrimEnd('\\'),
                    Path.GetFullPath(parentDirectory!).TrimEnd('\\'),
                    StringComparison.OrdinalIgnoreCase),
                "Executable directory and executable parent directory should match");
        }
    }
}
