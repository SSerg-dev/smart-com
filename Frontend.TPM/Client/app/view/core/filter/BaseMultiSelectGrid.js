Ext.define('App.view.core.filter.BaseMultiSelectGrid', {
    extend: 'Ext.grid.Panel',
    alias: 'widget.basemultiselectgrid',

    cls: 'multiselectgrid',
    scroll: false,
    bodyBorder: false,
    sortableColumns: false,
    enableColumnHide: false,
    enableColumnMove: false,
    hideHeaders: true,
    columnLines: true,
    selType: 'cellmodel',
    padding: '10 7 7 10',

    viewConfig: {
        markDirty: false,
        trackOver: false
    },

    plugins: [{
        ptype: 'cellediting',
        clicksToEdit: 2
    }],

    constructor: function () {
        this.callParent(arguments);
        this.getStore().on({
            scope: this,
            datachanged: this.onDataChanged,
            update: this.onDataChanged
        });
    },

    afterRender: function () {
        this.callParent(arguments);

        var editorConfig = this.up('multiselectwindow').atomConfig;

        if (editorConfig) {
            Ext.Array.each(this.columns, function (col) {
                if (!col.editor) {
                    col.editor = Ext.widget(editorConfig);
                }
            });
        }
    },

    getValue: function () {
        return this.value;
    },

    setValue: function (value) {
        this.updateView(value);
        this.value = value;
    },

    onDataChanged: Ext.emptyFn,

    updateView: function (value) {
        this.getStore().loadData(this.processValue(value) || {});
    },

    processValue: Ext.identityFn

});
