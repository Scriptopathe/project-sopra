app.controller('mainController', 
['serverService', '$scope', '$timeout',
function(serverService, $scope, $timeout) {
	$scope.server = serverService;
	// Connected username
	$scope.username = "Not logged in";
	$scope.logged = false;
	$scope.updateUser = function()
	{
		$scope.server.getRessource("user", {})
		.done(function(data, statusCode)
		{
		    var xml = $($.parseXML(data));
			$scope.$apply(function()
			{
			    $scope.username = xml.find("Username").text();
			    $scope.isAdmin = xml.find("IsAdmin").text() == "true";
				$scope.logged = true;
			});
		});
	};

	$scope.updateUser();
}]);