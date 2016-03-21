define(function (require, exports, module) {
    require("pages/base.js");
    require("/Scripts/json2.js");

    angular.module("app")
    .controller('changePasswordCtrl', function ($scope, $http, $modalInstance, item) {
        $scope.member = item;
        $scope.editModel = {
            memberId: item.id,
            newPassword: "",
            confirmPassword: ""
        };
        $scope.message = "";
        $scope.ok = function () {
            if ($scope.editModel.newPassword.length <= 0) {
                $scope.message = "新密码不能为空"
            }
            else if ($scope.editModel.newPassword != $scope.editModel.confirmPassword) {
                $scope.message = "两次输入的密码不一致"
            }
            else {
                var editModel = $scope.editModel;
                $scope.message = "提交中...";
                $http.post("/api/MemberAPI/changePassword/", editModel)
                .success(function (data, status, headers, config) {
                    //var json_data = JSON.stringify(data);
                    //document.write(json_data);
                    if (data.success) {
                        $modalInstance.close(data.id);
                    }
                    else {
                        $scope.message = data.message;
                    }
                })
                .error(function (data, status, headers, config) {
                    alert(status);
                });
            }

        };
        $scope.cancel = function () {
            $modalInstance.dismiss('cancel');
        };
    });

    angular.module("app")
    .controller('editCtrl', function ($scope, $http, $modalInstance, item) {
        $scope.editModel = {
            username: item.username,
            memberName: item.memberName,
            id: item.id,
            actor: item.actor
        };
        $scope.actors = [];
        $http.post("/api/MemberAPI/GetValidActors/", {})
        .success(function (data, status, headers, config) {
            if (data.success) {
                $scope.actors = data.content;
            }
            else {
                $scope.message = data.message;
            }
        })
        .error(function (data, status, headers, config) {
            alert(status);
        });
        
        $scope.message = "";
        $scope.current = null;
        $scope.ok = function () {
            if ($scope.editModel.memberName.length <= 0) {
                $scope.message = "姓名不能为空"
            }
            else {
                var editModel = $scope.editModel;
                $scope.message = "提交中...";
                $http.post("/api/MemberAPI/update/", editModel)
                .success(function (data, status, headers, config) {
                    //var json_data = JSON.stringify(data);
                    //document.write(json_data);
                    if (data.success) {
                        item.memberName = editModel.memberName;

                        $modalInstance.close(data.id);
                    }
                    else {
                        $scope.message = data.message;
                    }
                })
                .error(function (data, status, headers, config) {
                    alert(status);
                });
            }
        };
        $scope.cancel = function () {
            $modalInstance.dismiss('cancel');
        };
    });

    angular.module("app")
    .controller('addCtrl', function ($scope, $http, $modalInstance, items) {
        $scope.message = "";
        $scope.actors = [];
        $http.post("/api/MemberAPI/GetValidActors/", {})
        .success(function (data, status, headers, config) {
            if (data.success) {
                $scope.actors = data.content;
            }
            else {
                $scope.message = data.message;
            }
        })
        .error(function (data, status, headers, config) {
            alert(status);
        });
        $scope.addModel = {
            username: "",
            memberName: "",
            password: "",
            confirmPassword: "",
            actor: 0
        };

        $scope.ok = function () {
            if ($scope.addModel.username.length <= 0) {
                $scope.message = "账号不能为空"
            }
            else if ($scope.addModel.memberName.length <= 0) {
                $scope.message = "姓名不能为空"
            }
            else if ($scope.addModel.password.length <= 0) {
                $scope.message = "密码不能为空"
            }
            else if ($scope.addModel.password != $scope.addModel.confirmPassword) {
                $scope.message = "两次输入的密码不一致"
            }
            else {
                $scope.message = "提交中...";
                $http.post("/api/MemberAPI/add/", $scope.addModel)
                .success(function (data, status, headers, config) {
                    //var json_data = JSON.stringify(data);
                    //document.write(json_data);
                    if (data.success) {
                        $modalInstance.close(data.id);
                    }
                    else {
                        $scope.message = data.message;
                    }
                })
                .error(function (data, status, headers, config) {
                    alert(status);
                });
            }

        };
        $scope.cancel = function () {
            $modalInstance.dismiss('cancel');
        };
    });




    angular.module("app")
    .controller("memberController", ['$scope', '$http', '$location', 'alertbox', '$modal', function ($scope, $http, $location, alertbox, $modal) {
        //内容开始
        $scope.items = [];
        $scope.searchSource = {};
        $scope.searchModel = {
            totalCount: 0,
            pageIndex: 0,
            pageSize: 15,
            username: "",
            truename: "",
            mobile: "",
            birthday1: "",
            birthday2: "",
            regTime1: "",
            regTime2: ""
        };

        $scope.sortModel = {
            cols: [],
            orderName: "",
            orderType: "",
            order: function () {
                $scope.load();
            }
        };


        $scope.remove = function (id) {
            var d = createDialog();
            $http.post("/api/MemberAPI/delete", {
                id: id
            })
            .success(function (data, status, headers, config) {
                //var json_data = JSON.stringify(data);
                //document.write(json_data);
                if (data.success) {
                    d.close();
                    $scope.load();
                }
                else {
                    d.showError(data.message);
                }
            })
            .error(function (data, status, headers, config) {
                alert(status);
            });
        }

        $scope.load = function () {
            var d = createDialog();
            var data = $scope.searchModel;
            data.orderName = $scope.sortModel.orderName;
            data.orderType = $scope.sortModel.orderType;

            $http.post("/api/MemberAPI/GetPagedList", data)
            .success(function (data, status, headers, config) {
                //var json_data = JSON.stringify(data);
                //document.write(json_data);
                if (data.success) {
                    $scope.items = data.content.items;
                    $scope.searchModel.totalCount = data.content.totalCount;
                    $scope.searchModel.pageIndex = data.content.pageIndex;
                    $scope.searchModel.pageSize = data.content.pageSize;
                    d.close();
                }
                else {
                    d.showError(data.message);
                }
            })
            .error(function (data, status, headers, config) {
                alert(status);
            });
        }

        $scope.add = function () {
            var modalInstance = $modal.open({
                templateUrl: 'addBox.html',
                controller: 'addCtrl',
                size: 'sm',
                resolve: {
                    items: function () {
                        return $scope.items;
                    }

                }
            });

            modalInstance.result.then(function (id) {
                $scope.load();
            }, function () {
                //$log.info('Modal dismissed at: ' + new Date());
            });
        }

        $scope.edit = function (item) {
            var modalInstance = $modal.open({
                templateUrl: 'editBox.html',
                controller: 'editCtrl',
                size: 'sm',
                resolve: {
                    item: function () {
                        return item;
                    }
                }
            });

            modalInstance.result.then(function (id) {
                //$scope.load();
            }, function () {
                $log.info('Modal dismissed at: ' + new Date());
            });
        }


        $scope.changePassword = function (item) {
            var modalInstance = $modal.open({
                templateUrl: 'changePasswordBox.html',
                controller: 'changePasswordCtrl',
                size: 'sm',
                resolve: {
                    item: function () {
                        return item;
                    }
                }
            });

            modalInstance.result.then(function (id) {
                //$scope.load();
            }, function () {
                //$log.info('Modal dismissed at: ' + new Date());
            });
        }



        $scope.find = function (v) {
            $scope.pageIndex = 0;
            $scope.load();
        }

        $scope.load();
        //内容结束
    }]);
});