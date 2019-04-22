Ext.define('Ext.chart.theme.activityTheme', { // TODO: вынести тему
    extend: 'Ext.chart.theme.Base',
    constructor: function (config) {
        this.callParent([Ext.apply({
            colors: ['#FF8A65', '#4FC3F7', '#EBEBEB']
        }, config)]);
    }
});

Ext.define('App.view.tpm.promo.PromoActivityChart', {
    extend: 'Ext.chart.Chart',
    alias: 'widget.promoactivitychart',
    animate: true,
    shadow: false,
    insetPadding: 1,
    theme: 'activityTheme',
    legend: {
        position: 'right',
        boxStroke: 'none'
    },

    series: [{
        type: 'pie',
        field: 'value',
        showInLegend: true,
        donut: false,
        tips: {
            trackMouse: true,
            cls: 'summary-budget-tip',
            tpl: '<div>{headerText}</div>',
            renderer: function (storeItem, item) {
                var name = storeItem.get('name'),
                    val = storeItem.get('value');
                if (name != "No Data") {
                    if (val == 0) {
                        this.update({ headerText: '' });
                    } else {
                        var total = 0;
                        storeItem.store.each(function (rec) {
                            total += rec.get('value');
                        });
                        this.update({ headerText: Ext.String.format('{0}: {1}%', name, Math.round(val / total * 100)) });
                    }
                } else {
                    this.update({ headerText: 'No Data' });
                }
            }
        },
        renderer: function (sprite, record, attr, index, store) {
            var name = record.get('name');
            if (name == 'Baseline') {
                color = '#FF8A65';
            } else if (name == "Incremental LSV") {
                color = '#4FC3F7';
            } else {
                color = '#EBEBEB';
            }

            return Ext.apply(attr, {
                fill: color
            });
        },
        highlight: {
            segment: {
                margin: 5
            }
        },
        label: {
            field: 'name',
            display: 'rotate',
            color: '#ffffff',
            renderer: function (value, label, storeItem, item, i, display, animate, index) {
                var name = storeItem.get('name'),
                    val = storeItem.get('value');
                if (name != 'No Data' && val != 0) {
                    var total = 0;
                    storeItem.store.each(function (rec) {
                        total += rec.get('value');
                    });
                    return Math.round(storeItem.get('value') / total * 100) + '%';
                } else {
                    return '';
                }
            }
        }
    }]
});
