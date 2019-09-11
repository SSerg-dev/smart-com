Ext.define('App.model.core.constraint.ConstraintPrefix', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'ConstraintPrefix',
    fields: [
		{ name: 'Id', type: 'string' },
		{ name: 'Description', type: 'string' }
	],
    proxy: {
        type: 'breeze',
        resourceName: 'ConstraintPrefixes',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});