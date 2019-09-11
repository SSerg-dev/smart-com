Ext.define('App.view.core.associateduser.AssociatedDbUserContainer', {
    extend: 'App.view.core.common.AssociatedDirectoryView',
    alias: 'widget.associateddbusercontainer',

    defaults: {
        flex: 1,
        margin: '0 0 0 20'
    },

    items: [{
        xtype: 'associateddbuseruser',
        margin: '10 0 0 20',
        minHeight: 150,
		linkConfig: {
            'associateduseruserrole': {
                masterField: 'Id',
                detailField: 'UserId'
            }
		}
    }, {
        flex: 0,
        xtype: 'splitter',
        cls: 'associated-splitter'
    }, {
        xtype: 'associateduseruserrole',
        margin: '0 0 20 20',
        minHeight: 150,
        suppressSelection: true
	}]
});