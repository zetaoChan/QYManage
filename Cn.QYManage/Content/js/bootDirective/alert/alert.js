define(function (require, exports, module) {
    angular.module('ui.bootstrap', [])

    .controller('AlertController', ['$scope', '$attrs', function ($scope, $attrs) {
        $scope.closeable = 'close' in $attrs;
        this.close = $scope.close;
    } ])

    .directive('alert', function () {
        return {
            restrict: 'EA',
            controller: 'AlertController',
            templateUrl: '/content/js/bootDirective/alert/alert.html',
            transclude: true,
            replace: true,
            scope: {
                type: '@',
                close: '&',
                closeable:'@'
            }
        };
    })

    .directive('dismissOnTimeout', ['$timeout', function ($timeout) {
        return {
            require: 'alert',
            link: function (scope, element, attrs, alertCtrl) {
                $timeout(function () {
                    alertCtrl.close();
                }, parseInt(attrs.dismissOnTimeout, 10));
            }
        };
    } ]);
});
