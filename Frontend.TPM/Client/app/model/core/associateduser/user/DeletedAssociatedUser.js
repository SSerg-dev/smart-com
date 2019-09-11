Ext.define('App.model.core.associateduser.user.DeletedAssociatedUser', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'User',
    fields: [
		{ name: 'Id', hidden: true },
		{ name: 'DeletedDate', type: 'date', isDefault: true },
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
        resourceName: 'DeletedUsers',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});