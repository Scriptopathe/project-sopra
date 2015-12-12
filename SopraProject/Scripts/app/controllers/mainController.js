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
			$scope.$apply(function()
			{
				$scope.username = data;
				$scope.logged = true;
			});
		});
	};

	$scope.updateUser();
}]);