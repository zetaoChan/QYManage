define(function (require, exports, module) {
    require("pages/base.js");

    angular.module("app")
    .controller("sysConfigController", ['$scope', '$http', '$location', 'alertbox', '$modal', "hotkeys", function ($scope, $http, $location, alertbox, $modal, hotkeys) {
        //内容开始
        $scope.items = [];
        $scope.configModel = {};
        $scope.editModel = {};

        //  数据加载
        $scope.load = function () {
            var d = createDialog();
            $http.post("/api/sysConfigAPI/GetList")
            .success(function (data, status, headers, config) {
                if (data.success) {
                    $scope.items = data.content;
                    d.close();
                }
                else {
                    d.showError(data.message);
                }
            })
            .error(function (data, status, headers, config) {
                alert(status);
            });
        };

        $scope.edit = function (sysName) {
            for (var i = 0; i < $scope.items.length; i++){
                if ($scope.items[i].sysName == sysName) {
                    var str = JSON.stringify($scope.items[i]);
                    $scope.configModel = JSON.parse(str);
                    break;
                }
            }
            $('#configEditModal').modal('show');
        }

        $scope.save = function () {
            var data = $scope.configModel;
            $http.post("/api/sysConfigAPI/saveConfig", data)
            .success(function (data, status, headers, config) {
                if (data.success) {
                    for (var i = 0; i < $scope.items.length; i++) {
                        if ($scope.items[i].sysName == $scope.configModel.sysName) {
                            $scope.items[i] = $scope.configModel;
                            break;
                        }
                    }
                    $('#configEditModal').modal('hide');
                    createDialog().showTip('更改成功', 1500);
                }
                else {
                    alert(data.message);
                }
            })
            .error(function (data, status, headers, config) {
                alert(status);
            });
            
        }

        $scope.init = function () {
            $scope.load();
        };

        $scope.init();
        //内容结束
    }]);
});