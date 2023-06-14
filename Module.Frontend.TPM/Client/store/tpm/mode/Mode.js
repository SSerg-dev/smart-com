Ext.define('App.store.tpm.mode.Mode', {
    extend: 'Ext.data.Store',
    alias: 'store.modestore',

    fields: ['id', 'alias', 'text'],

    data: [
        { id: 0, alias: 'Off', text: 'Production'},
        { id: 1, alias: 'RS', text: 'Rolling Scenario'},
        { id: 2, alias: 'RA', text: 'Resource Allocation'}
    ]
});
