Ext.define('App.model.core.ImportModel', {
    extend: 'Ext.data.Model',
    idProperty: undefined,

    fields: [
        {
            name: 'Id',
            hidden: true
        },
        {
            name: 'HasErrors',
            type: 'boolean'
        },
        {
            name: 'ErrorMessage'
        },
        {
            name: 'ImportId',
            hidden: true,
            defaultValue: '00000000-0000-0000-0000-000000000000'
        }
    ],

    afterUpdate: function (grid) {
        if (grid && grid.importData && grid.importData.importId) {
            this.set('ImportId', grid.importData.importId);
        }
    }

});