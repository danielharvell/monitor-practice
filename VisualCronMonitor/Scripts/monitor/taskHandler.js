define(['knockout', 'tableHandler', 'mustache', 'moment'], function (ko, tableHandler, mustache, moment) {

    var Task = function (task) {
        this.Name = ko.observable(task.Name);
        this.TaskId = ko.observable(task.TaskId);
        this.Server = ko.observable(task.Server);
        this.Group = ko.observable(task.Group);
        this.JobName = ko.observable(task.JobName);
        var currentTime = $.format.date(task.CurrentRunTime, 'yyyy-MM-dd, HH:mm:ss');
        this.CurrentRunTime = ko.observable(currentTime);
        this.RunTimeHistory = ko.observableArray(JSON.parse(task.RunTimeHistory));
        var d = moment.duration(task.RunTimeFrequencyMinutes, 'minutes');
        var duration = d.get('days') + 'd ' + d.get('hours') + 'h ' + d.get('minutes') + 'm';
        this.RunTimeFrequencyMinutes = ko.observable(duration);
        this.SuccessHistory = ko.observableArray(JSON.parse(task.SuccessHistory));
        this.SuccessAverage = ko.observable(task.SuccessAverage.toFixed(2));
        this.ExecutionTimeHistory = ko.observableArray(JSON.parse(task.ExecutionTimeHistory));
        this.ExecutionTimeAverageSeconds = ko.observable(task.ExecutionTimeAverageSeconds.toFixed(2));
        this.StandardOutputHistory = ko.observableArray(JSON.parse(task.StandardOutputHistory));
        this.SubjectiveConcernHistory = ko.observableArray(JSON.parse(task.SubjectiveConcernHistory));
        this.CurrentSubjectiveConcernLevel = ko.observable(task.CurrentSubjectiveConcernLevel);
        this.CommandLine = ko.observable(task.CommandLine);
        this.Arguments = ko.observable(task.Arguments);
        this.WorkingDirectory = ko.observable(task.WorkingDirectory);
    }

    var viewModel = {

        tasks: ko.observableArray([])
    };

    var updateTasks = function (tasks) {

        tasks.forEach(function (servertask) {

            var match = ko.utils.arrayFirst(viewModel.tasks(), function (task) {
                var clientTaskId = task.TaskId();
                return servertask.TaskId === clientTaskId;
            });

            if (!match) {
                viewModel.tasks.push(new Task(servertask));
            } else {

                viewModel.tasks.remove(match);
                viewModel.tasks.push(new Task(servertask));
            }

        });
    };

    var selectedColumn = null;
    var decending = true;

    var sortArray = function () {

        var left;
        var right;

        if (decending) {
            left = 1;
            right = -1;
        } else {
            left = -1;
            right = 1;
        }

        return viewModel.tasks().sort(function (leftTask, rightTask) {
            return leftTask[selectedColumn]() === rightTask[selectedColumn]() ?
                0 :
                leftTask[selectedColumn]() < rightTask[selectedColumn]() ? left : right;
        });
    };

    ko.forcibleComputed = function (readFunc, context, options) {//this actually doesn't do what I hoped (instant update) it can be removed.
        var trigger = ko.observable(),
            target = ko.computed(function () {
                trigger();
                return readFunc.call(context);
            }, null, options);
        target.evaluateImmediate = function () {
            trigger.valueHasMutated();
        };
        return target;
    };

    var reorderTasks = function (column, selected) {

        if (selected && column === selectedColumn)
            decending = !decending;

        if (selected)
            selectedColumn = column;
        else if (!selectedColumn)
            selectedColumn = "CurrentRunTime";

        viewModel.sortedTasks = ko.forcibleComputed(sortArray);

        viewModel.concernLevelClicked = function (task) {
            $('#concern-modal').modal('toggle');
            $('#concern-tab').html("");
            $('#concern-content').html("");

            var count = 0;
            task.StandardOutputHistory().forEach(function (stdOutput) {
                count = count + 1;
                var active;
                if (count === 1)
                    active = true;
                else
                    active = null;
                var data = { output: stdOutput, count: count, active: active };

                $.get('../../MonitorTemplates/concernTab.html', function (template) {

                    var concernTabTpl = $(template).filter('#concern-tab-tpl').html();
                    var tabRender = mustache.to_html(concernTabTpl, data);
                    $('#concern-tab').append(tabRender);
                    var concernContentTpl = $(template).filter('#concern-content-tpl').html();
                    var contentRender = mustache.to_html(concernContentTpl, data);
                    $('#concern-content').append(contentRender);

                });
            });


            viewModel.taskClicked = function (item) {
                $('#task-modal').modal('toggle');
                $('#command-line').text('CommandLine: ' + item.CommandLine());
                $('#arguments').text('Arguments: ' + item.Arguments());
                $('#working-directory').text('WorkingDirectory: ' + item.WorkingDirectory());
            };
        };
    };

    $('body').on("reorder", function (event, selectedColumn, isSelected) {
        sortArray();
        reorderTasks(selectedColumn, isSelected);
        tableHandler.formatRows();
    });


    return {
        Task: Task,
        viewModel: viewModel,
        updateTasks: updateTasks
    }


});