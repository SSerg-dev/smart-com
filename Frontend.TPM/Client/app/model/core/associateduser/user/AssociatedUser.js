Ext.define('App.model.core.associateduser.user.AssociatedUser', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'UserDTO',
    fields: [
		{ name: 'Id', hidden: true },
		{ name: 'Name', type: 'string', isDefault: true },
		{
		    name: 'Sid', type: 'string', isDefault: true, hidden: function () {
		        return App.UserInfo.getAuthSourceType() == 'Database';
		    }
		},
        {
            name: 'Email', type: 'string', isDefault: true
            //, hidden: function () {
            //    return App.UserInfo.getAuthSourceType() == 'ActiveDirectory'
            //    ;
            //}
        },
		{ name: 'Password', type: 'string', hidden: true }
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'UserDTOs',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});