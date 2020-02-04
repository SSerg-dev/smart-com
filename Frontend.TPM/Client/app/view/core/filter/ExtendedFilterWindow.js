Ext.define('App.view.core.filter.ExtendedFilterWindow', {
    extend: 'App.view.core.base.BaseModalWindow',
    alias: 'widget.extfilter',
    autoScroll: true,
    resizeHandles: 'w e',
    cls: 'scrollable',

    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    title: l10n.ns('core', 'filter').value('filterTitle'),
    width: '80%',
    minHeight: 267,
    minWidth: 700,

    buttons: [{
        text: l10n.ns('core', 'filter', 'buttons').value('close'),
        itemId: 'cancel'
    }, {
        text: l10n.ns('core', 'filter', 'buttons').value('reject'),
        itemId: 'reject'
    }, {
        text: l10n.ns('core', 'filter', 'buttons').value('apply'),
        ui: 'green-button-footer-toolbar',
        itemId: 'apply'
    }],

    items: [{
        xtype: 'panel',
        itemId: 'modelcontainer',
        frame: true,
        ui: 'light-gray-panel',
        layout: 'fit',
        bodyPadding: '10 10 0 10',
        margin: '10 8 15 15',
        flex: 1,

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
                text: l10n.ns('core', 'selectablePanelButtons').value('toolbarCollapse'),
                cls: 'tr-radius-button',
                glyph: 0xf13d,
                glyph1: 0xf13e,
                target: function () {
                    return this.up('toolbar');
                }
            }, {
                itemId: 'efselectionmodelbutton',
                glyph: 0xf16b,
                text: l10n.ns('core', 'filter').value('selectionFilter'),
                tooltip: l10n.ns('core', 'filter').value('selectionFilter')
            }, {
                itemId: 'eftextmodelbutton',
                glyph: 0xf60e,
                text: l10n.ns('core', 'filter').value('textFilter'),
                tooltip: l10n.ns('core', 'filter').value('textFilter')
            }, '-', {
                itemId: 'efsettingsbutton',
                glyph: 0xf493,
                text: l10n.ns('core', 'filter', 'buttons').value('settings'),
                tooltip: l10n.ns('core', 'filter', 'buttons').value('settings')
            }, '-']

        }]

    }],

    constructor: function (ctx) {
        this.callParent();
        this.filterContext = ctx;

        this.modelContainer = this.down('#modelcontainer');
        this.settingsButton = this.down('#filtertoolbar #efsettingsbutton');
        this.modelToggleButtons = this.down('#filtertoolbar').query('button[itemId*=model]');

        this.contextEventHandlers = this.filterContext.on({
            scope: this,
            destroyable: true,
            filtermodelchange: function () {
                App.Util.callWhenRendered(this, this.renderContext);
            }
        });

        App.Util.callWhenRendered(this, function () {
            this.initButtons();
            this.renderContext(ctx.needShowTextFilterFirst);
        });
    },

    getFilterContext: function () {
        return this.filterContext;
    },

    renderContext: function (needShowTextFilterFirst) {
        var filterContext = this.getFilterContext();
        var filterModel = filterContext.getFilterModel();

        if (needShowTextFilterFirst) {
            var textFilterModel = filterContext.getFilterModelById('eftextmodel');
            if (textFilterModel) {
                textFilterModel.updateFromFilter(filterContext.filter);
                filterModel = textFilterModel;
            }
        }

        if (!filterModel) {
            return;
        }

        this.activeView = filterModel.getView();
        this.updateButtons(filterModel);

        Ext.suspendLayouts();
        this.modelContainer.removeAll();
        this.modelContainer.add(this.activeView);
        Ext.resumeLayouts(true);

        filterContext.needShowTextFilterFirst = false;
    },

    initButtons: function () {
        var models = this.getFilterContext().getSupportedModels();

        this.settingsButton.setDisabled(true);

        this.modelToggleButtons.forEach(function (button) {
            button.setDisabled(true);
        });

        models.forEach(function (model) {
            var modelButton = this.modelToggleButtons.filter(function (button) {
                return button.getItemId() === model.getModelId() + 'button';
            }, this);

            if (!Ext.isEmpty(modelButton)) {
                modelButton[0].setDisabled(false);
            }
        }, this);
    },

    updateButtons: function (filterModel) {
        this.modelToggleButtons.forEach(function (button) {
            if (button.getItemId() === filterModel.getModelId() + 'button') {
                button.setUI('blue-pressed-button-toolbar-toolbar');
            } else {
                button.setUI('gray-button-toolbar-toolbar');
            }
        });

        this.settingsButton.setDisabled(filterModel.getModelId() !== 'efselectionmodel');
    },

    onDestroy: function () {
        this.callParent(arguments);
        Ext.destroy(this.contextEventHandlers);
    }
});