define(['jquery', 'sparkline', 'color', 'date', 'bootstrap', 'jqueryUi'], function ($) {

    var formatRows = function () {

        var getMax = function (items) {

            var maxConcernLevel = 0;

            items.each(function () {

                var t0 = $(this).text();
                if (t0) {
                    var t1 = parseInt(t0);

                    if (t1 > maxConcernLevel)
                        maxConcernLevel = t1;
                }
            });

            return maxConcernLevel + 1;
        }

        var items = $("td.concern");
        var maxConcernLevel = getMax(items);

        var colors = new Hex(0x00FF00).range(new Hex(0xFF0000), maxConcernLevel, true);

        items.each(function () {
            var t0 = $(this).text();
            if (t0) {
                    var t1 = parseInt(t0);
                    var t2 = colors[t1].toString();
                    $(this).parent().css('cssText', 'background-color: ' + '#' + t2 + ' !important');
            }
        });

        $('.bar').sparkline('html', { type: 'bar', barColor: '#818183', barWidth: 11, barSpacing: 1, chartRangeMin: 0, colorMap: { 0: 'black', 1: '#818183' } });
        $('.line').sparkline('html', { type: 'line', lineWidth: 1, lineColor: 'black', fillColor: '#818183', width: 100, chartRangeMin: 0 });
    };


    $("th").click(function () {
        var val = $(this).text();
        var find = ' ';
        var re = new RegExp(find, 'g');
        var selectedColumn = val.replace(re, '');
        $('body').trigger("reorder", [selectedColumn, true]);
    });

    return {

        formatRows: formatRows
    }

});