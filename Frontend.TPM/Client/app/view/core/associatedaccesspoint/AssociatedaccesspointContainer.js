Ext.define('App.view.core.associatedaccesspoint.AssociatedaccesspointContainer', {
    extend: 'App.view.core.common.AssociatedDirectoryView',
    alias: 'widget.associatedaccesspointcontainer',

    defaults: {
        flex: 1,
        margin: '0 0 0 20'
    },

    items: [{
        xtype: 'associatedaccesspointaccesspoint',
        margin: '10 0 0 20',
        minHeight: 150,
		linkConfig: {
            'associatedaccesspointaccesspointrole': {
                masterField: 'Id',
                detailField: 'AccessPointId'
            }
		}
    }, {
        flex: 0,
        xtype: 'splitter',
        cls: 'associated-splitter'
    }, {
        xtype: 'associatedaccesspointaccesspointrole',
        margin: '0 0 20 20',
        minHeight: 150,
        suppressSelection: true
	}]

});