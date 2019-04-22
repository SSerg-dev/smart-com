Ext.define('App.view.core.security.RolesView', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.rolesview',
    mixins: {
        bindable: 'Ext.util.Bindable',
    },

    glyph: 0xf004,
    frame: true,
    ui: 'light-gray-panel',
    bodyPadding: '10 10 0 10',

    items: {
        xtype: 'panel',
        ui: 'transparent-panel',
        itemId: 'buttonscontainer',

        layout: {
            type: 'vbox',
            align: 'stretch'
        },

        defaults: {
            margin: '0 0 10 0'
        }

    },

    constructor: function () {
        this.callParent(arguments);
        this.mixins.bindable.constructor.call(this, arguments);
    },

    initComponent: function () {
        this.callParent(arguments);
        this.buttonsContainer = this.down('#buttonscontainer');
        this.bindStore(this.store, true);

        this.addEvents('selectionchange');

        this.updateViewState();
    },

    getRole: function () {
        return this.selectedRole;
    },

    setRole: function (role) {
        var oldRole = this.selectedRole;

        if (oldRole !== role) {
            this.selectedRole = role;
            this.updateViewState();
        }
    },

    getStoreListeners: function () {
        return {
            load: this.onStoreLoad
        };
    },

    onBindStore: function (store) {
        this.refreshList(store);
    },

    onStoreLoad: function (store, records) {
        this.refreshList(store);
    },

    onRoleButtonClick: function (button) {
        var oldRole = this.selectedRole,
            newRole = button.getItemId();

        this.selectedRole = newRole;
        this.highlightButton();
    },

    refreshList: function (store) {
        var records = store.getRange(),
            buttons = records.map(function (rec) {
                return {
                    xtype: 'button',
                    itemId: rec.get('SystemName'),
                    text: rec.get('DisplayName'),
                    tooltip: rec.get('DisplayName'),
                    ui: 'role-item-button',
                    glyph: 0xf004,
                    textAlign: 'left',
                    listeners: {
                        scope: this,
                        click: this.onRoleButtonClick
                    }
                }
            }, this);

        Ext.suspendLayouts();
        this.buttonsContainer.removeAll();
        this.buttonsContainer.add(buttons);
        Ext.resumeLayouts(true);

        this.highlightButton();
    },

    updateViewState: function () {
        var role = this.getRole();

        if (role) {
            this.setTitle(role);
        } else {
            this.setTitle(l10n.ns('core').value('defaultRoleButtonText'));
        }

        this.highlightButton();
    },

    highlightButton: function () {
        var role = this.getRole();

        this.hasSelectedRole = false;

        this.buttonsContainer.query('button').forEach(function (button) {
            if (button.getItemId() === role) {
                button.setUI('selected-role-item-button');
                this.hasSelectedRole = true;
                this.fireEvent('selectionchange', this);
            } else {
                button.setUI('role-item-button');
            }
        }, this);
    }

});