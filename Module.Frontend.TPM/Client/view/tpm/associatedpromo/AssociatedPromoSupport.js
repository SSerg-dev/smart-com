Ext.define('App.view.tpm.associatedpromo.AssociatedPromoSupport', {
    extend: 'App.view.core.common.AssociatedDirectoryView',
    alias: 'widget.associatedpromosupport',

    defaults: {
        flex: 1,
        margin: '0 0 0 20'
    },

    items: [{
        xtype: 'container',
        itemId: 'associatedpromosupportcontainer',
        margin: '10 0 20 20',
        layout: {
            type: 'vbox',
            align: 'stretch'
        },
        items: [{
            xtype: 'promosupport',
            minHeight: 150,
            flex: 1,
            suppressSelection: false,
            linkConfig: {
                'promolinkedticosts': { masterField: 'Id', detailField: 'PromoSupportId' }
            }
        }, {
            flex: 0,
            xtype: 'splitter',
            cls: 'associated-splitter'
        }, {
            xtype: 'promolinkedticosts',
            minHeight: 150,
            flex: 1,
            suppressSelection: false,
        }]
    }]
});