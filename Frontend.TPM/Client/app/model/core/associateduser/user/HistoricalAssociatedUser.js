Ext.define('App.model.core.associateduser.user.HistoricalAssociatedUser', {
    extend: 'Ext.data.Model',
    idProperty: '_Id',
    breezeEntityType: 'HistoricalUser',
    fields: [
        { name: '_Id', type: 'string', hidden: true },
		{ name: '_ObjectId', hidden: true },
		{ name: '_User', type: 'string', isDefault: true },
		{ name: '_Role', type: 'string', isDefault: true },
		{ name: '_EditDate', type: 'date', isDefault: true },
		{ name: '_Operation', type: 'string', isDefault: true },
		{ name: 'Name', type: 'string', isDefault: true },
		{
		    name: 'Sid', type: 'string', isDefault: true, hidden: function () {
		        return App.UserInfo.getAuthSourceType() == 'Database';
		    }
		},
        {
            name: 'Email', type: 'string', isDefault: true, hidden: function () {
                return App.UserInfo.getAuthSourceType() == 'ActiveDirectory';
            }
        }
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'HistoricalUsers',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});