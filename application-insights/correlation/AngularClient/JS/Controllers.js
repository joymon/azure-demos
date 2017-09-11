/// <reference path="../angular.min.js" />
/// <reference path="Modules.js" />
/// <reference path="Services.js" />

app.controller("RESTClientController", ['$scope', '$log', 'RESTClientService', function ($scope, $log, RESTClientService) {
    function createGuid() {
        return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
            var r = Math.random() * 16 | 0, v = c === 'x' ? r : (r & 0x3 | 0x8);
            return v.toString(16);
        });
    }
    $scope.testHeaders = function () {
        //alert("test headers");
        RESTClientService.EchoHeaders()
        .then(function (data) {
            alert(JSON.stringify( data, null, ' '));
        });
    }
    $scope.findArea = function () {
        $scope.operation_id = createGuid();
        window.appInsights.context.operation.id = $scope.operation_id;
        window.appInsights.trackEvent("Custom event from JS before service call", { operation_Id: $scope.operation_id });

        var promiseGet = RESTClientService.get($scope.radius, $scope.operation_id);
        promiseGet.then(function (pl) {
            $scope.message = pl.data;
        },
                  function (errorPl) {
                      $log.error('failure loading Company', errorPl);
                  });
    }

    $scope.firstAI = function () {
        $scope.operation_id = createGuid();
        window.appInsights.queue.push(function () {
            window.appInsights.context.addTelemetryInitializer(function (envelope) {
                envelope.tags["ai.operation.id"] = $scope.operation_id;
                envelope.tags["ai.operation.name"] = "my-client-custom-init-op";
                console.log("'ai.operation.id' init " + $scope.operation_id);
            });
        });
        window.appInsights.trackEvent("Custom init event from JS", { operation_Id: $scope.operation_id });

    }
    $scope.firstAI();
}]);

