/// <reference path="../angular.min.js" />
/// <reference path="../angular-resource.min.js" />
/// <reference path="Modules.js" />

app.service("RESTClientService", function ($http) {
    //http://localhost/FrontEndWCFService/FrontEndService.svc/GetAreaOfCircle?radius=3
    var url = "http://localhost/FrontEndWCFService/FrontEndService.svc/areaOf/3";
    this.get = function (operation_id) {
        var guid = operation_id;
        //How to pass this operation id from here to server so that server can use it as parent id????
        var cc = 'operationId=' + guid + ',operation_Id=' + guid + ',"x-ms-request-id"='+guid;
        return $http.get(url, {
            headers: {
                "operation_id": guid,
                "operationId": guid,
                operation_Id: guid,
                'Request-Id':guid,
                'x-ms-request-id':guid,
                "Correlation-Context" : cc,
                "operation_name": "Request from Angular"
            }
        }
        );
    };

    function createGuid() {
        function S4() {
            return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
        }
        return (S4() + S4() + "-" + S4() + "-4" + S4().substr(0, 3) + "-" + S4() + "-" + S4() + S4() + S4()).toLowerCase();
    }


});
