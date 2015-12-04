app.controller('indexController', 
['serverService', '$scope', 
function(serverService, $scope) {
	$scope.server = serverService;
	$scope.test = "Message depuis indexController " + $scope.server.serviceData;
}]);