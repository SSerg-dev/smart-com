Ext.define('App.view.tpm.promo.UserRolePromoEditor', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.userrolepromoeditor',
    width: 500,
    minWidth: 500,
    maxHeight: 500,
    cls: 'readOnlyFields',

    items: {
        xtype: 'editorform',
        columnsCount: 1,
        items: [{
            xtype: 'singlelinedisplayfield',
            name: 'Name',
            fieldLabel: l10n.ns('core', 'AssociatedUser').value('Name')
        },  {
            xtype: 'singlelinedisplayfield',
            name: 'Email',
            fieldLabel: l10n.ns('core', 'AdUser').value('Email'),
        },  ]
    },

    buttons: [{
        text: l10n.ns('core', 'buttons').value('close'),
        itemId: 'close'
    }, {
        text: l10n.ns('core', 'buttons').value('cancel'),
        itemId: 'canceledit',
        hidden: true,
        setVisible: function () {
            return false;
        }
    }, {
        text: l10n.ns('core', 'buttons').value('edit'),
        itemId: 'edit',
        ui: 'green-button-footer-toolbar',
        hidden: true,
        setVisible: function () {
            return false;
        }
    }, {
        text: l10n.ns('core', 'buttons').value('ok'),
        itemId: 'ok',
        ui: 'green-button-footer-toolbar',
        hidden: true,
        setVisible: function () {
            return false;
        }
    }]
});