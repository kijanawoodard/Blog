Get-ChildItem -Path "src/BlogEngine.Site/Content" -Recurse -Filter "*.md" | ForEach-Object {
    $content = Get-Content $_.FullName -Raw
    
    # Split the content at the comments marker
    $parts = $content -split "(?ms)---\s*#\s*comments\s+begin\s+here\s*\r?\n"
    
    if ($parts.Length -eq 2) {
        $mainContent = $parts[0]
        $comments = $parts[1].Trim()
        
        if ($comments) {
            # Split the main content to get front matter
            $mainParts = $mainContent -split "(?ms)^---\s*\r?\n(.*?)\r?\n---\s*\r?\n"
            if ($mainParts.Length -eq 3) {
                $beforeFrontMatter = $mainParts[0]
                $frontMatter = $mainParts[1].TrimEnd()
                $afterFrontMatter = $mainParts[2]
                
                # Reconstruct the file
                $newContent = "---`n" + 
                             $frontMatter + 
                             "`ncomments:`n" + 
                             ($comments -replace "(?m)^", "  ").TrimEnd() + 
                             "`n---`n" + 
                             $afterFrontMatter
                
                # Write the modified content back
                Set-Content -Path $_.FullName -Value $newContent -NoNewline
            }
        }
    }
} 