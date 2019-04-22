Ext.define('App.view.core.filter.MultiSelectGrid', {
    extend: 'App.view.core.filter.BaseMultiSelectGrid',
    alias: 'widget.multiselectgrid',

    store: {
        fields: ['value']
    },

    initComponent: function () {
        Ext.apply(this, {
            columns: [{
                dataIndex: 'value',
                renderer: this.itemRenderer,
                flex: 1
            }]
        });
        this.callParent(arguments);
    },

    processValue: function (value) {
        if (Ext.isEmpty(value)) {
            return null;
        }

        return Ext.Array.from(value.values, true).map(function (item) {
            return { value: item };
        });
    },

    onDataChanged: function () {
        var values = this.getStore().getRange().map(function (rec) {
            return rec.get('value');
        });

        this.value = new App.extfilter.core.ValueList(Ext.Array.unique(values));
    }

});
