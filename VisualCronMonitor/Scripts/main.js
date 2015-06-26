require.config({
    paths: {
        jquery: 'lib/jquery-2.1.3',
        knockout: 'lib/knockout-3.3.0',
        sparkline: "lib/jquery.sparkline",
        color: "lib/jquery.color-helper",
        date: "lib/jquery.dateFormat",
        taskHandler: "monitor/taskHandler",
        tableHandler: "monitor/tableHandler",
        bootstrap: "lib/bootstrap",
        jqueryUi: "lib/jquery-ui",
        mustache: "lib/mustache",
        moment: "lib/moment",
        app: "monitor/app",
        signalrCore: "lib/jquery.signalR-2.2.0",
        signalrHubs: "/visualcronmonitor/signalr/hubs?"
    },

    shim: {
        jquery: { exports: "$"},
        sparkline: { deps: ["jquery"], exports: "$.sparkline"},
        color: { deps: ["jquery"], exports: "Hex" },
        date: { deps: ["jquery"], exports: "$.format" },
        signalrCore: { deps: ["jquery"], exports: "$.connection" },
        signalrHubs: { deps: ["signalrCore"] },
        bootstrap: { deps: ['jquery'] }
    },
    config: {
        moment: {
            noGlobal: true
        }
    }
});

require(['app', 'domready!'], function (app) {

    app.init();

});