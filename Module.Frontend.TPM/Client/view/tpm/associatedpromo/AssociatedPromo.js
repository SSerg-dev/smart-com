﻿Ext.define('App.view.tpm.associatedpromo.AssociatedPromo', {
    extend: 'App.view.core.common.AssociatedDirectoryView',
    alias: 'widget.associatedpromo',

    items: [{
        xtype: 'promo',
        itemId: 'mainwindow',
        margin: '10 8 20 20',
        suppressSelection: true,
        linkConfig: {
            'sale': { masterField: 'Id', detailField: 'PromoId' }
        },
        minHeight: 383,
    }, {
        xtype: 'sale',
        itemId: 'linkedwindow',
        autoLoadStore: false,
        minHeight: 383,
    }]
});