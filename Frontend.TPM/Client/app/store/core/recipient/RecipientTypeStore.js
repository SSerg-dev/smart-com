Ext.define('App.store.core.recipient.RecipientTypeStore', {
    extend: 'Ext.data.Store',
    alias: 'store.recipienttypestore',

    fields: ['id', 'text'],

    data: [
        { id: 'Email', text: l10n.ns('core', 'AssociatedRecipient', 'RecipientTypeEnum').value('EMAIL') },
        { id: 'User', text: l10n.ns('core', 'AssociatedRecipient', 'RecipientTypeEnum').value('USER') },
        { id: 'Role', text: l10n.ns('core', 'AssociatedRecipient', 'RecipientTypeEnum').value('ROLE') },
        { id: 'Function', text: l10n.ns('core', 'AssociatedRecipient', 'RecipientTypeEnum').value('FUNCTION') }
    ]
});
