using System.Text.Json;
using UniGetUI.Core.Data;

namespace UniGetUI.Core.Tools.Tests;

public class IntegrityTesterTests
{
    [Fact]
    public void CheckIntegrity_IgnoresPortableSettingsEntries()
    {
        string exeDir = CoreData.UniGetUIExecutableDirectory;
        string integrityTreePath = Path.Join(exeDir, "IntegrityTree.json");
        string uniqueName = $"IntegrityIgnoreTest_{Guid.NewGuid():N}.json";
        string settingsRelativePath = $"Settings/Configuration/{uniqueName}";
        string settingsAbsolutePath = Path.Join(exeDir, "Settings", "Configuration", uniqueName);

        Directory.CreateDirectory(Path.GetDirectoryName(settingsAbsolutePath)!);
        File.WriteAllText(settingsAbsolutePath, "{\"test\": true}");

        bool hadOriginalTree = File.Exists(integrityTreePath);
        string? originalTree = hadOriginalTree ? File.ReadAllText(integrityTreePath) : null;

        try
        {
            Dictionary<string, string> tree = new()
            {
                [settingsRelativePath] = "00000000000000000000000000000000"
            };
            File.WriteAllText(integrityTreePath, JsonSerializer.Serialize(tree));

            var result = IntegrityTester.CheckIntegrity(allowRetry: false);
            Assert.True(result.Passed);
            Assert.Empty(result.MissingFiles);
            Assert.Empty(result.CorruptedFiles);
        }
        finally
        {
            if (hadOriginalTree)
            {
                File.WriteAllText(integrityTreePath, originalTree!);
            }
            else if (File.Exists(integrityTreePath))
            {
                File.Delete(integrityTreePath);
            }

            if (File.Exists(settingsAbsolutePath))
            {
                File.Delete(settingsAbsolutePath);
            }
        }
    }

    [Fact]
    public void CheckIntegrity_IgnoresNestedPublishEntries()
    {
        string exeDir = CoreData.UniGetUIExecutableDirectory;
        string integrityTreePath = Path.Join(exeDir, "IntegrityTree.json");
        string publishRelativePath = "publish/UniGetUI.exe";

        bool hadOriginalTree = File.Exists(integrityTreePath);
        string? originalTree = hadOriginalTree ? File.ReadAllText(integrityTreePath) : null;

        try
        {
            Dictionary<string, string> tree = new()
            {
                [publishRelativePath] = "00000000000000000000000000000000"
            };
            File.WriteAllText(integrityTreePath, JsonSerializer.Serialize(tree));

            var result = IntegrityTester.CheckIntegrity(allowRetry: false);
            Assert.True(result.Passed);
            Assert.Empty(result.MissingFiles);
            Assert.Empty(result.CorruptedFiles);
        }
        finally
        {
            if (hadOriginalTree)
            {
                File.WriteAllText(integrityTreePath, originalTree!);
            }
            else if (File.Exists(integrityTreePath))
            {
                File.Delete(integrityTreePath);
            }
        }
    }
}
