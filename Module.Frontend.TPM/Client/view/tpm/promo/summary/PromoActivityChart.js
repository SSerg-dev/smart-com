Ext.define('Ext.chart.theme.activityTheme', { // TODO: вынести тему
    extend: 'Ext.chart.theme.Base',
    constructor: function (config) {
        this.callParent([Ext.apply({
            colors: ['#0000A0', '#FFE000'],
            axis: {
                fill: '#5B5BE6',
                stroke: '#5B5BE6'
            },
            axisLabelBottom: {
                fill: '#0000A0',
            },
            axisTitleBottom: {
                fill: '#0000A0'
            },
        }, config)]);
    }
});

Ext.define('App.view.tpm.promo.PromoActivityChart', {
    extend: 'Ext.chart.Chart',
    alias: 'widget.promoactivitychart',
    animate: false,
    insetPadding: 5,
    theme: 'activityTheme',
    shadow: false,
    resizable: false,
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
        fields: 'name',
        minimum: 0
    }],
    series: [{
        type: 'column',
        gutter: 100,
        showInLegend: true,
        column: true,
        stacked: true,
        title: [l10n.ns('tpm', 'PromoSummary').value('Baseline'), l10n.ns('tpm', 'PromoSummary').value('IncrementalLSV')],
        yField: ['BL', 'Inc'],
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
                    case 'BL':
                        text = l10n.ns('tpm', 'PromoSummary').value('Baseline');
                    break;
                };
                this.update(text + ': ' + Ext.util.Format.round(item.value[1],2) + '%');
            },
        },
    }],
});
