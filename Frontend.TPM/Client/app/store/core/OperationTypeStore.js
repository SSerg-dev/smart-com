Ext.define('App.store.core.OperationTypeStore', {
    extend: 'Ext.data.Store',
    alias: 'store.operationtypestore',

    fields: ['id', 'text'],

    data: [
        { id: null, text: '\u00a0' },
        { id: 'Created', text: l10n.ns('core', 'enums', 'OperationType').value('Created') },
        { id: 'Updated', text: l10n.ns('core', 'enums', 'OperationType').value('Updated') },
        { id: 'Disabled', text: l10n.ns('core', 'enums', 'OperationType').value('Disabled') }
    ]
});
