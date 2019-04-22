Ext.define('App.view.tpm.promo.PromoFinanceChart', {
    extend: 'Ext.chart.Chart',
    alias: 'widget.promofinancechart',
    animate: true,
    shadow: false,
    insetPadding: 3,
    axes: [{
        type: 'Numeric',
        position: 'left',
        fields: ['planValue', 'factValue'],
        label: {
            renderer: Ext.util.Format.numberRenderer('0,0')
        },
        grid: true,
        minimum: 0,
    }, {
        type: 'Category',
        position: 'bottom',
        fields: ['name']
    }],
    series: [{
        type: 'column',
        axis: 'left',
        highlight: true,
        xField: 'name',
        yField: ['planValue', 'factValue'],
        tips: {
            trackMouse: true,
            cls: 'summary-budget-tip',
            tpl: '<div>{headerText}</div>',
            renderer: function (storeItem, item, tst1, tst2) {
                var fieldName = item.yField,
                    type = fieldName == 'factValue' ? 'Fact' : 'Plan',
                    formatedValue = item.value[1].toString().replace(/\B(?=(\d{3})+(?!\d))/g, " "),
                    rec = item.storeItem,
                    text = rec.get('name');
                text = Ext.String.format('{0} ({1}): {2}', text, type, formatedValue);
                this.update({ headerText: text });
            }
        },
        label: {
            display: 'insideStart',
            color: '#ffffff',
            field: ['planValue', 'factValue'],
            renderer: function (value, label, storeItem, item, i, display, animate, index) {
                var val = item.value[1],
                    res = val == 0 ? '' : val.toString().replace(/\B(?=(\d{3})+(?!\d))/g, " ");
                return res
            }
        },
        renderer: function (sprite, record, attr, index, store) {
            var color = this.colorDictionary[index]

            var needMoveRight = index % 2 == 0;
            var newX = needMoveRight ? attr.x + 10 : attr.x - 10;
            return Ext.apply(attr, {
                fill: color,
                x: newX,
                height: attr.height != 0 ? attr.height : 2,
                y: attr.height != 0 ? attr.y : attr.y - 2
            });
        },
        gutter: 10,
        colorDictionary: ['#9fa8da', '#7986cb', '#90caf9', '#64b5f6', '#aed581', '#9ccc65']
    }]
});
