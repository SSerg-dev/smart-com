Ext.define('Ext.chart.theme.activityInOutTheme', { // TODO: вынести тему
    extend: 'Ext.chart.theme.Base',
    constructor: function (config) {
        this.callParent([Ext.apply({
            colors: ['#FFE000'],
            axis: {
                fill: '#5B5BE6',
                stroke: '#5B5BE6'
            },
            axisLabelBottom: {
                fill: '#0000A0'
            },
            axisTitleBottom: {
                fill: '#0000A0'
            },
        }, config)]);
    }
});

Ext.define('App.view.tpm.promo.InOutPromoActivityChart', {
    extend: 'Ext.chart.Chart',
    alias: 'widget.inoutpromoactivitychart',
    animate: false,
    insetPadding: 5,
    theme: 'activityInOutTheme',
    shadow: false,
    legend: {
        position: 'bottom',
        boxStroke: 'none',
        customLegend: true,
        itemHSpacing: 5,
        itemWSpacing: 10,
        labelColor: '#0000A0'
    },
    axes: [{
        type: 'category',
        position: 'bottom',
        fields: 'name'
    }],
    series: [{
        type: 'column',
        gutter: 100,
        showInLegend: true,
        column: true,
        stacked: true,
        title: [l10n.ns('tpm', 'PromoSummary').value('IncrementalLSV')],
        yField: ['Inc'],
        xField: 'name',
        tips: {
            trackMouse: true,
            cls: 'summary-budget-tip',

            renderer: function (storeItem, item) {
                var fieldName = item.yField;
                var text;
                switch (fieldName) {
                    case 'Inc':
                        text = l10n.ns('tpm', 'PromoSummary').value('IncrementalLSV');
                    break;
                };
                this.update(text + ': ' + item.value[1] + '%');
            },

        },
    }],
});
