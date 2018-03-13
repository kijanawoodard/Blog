---
title: FubuMVC, Validation, and Re-Hydrating the View
published: February 12, 2012
tags: 
---

Last week, I started experimenting with FubuMVC. About two months ago, I met three of the Fubu guys down in Austin and they sparked my curiosity about [FubuMVC]. Last month I took [Udi Dahan’s excellent SOA course][SOA Course] and asked him about FubuMVC in light of his views on SOA. His response was to challenge me to give FubuMVC a try and find out.

I started working my way through the beginner material on FubuMVC when I struck upon an issue present in any web framework: when you POST data and hit a problem, how do you re-hydrate the view with the data the user entered and show the user about what went wrong.

I recently came up with a solution that I was happy with in ASP.Net MVC3. However, the solution relied on a base class and FubuMVC steers us toward a compositional approach to development. I ran across these [stack] [overflow] posts and they meshed very well with my solution for ASP.Net MVC3.

The crux of the idea is that the Input Model for the POST is symmetrical with the View Model for the GET. I took the idea just a bit further.

    public class FooEditRequestModel
    {
      public int FooId { get; set; }
    }
 
    public class FooEditViewModel : IRedirectable
    {
      public int FooId { get; set; }
 
      [Required]
      public int BarId { get; set; }
      public IEnumerable<Bar> Bars { get; set; } //reference/lookup data
 
      public FubuContinuation RedirectTo { get; set; }
    }
 
    public class FooEditInputModel
    {
      public int FooId { get; set; }
 
      [Required]
      public int BarId { get; set; }
    }

I defined a RequestModel for the GET, a ViewModel which might contain reference data, and an InputModel which is POSTed back to the server.

The reference data is the kicker. I really don’t want to post all the reference data back to the server when most of my posts will be fine. On more complex pages, it’s just difficult to do. However, once on the server, it’s tricky to marshal the user’s input data over to the GET method. I also don’t want my GET method to have to deal with the possibility of Input model data showing up as parameters.

Here’s some non working code I threw together to see how I liked my idea.

    public FooEditViewModel Execute(FooEditRequestModel request)
    {
        var foo = db.GetFooById(request.FooId);
        var bars = db.GetBars();
        var model = new FooEditViewModel() {FooId = foo.FooId, BarId = foo.BarId, Bars = bars};
        return model;
    }
 
    public FooEditViewModel Execute(FooEditInputModel input)
    {
        var model = new FooEditViewModel();
 
        try
        {
            db.UpdateBarId(input.FooId, input.BarId);
            model.RedirectTo = FubuContinuation.RedirectTo<SomeOtherRequestModel>();
        }
        catch (MyValidationBusinessRuleOrDBExceptions e)
        {
            //Auto Mapper the properties needed for the request
            var request = input.MapTo<FooEditRequestModel>(); 
            model = Execute(request);
            model = input.MapTo(model); //Auto Mapper the input data to rehydrate the view
        }
        return model;
    }

I’m using some FubuMVC patterns here, so you’ll kinda have to accept that this is possible if you’re coming from another web server stack. Both my GET and POST methods return the view model. I taught FubuMVC that any method the takes a class with "Request" in the name is a GET and any method that takes a class with "Input" in the name is a POST.

The GET is pretty standard. The POST is where the fun begins.

First, I’m not happy with the Try/Catch. I’d rather wrap that up with a [Fubu Behavior]. But let’s move on for a second.

If all goes well, I’m going to update my Foo and redirect to wherever using the [IRedirectable] interface. A lot of FubuMVC POST examples return FubuContinuation to handle the redirection, but I wanted to be able to return my view model directly from the POST to avoid having to find a way to get the data over to the GET method.

If my request goes sideways, I *should* be able to get all the data I need from my InputModel in order to build a RequestModel. With that RequestModel, I can simply execute the GET and obtain a ViewModel. Now I can just use [AutoMapper] to copy over the InputModel properties over the ViewModel and I should be golden. FubuMVC really shines here. In my ASP.Net version of this, the action method returns a ViewResult which I had to crack open in order to find and modify the view model within.

Now back to that try/catch I don’t like. The FubuMVC examples I’ve seen show how to set up validation failure handlers to do something when the InputModel doesn’t validate.

It seems to me there are 4 general concepts of correctness for POSTing an InputModel.

1. The data should be in the right shape (i.e., required fields are present and fields are of the right type, length, etc). This is handled by FubuValidation or Model.IsValid in ASP.Net MVC.
2. The data should meet all business rules. If you GET the page a 2:55PM and submit at 3:01PM, a business rule stating that a particular BarId is not valid after 3 would invalidate the request.
3. The data should pass all concurrency checks when submitted to the database. In addition, the database simply being unavailable could fail the request here.
4.  The data and database are in great shape, the user is authorized to make the request, but the user is POSTing data they are unauthorized for. Image a user who is allowed to POST BarIds 1-5. The user is smart and knows that BarId 15 exists and uses Firebug to alter the form before clicking submit.

In the last case, I prefer to send the user to a "Fail Whale" type of generic error page and issue a priority 1 alert to OPS. Either, someone is hacking the system, or we have a serious bug in our UI that is presenting invalid options to users. Either way, someone should deal with the problem quickly.

In the other cases, it’s nice to present the form back to the user telling them what’s wrong and letting them make a choice. The PRG pattern above allows for this.

Using FubuMVC, I can imagine ways to get this into the behavior chain based on our conventions and have it working seamlessly. I’ve been growing increasingly wary of inheritance, but I can see defining our classes like this.

    public class FooEditRequestModel
    ...
    public class FooEditInputModel : FooEditRequestModel
    ....
    public class FooEditViewModel : FooEditInputModel, IRedirectable   
    ....

Now our behaviors could know exactly which methods to execute and which data to copy when doing it’s work.

Boy…This was a lot of work.

It looks like the Fubu team is working hard to get some similar behavior using symmetrical models baked into FubuMVC. For "completeness", they probably should.

After going through this exercise, I had to ask myself how the Fubu team themselves have managed to get by without all this. The answer is that they mostly POST via ajax.

I read about [AjaxContinuations] when I was first starting to dig into FubuMVC, but I ignored it as an oddity. Of course I want to POST my entire page back to the server, because…well, because….because that’s what’s done.

Now imagine my form POSTed via ajax. Immediately, my "re-hydrate" the ViewModel problem evaporates. All my thought on symmetrical models seems meaningless. My how do I wrap all this in a Behavior/Try/Catch simplifies as well.

In addition I gain some interesting choices for the Concurrency Violation / DB Down case. I can auto-repost or I can tell the user who changed what data or I can just do the normal "submit again" messaging.

Of course I have some work to do on the client like tie error messages back to fields and follow urls that come in via the ajax response. It turns out the Fubu team has been [working on all that stuff][continuations work], but that wouldn’t be too hard to cook up yourself either.

For the last couple of hours I’ve been trying to come up with a scenario where it was unacceptable to POST via ajax. For web "applications", dependence on javascript is standard. I wouldn’t want to try and build a complex app without it. For e-commerce, I can see not forcing javascript on users. However, it turns out that they are mostly posting data that can’t fail an authorization check. By that I mean if they spoof another valid ProductId on their form, so what, they just found a new way to fill out their shopping cart. The only places you may have an issue is when they submit CC info or Address information. But these places are limited compared to a web "application" where most of the pages involve modifying data in some way.

I’m going to try to POST via ajax most of the time.

[FubuMVC]:https://fubumvc.github.io/
[SOA Course]:http://udidahan.com/training/
[stack]:https://stackoverflow.com/questions/6759287/how-to-set-up-fubumvc-validation
[overflow]:https://stackoverflow.com/questions/8856390/fubumvc-simple-forms-validation-using-ifailurevalidationpolicy
[Fubu Behavior]:https://lostechies.com/chadmyers/2011/06/23/cool-stuff-in-fubumvc-no-1-behaviors/
[IRedirectable]:https://github.com/ianbattersby/FubuMVC.Recipes/tree/master/src/Continuations/IRedirectable
[AutoMapper]:https://automapper.org/
[AjaxContinuations]:https://lostechies.com/josharnold/2012/01/06/our-ajax-conventions-the-ajaxcontinuation/
[continuations work]:https://lostechies.com/josharnold/2012/01/06/our-ajax-conventionsclientside-continuations/

---
# comments begin here

- Email: "nospam@ventaur.com"
  Message: "<p>This post brings to light quite a few good points. The Fubu team has not focused on PRG for the very reason you point out; they use AJAX most of the time. They have admitted this fact for quite a while now.</p><p>I get the benefits too. However, a lot of my clients are trainers of various materials and expect that their students will be on disparate systems with older browsers. Furthermore, there still some companies out there that insist on disabling JavaScript across the entire corporation. It boggles my mind that this is still common practice, but it is nonetheless.</p><p>So, I still need to be able to support both POST scenarios. Typically, I just hook the form's with my AJAX. If JavaScript is disabled, the form's action takes over and a full POST occurs. Like you, I then run into the re-hydration problems. I hate that I even have to bother with the classic PRG, but I do.</p><p>Finally, if this does get baked into FubuMvc sometime fairly soon, I'll want to use behaviors to detect an AJAX POST vs. a standard one and handle all the cruft for each for me. For now, I'm going to think about your PRG solution and do some trials. It's a very interesting technique and, I don't see any waste if an AJAX POST does happen; the standard continuation just won't happen.</p><p>Great post!</p>"
  Name: "Matt S."
  When: "2012-02-28 07:43:45.000"