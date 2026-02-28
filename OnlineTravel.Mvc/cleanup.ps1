$controllersDir = "c:\Users\Muhammad\source\repos\OnlineTravel\OnlineTravel.Mvc\Controllers"
$files = Get-ChildItem -Path $controllersDir -Filter "*.cs"
foreach ($file in $files) {
    $content = Get-Content -Path $file.FullName -Raw
    # Regex to find fully qualified names
    $pattern = "\b(OnlineTravel\.(?:Application|Domain|Mvc)(?:\.\w+)+)\.([A-Z]\w+)\b"
    $matches = [regex]::Matches($content, $pattern)
    # Debug: Print matches
    if ($matches.Count -gt 0) {
        Write-Host "File: $($file.Name)"
    }
    $usingsToAdd = @()
    foreach ($match in $matches) {
        $namespace = $match.Groups[1].Value
        $className = $match.Groups[2].Value
        $usingsToAdd += "using $namespace;"
        Write-Host "  Replacing $($match.Value) -> $className (adding using $namespace)"
        $content = $content.Replace($match.Value, $className)
    }
    if ($usingsToAdd.Count -gt 0) {
        $uniqueUsings = $usingsToAdd | Select-Object -Unique | Sort-Object
        $lines = $content -split "`r`n"
        if ($lines.Length -eq 1 -and $content -match "`n") {
            $lines = $content -split "`n"
        }
        $insertAt = 0
        for ($i = 0; $i -lt $lines.Length; $i++) {
            if ($lines[$i] -match "^using ") {
                $insertAt = $i + 1
            } elseif ($lines[$i] -match "^namespace ") {
                if ($insertAt -eq 0) {
                    $insertAt = $i
                }
                break
            }
        }
        $newUsings = @()
        foreach ($u in $uniqueUsings) {
            # Only add if not already in the file
            if ($content -notmatch [regex]::Escape($u)) {
                $newUsings += $u
            }
        }
        if ($newUsings.Count -gt 0) {
            $contentArray = @()
            for ($i = 0; $i -lt $lines.Length; $i++) {
                if ($i -eq $insertAt) {
                    $contentArray += $newUsings
                }
                $contentArray += $lines[$i]
            }
            $content = $contentArray -join "`r`n"
        }
        Set-Content -Path $file.FullName -Value $content
    }
}
Write-Host "Done"
