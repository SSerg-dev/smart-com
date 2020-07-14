Ext.define('App.model.tpm.mechanictype.MechanicTypeForClient', {
    extend: 'App.model.tpm.mechanictype.MechanicType',

    proxy: {
        type: 'breeze',
        resourceName: 'MechanicTypes',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
