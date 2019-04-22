Ext.define('App.view.tpm.filter.FilterConstructior', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.filterconstructor',
    name: 'filterconstructor',
    title: 'Filter',//l10n.getLocal('main').filterConstructorTitle,

    cls: 'scrollpanel',
    autoScroll: true,
    animCollapse: false,
    frame: true,
    ui: 'selectable-panel',
    minHeight: 250,

    systemHeaderItems: [{
        xtype: 'expandbutton',
        glyph: 0xf063,
        glyph1: 0xf04b,
        itemId: 'collapse',
        tooltip: l10n.ns('core', 'selectablePanelButtons').value('collapse'),
        target: function () {
            return this.up('filterconstructor');
        }
    }],

    customHeaderItems: [],

    customHeaderItemsDefaults: {
        border: '0 0 0 1',
        cls: 'custom-button'
    },

    systemHeaderItemsDefaults: {
        padding: '6 6 6 16'
    },

    header: {
        titlePosition: 0,
        height: 30,

        defaults: {
            xtype: 'button',
            ui: 'white-button',
            padding: 6 //TODO: временно
        }
    },

    layout: 'fit',

    items: [{
        xtype: 'container',
        name: 'filtercontainer',
        autoScroll: true,
        padding: 5
    }],
    buttons: [{
        text: 'Reset',
        itemId: 'reset'
    }, {
        text: 'Apply',
        itemId: 'apply'
    }],

    initComponent: function () {
        this.callParent(arguments);

        var filterModel = Ext.create('App.ExtSelectionFilterModel', {
            model: 'App.model.tpm.product.Product',
            modelId: 'efselectionmodel'
        });

        var filterEntries = filterModel.getFilterEntries(),
            data = [];
        filterEntries.forEach(function (item, index, array) {
            data.push({ FieldName: item.data.id, DisplayName: item.data.name, Field: item });
        });

        this.store = Ext.create('Ext.data.Store', {
            model: 'App.model.tpm.filter.FilterField',
            data: data
        });
    },

    updateHeader: function () {
        this.updateHeaderInternal(); // workaround for correct replace customHeaderItems
        this.callParent(arguments);
    },

    updateHeaderInternal: function () {
        var systemHeaderItems = Ext.Array.from(this.systemHeaderItems, true),
            customHeaderItems = Ext.Array.from(this.customHeaderItems, true);

        customHeaderItems.forEach(function (item) {
            Ext.applyIf(item, this.customHeaderItemsDefaults);
        }, this);

        systemHeaderItems.forEach(function (item) {
            Ext.applyIf(item, this.systemHeaderItemsDefaults);
        }, this);

        this.header.items = customHeaderItems.concat(systemHeaderItems);
    }
});