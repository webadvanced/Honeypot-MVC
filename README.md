#What is a honeypot?#

*From wikipedia*: in computer terminology, a honeypot is a trap set to detect, deflect, or in some manner counteract attempts at unauthorized use of information systems. Generally it consists of a computer, data, or a network site that appears to be part of a network, but is actually isolated and monitored, and which seems to contain information or a resource of value to attackers. Simple Honeypot is a honeypot solution for ASP.NET MVC2+.

##Simple honeypot is, _simple_ ##

Simple honeypot will simply stop bots from submitting spam via public forms on your website. There are some plans to make simple honeypot protect agents more types of spam, so check for new releases.

#Changelog:#

##Version 2.0.##

- Breaking: Constructor overload for HoneypotAttribute has changed.
- Removed dependency on TempData.
- Added token hidden input.
- Added HoneypotRuleCollection so users can add custom rules to detect bots.
- Fixed error when forms are submitted with Ajax.

#Usage#

Install package from nuget: **Install-Package SimpleHoneypot.MVC** 

###What is adds to your solution###
 
- SimpleHoneypot.dll
- WebActivator.dll
- App_Start directory
- App_Start/SimpleHoneypot.cs
 
###Default configuration of SimpleHoneypot.cs###

	public static void Start() {
				RegisterHoneypotInputNames(Honeypot.InputNames);
			}
			
			public static void RegisterHoneypotInputNames(HoneypotInputNameCollection collection) {
				//Honeypot will use 2 words at random to create the input name {0}-{1}
				collection.Add(new[]
							   {
								   "User",
								   "Name",
								   "Age",
								   "Question",
								   "List",
								   "Why",
								   "Type",
								   "Phone",
								   "Fax",
								   "Custom",
								   "Relationship",
								   "Friend",
								   "Pet",
								   "Reason"
							   });
			}
			
###Other configuration options###

By Default, Simple Honeypot will render an input with a CSS class name called 'input-imp-long'. You can change this CSS by calling `Honeypot.SetCssClassName("newCssName")` in the Start function of the SimpleHoneypot.cs class.

###The controller###

You will need to annotate the Action of your controller with the `[Honeypot]` Filter Attribute.

	[Honeypot, HttpPost]
	public ActionResult Comment(BlogComment comment) {
		/* Some Processing */
		return RedirectToAction("Index");
	}

The HoneypotAttribute optionally takes a url to redirect to as a String `[Honeypot("/Home/Honeytrap")]`

###The View###

Next, you will need to add a call to the HoneypotInput HtmlHelper in your form.

	@using SimpleHoneypot.HtmlHelpers
	@model BlogComment

	@using(Html.BeginForm("Comment", "Blog")) {
		@Html.EditorForModel()
		@Html.HoneypotInput()
		<p>
			<input type="submit" value="Comment" />
		</p>
	}
	
###Adding a class to your style sheet###

By default Simple Honeypot will use 'input-imp-long' for the input generated. However, if you used the `Honeypot.SetCssClassName("newCssName")` you will need to use the custom css name.

	.input-imp-long { display: none; }
**or**

	.newCssName { display: none; }
