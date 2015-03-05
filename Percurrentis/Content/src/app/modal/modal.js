(function () {
    'use strict';

    var serviceId = 'modalService';

    // Define the factory on the module.
    // Inject the dependencies. 
    // Point to the factory definition function.
    angular.module('app').factory(serviceId, ['$modal', '$sanitize', modal]);

    function modal($modal, $sanitize) {
        // Define the functions and properties to reveal.
        var service = {
            openDelete: openDelete,
            open: open
        };

        return service;

        function open(title, body, fnPrimary, fnCancel, btnSetUrl) {
            console.log(btnSetUrl);
            if (_.isUndefined(btnSetUrl)) {
                btnSetUrl = '/TravelAgency/Content/src/app/modal/primaryBtnSet.tpl.html';
            }

            var modalInstance = $modal.open({
                templateUrl: '/TravelAgency/Content/src/app/modal/modal.tpl.html',
                controller: modalCtrl,
                contentClass: 'col-md-8 col-md-offset-2',
                resolve: {
                    title: function() {
                        return title;
                    },
                    body: function () {
                        return body;
                    },
                    btnSetUrl: function () {
                        return btnSetUrl;
                    }
                }
            });

            function modalCtrl($scope, $modalInstance, title, body) {
                $scope.title     = $sanitize(title);
                $scope.body      = $sanitize(body);
                $scope.btnSetUrl = btnSetUrl;
                $scope.primary = {
                    disabled: false,
                    action: primaryAction
                };

                function primaryAction() {
                    $scope.primary.disabled = true;
                    $modalInstance.close('delete');
                };

                $scope.cancel = function () {
                    $modalInstance.dismiss('cancel');
                };
            }

            modalInstance.result.then(function (selected) {
                fnPrimary();
            }, function () {
                if (_.isFunction(fnCancel)) {
                    fnCancel();
                }
            });
        }

        function openDelete(entityName, fnPrimary, fnCancel) {
            open('<i class="fa fa-times"></i> Delete ' + entityName,
                 'Are you sure you want to delete this ' + entityName + '? This action can not be undone.',
                 fnPrimary,
                 fnCancel,
                 '/TravelAgency/Content/src/app/modal/deleteBtnSet.tpl.html');
        }
    }
})();