Ext.define('App.view.core.filter.ExtendedSelectionFilter', {
    extend: 'Ext.form.Panel',
    alias: 'widget.extselectionfilter',
    scrollable: true,
    ui: 'transparent-panel',
    minHeight: 150,

    layout: {
        type: 'vbox',
        pack: 'start',
        align: 'stretch'
    },

    constructor: function (model) {
        this.callParent();
        this.filterModel = model;

        this.filterModelEventHandlers = this.filterModel.on({
            scope: this,
            destroyable: true,
            clear: this.onClearModel,
            remove: this.onRemoveEntry,
            removeFull: this.onRemoveFullEntry,
            add: this.onAddEntry,
            addFull: this.onAddFullEntry

        });

        App.Util.callWhenRendered(this, this.renderModel);
    },

    renderModel: function () {
        var filterEntries = this.filterModel.getFilterEntries(),
            rows = this.createRowWidgets(filterEntries);

        Ext.suspendLayouts();
        this.removeAll();
        this.add(rows);
        Ext.resumeLayouts(true);
    },
    onAddFullEntry: function (entry) {
        var filterEntries = entry.getFilterEntries(),
            rows = this.createRowWidgets(filterEntries);

        Ext.suspendLayouts();
        this.removeAll();
        this.add(rows);
        Ext.resumeLayouts(true);
    },
    onRemoveFullEntry: function (entry) {
        var filterEntries = entry.getFilterEntries(),
            rows = this.createRowWidgets(filterEntries);
        Ext.suspendLayouts();
        this.removeAll();
        this.add(rows);
        Ext.resumeLayouts(true);
    },
    onAddEntry: function (model, entry) {
        this.add(this.createRowWidgets(entry));
    },

    onRemoveEntry: function (model, entry) {
        this.remove(entry.get('id'));
    },

    onClearModel: function () {
        this.renderModel();
    },

    onDestroy: function () {
        this.callParent(arguments);
        Ext.destroy(this.filterModelEventHandlers);
    },

    createRowWidgets: function (entries) {
        return Ext.Array.from(entries).map(function (item) {
            return Ext.createByAlias('widget.extfilterrow', item, App.view.core.filter.ValueEditorFactory);
        });
    },

    commitChanges: function () {
        this.query('extfilterrow')
            .forEach(function (row) {
                row.commitChanges();
            }, this);
    }

});