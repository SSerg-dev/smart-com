Ext.define('App.store.tpm.scenariotype.ScenarioType', {
    extend: 'Ext.data.Store',
    alias: 'store.scenariotypestore',

    fields: ['text'],

    data: [
        { text: 'BASE' },
        { text: 'LOW' },
        { text: 'MEDIUM' },
        { text: 'HIGH' }
    ]
});
