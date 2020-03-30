Ext.define('App.view.tpm.associatedpromo.AssociatedBTLPromo', {
    extend: 'App.view.core.common.AssociatedDirectoryView',
    alias: 'widget.associatedbtlpromo',

    defaults: {
        flex: 1,
        margin: '0 0 0 20'
    },
    items: [{
        xtype: 'container',
        itemId: 'associatedbtlsupportcontainer',
        margin: '10 0 20 20',
        layout: {
            type: 'vbox',
            align: 'stretch'
        },
        items: [{
            xtype: 'btl',
            itemId: 'mainwindow',
            minHeight: 150,
            flex: 1,
            suppressSelection: true,
            linkConfig: {
                'btlpromo': { masterField: 'Id', detailField: 'BTLId' }
            }
        }, {
            flex: 0,
            xtype: 'splitter',
            cls: 'associated-splitter'
        }, {
            xtype: 'btlpromo',
            itemId: 'linkedwindow',
            minHeight: 150,
            flex: 1,
            autoLoad: false,
        }]
	}]
});