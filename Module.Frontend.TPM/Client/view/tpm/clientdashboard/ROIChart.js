Ext.define('App.view.tpm.clientdashboard.ROIChart', {
    extend: 'Ext.chart.Chart',
    alias: 'widget.roichart',
    animate: false,
    insetPadding: 5,
    shadow: false,
    sprites: [],

    legend: {
        position: 'bottom',
        boxStroke: 'none',
        customLegend: true,
        itemHSpacing: 0,
        itemWSpacing: 10,
        labelColor: '#0000A0',
        gaugeMouse: true,
        displayValue: true,
        valueHSpacing: 6,
        valueFont: 'Bold 15px Arial',
    },
    series: [{
        type: 'gauge',
        showInLegend: true,
        field: 'value',
        custom: true,
        strokeColor: '#FFE000',
        minimum: 0,
        maximum: 100,
        donut: 40,
        colorSet: ['#0000A0', '#00D7B8', '#FFE000', '#DADDDC']
    }],
    listeners: {
        resize: function () {
        },
        beforerender: function (me) {
            me.series.items[0].maximum = me.maximum;
        }
    },
});
