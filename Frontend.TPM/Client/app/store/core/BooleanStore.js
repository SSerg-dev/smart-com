Ext.define('App.store.core.BooleanStore', {
    extend: 'Ext.data.Store',
    alias: 'store.booleanstore',

    fields: ['id', 'text'],

    data: [
        { id: null, text: '\u00a0' },
        { id: true, text: l10n.ns('core', 'booleanValues').value('true') },
        { id: false, text: l10n.ns('core', 'booleanValues').value('false') }
    ]
});
