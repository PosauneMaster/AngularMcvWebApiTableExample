angular.module('WelcomeLetter')
    .controller('WelcomeLetterController', ['$scope', '$http', function ($scope, $http) {

        $scope.gridOptions = {};


        $http.get('/Home/WelcomeLetters')
            .success(function (response) {

                $scope.welcomeKitsData = response;

                $scope.gridOptions = {
                    data: $scope.welcomeKitsData,

                    columnDefs: [
                        { name: 'FileId', field: 'ValidationFileId', resizable: 'true' },
                        { name: 'FileName', field: 'FileName', resizable: 'true' },
                        { name: 'Provider', field: 'Provider', resizable: 'true' },
                        { name: 'TPA', field: 'TPA', resizable: 'true' },
                        { name: 'Sponsor', field: 'Sponsor', resizable: 'true' },
                        { name: 'SponsorList', field: 'SponsorList', resizable: 'true' },
                        { name: 'FundedDate', field: 'FundedDate', resizable: 'true' },
                        { name: 'FundedDateYMD', field: 'FundedDateYMD', resizable: 'true' },
                        { name: 'Accounts', field: 'Accounts', resizable: 'true' },
                        { name: 'FundedAmount', field: 'FundedAmount', resizable: 'true' }
                    ],
                    showGroupPanel: true
                };

                console.log($scope.welcomeKitsData);

            })
            .error(function (data, status, header, config) {
                alert("Error");
            });

    }]);

