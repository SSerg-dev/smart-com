Ext.define('App.store.tpm.mode.Mode', {
    extend: 'Ext.data.Store',
    alias: 'store.modestore',

    fields: ['id', 'alias', 'text'],

    data: [
        TpmModes.Prod,
        TpmModes.RS,
        TpmModes.RA
    ]
});
