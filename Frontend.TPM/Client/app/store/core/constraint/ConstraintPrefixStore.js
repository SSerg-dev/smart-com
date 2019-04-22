Ext.define('App.store.core.constraint.ConstraintPrefixStore', {
    extend: 'Ext.data.Store',
    alias: 'store.constraintprefixstore',

    fields: ['id', 'text'],

    data: [
        //{ id: 'TEST_PROPERTY', text: l10n.ns('core', 'Constraint', 'ConstraintPrefixEnum').value('TEST_PROPERTY') }
    ]
});
