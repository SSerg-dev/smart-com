Ext.define('App.view.core.security.ModesView', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.modesview',
    mixins: {
        bindable: 'Ext.util.Bindable',
    },

    glyph: 0xf493,
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

    getMode: function () {
        return this.selectedMode;
    },

    setMode: function (mode) {
        var oldMode = this.selectedMode;

        if (oldMode !== mode) {
            this.selectedMode = mode;
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

    onModeButtonClick: function (button) {
        this.selectedMode = button.itemId;
        this.highlightButton();
    },

    refreshList: function (store) {
        var records = store.getRange();
        var buttons = records.map(function (rec) {
            return {
                xtype: 'button',
                itemId: rec.get('id'),
                text: rec.get('text'),
                tooltip: rec.get('text'),
                ui: 'role-item-button',
                glyph: 0xf493,
                textAlign: 'left',
                listeners: {
                    scope: this,
                    click: this.onModeButtonClick
                }
            }
        }, this);

        Ext.suspendLayouts();
        this.buttonsContainer.removeAll();
        this.buttonsContainer.add(buttons);
        Ext.resumeLayouts(true);

        this.highlightButton();
    },

    getTpmModeAlias: function(mode) {
        var item = this.store.findRecord('id', mode);
        if (!Ext.isEmpty(item)) {
            return item.data.alias;
        } else {
            return '';
        }
    },

    getTpmModeName: function(mode) {
        var item = this.store.findRecord('id', mode);
        if (!Ext.isEmpty(item)) {
            return item.data.text;
        } else {
            return '';
        }
    },
    
    updateViewState: function () {
        var mode = this.getMode();

        if (!Ext.isEmpty(mode)) {
            this.setTitle(this.getTpmModeName(mode));
        } else {
            this.setTitle('[The TPM mode is not selected]');
        }

        this.highlightButton();
    },

    highlightButton: function () {
        var mode = this.getMode();

        this.hasSelectedMode = false;

        this.buttonsContainer.query('button').forEach(function (button) {
            if (button.itemId === mode) {
                button.setUI('selected-role-item-button');
                this.hasSelectedMode = true;
                this.fireEvent('selectionchange', this);
            } else {
                button.setUI('role-item-button');
            }
        }, this);
    }
});