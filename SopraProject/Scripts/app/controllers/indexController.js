app.controller('indexController', 
['serverService', '$scope', '$timeout',
function(serverService, $scope, $timeout) {
	$scope.server = serverService;
	// User location (site identifier). -1 for none => not loaded.
	$scope.defaultUserLocation = "-1";
	$scope.userLocation = "-1";
	// Existing sites
	$scope.sites = {};
	$scope.rooms = {};
    // Meeting duration
	$scope.meetingDuration = 15;
	$scope.durations = [{ h: "15min", v: 15 }, { h: "30min", v: 30 }, { h: "45min", v: 45 }, { h: "1h", v: 60 },
	                    { h: "1h15min", v: 75 }, { h: "1h30min", v: 90 }, { h: "1h45min", v: 105 }, { h: "2h", v: 120 },
	                    { h: "2h15min", v: 135 }, { h: "2h30min", v: 150 }, { h: "2h45min", v: 165 }, { h: "3h", v: 180 },
	                    { h: "3h15min", v: 195 }, { h: "3h30min", v: 210 }, { h: "3h45min", v: 225 }, { h: "4h", v: 240 }];

    // Particularities
	$scope.particularities = {}; // id, name, selected

	$scope.load = function() 
	{
		$scope.$apply(function()
		{
			$scope.updateRooms();
			$scope.updateSites();
			$scope.updateLocation();
			$scope.updateUser();
			$scope.loadParticularities();
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
			var xml = $( $.parseXML( data ) );

			// On parcourt tous les noeuds "Room"
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

	// Loads the user's default location.
	$scope.updateLocation = function() 
	{
		$scope.server.getRessource("location", {})
		.done(function(data, statusCode)
		{
			$scope.$apply(function()
			{
				$scope.defaultUserLocation = data;
				$scope.userLocation = data;
			});
		});
	};

    // Load particularities
	$scope.loadParticularities = function () {
	    $scope.server.getRessource("particularities", {})
        .done(function(data, statusCode)
        {
            $scope.particularities = {};
            var xml = $( $.parseXML( data ) );
            xml.find("Particularity").each(function()
            {
                var particularity = $(this);
                var name = particularity.children("Name").text();
                var id = particularity.attr("id");
                $scope.$apply(function () {
                    $scope.particularities[id] = { "id": id, "name": name, "selected": false };
                });
            });
        });
	};


	// Set default location.
	$scope.setDefaultLocation = function(loc)
	{
		$scope.server.postRessource("location", { siteId : loc })
		.done(function(data, statusCode)
		{
			$scope.$apply(function()
			{
				$scope.defaultUserLocation = loc;
			});
		});
	};


	// Room searching
	//(loc, date, duration, nbpers, part)
	$scope.roomSearching = function(loc, nbpers, part, sDate, eDate)
	{
		
		$scope.server.getRessource("searchwithdate", { siteId : loc, personCount : nbpers, particularities : part, startDate : sDate , endDate : eDate })
		.done(function(data, statusCode)
		{
		    alert(statusCode);
			$scope.rrooms = {};
			var xml = $($.parseXML(data));
			xml.find("Room").each(function()
			{
				var room = $(this);
				$scope.$apply(function()
				{
					var roomId = room.attr("id");
					var roomName = room.children("Name").text();
					var roomCapacity = room.children("Capacity").text();
					console.log(roomId);
					$scope.rrooms[roomId] = { "id" : roomId, "name" : roomName, "capacity" : roomCapacity };
				});
			});

		})
	    .fail(function (xhr, statusCode, error) {
	        alert(xhr.responseText);
	    });
	}; 


	// Loads the data later
	$timeout(function () { $scope.load(); });
}]);