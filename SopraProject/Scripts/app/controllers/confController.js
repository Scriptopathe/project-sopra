app.controller('confController',
['serverService', '$scope', '$timeout',
function (serverService, $scope, $timeout) {
    $scope.server = serverService;
    $scope.enableCache = true;
    $scope.searchDayStart = 0;
    $scope.searchDayEnd = 0;
    $scope.searchDayOptions = [];

    // Indicates if the set operation has been successfull
    $scope.successMessage = "ok";
    $scope.errorMessage = "error";
    $scope.updatingMessage = "updating";

    $scope.searchDayStartState = ""; // "ok", "updating", "error", "none"
    $scope.searchDayEndState = "";
    $scope.enableCacheState = "";
    $scope.invalidateCacheState = "";

    for (var i = 1; i <= 23; i++) $scope.searchDayOptions.push(i + "h");

    // Loads server configuration state
    $scope.loadState = function () {
        // Cache enabled
        $scope.server.getConf("cacheEnabled", {})
		.done(function (data, statusCode) {
		    $scope.enableCache = (data == "True" || data == "true");
		});

        // Search day start
        $scope.server.getConf("searchDayStart", {})
		.done(function (data, statusCode) {
		    $scope.searchDayStart = parseInt(data);
		});

        // Search day end
        $scope.server.getConf("searchDayEnd", {})
		.done(function (data, statusCode) {
		    $scope.searchDayEnd = parseInt(data);
		});
    };

    // Push the configuration state to the server
    $scope.pushState = function () {
        $scope.searchDayStartState = $scope.updatingMessage;
        $scope.searchDayEndState = $scope.updatingMessage;
        $scope.enableCacheState = $scope.updatingMessage;
        $scope.pushCacheEnabled();
        $scope.pushDayEnd();
        $scope.pushDayStart();

    };

    $scope.pushCacheEnabled = function () {

        // Cache enabled
        $scope.server.setConf("cacheEnabled", { value: $scope.enableCache })
        .done(function(data, statusCode)
        {
            $scope.enableCacheState = $scope.successMessage;
            $scope.$apply();
        })
        .fail(function (xhr, statusCode, error) {
            $scope.showServerInputError(xhr.responseText);
            $scope.enableCacheState = $scope.errorMessage;
            $scope.$apply();
        });
    };

    $scope.pushDayStart = function () {
        // Search day start
        $scope.server.setConf("searchDayStart", { dayStart: $scope.searchDayStart })
        .done(function(data, statusCode)
        {
            $scope.searchDayStartState = $scope.successMessage;
            $scope.$apply();
        })
        .fail(function (xhr, statusCode, error) {
            $scope.showServerInputError(xhr.responseText);
            $scope.searchDayStartState = $scope.errorMessage;
            $scope.$apply();
        });
    };


    $scope.pushDayEnd = function () {
        // Search day end
        $scope.server.setConf("searchDayEnd", { dayEnd: $scope.searchDayEnd })
        .done(function (data, statusCode)
        {
            $scope.searchDayEndState = $scope.successMessage;
            $scope.$apply();
        })
	    .fail(function (xhr, statusCode, error) {
	        $scope.showServerInputError(xhr.responseText);
	        $scope.searchDayEndState = $scope.errorMessage;
	        $scope.$apply();
	    });
    };

    $scope.invalidateCache = function () {
        $scope.invalidateCacheState = $scope.updatingMessage;
        $scope.server.getConf("InvalidateCache", { })
        .done(function (data, statusCode) {
            $scope.invalidateCacheState = $scope.successMessage;
            $scope.$apply();
        })
	    .fail(function (xhr, statusCode, error) {
	        $scope.invalidateCacheState = $scope.errorMessage;
	        $scope.$apply();
	    });
    };

    // Displays a server input error on the web page.
    $scope.inputError = false;
    $scope.inputErrorText = "";
    $scope.showServerInputError = function (error) {
        var xml = $($.parseXML(error));
        var inputError = xml.find("InputError");
        var parameter = inputError.attr("name");
        var message = inputError.attr("message");

        $scope.$apply(function () {
            $scope.inputErrorText += "The server returned with an error : parameter '" + parameter + "' is invalid. Reason : " + message + "<br />";
            $scope.inputError = true;
        });
    };

    $scope.loadState();
}]);