app.controller('indexController', 
['serverService', '$scope', 
function(serverService, $scope) {
	$scope.server = serverService;
	$scope.test = "Message depuis indexController " + $scope.server.serviceData;
	$scope.rooms = []

	// Fonction de test ! :p
	$scope.updateRooms = function() 
	{
		$scope.server.getRessource("rooms", {})
			.done(function(data, statusCode)
			{
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
						$scope.rooms.push({ "id" : roomId, "name" : roomName });
					});
				});
			});
	}
}]);