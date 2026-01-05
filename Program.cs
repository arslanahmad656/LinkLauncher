using System.Diagnostics;

var linkFilePath = Path.Combine(AppContext.BaseDirectory, "link");

if (!File.Exists(linkFilePath))
{
    Console.Error.WriteLine($"Link file not found at: {linkFilePath}");
    Console.Error.WriteLine("Create a file named 'link' beside the executable with a base URL on the first line and path parts on following lines.");
    return 1;
}

var lines = await File.ReadAllLinesAsync(linkFilePath);
if (lines.Length == 0)
{
    Console.Error.WriteLine("Link file is empty. Expected a base URL on the first line.");
    return 1;
}

var baseRaw = lines[0].Trim();
if (string.IsNullOrWhiteSpace(baseRaw))
{
    Console.Error.WriteLine("Base URL on the first line is missing or blank.");
    return 1;
}

if (!Uri.TryCreate(baseRaw, UriKind.Absolute, out _))
{
    Console.Error.WriteLine($"Base URL is not valid: {baseRaw}");
    return 1;
}

var suffixes = lines
    .Skip(1)
    .Select(l => l.Trim())
    .Where(l => !string.IsNullOrWhiteSpace(l))
    .ToList();

if (suffixes.Count == 0)
{
    Console.Error.WriteLine("No suffix lines found to launch. Add one URL part per line after the base URL.");
    return 1;
}

var baseUrl = baseRaw.TrimEnd('/');
var hadFailure = false;

foreach (var suffix in suffixes)
{
    var combined = Combine(baseUrl, suffix);

    try
    {
        Console.WriteLine($"Opening: {combined}");
        Process.Start(new ProcessStartInfo
        {
            FileName = combined,
            UseShellExecute = true
        });
    }
    catch (Exception ex)
    {
        hadFailure = true;
        Console.Error.WriteLine($"Failed to open {combined}: {ex.Message}");
    }
}

return hadFailure ? 1 : 0;

static string Combine(string baseUrl, string suffix)
{
    var cleanedSuffix = suffix.Trim().Trim('/');
    return string.IsNullOrEmpty(cleanedSuffix)
        ? baseUrl
        : $"{baseUrl}/{cleanedSuffix}";
}
