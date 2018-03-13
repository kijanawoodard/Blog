---
title: Setting Up Azure CDN
created: 8/4/2016 9:57:46 AM
published: 8/4/2016 9:57:46 AM
tags: blog, notes
---

Using a CDN is a well known way to increase the performance of your web site. At $0.087 per GB, I figured why not give the [Azure CDN] a try.

My first concern was I didn't want to have to push assets, especially css, to the CDN as a separate step in my release process. I found a decent guide on [setting up origin pull for the Azure CDN]. That post shows the old portal. The process in the new portal is a bit nicer, but the concepts are the same.

I did run into a couple of issues which may help you out.

I was confused at first, but the simple choice was setting the Endpoint Type as web app. Once I chose my site, most of the other options were filled in. 

For some reason, the Standard Verizon option didn't work well for me. It has a 90 minute propagation time and I couldn't quite seem to get things working consistently. Since the Standard Akamai option has a 1 minute propagation time, I switched to that. The CDN worked quite smoothly from there.

To switch, I had to delete the CDN in azure portal and set it back up again. Fortunately, the names I used were released and avaialble again the second time.

The other thing I ran into was [Font Awesome] webfonts were not loading correctly. It turned out to be a CORS issue. After wandering around for a bit trying to figure out how to change the CORS settings for the CDN itself, I found out I could add the settings to the web.config of my site.

    <system.webServer>
        <httpProtocol>
          <customHeaders>
            <add name="Access-Control-Allow-Origin" value="*"/>
          </customHeaders>
        </httpProtocol>
    </system.webServer>

I also managed to setup a custom domain, so instead of `kijanawoodard.azureedge.net`, my static files are served from `cdn.kijanawoodard.com`.

Everything appears to be serving correctly now. It's amazing how easy doing things like this has become.

[Azure CDN]: https://azure.microsoft.com/en-us/services/cdn/
[setting up origin pull for the Azure CDN]: https://josephwoodward.co.uk/2015/08/setting-up-origin-pull-azure-cdn/
[Font Awesome]: https://fontawesome.io/

---
# comments being here

---
