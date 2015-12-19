/// <reference path="../typings/angularjs/angular.d.ts"/>
var app = angular.module('app', ['ui.bootstrap']);

// Injects the filter provider into the app.
app.config(['$filterProvider', function($filterProvider)
{
	app.filter = $filterProvider.register;
}]);