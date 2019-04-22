Ext.define('App.view.tpm.associatedpromo.AssociatedCostProduction', {
    extend: 'App.view.core.common.AssociatedDirectoryView',
    alias: 'widget.associatedcostproduction',

    defaults: {
        flex: 1,
        margin: '0 0 0 20'
    },

    items: [{
        xtype: 'container',
        itemId: 'associatedcostproductioncontainer',
        margin: '10 0 20 20',
        layout: {
            type: 'vbox',
            align: 'stretch'
        },
        items: [{
            xtype: 'costproduction',
            minHeight: 150,
            flex: 1,
            suppressSelection: false,
            linkConfig: {
                'promolinkedcostprod': { masterField: 'Id', detailField: 'PromoSupportId' }
            }
        }, {
            flex: 0,
            xtype: 'splitter',
            cls: 'associated-splitter'
        }, {
            xtype: 'promolinkedcostprod',
            minHeight: 150,
            flex: 1,
            suppressSelection: false,
        }]
    }]
});