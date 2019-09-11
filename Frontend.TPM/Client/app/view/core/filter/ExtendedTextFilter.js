Ext.define('App.view.core.filter.ExtendedTextFilter', {
    extend: 'Ext.form.Panel',
    alias: 'widget.exttextfilter',
    ui: 'transparent-panel',
    minHeight: 300,

    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    items: [{
        xtype: 'textarea',
        itemId: 'filtertext',
        flex: 1
    }],

    constructor: function (model) {
        this.callParent(arguments);

        this.filterModel = model;

        var filterTextCmp = this.filterTextCmp = this.down('#filtertext');

        //filterTextCmp.on('change', this.onFilterTextChange, this);

        this.filterModelEventHandlers = this.filterModel.on({
            scope: this,
            destroyable: true,
            edit: this.onModelChange,
            reject: this.onModelChange,
            clear: this.onModelChange
        });

        this.updateView();
    },

    updateView: function () {
        this.setValueSilent(this.filterTextCmp, this.filterModel.get('filterText'));
    },

    onModelChange: function () {
        this.updateView();
    },

    //onFilterTextChange: function (cmp, newValue) {
    //    this.filterModel.set('filterText', newValue);
    //},

    onDestroy: function () {
        this.callParent(arguments);
        Ext.destroy(this.filterModelEventHandlers);
    },

    setValueSilent: function (cmp, value) {
        if (!cmp) {
            return;
        }

        cmp.suspendEvents();
        cmp.setValue(value);
        cmp.resumeEvents();
    },

    commitChanges: function () {
        this.filterModel.set('filterText', this.filterTextCmp.getValue());
    }

});