define(['jquery', 'knockout', 'taskHandler', 'signalrHubs'], function ($, ko, taskHandler) {

    var init = function () {

        var isLocalHost = function()
        {
            var _location = document.location.toString();
            var applicationNameIndex = _location.indexOf('/', _location.indexOf('://') + 3);
            var applicationName = _location.substring(0, applicationNameIndex) + '/';
            return applicationName.indexOf('localhost') > -1;
        }

        var visualCronHub = $.connection.visualCronHub;
        var paused = false;

        var getTasks = function () {

            var apiPath = isLocalHost() ? 'api/Task' : 'visualcronmonitor/api/Task';

            $.get(apiPath, function (tasks) {
                taskHandler.updateTasks(tasks);
                $('body').trigger("reorder", ["CurrentRunTime"]);
            })
                .fail(function() {
                    console.log("Error Getting All Tasks on Hub Start.");
                });
        };

        var connect = function () {

            if (paused) {
                console.log('Paused Click Play Button to Reconnect');
                return;
            }

            $.connection.hub.start().done(function () {
                console.log('Hub Connected');
                paused = false;
                getTasks();
            });
        }

        visualCronHub.client.broadcastMessage = function (tasks) {

            taskHandler.updateTasks(tasks);
            $('body').trigger("reorder", ["CurrentRunTime"]);
        };

        $.connection.hub.disconnected(function () {
            console.log('Hub Disconnected');

            if (paused) {
                console.log('Paused Click Play Button to Reconnect');
                return;
            }

            if ($.connection.hub.lastError) {
                console.log("Hub Disconnected. Reason: " + $.connection.hub.lastError.message);
            }

            setTimeout(connect(), 5000);
        });

        $('body').trigger("reorder", ["CurrentRunTime"]);
        
        $('#pause').click(function () {

            if (paused)
                paused = false;
            else
                paused = true;


            if (paused) {
                console.log('Stopping Hub Connection');
                $.connection.hub.stop();
            } else {
                connect();
            }

            $('#pause').toggleClass('glyphicon-pause glyphicon-play');
        });

        ko.applyBindings(taskHandler.viewModel);

        connect();

    };

    return {
        init: init
    }

});