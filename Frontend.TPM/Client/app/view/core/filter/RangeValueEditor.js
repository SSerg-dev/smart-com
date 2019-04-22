Ext.define('App.view.core.filter.RangeValueEditor', {
    extend: 'Ext.container.Container',
    alias: 'widget.rangevalueeditor',
    mixins: {
        field: 'Ext.form.field.Field',
    },

    layout: {
        type: 'hbox',
        pack: 'start',
        align: 'middle'
    },

    constructor: function (config) {
        this.callParent(arguments);
        this.mixins.field.constructor.call(this, arguments);
        Ext.apply(this, config);
    },

    initComponent: function () {
        Ext.apply(this, {

            defaults: Ext.apply({
                padding: '0 5 0 0',
            }, this.atomConfig),

            items: [{
                xtype: 'label',
                text: l10n.ns('core', 'filter').value('labelFrom')
            }, {
                itemId: 'fromfield',
                flex: 1
            }, {
                xtype: 'label',
                text: l10n.ns('core', 'filter').value('labelTo')
            }, {
                itemId: 'tofield',
                flex: 1,
                padding: 0
            }]

        });

        this.callParent(arguments);

        this.fromCmp = this.down('#fromfield');
        this.toCmp = this.down('#tofield');

        this.initField();
    },

    initEvents: function () {
        this.callParent(arguments);
        this.fromCmp.on({
            scope: this,
            change: this.onRangeFieldsChanged
        });
        this.toCmp.on({
            scope: this,
            change: this.onRangeFieldsChanged
        });
    },

    onRangeFieldsChanged: function (cmp, newValue, oldValue) {
        this.value = Ext.create('App.extfilter.core.Range', this.fromCmp.getValue(), this.toCmp.getValue());
        this.checkChange();
    },

    setValue: function (value) {
        this.fromCmp.setValue(value ? value.from : null);
        this.toCmp.setValue(value ? value.to : null);
        return this.mixins.field.setValue.call(this, value);
    }

});