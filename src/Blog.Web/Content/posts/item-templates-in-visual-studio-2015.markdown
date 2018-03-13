---
title: Item Templates in Visual Studio 2015
created: 8/9/2016 5:46:49 PM
published: 8/10/2016 5:46:49 AM
tags: blog, notes
---

Now that blog posts, metadata, and comments are combined in one file, it's less work to start a new post. I figured I'd reduce friction even further by using a Visual Studio Item Template. It's best to remove every barrier to writing new posts. ;-)

I found a nice article on [Visual Studio templates by Eric Sowell][Eric Sowell] which answered the basic questions and another about [multiple file templates] that happened to get me past a stumbling block when editing a template.

Mostly everything is the same as in those posts, except the directories are different for the newer version of Visual Studio. There are a couple of caveats when doing a template for a file that doesn't end in `.cs`. To make it easier to see what I'm doing, I also made a [companion video].

Here's the template I'm using for new posts:

    ---
    title: $safeitemname$
    created: $time$
    published: $time$
    tags: 
    ---



    ---
    # comments being here

    ---

To make a template, you simply create a file in Visual Studio, put whatever text you want in it, and then go to `File | Export Template` and follow the prompts.

The words surrounded by dollar signs are template parameters. There's a nice list of [template parameters] on msdn.

For whatever reason, the template paramters are not always replaced automatically. For `.cs` files, it worked with no further effort. For my `.markdown` template, I had to extract the exported template and edit the `vstemplate` xml file. There is an attribute of the `ProjectItem` nodde called `ReplaceParameters`. Set it's value to true.

Now, here's the stumbling block I mentioned before. You must zip up the files directly. Do *not*, like I did, zip the folder. It creates an extra level of nesting. There won't be any warning in visual studio. The template just won't show up.

After editing the template, I found it necessary to manually move the file from `Users\{user}\Documents\Visual Studio 2015\My Exported Templates` to `Users\{user}\Documents\Visual Studio 2015\Templates\ItemTemplates`. 

Finally, I needed to restart Visual Studio. Sometimes using the suggestion of issuing `devenv /installvstemplates` from a Visual Studio command prompt seemed to work. Other times it didn't. Restarting Visual Studio worked consistently. Once it's done, you don't have to think about it again, so I didn't spend more time trying to nail down the fastest way to refresh the template cache.

Hopefully those tips will help you get your own templates up and running smoothly.

If anything is unclear, try watching the [companion video] on YouTube. Please [contact me] with any comments, ommissions, or clarifications. Making the video was fun. I'll mix in more video content with future posts.


[Eric Sowell]: http://ericsowell.com/blog/2007/5/22/how-to-edit-visual-studio-templates
[multiple file templates]: http://kerlagon.com/Blog/item-templates-saving-you-time-in-visual-studio/
[companion video]: https://www.youtube.com/watch?v=Xf3d5PKjUNc
[template parameters]: https://msdn.microsoft.com/en-us/library/eehb4faa.aspx?f=255&MSPPError=-2147217396
[contact me]: /contact

---
# comments being here

---