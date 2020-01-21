Ext.define('App.model.tpm.promo.UserRolePromo', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'UserDTO',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'Name', type: 'string', isDefault: true },
        {
            name: 'Email', type: 'string', isDefault: true
            //, hidden: function () {
            //    return App.UserInfo.getAuthSourceType() == 'ActiveDirectory'
            //    ;
            //}
        }
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