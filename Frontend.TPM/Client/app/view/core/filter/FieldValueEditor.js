Ext.define('App.view.core.filter.FieldValueEditor', {
    extend: 'Ext.form.field.ComboBox',
    alias: 'widget.fieldvalueeditor',

    valueField: 'id',
    queryMode: 'local',
    caseSensitive: false,
    anyMatch: true,
    forceSelection: true,
    maxHeight: 30,

    store: {
        fields: ['id', 'text'],

        sorters: [{
            property: 'text',
            direction: 'asc'
        }]
    },

    constructor: function () {
        this.callParent(arguments);
    },

    beforeBlur: function () {
        var value = this.getRawValue();
        if (value == '') {
            this.lastSelection = [];
        }
        this.doQueryTask.cancel();
        this.assertValue();
    }

});