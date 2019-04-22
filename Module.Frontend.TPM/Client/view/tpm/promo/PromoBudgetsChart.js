Ext.define('Ext.chart.theme.budgetsTheme', { // TODO: вынести тему
    extend: 'Ext.chart.theme.Base',
    constructor: function (config) {
        this.callParent([Ext.apply({
            colors: ['#ec407a', '#ab47bc', '#5c6bc0', '#26a69a', '#ffa726']
        }, config)]);
    }
}); 

Ext.define('App.view.tpm.promo.PromoBudgetsChart', {
    extend: 'Ext.chart.Chart',
    alias: 'widget.promobudgetschart',
    animate: true,
    shadow: true,
    theme: 'budgetsTheme',
    insetPadding: 5,
    axes: [{
        type: 'Numeric',
        position: 'bottom',
        fields: ['shopper', 'marketing', 'cost', 'brand', 'btl'],
        title: false,
        grid: true,
        
        label: {
            //renderer: function (v, tst1, tst2) {
            //    debugger;
            //    return String(v).replace(/(.)00000$/, '.$1M');
            //}
        }
    }, {
        type: 'Category',
        position: 'left',
        fields: ['type'],
        title: false,
    }],
    series: [{
        type: 'bar',
        axis: 'bottom',
        gutter: 5,
        xField: 'type',
        yField: ['shopper', 'marketing', 'cost', 'brand', 'btl'],
        stacked: true,
        tips: {
            trackMouse: true,
            cls: 'summary-budget-tip',
            tpl: '<div>{headerText}</div> <div>{footerText}</div>',
            renderer: function (storeItem, item, tst1, tst2) {
                var fieldName = item.yField;
                var formatedValue = item.value[1].toString().replace(/\B(?=(\d{3})+(?!\d))/g, " ");
                var rec = item.storeItem;
                var total = rec.get('shopper') + rec.get('marketing') + rec.get('cost') + rec.get('brand') + rec.get('btl');
                total = total.toString().replace(/\B(?=(\d{3})+(?!\d))/g, " ");
                var text;
                switch (fieldName) {
                    case 'shopper':
                        text = 'Shooper TI';
                        break;
                    case 'marketing':
                        text = 'Marketing TI';
                        break;
                    case 'cost':
                        text = 'Cost Production';
                        break;
                    case 'brand':
                        text = 'Branding';
                        break;
                    case 'btl':
                        text = 'BTL';
                        break;
                    default:
                        text = '';
                        break;
                };
                
                text = Ext.String.format('{0} ({1}): {2}', text, item.value[0], formatedValue)
                var totalText = Ext.String.format('Total: {0}', total);
                this.update({ headerText: text, footerText: totalText});
            }
        },

        label: {
            display: 'insideStart',
            color: '#ffffff',
            field: ['shopper', 'marketing', 'cost', 'brand', 'btl'],
            renderer: function (value, label, storeItem, item, i, display, animate, index) {
                var fieldName = item.yField;
                var formatedValue = value.toString().replace(/\B(?=(\d{3})+(?!\d))/g, " ");
                var text;
                switch (fieldName) {
                    case 'shopper':
                        text = 'Shooper TI';
                        break;
                    case 'marketing':
                        text = 'Marketing TI';
                        break;
                    case 'cost':
                        text = 'Cost Production';
                        break;
                    case 'brand':
                        text = 'Branding';
                        break;
                    case 'btl':
                        text = 'BTL';
                        break;
                    default:
                        text = '';
                        break;
                };
                var formatedText = Ext.String.format('{0} {1}', text, formatedValue);
                // текст 11px - если не влезает - не показывать.
                return formatedText.length * 6 > item.attr.width ? '' : formatedText;
              
            }
        },
    }]
});
