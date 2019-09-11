Ext.define('App.store.core.BooleanNonEmptyStore', {
    extend: 'Ext.data.Store',
    alias: 'store.booleannonemptystore',

    fields: ['id', 'text'],

    data: [      
        { id: true, text: l10n.ns('core', 'booleanValues').value('true') },
        { id: false, text: l10n.ns('core', 'booleanValues').value('false') }
    ]
});
