app.controller('reportController',
['serverService', '$scope' , '$timeout',
function (serverService, $scope, $timeout) {
	$scope.roomFilter = function(input){
        return true;
	};

    $scope.server = serverService;

    $scope.selectedLocation = "-1";
    $scope.selectedRoom = "-1";
    var today = new Date();
    today = moment(today).format('DD/MM/YYYY');
    $scope.startDate = "01/11/2015";
    $scope.endDate = "30/11/2015";
    $scope.sites = {};
    $scope.rooms = {};

    $scope.chartNames = { occupationRate: "Occupation Rate (%)", fillRate: "Fill Rate (%)", meetingCount: "Meeting Count" };
    $scope.days = [];
    $scope.metrics = {};// keys: occupationRate, fillRate, meetingCount
    $scope.stats = {}; // keys: occupationRate, fillRate, meetingCount
    $scope.charts = {};

    $scope.load = function() 
	{
		$scope.$apply(function()
		{
			$scope.updateSites();
			$scope.updateRooms();
			$scope.loadCharts();
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

	// Loads the site list into the $scope.sites variable.
	$scope.updateRooms = function() 
	{
		$scope.rooms = {};
		if($scope.selectedLocation >= 0) {
			$scope.server.getRessource("roomsbysite", { siteId : $scope.selectedLocation })
			.done(function(data, statusCode)
			{
				$scope.rooms = {};
				var xml = $( $.parseXML( data ) );
				xml.find("Room").each(function()
				{
					var room = $(this);
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
		}
	};

    $scope.loadCharts = function () {
        $scope.server.getRessource("report", {startDate : $scope.startDate, endDate : $scope.endDate, roomId : $scope.selectedRoom})
		.done(function (data, statusCode) {
		    // Reset variables
		    $scope.metrics = { occupationRate: [], fillRate: [], meetingCount: [] };
		    $scope.days = [];
		    $scope.stats = {}; // keys: occupationRate, fillRate, meetingCount
		    $scope.descriptions = {occupationRate : "Gets the occupation rate of the room for the given day. " ,
		    					   fillRate :"Gets the fill rate per day of the room for the given day. The fill rate is the number of participant in relation of the capacity of the room.",
		    					   meetingCount :"Gets the number of meetings for the given day." };
            // Value processing functions.
		    var percent = function (value) { return (100 * value); };
		    var raw = function (value) { return value; };
		    var toint = function (value) { return value | 0; };
		    var preprocess = { 'occupationRate': percent, 'fillRate': percent, 'meetingCount': toint };

		    // ------------------
		    // Parses the XML
		    // ------------------
		    var xml = $($.parseXML(data))
		    xml.find("DailyMetricSet").each(function () {
		        var metricset = $(this);

		        $scope.$apply(function () {
		            var day = metricset.attr("day");
		            for (metric in $scope.metrics)
		            {
		                var value = preprocess[metric](parseFloat(metricset.children(metric).text()));
		                $scope.metrics[metric].push(value);
		            }
		            $scope.days.push(day);
		        });
		    });


		    // Gets the statistics
		    for(key in $scope.metrics)
		    {
		        xml.find(key + "Stats").each(function () {
		            var stats = $(this);
		            $scope.stats[key] = {};
		            $scope.$apply(function () {
		                for (stat in { "average": null, "stddev": null, "median": null, "min": null, "max": null })
		                {
		                    var value = parseFloat(stats.children(stat).text());
		                    $scope.stats[key][stat] = preprocess[key](value);
		                }
		            });
		        });
		    }

            // ------------------
		    // Creates the charts
		    // ------------------
		    Chart.defaults.global.responsive = false;

		    var colors = {
		        "occupationRate": "rgba(255, 0, 0, ",
		        "fillRate": "rgba(0, 255, 0, ",
		        "meetingCount": "rgba(0, 0, 255, "
		    };

		    for (key in $scope.metrics)
		    {
		        var elementName = "a[href=#" + key + "Tab]";

		        var container = $("#" + key + "ChartContainer");
		        container.empty();
		        container.append('<div class="container-fluid" style="margin-right: 1.5em;"><canvas width="600" height="120" id="' + key + 'Chart' + '"></canvas></div>'); 

		        var ctx = $("#" + key + "Chart").get(0).getContext("2d");
		        var chartData = {
		            labels: $scope.days,
		            datasets: [{
		                label: "label",
		                fillColor: colors[key] + "0.2)",
		                strokeColor: colors[key] + "1)",
		                pointColor: colors[key] + "1)",
		                pointStrokeColor: "#fff",
		                pointHighlightFill: "#fff",
		                pointHighlightStroke: colors[key] + "1)",
		                data: $scope.metrics[key],
		            }]
		        };


		        var chart = new Chart(ctx);
		        chart.Bar(chartData, { pointHitDetectionRadius: 1, responsive: true, animation: true });
		        $scope.charts[key] = chart;
			    

			  
		    }



		});
    };



    $timeout(function () { $scope.load(); });
}]);