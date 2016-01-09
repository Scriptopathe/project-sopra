app.service("serverService", function() {
	// Gets the ressource under api/ with the given partial URI and arguments.
	// For instance : getRessource("rooms", { "id" : 1 }) will direct to 
	// 				  www.domain.com/api/rooms?id=1
	// Utilisation example :
	// getRessource("Rooms, {})
	//	.done(function(data, statusCode) { alert(data); });
	//  .fail(...)			[ Optional ]
    //  .always(...)		[ Optional ]

    jQuery.ajaxSettings.traditional = true;
	this.getRessource = function(partialUri, args)
	{
		return $.get("../api/" + partialUri, args)
	};

	this.postRessource = function(partialUri, args)
	{
		return $.post("../api/" + partialUri, args)
	};
});