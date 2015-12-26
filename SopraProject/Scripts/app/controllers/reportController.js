app.controller('reportController',
['serverService', '$scope', '$timeout',
function (serverService, $scope, $timeout) {
    $scope.server = serverService;

    $scope.chartNames = { occupationRate: "Occupation Rate (%)", fillRate: "Fill Rate (%)", meetingCount: "Meeting Count" };
    $scope.days = [];
    $scope.metrics = {};// keys: occupationRate, fillRate, meetingCount
    $scope.stats = {}; // keys: occupationRate, fillRate, meetingCount

    $scope.loadCharts = function () {
        $scope.server.getRessource("report", {startDate : "11/01/2015", endDate : "11/30/2015", roomId : 5})
		.done(function (data, statusCode) {
		    // Reset variables
		    $scope.metrics = { occupationRate: [], fillRate: [], meetingCount: [] };
		    $scope.days = [];
		    $scope.stats = {}; // keys: occupationRate, fillRate, meetingCount

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

		        var ctx = $("#" + key + "Chart").get(0).getContext("2d");
		        var chart = new Chart(ctx);
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
		        chart.Bar(chartData, { pointHitDetectionRadius: 1, responsive: true, animation: true });
		    }



		});
    };



    $timeout(function () { $scope.loadCharts(); });
}]);