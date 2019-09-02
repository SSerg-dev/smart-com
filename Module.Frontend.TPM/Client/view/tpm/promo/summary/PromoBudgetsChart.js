Ext.define('App.view.tpm.promo.PromoBudgetsChart', {
    extend: 'Ext.chart.Chart',
    alias: 'widget.promobudgetschart',
    animate: true,
    shadow: false,
    insetPadding: 5,
    spriteCost: undefined,
    spritePlanOrActual: undefined,
    cls: 'chart',
    legend: {
        position: 'bottom',
        boxStroke: 'none',
        customLegend: true,
        itemHSpacing: 5,
        itemWSpacing: 10,
        labelColor: '#0000A0',
    },
    cls: 'legend-style',
    series: [{
        type: 'pie',
        showInLegend: true,
        donut: 60,
        field: 'value',
        colorSet: ['#0000A0', '#00D7B8', '#5B5BE6', '#FFE000', '#8888CF'],
        tips: {
            trackMouse: true,
            cls: 'summary-budget-tip',

            renderer: function (storeItem, item) {
                this.update(storeItem.get('name') + ': ' + (storeItem.get('value') / 1000000.0));
            }
        },
    }],
    listeners: {
        resize: function (me) {
            var surface = me.surface,
                totalcost = me.cost / 1000000.0,
                totalcost = Ext.util.Format.round(totalcost, 2);

            if (me.spriteCost) {
                me.spriteCost.remove();
            };
            me.spriteCost = surface.add({
                type: 'text',
                text: totalcost,
                y: 0,
                x: 0,
            });
            me.spriteCost.show(true);
            me.spriteCost.addCls('bigSummaryText spriteText');
            var bbox = me.spriteCost.getBBox();
            me.spriteCost.setAttributes({
                x: me.series.items[0].centerX - bbox.width / 2,
                y: me.series.items[0].centerY + 3,
            }, true);

            if (me.spritePlanOrActual) {
                me.spritePlanOrActual.remove();
            };
            me.spritePlanOrActual = surface.add({
                type: 'text',
                text: me.planOrActual,
                x: 5,
                y: 0,
            });
            me.spritePlanOrActual.show(true);
            me.spritePlanOrActual.addCls('mediumSummaryText spriteText actualPlanBudgets');
            bbox = me.spritePlanOrActual.getBBox();
            me.spritePlanOrActual.setAttributes({
                y: bbox.height / 2,
            }, true);
        }
    },
});
