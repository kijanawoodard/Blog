$blogPath = "src/BlogEngine.Site/Content/Blog"
$files = Get-ChildItem -Path $blogPath -Filter "*.md"

foreach ($file in $files) {
    $content = Get-Content $file.FullName -Raw
    if ($content -match "published:\s*(.+?)(\r?\n|$)") {
        $publishedDate = [DateTime]::Parse($matches[1])
        $newPrefix = $publishedDate.ToString("yyyy-MM-dd")
        
        # Only add the prefix if it's not already there
        if (-not ($file.Name -match "^\d{4}-\d{2}-\d{2}-")) {
            $newName = "$newPrefix-$($file.Name)"
            $newPath = Join-Path $file.Directory.FullName $newName
            
            Write-Host "Renaming $($file.Name) to $newName"
            Rename-Item -Path $file.FullName -NewName $newName -Force
        }
    } else {
        Write-Host "Warning: No published date found in $($file.Name)"
    }
} 