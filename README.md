# URL Launcher

Console app (.NET 8) that reads a `link` file placed beside the executable and opens composed URLs in the default browser.

## Usage
- Build/publish the project; the executable name is `URL Launcher`.
- Place a text file named `link` next to the executable.
- File format:
  - First line: base URL (e.g., `https://abc.com/items`), trailing slash optional.
  - Following lines: path parts to append (one per line), e.g., `123`.
- Running the app will open each combined URL, e.g., `https://abc.com/items/123`.

## Notes
- Root namespace: `RA656.UtilityLauncher`.
- Empty or invalid entries are reported; processing continues for remaining items.

