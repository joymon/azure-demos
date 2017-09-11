app.service("RESTClientService", function ($http) {

    this.get = function (radius, operation_id) {
        var url = "http://localhost/FrontEndWCFService/FrontEndService.svc/areaOf/" + radius;
        var guid = operation_id;
        //The RequestId header passed from here is taken as operation id at webhttp WCF side to correlate.
        return $http.get(url, {
            headers: {
                'Request-Id': guid,
                //'x-ms-request-id':guid,
            }
        }
        );
    };
    this.EchoHeaders = function () {
        var url = "AngularClient/MVCWCFClient/EchoHeaders";
        var guid = "operation_id";
        //The RequestId header passed from here is taken as operation id at webhttp WCF side to correlate.
        return $http.get(url, {
            headers: {
                'Request-Id': guid,
                'joys-header':'joys header value',
            }
        }
        );
    }
});
