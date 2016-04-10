define(function (require, exports, module) {
    require("pages/base.js");

    angular.module("app")
    .controller("articleController", ['$scope', '$http', '$location', 'alertbox', '$modal', "hotkeys", function ($scope, $http, $location, alertbox, $modal, hotkeys) {
        //内容开始
        $scope.items = [];
        $scope.pageSizes = pageSizes;
        $scope.selArticleIds = [];

        //  分页参数
        $scope.searchModel = {
            totalCount: 0,
            pageIndex: 0,
            pageSize: 10,
            totalPageCount: 0
        };

        //  设置每次记录数
        $scope.find = function () {
            $scope.searchModel.pageIndex = 1;
            $scope.load();
        };

        //  数据加载
        $scope.load = function () {
            var d = createDialog();
            var data = $scope.searchModel;
            $http.post("/api/articleAPI/getPagedList", data)
            .success(function (data, status, headers, config) {
                if (data.success) {
                    $scope.items = data.content.items;
                    $scope.searchModel.totalCount = data.content.totalCount;
                    $scope.searchModel.pageIndex = data.content.pageIndex;
                    $scope.searchModel.pageSize = data.content.pageSize;
                    $scope.searchModel.totalPageCount = data.content.totalPageCount;
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


        $scope.selArticle = function (id) {
            if ($scope.selArticleIds.indexOf(id) == -1) {
                $scope.selArticleIds.push(id);
            }
            else {
                $scope.selArticleIds.remove(id);
            }
            $('button[name="delArticleBtn"]').attr('disabled', true);
            if ($scope.selArticleIds.length > 0) {
                $('button[name="delArticleBtn"]').removeAttr('disabled');
            }
        }

        //  批量删除
        $scope.batchRemove = function () {
            $http.post("/api/articleAPI/batchRemove", { Ids: $scope.selArticleIds })
            .success(function (data, status, headers, config) {
                if (data.success) {
                    var delList = [];
                    for (var i = 0; i < $scope.items.length; i++) {
                        if ($scope.selArticleIds.indexOf($scope.items[i].id) != -1) {
                            delList.push($scope.items[i]);
                        }
                    }
                    for (var i = 0; i < delList.length; i++) {
                        $scope.items.remove(delList[i]);
                    }
                    createDialog().showTip("删除成功", 1500);
                    $scope.selArticleIds = [];
                    $('button[name="delArticleBtn"]').attr('disabled', true);
                }
                else {
                    createDialog().showTip("删除失败," + data.message, 2500);
                }
            })
            .error(function (data, status, headers, config) {
                alert(status);
            });
        };

        $scope.init = function () {
            $scope.load();
        };

        $scope.init();
        //内容结束
    }]);
    
});