app.controller('indexController', 
['serverService', '$scope', '$timeout',
function(serverService, $scope, $timeout) {
	$scope.server = serverService;
	// User location (site identifier). -1 for none => not loaded.
	$scope.userLocation = "-1";
	// Existing sites
	$scope.sites = {};
	$scope.rooms = {};

	$scope.load = function() 
	{
		$scope.$apply(function()
		{
			$scope.updateRooms();
			$scope.updateSites();
		});
	};

	// Fonction de test ! 
	$scope.updateRooms = function() 
	{
		$scope.server.getRessource("rooms", {})
		.done(function(data, statusCode)
		{
			$scope.rooms = {};
			// On parse le XML qu'on a récupéré du serveur.
			var xml = $( $.parseXML( data ) )

			// On parcours tous les noeuds "Room"
			xml.find("Room").each(function()
			{
				// On obtient une représentation du Noeud <Room>
				var room = $(this);

				// $scope.$apply permet de faire en sorte qu'angular
				// force la vérification des modifications et mette à jour
				// la vue une fois qu'on a modifié $scope.rooms !
				$scope.$apply(function() 
				{
					// On prend l'attribut id de la room
					var roomId = room.attr("id");
					// On récupère le texte contenu dans le champ "Name"
					var roomName = room.children("Name").text();
					// On ajoute le tout dans $scope.rooms
					$scope.rooms[roomId] = { "id" : roomId, "name" : roomName };
				});
			});
		});
	};

	// Loads the site list into the $scope.sites variable.
	$scope.updateSites = function() 
	{
		$scope.server.getRessource("sites", {})
		.done(function(data, statusCode)
		{
			$scope.sites = {};
			var xml = $( $.parseXML( data ) );
			xml.find("Site").each(function()
			{
				var site = $(this);
				$scope.$apply(function() 
				{
					var siteId = site.attr("id");
					var siteName = site.children("Name").text();
					var siteAddress = site.children("Address").text();
					$scope.sites[siteId] = { "id" : siteId, "name" : siteName, "address" : siteAddress };
				});
			});
		});
	};

	// Loads the data later
	$timeout(function() { $scope.load(); });
}]);