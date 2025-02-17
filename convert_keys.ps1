# Install PowerShell-yaml module if not already installed
if (-not (Get-Module -ListAvailable -Name powershell-yaml)) {
    Install-Module -Name powershell-yaml -Force -Scope CurrentUser
}

Import-Module powershell-yaml

function Process-MarkdownFile {
    param (
        [string]$FilePath
    )

    try {
        # Read the file content as an array of lines
        $lines = Get-Content -Path $FilePath
        
        # Check if file starts with front matter
        if ($lines[0] -ne "---") {
            return
        }
        
        # Find the second marker
        $endIndex = -1
        for ($i = 1; $i -lt $lines.Count; $i++) {
            if ($lines[$i] -eq "---") {
                $endIndex = $i
                break
            }
        }
        
        if ($endIndex -eq -1) {
            return
        }

        # Check if the file has comments
        $hasComments = $false
        for ($i = 0; $i -lt $endIndex; $i++) {
            if ($lines[$i] -match "comments:") {
                $hasComments = $true
                break
            }
        }
        
        if (-not $hasComments) {
            return
        }

        $modified = $false
        
        # Process each line in the front matter
        for ($i = 0; $i -lt $endIndex; $i++) {
            $line = $lines[$i]
            
            # Check for comment keys and convert to lowercase
            if ($line -match '^\s*-\s*Email:') {
                $lines[$i] = $line -replace 'Email:', 'email:'
                $modified = $true
            }
            elseif ($line -match '^\s+Message:') {
                $lines[$i] = $line -replace 'Message:', 'message:'
                $modified = $true
            }
            elseif ($line -match '^\s+Name:') {
                $lines[$i] = $line -replace 'Name:', 'name:'
                $modified = $true
            }
            elseif ($line -match '^\s+When:') {
                $lines[$i] = $line -replace 'When:', 'when:'
                $modified = $true
            }
        }
        
        # Only rewrite if changes were made
        if ($modified) {
            [System.IO.File]::WriteAllLines($FilePath, $lines)
            Write-Host "Updated $FilePath"
        }
    }
    catch {
        Write-Host "Error processing $FilePath"
        Write-Host $_.Exception.Message
    }
}

# Process all .md files in the Content directory
Get-ChildItem -Path "src/BlogEngine.Site/Content" -Recurse -Filter "*.md" | ForEach-Object {
    Process-MarkdownFile -FilePath $_.FullName
} 