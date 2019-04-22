Ext.define('App.store.tpm.priority.Priority', {
    extend: 'Ext.data.Store',
    alias: 'store.prioritystore',

    fields: ['id', 'text'],

    data: [
        { id: 1, text: '1' },
        { id: 2, text: '2' },
        { id: 3, text: '3' },
        { id: 4, text: '4' },
        { id: 5, text: '5' }
    ]
});
