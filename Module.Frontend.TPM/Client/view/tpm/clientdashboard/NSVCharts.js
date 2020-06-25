Ext.define('Ext.chart.theme.NSVTheme', { // TODO: вынести тему
    extend: 'Ext.chart.theme.Base',
    constructor: function (config) {
        this.callParent([Ext.apply({
            colors: ['#0000A0', '#FFE000'],
            axis: {
                fill: '#000',
                stroke: '#000'
            },
            axisLabelLeft: {
                fill: '#000',
            },
            axisTitleLeft: {
                fill: '#000'
            },
        }, config)]);
    }
});

Ext.define('App.view.tpm.clientdashboard.NSVChart', {
    extend: 'Ext.chart.Chart',
    alias: 'widget.nsvchart',
    animate: false,
    insetPadding: 0,
    theme: 'NSVTheme',
    shadow: false,
    created: false,
    minColumnHeight: 3,
    axes: [{
        type: 'Numeric',
        position: 'left',
        fields: 'value',
        stroke: '#fff',
        blockRender: true,
        grid: {
            odd: {
                stroke: '#fff'
            },
            even: {
                stroke: '#fff'
            }
        },
    }, {
        type: 'Category',
        position: 'bottom',
        fields: 'name',
        hidden: true,
    }],
    series: [{
        type: 'minheightcolumn',
        gutter: 32.5,
        yField: 'value',
        xField: 'name',
        tips: {
            trackMouse: true,
            cls: 'summary-budget-tip',

            renderer: function (storeItem, item) {
                var originalData = item.series.chart.originalData
                var fieldName = item.value[0];
                if (originalData) {
                    switch (fieldName) {
                        case 'Plan':
                            this.update(fieldName + ': ' + Ext.util.Format.round(originalData.Plan, 2));
                            break;
                        case 'YTD':
                            this.update(fieldName + ': ' + Ext.util.Format.round(originalData.YTD, 2));
                            break;
                        case 'YEE':
                            this.update(fieldName + ': ' + Ext.util.Format.round(originalData.YEE, 2));
                            break;
                    }
                } else {
                    this.update(fieldName + ': ' + Ext.util.Format.round(item.value[1],2));
                }
            },
        },
        renderer: function (sprite, storeItem, barAttr, i, store) {
            switch (i) {
                case 0:
                    barAttr.fill = '#0000A0';
                    break;
                case 1:
                    barAttr.fill = '#00D7B8';
                    break;
                case 2:
                    barAttr.fill = '#FFE000';
                    break;
            }
            return barAttr;
        },
    }],
    listeners: {
        resize: function (me) {
            if (me.created) {
            var controller = App.app.getController('tpm.clientdashboard.ClientDashboard');
            //controller.deleteNSVChartLabelsAndTitles(me);
            controller.setNSVChartZeroLine(me.axes.items[0]);
            controller.setNSVChartSeriesLabels(me);
            }
        }
    },
    
});
