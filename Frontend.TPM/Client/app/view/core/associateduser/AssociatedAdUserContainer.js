Ext.define('App.view.core.associateduser.AssociatedAdUserContainer', {
    extend: 'App.view.core.common.AssociatedDirectoryView',
    alias: 'widget.associatedadusercontainer',

    defaults: {
        flex: 1,
        margin: '0 0 0 20'
    },

    items: [{
        xtype: 'associatedaduseruser',
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