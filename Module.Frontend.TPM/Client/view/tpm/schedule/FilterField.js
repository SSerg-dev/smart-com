Ext.define('App.view.tpm.schedule.FilterField', {
    extend: 'Ext.form.field.ComboBox',
    xtype: 'schedulefilterfield',

    trigger1Cls: Ext.baseCSSPrefix + 'form-arrow-trigger',
    trigger2Cls: Ext.baseCSSPrefix + 'form-clear-trigger',
    tpl: new Ext.XTemplate('<tpl for=".">', '<div class="scheduler-client-combo x-boundlist-item">', '<div class="x-grid-row-checker" role="presentation"></div>', '<div class="scheduler-client-combo-text-overflow">{Name}</div>', '</div>', '</tpl>'),
    valueField: 'Name',
    displayField: 'Name',

    multiSelect: true,
    editable: false,
    autoSelect: false,
    forceSelection: true,

    onTrigger2Click: function () {
        var me = this;
        me.uncheckAll();
        me.clearValue();
        me.clearFilter();
        me.collapse();
    },

    store: {
        type: 'simplestore',
        idProperty: 'Id',
        model: 'App.model.tpm.baseclient.BaseClient',
        autoLoad: true,
        storeId: 'baseclientfiltersimplestore',
    },

    initComponent: function () {
        this.store = Ext.StoreMgr.lookup(this.store);
        this.callParent(arguments);
    },

    clearValue: function () {
        this.setValue(null);
    },

    uncheckAll: function () {
        var picker = this.getPicker();
        var nodes = picker.getNodes();
        Ext.each(nodes, function (node) {
            Ext.get(node).removeCls('x-grid-row-checked');
        });
    },

    onKeyUp: function (e) {
        if (e.getKey() === Ext.EventObject.ENTER) {
            var value = this.getFixedValue();
            this.filter(value);
        }
    },

    onBlur: function (e) {
        var value = this.getFixedValue();
        this.filter(value);
    },

    getFixedValue: function () {
        var value = this.getValue();
        if (Ext.isArray(value)) {
            if (value.length == 0) {
                value = '';
            } else if (value.length == 1) {
                value = value[0].toLowerCase();
            }
        } else {
            value = value.toLowerCase();
        }
        return value;
    },

    filter: function (value) {
        var me = this;
        var store = Ext.StoreMgr.lookup('MyResources');

        if (!Ext.isEmpty(value)) {
            var filterFn = function (item) {
                var re = new RegExp(value, 'i');
                return re.test(item.get('Name'));
            };
            var operator = 'like';
            if (Ext.isArray(value)) {
                if (value.length > 1) {
                    operator = 'in';
                    filterFn = function (item) {
                        var re = new RegExp('^' + value.join('|') + '$', 'i');
                        return re.test((Ext.isEmpty('Name') ? me.autoStoresNullValue : item.get('Name')));
                    }
                } else if (value.length == 1) {
                    value = value[0];

                }
            }
            
            var filter = Ext.create('Ext.util.Filter', {
                property: 'Name',
                value: value,
                type: 'string',
                operator: operator,
                filterFn: filterFn
            })
            me.clearFilter(store);
            store.filter(filter);
        } else {
            me.clearFilter(store);
            me.setValue('');
        }
    },

    clearFilter: function (store) {
        store = store ? store : Ext.StoreMgr.lookup('MyResources');
        store.clearFilter();
    },

    listeners: {
        keyup: 'onKeyUp',
        buffer: 200,
        blur: function () { this.onBlur(); },

        select: function (combo, records) {
            var node;
            Ext.each(records, function (rec) {
                node = combo.getPicker().getNode(rec);
                Ext.get(node).addCls('x-grid-row-checked');
            });
        },
        beforedeselect: function (combo, rec) {
            var node = combo.getPicker().getNode(rec);
            Ext.get(node).removeCls('x-grid-row-checked');
        }
    }
});
