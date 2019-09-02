Ext.define('App.view.core.filter.RangeMultiSelectGrid', {
    extend: 'App.view.core.filter.BaseMultiSelectGrid',
    alias: 'widget.rangemultiselectgrid',

    store: {
        fields: ['from', 'to']
    },

    columns: [{
        dataIndex: 'from',
        xtype: 'datecolumn',
        renderer: this.itemRenderer,
        flex: 1
    }, {
        dataIndex: 'to',
        xtype: 'datecolumn',
        renderer: this.itemRenderer,
        flex: 1
    }],

    processValue: function (value) {
        return Ext.Array.from(value.ranges, true);
    },

    onDataChanged: function () {
        var values = this.getStore().getRange().reduce(function (result, rec) {
            var fromValue = rec.get('from'),
                toValue = rec.get('to'),
                key = '' + fromValue + toValue;

            if (!Ext.isEmpty(key) && !result[key]) {
                result[key] = new App.extfilter.core.Range(fromValue, toValue);
            }

            return result;
        }, {});

        this.value = new App.extfilter.core.RangeList(Ext.Object.getValues(values));
    }

});
