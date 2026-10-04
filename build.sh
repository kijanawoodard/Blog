#!/bin/sh
curl -sSL https://dot.net/v1/dotnet-install.sh > dotnet-install.sh
chmod +x dotnet-install.sh
./dotnet-install.sh -c 9.0 -InstallDir ./dotnet
./dotnet/dotnet --version
./dotnet/dotnet run --launch-profile "static-site-generator" --project ./src/BlogEngine.Site/

# BlazorStatic lists folder-style pages without a trailing slash, but Cloudflare Pages redirects them to the slash form.
# Rewrite the sitemap to the final URLs so it contains no redirects.
sed -i -E 's#(https://kijanawoodard.com/(about|contact|tags))</loc>#\1/</loc>#' ./src/BlogEngine.Site/output/sitemap.xml
rm -f ./src/BlogEngine.Site/output/sitemap.xml.gz
