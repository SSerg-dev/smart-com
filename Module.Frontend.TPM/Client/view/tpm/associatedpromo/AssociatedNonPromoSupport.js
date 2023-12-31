﻿Ext.define('App.view.tpm.associatedpromo.AssociatedNonPromoSupport', {
	extend: 'App.view.core.common.AssociatedDirectoryView',
	alias: 'widget.associatednonpromosupport',

	defaults: {
		flex: 1,
		margin: '0 0 0 20'
	},

	items: [{
		xtype: 'container',
        itemId: 'associatednonpromosupportcontainer',
		margin: '10 0 20 20',
		layout: {
			type: 'vbox',
			align: 'stretch'
		},
		items: [{
			xtype: 'nonpromosupport',
			itemId: 'mainwindow',
			minHeight: 150,
			flex: 1,
            suppressSelection: false,
            linkConfig: {
                'nonpromosupportbrandtechdetail': { masterField: 'Id', detailField: 'NonPromoSupportId' }
            }
        }, {
            flex: 0,
            xtype: 'splitter',
            cls: 'associated-splitter'
        }, {
            xtype: 'nonpromosupportbrandtechdetail',
			itemId: 'linkedwindow',
            minHeight: 150,
            flex: 1,
            suppressSelection: false
        }]
	}]
});