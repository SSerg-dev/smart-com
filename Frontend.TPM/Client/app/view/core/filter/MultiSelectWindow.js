Ext.define('App.view.core.filter.MultiSelectWindow', {
    extend: 'App.view.core.base.BaseModalWindow',
    alias: 'widget.multiselectwindow',
    autoScroll: true,
    resizeHandles: 'w e',
    cls: 'scrollable',

    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    width: 700,
    minWidth: 400,
    minHeight: 300,
    maxHeight: 700,

    title: l10n.ns('core', 'multiSelect').value('windowTitle'),

    buttons: [{
        text: l10n.ns('core', 'buttons').value('cancel'),
        itemId: 'cancel'
    }, {
        text: l10n.ns('core', 'buttons').value('ok'),
        ui: 'green-button-footer-toolbar',
        itemId: 'ok'
    }],

    constructor: function () {
        this.callParent(arguments);
        this.addEvents('select');
        App.Util.callWhenRendered(this, this.updateSwitchViewButtonsState);
    },

    initComponent: function () {

        Ext.apply(this, {
            items: [{
                xtype: 'panel',
                itemId: 'viewcontainer',
                frame: true,
                ui: 'light-gray-panel',
                layout: 'card',
                minHeight: 200,
                margin: '10 8 15 15',

                items: [{
                    xtype: 'multiselectgrid',
                    itemId: 'valuelistview',
                    itemRenderer: this.itemRenderer
                }, {
                    xtype: 'rangemultiselectgrid',
                    itemId: 'rangelistview',
                    itemRenderer: this.itemRenderer
                }],

                dockedItems: [{
                    xtype: 'toolbar',
                    ui: 'light-gray-toolbar',
                    cls: 'directorygrid-toolbar',
                    dock: 'right',
                    itemId: 'filtertoolbar',

                    width: 30,
                    minWidth: 30,
                    maxWidth: 250,

                    layout: {
                        type: 'vbox',
                        align: 'stretch',
                        pack: 'start'
                    },

                    defaults: {
                        ui: 'gray-button-toolbar',
                        padding: 6, //TODO: временно
                        textAlign: 'left'
                    },

                    items: [{
                        xtype: 'widthexpandbutton',
                        ui: 'fill-gray-button-toolbar',
                        cls: 'tr-radius-button',
                        glyph: 0xf13d,
                        glyph1: 0xf13e,
                        target: function () {
                            return this.up('toolbar');
                        }
                    }, {
                        itemId: 'valuelist',
                        glyph: 0xf56a,
                        tooltip: l10n.ns('core', 'multiSelect').value('multiValueButtonText'),
                        text: l10n.ns('core', 'multiSelect').value('multiValueButtonText')
                    }, {
                        itemId: 'rangelist',
                        glyph: 0xf570,
                        tooltip: l10n.ns('core', 'multiSelect').value('multiRangeButtonText'),
                        text: l10n.ns('core', 'multiSelect').value('multiRangeButtonText')
                    }, '-', {
                        itemId: 'add',
                        glyph: 0xf412,
                        tooltip: l10n.ns('core', 'multiSelect', 'buttons').value('add'),
                        text: l10n.ns('core', 'multiSelect', 'buttons').value('add')
                    }, {
                        itemId: 'remove',
                        glyph: 0xf413,
                        tooltip: l10n.ns('core', 'multiSelect', 'buttons').value('delete'),
                        text: l10n.ns('core', 'multiSelect', 'buttons').value('delete')
                    }, '-']

                }]

            }]
        });

        this.callParent(arguments);
        this.addButton = this.down('#add');
        this.removeButton = this.down('#remove');
        this.cancelButton = this.down('#cancel');
        this.okButton = this.down('#ok');
        this.switchViewButtons = [
            this.down('#valuelist'),
            this.down('#rangelist')
        ];

        this.switchViewButtons[1].setDisabled(Ext.isDefined(this.allowRange) && !this.allowRange);
    },

    initEvents: function () {
        this.callParent(arguments);
        this.addButton.on('click', this.onAddButtonClick, this);
        this.removeButton.on('click', this.onRemoveButtonClick, this);
        this.cancelButton.on('click', this.onCancelButtonClick, this);
        this.okButton.on('click', this.onOkButtonClick, this);
        this.switchViewButtons.forEach(function (button) {
            button.on('click', this.onSwitchViewButtonClick, this);
        }, this);
    },

    setValue: function (value) {
        App.Util.callWhenRendered(this, function () {
            var activeView = this.switchView(this.getFormatForValue(value));
            activeView.setValue(value);
        });
    },

    getValue: function () {
        return this.down('#viewcontainer')
            .getLayout()
            .getActiveItem()
            .getValue();
    },

    switchView: function (format) {
        var layout = this.down('#viewcontainer').getLayout(),
            view;

        layout.getActiveItem().getStore().removeAll();
        view = layout.setActiveItem(format + 'view') || layout.getActiveItem();
        this.updateSwitchViewButtonsState();

        return view;
    },

    getFormatForValue: function (value) {
        switch (Ext.getClassName(value)) {
            case 'App.extfilter.core.ValueList':
                return 'valuelist';
            case 'App.extfilter.core.RangeList':
                return 'rangelist';
            default:
                return 'valuelist';
        }
    },

    onAddButtonClick: function () {
        this.down('#viewcontainer')
            .getLayout()
            .getActiveItem()
            .getStore()
            .add({});
    },

    onRemoveButtonClick: function () {
        var grid = this.down('#viewcontainer').getLayout().getActiveItem(),
            selections = grid.getSelectionModel().getSelection();

        grid.getStore().remove(selections);
    },

    onCancelButtonClick: function () {
        this.close();
    },

    onOkButtonClick: function () {
        this.fireEvent('select', this, this.getValue());
        this.close();
    },

    updateSwitchViewButtonsState: function () {
        var format = this.down('#viewcontainer').getLayout().getActiveItem().getItemId();

        this.switchViewButtons.forEach(function (button) {
            //button.setDisabled(button.getItemId() + 'view' === format);
            if (button.getItemId() + 'view' === format) {
                button.setUI('blue-pressed-button-toolbar-toolbar');
            } else {
                button.setUI('gray-button-toolbar-toolbar');
            }
        });
    },

    onSwitchViewButtonClick: function (button) {
        this.switchView(button.getItemId());
    }

});